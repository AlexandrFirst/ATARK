using System.Collections.Generic;
using System.Xml.Serialization;

namespace FireSaverApi.Common
{
    public class RegionXmlClass
    {
        [XmlRoot(ElementName = "region")]
        public class Region
        {
            [XmlElement(ElementName = "alert")]
            public string Alert { get; set; }
        }

        [XmlRoot(ElementName = "regions")]
        public class Regions
        {
            [XmlElement(ElementName = "region")]
            public List<Region> Region { get; set; }
        }
    }
}