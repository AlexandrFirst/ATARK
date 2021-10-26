using System.Threading.Tasks;
using FireSaverApi.Dtos;

namespace FireSaverApi.Contracts
{
    public interface IUserService
    {
        Task<UserInfoDto> CreateNewUser(RegisterUserDto newUserInfo);
        Task<UserInfoDto> UpdateUserInfo(UserInfoDto newUserInfo);
        Task<UserInfoDto> GetUserInfoById(int userId);
        
    }
}