using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.MessageDtos;
using FireSaverApi.hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class SocketService : ISocketService
    {
        private readonly IHubContext<SocketHub> socketHub;
        private readonly DatabaseContext dataContext;
        private readonly IIotControllerService iotControllerService;

        public SocketService(IHubContext<SocketHub> socketHub,
                            DatabaseContext dataContext,
                            IIotControllerService iotControllerService)
        {
            this.iotControllerService = iotControllerService;
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
            foreach (var user in inboundBuildingUsers.Item1)
            {
                await socketHub.Clients.Group(user.Id.ToString()).SendAsync("Alarm");
            }

            foreach (var iot in inboundBuildingUsers.Item2)
            {
                await iotControllerService.SetAlarm(iot.Id);
            }

            

        }

        public async Task SwitchOffAlarmForBuilding(int buildingId)
        {
            var inboundBuildingUsers = await GetAllBuildingUsers(buildingId);
            foreach (var user in inboundBuildingUsers.Item1)
            {
                await socketHub.Clients.Group(user.Id.ToString()).SendAsync("AlarmOff");
            }

            foreach (var iot in inboundBuildingUsers.Item2)
            {
                await iotControllerService.CloseDoor(iot.Id);
            }
        }

        private async Task<(IList<User>, IList<IoT>)> GetAllBuildingUsers(int buildingId)
        {
            var responsibleUsers = await dataContext.Users.Include(u => u.ResponsibleForBuilding).Where(u => u.ResponsibleForBuilding.Id == buildingId).ToListAsync();

            var buildingWithFloors = dataContext.Buildings.Include(b => b.Floors);

            var buildingFloors = await buildingWithFloors
                .ThenInclude(f => f.InboundUsers)
                .Include(b => b.Floors)
                .ThenInclude(i => i.Iots)
                .FirstOrDefaultAsync(b => b.Id == buildingId);

            var buildingRooms = await buildingWithFloors
                .ThenInclude(f => f.Rooms)
                .ThenInclude(r => r.InboundUsers)
                .Include(b => b.Floors)
                .ThenInclude(f => f.Rooms)
                .ThenInclude(i => i.Iots)
                .FirstOrDefaultAsync(b => b.Id == buildingId);

            var floorUsers = GetAllBuildingFloorUsers(buildingFloors);
            var roomUsers = GetAllBuildingRoomUsers(buildingRooms);

            var allUsers = floorUsers.Union(roomUsers).Union(responsibleUsers).Distinct().ToList();

            var floorIots = GetAllFloorIoTs(buildingFloors);
            var roomIots = GetAllRoomIots(buildingFloors);

            var allIots = floorIots.Union(roomIots).Distinct().ToList();

            return (allUsers, allIots);
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

        private List<IoT> GetAllRoomIots(Building buildingWithRooms)
        {
            List<IoT> roomIots = buildingWithRooms.Floors.SelectMany(f => f.Rooms.SelectMany(i => i.Iots)).ToList();
            return roomIots;
        }

        private List<IoT> GetAllFloorIoTs(Building buildingWithRooms)
        {
            List<IoT> floorIots = buildingWithRooms.Floors.SelectMany(i => i.Iots).ToList();
            return floorIots;
        }

        public async Task SendMessage(UserInfoDto fromUser, int toUserId, MessageType messageType, int messageId, string placeDescription)
        {
            await socketHub.Clients.Group(toUserId.ToString()).SendAsync("MessageRecieved", new MessageDto
            {
                Id = messageId,
                MessageType = messageType,
                SendTime = DateTime.Now,
                User = new UserInfoDto()
                {
                    Name = fromUser.Name,
                    Surname = fromUser.Surname,
                    TelephoneNumber = fromUser.TelephoneNumber
                },
                PlaceDescription = placeDescription
            });
        }

        public async Task DeleteMessage(int userIdMeesageToDeleteOn, int messageId)
        {
            await socketHub.Clients.Group(userIdMeesageToDeleteOn.ToString()).SendAsync("DeleteMessage",
                new { MessageId = messageId });
        }
    }
}