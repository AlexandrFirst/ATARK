using System.Threading.Tasks;

namespace FireSaverApi.Contracts
{
    public interface ISocketService
    {
         Task OpenDoorWithIot(int iotId);
         Task SetAlarmForBuilding(int buildingId);
         Task LogoutUser(int userId);
    }
}