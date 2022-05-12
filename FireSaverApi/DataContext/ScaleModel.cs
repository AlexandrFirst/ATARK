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

        public double ImageXToRealXProjectCoef { get; set; }
        public double ImageYToRealXProjectCoef { get; set; }
        public double ImageXToRealYProjectCoef { get; set; }
        public double ImageYToRealYProjectCoef { get; set; }

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
        public int? ApplyingEvacPlansId { get; set; }
    }
}