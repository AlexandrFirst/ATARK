using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models.BuildingModels
{
    public class ShelterDto
    {
        public int Id { get; set; } = 0;
        public string Address { get; set; }
        public int Capacity { get; set; }
        public string Info { get; set; }
        public Position ShelterPosition { get; set; }
        public int Distance { get; set; }
        public int TotalPeople { get; set; }
    }
}
