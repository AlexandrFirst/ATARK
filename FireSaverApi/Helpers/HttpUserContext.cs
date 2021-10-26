using System.Collections.Generic;

namespace FireSaverApi.Helpers
{
    public class HttpUserContext
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public List<string> Roles { get; set; }
    }
}