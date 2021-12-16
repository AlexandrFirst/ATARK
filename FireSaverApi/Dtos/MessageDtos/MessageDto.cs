using System;
using FireSaverApi.DataContext;

namespace FireSaverApi.Dtos.MessageDtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public DateTime SendTime { get; set; }
        public UserInfoDto User { get; set; }
        public MessageType MessageType { get; set; }
        public string PlaceDescription { get; set; }
    }
}