using System.Collections.Generic;

namespace FireSaverApi.Dtos.ScaleModel
{
    public class ScaleModelDto
    {
        public double FromPixelXToCoordXCoef { get; set; }
        public double FromCoordXToPixelXCoef { get; set; }

        public double FromPixelYToCoordYCoef { get; set; }
        public double FromCoordYToPixelYCoef { get; set; }
        public List<ScalePointDto> ScalePoints { get; set; }
    }
}