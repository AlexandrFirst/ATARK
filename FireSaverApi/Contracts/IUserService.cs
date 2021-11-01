using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;

namespace FireSaverApi.Contracts
{
    public interface IUserService
    {
        Task<UserInfoDto> CreateNewUser(RegisterUserDto newUserInfo);
        Task<UserInfoDto> UpdateUserInfo(UserInfoDto newUserInfo);
        Task<UserInfoDto> GetUserInfoById(int userId);
        Task ChangeOldPassword(int userId, NewUserPasswordDto newUserPassword);
        Task<List<RoutePoint>> BuildEvacuationRootForCompartment(int userId);
        
    }
}