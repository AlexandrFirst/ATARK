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

            int currentThreshold = 0;
            bool isDoorOpened = false;
            bool isAlarmOn = false;

            TimerController timer = new TimerController("Timer 1");
            timer.InitTimer();
            timer.TimerElapsed += timeElapedHandler;

            void timeElapedHandler()
            {
                if (!isAlarmOn)
                {
                    System.Console.WriteLine("Door is closed");
                    isDoorOpened = false;
                }
            }

            AuthResponse response = await PostBasicAsync<AuthResponse, AuthRequest>("http://192.168.0.2:5000/IoT/loginIot",
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

                string topic = e.ApplicationMessage.Topic;
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                Console.WriteLine($"+ Topic = {topic}");
                Console.WriteLine($"+ Payload = {payload}");
                Console.WriteLine();

                if (topic == "door/" + response.UserId)
                {
                    if (payload == "open")
                    {
                        isDoorOpened = true;
                        System.Console.WriteLine("Door is open");

                        if (!isAlarmOn)
                            timer.StartTimer();
                    }
                    else if (payload == "close")
                    {
                        if (isAlarmOn)
                        {
                            isAlarmOn = false;
                            System.Console.WriteLine("Alram is off");
                        }

                        if (isDoorOpened)
                        {
                            if (timer.timer.Enabled)
                                timer.StopTimer();
                            else
                                isDoorOpened = false;

                            System.Console.WriteLine("Door is closed");
                        }
                    }
                    else if (payload == "alarm" && !isAlarmOn)
                    {
                        isAlarmOn = true;
                        System.Console.WriteLine("Alarm is on");
                        System.Console.WriteLine("Door is opened");
                        timer.StartTimer();
                        isDoorOpened = true;
                    }
                }

            });

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            System.Console.WriteLine("Connected...");

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    return;
                }
                float sensorValue = float.Parse(input);
                if (sensorValue > 50)
                {
                    currentThreshold++;
                }
                if (currentThreshold > 5)
                {
                    await PostBasicAsync<ServerResponse, IoTDataInfo>($"http://192.168.0.2:5000/IoT/iotDataSent/{identifier}", new IoTDataInfo()
                    {
                        SensorValue = sensorValue
                    }, new CancellationToken(), response.Token);

                    currentThreshold = 0;
                }
            }
        }

        private static async Task<Response> PostBasicAsync<Response, Request>(string Url, Request content, CancellationToken cancellationToken, string token = null)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, Url))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
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
                        Response serverResponse = JsonConvert.DeserializeObject<Response>(s);
                        return serverResponse;
                    }
                }
            }
        }
    }
}
