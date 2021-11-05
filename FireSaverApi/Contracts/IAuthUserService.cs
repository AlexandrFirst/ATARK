using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Helpers;

namespace FireSaverApi.Contracts
{
    public interface IAuthUserService
    {
        Task<MyHttpContext> GetUserContext(int userId);
        Task<AuthResponseDto> AuthUser(AuthUserDto userAuth);
        Task<AuthResponseDto> AuthGuest();
        Task LogoutGuest(int guestId);
        Task<IList<User>> GetAllGuests();

    }
}