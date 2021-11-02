using System.Threading.Tasks;
using FireSaverApi.DataContext;

namespace FireSaverApi.Contracts
{
    public interface IIoTHelper
    {
         Task<IoT> GetIoTById(string IotIdentifier);
    }
}