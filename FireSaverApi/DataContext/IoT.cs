using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class IoT //gas detector is MQ-135
    {
        public IoT()
        {
            Messages = new List<Message>();
        }
        public int Id { get; set; }
        public string MapPosition { get; set; }
        public string IotIdentifier { get; set; }
        public bool IsAuthNeeded { get; set; }
        public float SensorValue { get; set; }
        public Compartment Compartment { get; set; }
        public IList<Message> Messages { get; set; }
    }
}