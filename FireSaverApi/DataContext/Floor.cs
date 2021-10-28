using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class Floor : Compartment
    {
        public Floor() : base()
        {
            Rooms = new List<Room>();
        }

        public int Level { get; set; }
        public IList<Room> Rooms { get; set; }
        public Building BuildingWithThisFloor { get; set; }
        public string Address { get; set; }
        public string Info { get; set; }

    }
}