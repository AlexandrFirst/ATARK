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
        Task<CompartmentCommonInfo> SetUserCompartment(int userId, int compartmentId);
        Task SendMessage(MessageDto messageDto);
        Task BlockPoint(int pointId);
    }
}
