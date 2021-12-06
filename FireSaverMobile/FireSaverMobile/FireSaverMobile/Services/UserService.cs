using FireSaverMobile.Contracts;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Services
{
    public class UserService : BaseHttpService, IUserService
    {
        public async Task<UserInfoDto> GetUserInfoById(int userId)
        {
            UserInfoDto response = await client.GetRequest<UserInfoDto>($"http://{serverAddr}/User/{userId}");
            return response;

        }

        public async Task<UserInfoDto> UpdateUserInfo(UserInfo userInfo)
        {
            if (userInfo == null)
                return null;
            var updatingUserInfo = userInfo;
            HttpResponseMessage response = await userInfo.PutRequest(client, $"http://{serverAddr}/User/updateInfo");

            UserInfoDto updatedUserInfo = await transformHttpResponse<UserInfoDto>(response);
            return updatedUserInfo;
        }
    }
}
