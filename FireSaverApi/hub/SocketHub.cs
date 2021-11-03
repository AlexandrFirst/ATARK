using System;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace FireSaverApi.hub
{
    [Authorize]
    public class SocketHub : Hub
    {
        private readonly IUserContextService userContextService;

        public SocketHub(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }

        public async override Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userContextService.GetUserContext().Id.ToString());
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userContextService.GetUserContext().Id.ToString());
            System.Console.WriteLine("Connection is close");
            await base.OnDisconnectedAsync(exception);
        }

        // public async Task SendMessageTo(int Id, string message)
        // {
        //     await Clients.Group(Id.ToString()).SendAsync("RecieveMessage", message);
        // }
    }
}