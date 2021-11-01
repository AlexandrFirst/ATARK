using System.Collections.Generic;
using FireSaverApi.DataContext;

namespace FireSaverApi.Dtos
{
    public class RoutePointDto
    {
        public int Id { get; set; }
        public PositionDto PointPostion { get; set; }
        public List<RoutePointDto> ChildrenPoints { get; set; }
        public RoutePointDto ParentPoint { get; set; }
        public RoutePointType RoutePointType { get; set; }
    }
}