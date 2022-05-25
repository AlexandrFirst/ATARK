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
            var userInfo = await transformHttpResponse<UserInfoDto>(response);
            return userInfo.LastSeenBuildingPosition;
        }

        public async Task<Position> GetTransformedWorldPosition(int evacPlanId, Position worldPostion)
        {
            var response = await worldPostion.PostRequest(client, $"http://{serverAddr}/User/GetTransformedPostionsByEvacPlanId/{evacPlanId}");
            var mappedPos = await transformHttpResponse<Position>(response);
            return mappedPos;
        }

        public async Task<CompartmentCommonInfo> SetUserCompartmentByCompartmentId(int userId, int compartmentId)
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

        public async Task<CompartmentCommonInfo> SetUserCompartmentByEvacPlanId(int userId, int evacPlanId)
        {
            var dataToSend = new UserCompartmentDto()
            {
                CompartmentId = evacPlanId,
                UserId = userId
            };

            var response = await dataToSend.PostRequest<UserCompartmentDto>(client, $"http://{serverAddr}/User/setUserCompartmentByEvacPlan");
            var compartmentInfo = await transformHttpResponse<CompartmentCommonInfo>(response);
            return compartmentInfo;
        }



        public async Task SendMessage(MessageDto messageDto)
        {
            await messageDto.PostRequest(client, $"http://{serverAddr}/Message/SendMessage");
        }

        public async Task BlockPoint(Position blockPos)
        {
            var response = await blockPos.PostRequest<Position>(client, $"http://{serverAddr}/RouteBuilder/addBlockePoint/40");
            var blockPointResponse = await transformHttpResponse<ServerResponse>(response);
        }
    }
}
