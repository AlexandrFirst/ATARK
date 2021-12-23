using System.Threading.Tasks;

namespace FireSaverApi.Contracts
{
    public interface IIotControllerService
    {
         Task OpenDoor(int IoTId);
         Task CloseDoor(int IoTId);
         Task SetAlarm(int IoTId);
    }
}