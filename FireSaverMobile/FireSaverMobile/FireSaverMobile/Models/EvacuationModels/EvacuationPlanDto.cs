using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
{
    public class EvacuationPlanDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public ScaleModelDto ScaleModel { get; set; }
    }
}
