using System.Collections.Generic;

namespace FireSaverApi.Helpers
{
    public class MyHttpContext
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public List<string> RolesList { get; set; }
    }
}