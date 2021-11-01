namespace FireSaverApi.Models
{
    public class LocationPointModel
    {
        public double FromPixelXToCoordXCoef { get; set; }
        public double FromCoordXToPixelXCoef { get; set; }

        public double FromPixelYToCoordYCoef { get; set; }
        public double FromCoordYToPixelYCoef { get; set; }

        public bool isInvalid()
        {
            return FromPixelXToCoordXCoef == 0 || 
                   FromCoordXToPixelXCoef == 0 || 
                   FromPixelYToCoordYCoef == 0 || 
                   FromCoordYToPixelYCoef == 0;
        }

    }
}