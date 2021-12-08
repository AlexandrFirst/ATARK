using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.MessageDtos;

namespace FireSaverApi.Contracts
{
    public interface IMessageService
    {
         Task SendMessage(int fromUserId, MessageType messageType);
         Task DeleteMessage(int messageId, int userId);
         Task<List<MessageDto>> GetAllMessagesForBuilding(int userId);
    }
}