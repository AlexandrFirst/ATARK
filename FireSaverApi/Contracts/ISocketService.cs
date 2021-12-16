using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;

namespace FireSaverApi.Contracts
{
    public interface ISocketService
    {
         Task OpenDoorWithIot(int iotId);
         Task SetAlarmForBuilding(int buildingId);
         Task SwitchOffAlarmForBuilding(int buildingId);
         Task LogoutUser(int userId);
         Task SendMessage(UserInfoDto fromId, int toUserId, MessageType messageType, int messageId, string placeDescription);
         Task DeleteMessage(int userIdMeesageToDeleteOn, int messageId);
    }
}