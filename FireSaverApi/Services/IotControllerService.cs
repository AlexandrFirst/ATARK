using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.Services.shared;
using MQTTnet;
using MQTTnet.Server;

namespace FireSaverApi.Services
{
    public class IotControllerService : IIotControllerService
    {
        public async Task OpenDoor(int IoTId)
        {
            var service = ServiceLocator.Instance.GetService(typeof(MQTTnet.AspNetCore.MqttHostedServer));
            var messager = (MQTTnet.AspNetCore.MqttHostedServer)service;

            var msg = new MqttApplicationMessageBuilder()
                .WithPayload("open")
                .WithTopic($"door/{IoTId}");
               
            await messager.PublishAsync(msg.Build());
        }
    }
}