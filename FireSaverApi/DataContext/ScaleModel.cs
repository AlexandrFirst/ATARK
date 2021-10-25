using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class ScaleModel
    {
        public ScaleModel()
        {
            ScalePoints = new List<ScalePoint>();
        }

        public int Id { get; set; }
        public IList<ScalePoint> ScalePoints { get; set; }
        public double CoordToPixelCoef { get; set; }
        public double PixelToXoordCoef
        {
            get
            {
                if (CoordToPixelCoef != 0)
                    return 1 / CoordToPixelCoef;
                return 0;
            }
        }
        public double MinDistanceDifferenceLatitudeCoef
        {
            get
            {
                return 0.172;
            }
        }
        public double MinDistanceDifferenceLongtitudeCoef
        {
            get
            {
                return 0.002;
            }
        }

        public EvacuationPlan ApplyingEvacPlans { get; set; }
    }
}