using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
{
    public class BuildingInfoDto
    {
        public int Id { get; set; }
        public List<CompartmentDto> Floors { get; set; }
        public string Address { get; set; }
        public string Info { get; set; }
    }
}
