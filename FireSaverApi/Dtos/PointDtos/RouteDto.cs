using System.Collections.Generic;
using FireSaverApi.DataContext;

namespace FireSaverApi.Dtos
{
    public class RouteDto
    {
        public double DangerFactor { get; set; }
        public List<PositionDto> RoutePoints { get; set; }
    }
}