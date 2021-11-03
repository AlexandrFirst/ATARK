using System.Threading.Tasks;
using FireSaverApi.DataContext;

namespace FireSaverApi.Contracts
{
    public interface IGuestStorage
    {
         Task AddGuest(int userId);
         Task RemoveGuest(int userId);
    }
}