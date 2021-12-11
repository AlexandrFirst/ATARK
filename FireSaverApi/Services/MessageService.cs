using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.MessageDtos;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly DatabaseContext context;
        private readonly ISocketService socketService;
        private readonly IBuildingHelper buildingHelper;
        private readonly IMapper mapper;
        private readonly IUserHelper userHelper;

        public MessageService(DatabaseContext context,
                            ISocketService socketService,
                            IBuildingHelper buildingHelper,
                            IMapper mapper,
                            IUserHelper userHelper)
        {
            this.buildingHelper = buildingHelper;
            this.mapper = mapper;
            this.userHelper = userHelper;
            this.socketService = socketService;
            this.context = context;
        }

        public async Task DeleteMessage(int messageId, int userId)
        {
            var messageToDelete = await context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);

            var currentUser = await userHelper.GetUserById(userId);

            var currentBuilding = await buildingHelper.GetBuildingById(currentUser.ResponsibleForBuilding.Id);
            var responsibleUsersForBuilding = currentBuilding.ResponsibleUsers;

            foreach (var user in responsibleUsersForBuilding)
            {
                if(user.Id == userId)
                    continue;
                await socketService.DeleteMessage(user.Id, messageToDelete.Id);
            }

            context.Remove(messageToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<List<MessageDto>> GetAllMessagesForBuilding(int userId)
        {
            var user = await userHelper.GetUserById(userId);

            List<Message> messages = await context.Messages.Include(b => b.Building).Where(m => m.Building.Id == user.ResponsibleForBuilding.Id).ToListAsync();
            var messagesDto = mapper.Map<List<MessageDto>>(messages);
            return messagesDto;
        }

        public async Task SendMessage(int fromUserId, MessageType messageType)
        {
            var user = await userHelper.GetUserById(fromUserId);
            if (user.CurrentCompartment == null)
                throw new System.Exception("Can't send messages without compartment");

            var currentBuilding = await buildingHelper.GetBuildingByCompartment(user.CurrentCompartment.Id);
            var messageToSend = new Message()
            {
                Building = currentBuilding,
                MessageType = messageType,
                SendTime = DateTime.Now,
                User = user,
                PlaceDescription = user.CurrentCompartment.Name + " " + 
                    user.CurrentCompartment.Description
            };

            context.Add(messageToSend);
            await context.SaveChangesAsync();

            foreach (var respUsers in currentBuilding.ResponsibleUsers)
            {
                await socketService.SendMessage(
                    mapper.Map<UserInfoDto>(user),
                    respUsers.Id, 
                    messageType, 
                    messageToSend.Id,
                    user.CurrentCompartment.Name + " " + 
                    user.CurrentCompartment.Description);
            }

        }
    }
}