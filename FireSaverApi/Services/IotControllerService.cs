using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.Services.shared;
using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.Server;

namespace FireSaverApi.Services
{
    public class IotControllerService : IIotControllerService
    {
        private readonly MqttHostedServer messager;
        public IotControllerService()
        {
            var service = ServiceLocator.Instance.GetService(typeof(MqttHostedServer));
            messager = (MQTTnet.AspNetCore.MqttHostedServer)service;
        }

        public async Task CloseDoor(int IoTId)
        {
            var msg = new MqttApplicationMessageBuilder()
                .WithPayload("close")
                .WithTopic($"door/{IoTId}");

            await messager.PublishAsync(msg.Build());
        }

        public async Task OpenDoor(int IoTId)
        {
            var msg = new MqttApplicationMessageBuilder()
                .WithPayload("open")
                .WithTopic($"door/{IoTId}");

            await messager.PublishAsync(msg.Build());
        }

        public async Task SetAlarm(int IoTId)
        {
            var msg = new MqttApplicationMessageBuilder()
                .WithPayload("alarm")
                .WithTopic($"door/{IoTId}");

            await messager.PublishAsync(msg.Build());
        }
    }
}