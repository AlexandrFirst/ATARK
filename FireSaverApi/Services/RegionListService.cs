using System.Collections.Generic;
using System.IO;
using System.Xml;
using FireSaverApi.Common;
using static FireSaverApi.Common.RegionXmlClass;

namespace FireSaverApi.Services
{
    public class RegionListService
    {
        private readonly Regions regions;

        public RegionListService(Stream input)
        {
            var s = new System.Xml.Serialization.XmlSerializer(typeof(Regions));
            regions = (Regions)s.Deserialize(XmlReader.Create(input));
        }

        public List<Region> getAllRegions()
        {
            return regions.Region;
        }
    }
}