using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.CompartmentDtos;
using FireSaverApi.Dtos.TestDtos;

namespace FireSaverApi.Contracts
{
    public interface IUserService
    {
        Task<UserInfoDto> CreateNewUser(RegisterUserDto newUserInfo, string Role);
        Task<UserInfoDto> UpdateUserInfo(UserInfoDto newUserInfo);
        Task<UserInfoDto> GetUserInfoById(int userId);
        Task ChangeOldPassword(int userId, NewUserPasswordDto newUserPassword);
        Task<List<RoutePointDto>> BuildEvacuationRootForCompartment(int userId);
        Task<TestOutputDto> EnterCompartmentById(int userId, int compartmentId, int? iotId);
        Task<CompartmentCommonInfo> SetCompartment(int userId, int compartmentId);
        Task<CompartmentCommonInfo> SetCompartmentByEvacPlan(int userId, int evacPlanId);
        Task SetAlaramForBuilding(int userId);
        Task SwitchOffAlaramForBuilding(int userId);
        
        Task<UserInfoDto> SetWorldPostion(int userId, PositionDto worldUserPostion);
        Task<bool> CheckIfUserCanBeResponsible(string userMail);
        Task<PositionDto> TransformWorldPostionToMap(PositionDto worldPostion, int compartmentId);
        Task<PositionDto> TransformWorldPostionToMapByEvacPlanId(PositionDto worldPostion, int EvacPlanId);
    }
}