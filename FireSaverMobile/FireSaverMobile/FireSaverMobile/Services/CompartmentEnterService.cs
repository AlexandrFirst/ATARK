using FireSaverMobile.Contracts;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using FireSaverMobile.Models.TestCompartmentModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Services
{
    public class CompartmentEnterService : BaseHttpService, ICompartmentEnterService
    {
        public async Task<ServerResponse> EnterShelter(int shelterId)
        {
            var response = await client.GetRequest<ServerResponse>($"http://{serverAddr}/Building/shelter/enter/{shelterId}");
            return response;
        }

        public async Task<CompartmentCommonInfo> GetCompartmentById(int compartmentId)
        {
            var compartmentInfo = await client.GetRequest<CompartmentCommonInfo>($"http://{serverAddr}/Building/compartment/{compartmentId}");
            return compartmentInfo;

        }

        public async Task<ServerResponse> LeaveShelter(int shelterId)
        {
            var response = await client.DeleteRequest<ServerResponse>($"http://{serverAddr}/Building/shelter/leave");
            return response;
        }

        public async Task<TestCompModel> SendCompartmentData(UserEnterCompartmentDto compartmentEnterInfo)
        {
            var response = await compartmentEnterInfo.PostRequest(client, $"http://{serverAddr}/User/enter");
            TestCompModel test = await transformHttpResponse<TestCompModel>(response);
            return test;
        }

        public async Task<ServerResponse> SendTestAnswers(AnswerListModel compartmentEnterData)
        {
            var response = await compartmentEnterData.PostRequest(client, $"http://{serverAddr}/Test/answerCompartmentTest");
            ServerResponse serverResponse = await transformHttpResponse<ServerResponse>(response);
            return serverResponse;
        }
    }
}
