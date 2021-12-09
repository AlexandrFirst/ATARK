using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
{
    public enum UserMessageType { FIRE, PERSONAL_HELP, OTHER_HELP }

    public class MessageDto
    {
        public int UserId { get; set; }
        public UserMessageType MessageType { get; set; }
    }
}
