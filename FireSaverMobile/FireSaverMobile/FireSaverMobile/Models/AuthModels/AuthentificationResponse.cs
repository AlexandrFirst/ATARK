using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
{
    public class AuthentificationResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public int? ResponsibleBuildingId { get; set; }
    }
}
