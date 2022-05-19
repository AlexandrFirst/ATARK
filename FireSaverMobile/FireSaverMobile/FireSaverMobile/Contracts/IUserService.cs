using FireSaverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Contracts
{
    public interface IUserService
    {
        Task<UserInfoDto> UpdateUserInfo(UserInfo userInfo);
        Task<UserInfoDto> GetUserInfoById(int userId);
        Task<Position> UpdateUserWorldPosition(Position newPos);
        Task<Position> GetTransformedWorldPosition(int compartmentId, Position worldPostion);
        Task<CompartmentCommonInfo> SetUserCompartmentByCompartmentId(int userId, int compartmentId);
        Task<CompartmentCommonInfo> SetUserCompartmentByEvacPlanId(int userId, int evacPlanId);
        Task SendMessage(MessageDto messageDto);
        Task BlockPoint(Position positionToBlock);
    }
}
