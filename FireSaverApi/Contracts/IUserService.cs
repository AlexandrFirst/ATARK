using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.TestDtos;

namespace FireSaverApi.Contracts
{
    public interface IUserService
    {
        Task<UserInfoDto> CreateNewUser(RegisterUserDto newUserInfo, string Role);
        Task<UserInfoDto> UpdateUserInfo(UserInfoDto newUserInfo);
        Task<UserInfoDto> GetUserInfoById(int userId);
        Task ChangeOldPassword(int userId, NewUserPasswordDto newUserPassword);
        Task<List<RoutePoint>> BuildEvacuationRootForCompartment(int userId);
        Task<TestOutputDto> EnterCompartmentById(int userId, int compartmentId, int? iotId);
        Task SetAlaramForBuilding(int userId);
        Task<IList<User>> GetAllGuests();
    }
}