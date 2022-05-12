using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class Building
    {
        public Building()
        {
            ResponsibleUsers = new List<User>();
            Floors = new List<Floor>();
            Messages = new List<Message>();
        }

        public int Id { get; set; }
        public IList<User> ResponsibleUsers { get; set; }
        public IList<Floor> Floors { get; set; }
        public IList<Message> Messages { get; set; }
        public IList<Shelter> Shelters { get; set; }
        public string Address { get; set; }
        public string Info { get; set; }
        public string BuildingCenterPosition { get; set; }
        public double SafetyDistance { get; set; }
        public string Region { get; set; }
    }
}