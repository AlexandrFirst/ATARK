using System.Threading.Tasks;

namespace FireSaverApi.Contracts
{
    public interface ISocketService
    {
         Task OpenDoorWithIot(int iotId);
         Task SetAlarmForBuilding(int buildingId);
         Task SendMessageToResponsibleBuildingUsers(int buildingId, string message);
    }
}