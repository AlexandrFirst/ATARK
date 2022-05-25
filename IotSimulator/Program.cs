using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;

namespace IotSimulator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            String identifier = "abcdef";
            String mqtt_username = "iot";
            String mqtt_password = "password";

            AuthResponse response = await PostBasicAsync("http://192.168.0.2:5000/IoT/loginIot",
                new AuthRequest { Identifier = identifier },
                new CancellationToken());
            System.Console.WriteLine("Authorizing");
            System.Console.WriteLine(response.UserId + ": " + response.Token);

            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            string clientId = Guid.NewGuid().ToString();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost")
                .WithClientId(clientId)
                .WithCleanSession(false)
                .WithCredentials(mqtt_username, mqtt_password)
                .WithKeepAlivePeriod(System.TimeSpan.FromSeconds(60))
                .Build();

            System.Console.WriteLine("Connecting...");

            
            mqttClient.UseDisconnectedHandler(async e =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await mqttClient.ConnectAsync(options, CancellationToken.None);
                }
                catch
                {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            });

            mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic($"door/{response.UserId}").Build());

                Console.WriteLine("### SUBSCRIBED ###");
            });

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            System.Console.WriteLine("Connected...");


            Console.ReadKey();
        }

        private static async Task<AuthResponse> PostBasicAsync(string Url, AuthRequest content, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, Url))
            {
                var json = JsonConvert.SerializeObject(content);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        string s = await response.Content.ReadAsStringAsync();
                        AuthResponse serverResponse = JsonConvert.DeserializeObject<AuthResponse>(s);
                        return serverResponse;
                    }
                }
            }
        }
    }
}
