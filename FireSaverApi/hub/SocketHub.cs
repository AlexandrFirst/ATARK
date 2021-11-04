using System;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.Dtos.IoTDtos;
using FireSaverApi.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace FireSaverApi.hub
{
    [Authorize]
    public class SocketHub : Hub
    {
        private readonly IUserContextService userContextService;
        private readonly IIoTService iotService;
        private readonly IUserHelper userHelper;
        private readonly IBuildingHelper buildingHelper;
        private readonly ISocketService socketService;

        public SocketHub(IUserContextService userContextService,
                        IIoTService iotService,
                        IUserHelper userHelper,
                        IBuildingHelper buildingHelper,
                        ISocketService socketService)
        {
            this.userHelper = userHelper;
            this.buildingHelper = buildingHelper;
            this.socketService = socketService;
            this.iotService = iotService;
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

        public async Task RecieveMessage(int fromUserId, string message)
        {
            var compartmentId = (await userHelper.GetUserById(fromUserId)).Id;
            var buildingId = await iotService.FindBuildingWithCompartmentId(compartmentId);
            await socketService.SendMessageToResponsibleBuildingUsers(buildingId, message);
        }

        public async Task RecieveIotInfo(string id, IoTDataInfo dataInfo)
        {
            await iotService.AnalizeIoTDataInfo(id, dataInfo);
        }
    }
}