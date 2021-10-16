using System.Collections.Generic;

namespace FireSaverApi.Dtos
{
    public class RoutePointDto
    {
        public int Id { get; set; }
        public PositionDto PointPostion { get; set; }
        public List<RoutePointDto> ChildrenPoints { get; set; }
        public RoutePointDto ParentPoint { get; set; }
    }
}