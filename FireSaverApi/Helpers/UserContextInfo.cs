using System.Collections.Generic;

namespace FireSaverApi.Helpers
{
    public class UserContextInfo
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public List<string> Roles { get; set; }
    }
}