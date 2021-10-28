namespace FireSaverApi.Dtos.ScaleModel
{
    public class ScaleModelDto
    {
        public ScaleModelDto(double CoordToPixelCoef,
                            double PixelToXoordCoef,
                            double MinDistanceDifferenceLatitudeCoef,
                            double MinDistanceDifferenceLongtitudeCoef)
        {
            this.CoordToPixelCoef = CoordToPixelCoef;
            this.PixelToXoordCoef = PixelToXoordCoef;
            this.MinDistanceDifferenceLatitudeCoef = MinDistanceDifferenceLatitudeCoef;
            this.MinDistanceDifferenceLongtitudeCoef = MinDistanceDifferenceLongtitudeCoef; 
        }
        public double CoordToPixelCoef { get; set; }
        public double PixelToXoordCoef { get; }

        public double MinDistanceDifferenceLatitudeCoef { get; }

        public double MinDistanceDifferenceLongtitudeCoef { get; }
    }
}