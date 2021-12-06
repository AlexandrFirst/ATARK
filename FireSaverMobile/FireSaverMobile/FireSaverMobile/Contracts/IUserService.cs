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
    }
}
