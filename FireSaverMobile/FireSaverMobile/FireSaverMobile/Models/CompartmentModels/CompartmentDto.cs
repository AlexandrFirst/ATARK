using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
{
    public class CompartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CompartmentDto> Rooms { get; set; }

        public bool IsFloor
        {
            get
            {
                return Rooms != null && Rooms.Count > 0;
            }
        }
    }
}
