using FireSaverMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FireSaverMobile.Services
{
    public class BaseHttpService
    {
        protected const string serverAddr = "192.168.0.2:5000";
        protected static HttpClient client;

        private async Task<string> getToken()
        {
            var token = await SecureStorage.GetAsync("token");
            return token;
        }

        protected async Task setToken(string tokenValue)
        {
            var token = tokenValue;
            await SecureStorage.SetAsync("token", token);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        protected async Task writeAuthValues(AuthentificationResponse authResponse)
        {
            deleteAuthValues();
            await setToken(authResponse.Token);
            await SecureStorage.SetAsync("roles", string.Join(",", authResponse.Roles));

            if (authResponse.ResponsibleBuildingId.HasValue)
            {
                await SecureStorage.SetAsync("responsibleBuildingId", authResponse.ResponsibleBuildingId.ToString());
            }

            await SecureStorage.SetAsync("userId", authResponse.UserId.ToString());
        }

        protected void deleteAuthValues() 
        {
            SecureStorage.RemoveAll();
        }

        public async Task<AuthentificationResponse> retrieveAuthValues() 
        {
            return new AuthentificationResponse()
            {
                ResponsibleBuildingId = await retrieveBuildingId(),
                Roles = await retrieveUserRoles(),
                Token = await getToken(),
                UserId = await retrieveUserId()
            };
        }

        private async Task<int?> retrieveBuildingId() 
        {
            var buildingId = await SecureStorage.GetAsync("responsibleBuildingId");
            if (buildingId != null)
                return Convert.ToInt32(buildingId.ToString());
            return null;
        }

        private async Task<List<string>> retrieveUserRoles() 
        {
            string storedUserRoles = await SecureStorage.GetAsync("roles");
            if (storedUserRoles != null) 
            {
                return storedUserRoles.Split(',').ToList();
            }
            return new List<string>();
        }

        private async Task<int> retrieveUserId() 
        {
            var userId = await SecureStorage.GetAsync("userId");
            if (userId == null)
                return -1;
            return Convert.ToInt32(userId);
        }

        protected async Task<T> transformHttpResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                if(body!= null)
                    return await JsonConvert.DeserializeObjectAsync<T>(body);
            }
            return default(T);
        }

        private void initClient()
        {
            new Task(async () =>
            {
                var token = await getToken();
                if (token != null && client != null)
                    await setToken(token);
            }).RunSynchronously();
        }

        public BaseHttpService()
        {
            client = new HttpClient();
            initClient();

        }


    }
}
