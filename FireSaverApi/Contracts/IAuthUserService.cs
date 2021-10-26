using System.Threading.Tasks;
using FireSaverApi.Dtos;
using FireSaverApi.Helpers;

namespace FireSaverApi.Contracts
{
    public interface IAuthUserService
    {
         Task<HttpUserContext> GetUserContext(int userId);
         Task<UserAuthResponseDto> AuthUser(AuthUserDto userAuth);
    }
}