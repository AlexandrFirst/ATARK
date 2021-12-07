using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
{
    public class ScalePointDto
    {
        public int Id { get; set; }
        public Position MapPosition { get; set; }
        public Position WorldPosition { get; set; }
    }
}
