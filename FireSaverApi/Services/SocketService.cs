using System.Linq;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class SocketService : ISocketService
    {
        private readonly IHubContext<SocketHub> socketHub;
        private readonly DatabaseContext dataContext;

        public SocketService(IHubContext<SocketHub> socketHub,
                            DatabaseContext dataContext)
        {
            this.socketHub = socketHub;
            this.dataContext = dataContext;
        }

        public async Task OpenDoorWithIot(int iotId)
        {
            await socketHub.Clients.Group(iotId.ToString()).SendAsync("OpenDoor");
        }

        public async Task SendMessageToResponsibleBuildingUsers(int buildingId, string message)
        {
            var building = await dataContext.Buildings.Include(b => b.ResponsibleUsers).FirstOrDefaultAsync(b => b.Id == buildingId);
            if (building == null)
            {
                throw new System.Exception("Building is not found");
            }

            foreach (var user in building.ResponsibleUsers)
            {
                await socketHub.Clients.Group(user.Id.ToString()).SendAsync("RecieveMessage", message);
            }
        }

        public async Task SetAlarmForBuilding(int buildingId)
        {
            var buildingWithFloors = dataContext.Buildings.Include(b => b.Floors);

            var floorUsers = await buildingWithFloors.ThenInclude(f => f.InboundUsers).FirstOrDefaultAsync(b =>  b.Id == buildingId);
            var roomUsers = await buildingWithFloors.ThenInclude(f => f.Rooms).ThenInclude(r => r.InboundUsers).FirstOrDefaultAsync(b =>  b.Id == buildingId);
        
            foreach(var user in floorUsers.Floors.SelectMany(f => f.InboundUsers))
            {
                await socketHub.Clients.Group(user.Id.ToString()).SendAsync("Alarm");
            }
        
            foreach(var user in floorUsers.Floors.SelectMany(f => f.Rooms.SelectMany(r => r.InboundUsers)))
            {
                await socketHub.Clients.Group(user.Id.ToString()).SendAsync("Alarm");
            }
        }
        
    }
}