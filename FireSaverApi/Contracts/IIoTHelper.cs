using System.Threading.Tasks;
using FireSaverApi.DataContext;

namespace FireSaverApi.Contracts
{
    public interface IIoTHelper
    {
         Task<IoT> GetIoTByIdentifier(string IotIdentifier);
         Task<IoT> GetIoTById(int iotId);
    }
}