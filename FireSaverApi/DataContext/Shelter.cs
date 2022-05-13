using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class Shelter
    {
        public Shelter()
        {
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public string Info { get; set; }
        public string ShelterPosition { get; set; }
        public IList<User> Users { get; set; }
        public Building Building { get; set; }
    }
}