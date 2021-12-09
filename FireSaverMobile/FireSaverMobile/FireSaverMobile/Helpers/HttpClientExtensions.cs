using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Helpers
{



    public static class HttpClientExtensions
    {

        public static async Task<HttpResponseMessage> PostRequest<T>(this T model, HttpClient client, string url)
        {
            HttpContent content = await prePostPutRequestWork(model);

            HttpResponseMessage response = await client.PostAsync(url, content);

            return response;
        }

        public static async Task<HttpResponseMessage> PutRequest<T>(this T model, HttpClient client, string url)
        {
            HttpContent content = await prePostPutRequestWork(model);

            HttpResponseMessage response = await client.PutAsync(url, content);

            return response;
        }
        

        public static async Task<T> GetRequest<T>(this HttpClient client, string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                return await postGetDeleteRequestWord<T>(response);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                return default(T);
            }
        }

        public static async Task<T> DeleteRequest<T>(this HttpClient client, string url)
        {
            HttpResponseMessage response = await client.DeleteAsync(url);

            return await postGetDeleteRequestWord<T>(response);
        }


        private static async Task<dynamic> postGetDeleteRequestWord<T>(HttpResponseMessage response) 
        {
            dynamic output = null;

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                try
                {
                    output = JsonConvert.DeserializeObject<T>(content);
                }
                catch (Exception e) 
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

            return output;
        }

        private static async Task<HttpContent> prePostPutRequestWork<T>(T model) 
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            string json = await JsonConvert.SerializeObjectAsync(model, Formatting.None, serializerSettings);


            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            return content;
        }
    }
}
