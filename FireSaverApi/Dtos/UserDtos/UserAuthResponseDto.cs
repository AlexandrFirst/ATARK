using System.Collections.Generic;

namespace FireSaverApi.Dtos
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public int? ResponsibleBuildingId { get; set; }
    }
}