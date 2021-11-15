using System;
using System.Collections.Generic;
using System.Linq;

namespace FireSaverApi.Dtos
{
    public class PositionDto
    {
        public PositionDto(string coords)
        {
            List<double> posCoords = coords.Split(';').Select(s => Convert.ToDouble(s)).ToList();
            Longtitude = posCoords[0];
            Latitude = posCoords[1];
        }

        public PositionDto()
        {
            
        }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
    }
}