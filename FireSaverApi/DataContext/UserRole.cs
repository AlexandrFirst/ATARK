using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class UserRole
    {
        public UserRole()
        {
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<User> Users { get; set; }
    }
}