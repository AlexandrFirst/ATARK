using System.Threading.Tasks;

namespace FireSaverApi.Contracts
{
    public interface IIotControllerService
    {
         Task OpenDoor(int IoTId);
    }
}