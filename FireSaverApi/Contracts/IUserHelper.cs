using System.Threading.Tasks;
using FireSaverApi.DataContext;

namespace FireSaverApi.Contracts
{
    public interface IUserHelper
    {
        Task<User> GetUserById(int userId);
    }
}