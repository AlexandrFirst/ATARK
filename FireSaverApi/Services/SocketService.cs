using System.Collections.Generic;
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
        public async Task LogoutUser(int userId)
        {
            await socketHub.Clients.Group(userId.ToString()).SendAsync("LogOut");
        }

        public async Task SetAlarmForBuilding(int buildingId)
        {
            var inboundBuildingUsers = await GetAllBuildingUsers(buildingId);
            foreach (var user in inboundBuildingUsers)
            {
                await socketHub.Clients.Group(user.Id.ToString()).SendAsync("Alarm");
            }

        }

        private async Task<IList<User>> GetAllBuildingUsers(int buildingId)
        {
            var buildingWithFloors = dataContext.Buildings.Include(b => b.Floors);

            var buildingFloors = await buildingWithFloors.ThenInclude(f => f.InboundUsers).FirstOrDefaultAsync(b => b.Id == buildingId);
            var buildingRooms = await buildingWithFloors.ThenInclude(f => f.Rooms).ThenInclude(r => r.InboundUsers).FirstOrDefaultAsync(b => b.Id == buildingId);

            var floorUsers = GetAllBuildingFloorUsers(buildingFloors);
            var roomUsers = GetAllBuildingRoomUsers(buildingRooms);

            var allUsers = floorUsers.Union(roomUsers).ToList();
            return allUsers;
        }

        private List<User> GetAllBuildingFloorUsers(Building buildingWithFloors)
        {
            List<User> floorUsers = buildingWithFloors.Floors.SelectMany(f => f.InboundUsers).ToList();
            return floorUsers;
        }

        private List<User> GetAllBuildingRoomUsers(Building buildingWithRooms)
        {
            List<User> roomUsers = buildingWithRooms.Floors.SelectMany(f => f.Rooms.SelectMany(r => r.InboundUsers)).ToList();
            return roomUsers;
        }
    }
}