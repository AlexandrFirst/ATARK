using System.Collections.Generic;

namespace FireSaverApi.Common
{
    public class GoogleApiDistanceResponse
    {
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<DistanceElements> rows { get; set; }
        public string status { get; set; }
    }

    public class DistanceElements
    {
        public List<DistanceElementsValues> elements { get; set; }
    }

    public class DistanceElementsValues
    {
        public DistanceModule distance { get; set; }
        public DistanceModule duration { get; set; }
        public string status { get; set; }
    }

    public class DistanceModule
    {
        public string text { get; set; }
        public int value { get; set; }
    }
}