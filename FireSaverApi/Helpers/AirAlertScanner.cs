using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TL;

namespace FireSaverApi.Helpers
{
    public class AirAlertScanner : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly TelegramData telegramData;

        private readonly Dictionary<long, ChatBase> Chats = new();
        private readonly Dictionary<long, TL.User> Users = new();

        private IServiceScope scope;
        private ISocketService socketService;
        private DatabaseContext databaseContext;

        private WTelegram.Client Client;
        private TL.User My;

        private string regexPattern = @"^.+(\d{2}\:\d{2}) (?<status>Повітряна тривога в |Відбій тривоги в )(?<place>.+)$";

        private Regex rx;
        public AirAlertScanner(IServiceScopeFactory scopeFactory, IOptions<TelegramData> telegramData)
        {
            this.scopeFactory = scopeFactory;
            this.telegramData = telegramData.Value;

            rx = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public void Dispose()
        {
            scope?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            scope = scopeFactory.CreateScope();
            socketService = scope.ServiceProvider.GetRequiredService<ISocketService>();
            databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            Client = new WTelegram.Client(s =>
            {
                switch (s)
                {
                    case "api_id": return telegramData.API_ID;
                    case "api_hash": return telegramData.API_HASH;
                    case "phone_number": return telegramData.PHONE_NUMBER;
                    default: return null;
                }
            });

            Client.Update += Client_Update;
            WTelegram.Helpers.Log = (lvl, str) => { };
            My = await Client.LoginUserIfNeeded();
            var dialogs = await Client.Messages_GetAllDialogs();
            dialogs.CollectUsersChats(Users, Chats);
        }

        private void Client_Update(IObject arg)
        {
            if (arg is not UpdatesBase updates) return;
            foreach (var update in updates.UpdateList)
                switch (update)
                {
                    case UpdateNewMessage unm:
                        {
                            MessageBase messageBase = unm.message;
                            switch (messageBase)
                            {
                                case TL.Message m:
                                    {
                                        string chatName = GetChatName(m.peer_id);
                                        System.Console.WriteLine(chatName);
                                        if (chatName == "Повітряна Тривога")
                                        {
                                            string message = m.message;
                                            MatchCollection matches = rx.Matches(message);
                                            foreach (Match match in matches)
                                            {
                                                GroupCollection groups = match.Groups;
                                                Group statusGroup, areaGroup;

                                                groups.TryGetValue("status", out statusGroup);
                                                groups.TryGetValue("place", out areaGroup);

                                                if (statusGroup == null || areaGroup == null)
                                                    return;

                                                System.Console.WriteLine("Place: " + areaGroup.Value + "; status: " + statusGroup.Value);

                                                string[] regions = areaGroup.Value.Split(" та ");

                                                foreach (string region in regions)
                                                {
                                                    int[] buildingsId = databaseContext.Buildings
                                                        .Where(b => b.Region == region)
                                                        .Select(b => b.Id).ToArray<int>();

                                                    foreach (int buildingId in buildingsId)
                                                    {
                                                        if (statusGroup.Value == "Повітряна тривога в ")
                                                        {
                                                            Task.Run(async () => await socketService.SetAlarmForBuilding(buildingId));
                                                        }
                                                        else if (statusGroup.Value == "Відбій тривоги в ")
                                                        {
                                                            Task.Run(async () => await socketService.SwitchOffAlarmForBuilding(buildingId));
                                                        }
                                                    }

                                                }

                                            }
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                }
        }

        private string GetChatName(Peer peer)
        {
            if (peer is PeerChannel)
            {
                return Chat(peer.ID);
            }
            return null;
        }

        private string Chat(long id) => Chats.TryGetValue(id, out var chat) ? chat.Title : null;
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}