namespace FireSaverApi.Models
{
    public class LocationPointModel
    {
        public double ImageXToRealXProjectCoef { get; set; }
        public double ImageYToRealXProjectCoef { get; set; }
        public double ImageXToRealYProjectCoef { get; set; }
        public double ImageYToRealYProjectCoef { get; set; }

        public bool isInvalid()
        {
            return ImageXToRealXProjectCoef == 0 ||
                    ImageYToRealXProjectCoef == 0 ||
                    ImageXToRealYProjectCoef == 0 ||
                    ImageYToRealYProjectCoef == 0;
        }
    }
}