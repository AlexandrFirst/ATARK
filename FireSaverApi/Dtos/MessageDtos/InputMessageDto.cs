using System;
using FireSaverApi.DataContext;

namespace FireSaverApi.Dtos.MessageDtos
{
    public class InputMessageDto
    {
        public int UserId { get; set; }
        public MessageType MessageType { get; set; }
    }
}