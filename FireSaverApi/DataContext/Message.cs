using System;

namespace FireSaverApi.DataContext
{
    public enum MessageType { FIRE, PERSONAL_HELP, OTHER_HELP }
    public class Message
    {
        public int Id { get; set; }
        public DateTime SendTime { get; set; }
        public User User { get; set; }
        public Building Building { get; set; }
        public MessageType MessageType { get; set; }
        public string PlaceDescription { get; set; }
    }
}