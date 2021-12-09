using FireSaverMobile.Contracts;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using FireSaverMobile.Models.CompartmentModels;
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

        public async Task<Position> UpdateUserWorldPosition(Position newPos)
        {
            var response = await newPos.PostRequest(client, $"http://{serverAddr}/User/setWorldPosition");
            var updatePos = await transformHttpResponse<Position>(response);
            return updatePos;
        }

        public async Task<Position> GetTransformedWorldPosition(int compartmentId, Position worldPostion)
        {
            var response = await worldPostion.PostRequest(client, $"http://{serverAddr}/User/GetTransformedPostions/{compartmentId}");
            var mappedPos = await transformHttpResponse<Position>(response);
            return mappedPos;
        }

        public async Task<CompartmentCommonInfo> SetUserCompartment(int userId, int compartmentId)
        {
            var dataToSend = new UserCompartmentDto()
            {
                CompartmentId = compartmentId,
                UserId = userId
            };

            var response = await dataToSend.PostRequest<UserCompartmentDto>(client, $"http://{serverAddr}/User/setUserCompartment");
            var compartmentInfo = await transformHttpResponse<CompartmentCommonInfo>(response);
            return compartmentInfo;
        }

        public async Task SendMessage(MessageDto messageDto)
        {
            await messageDto.PostRequest(client, $"http://{serverAddr}/Message/SendMessage");
        }

        public async Task BlockPoint(int pointId)
        {
            await client.DeleteRequest<ServerResponse>($"http://{serverAddr}/RouteBuilder/routepoint/block/{pointId}");
        }
    }
}
