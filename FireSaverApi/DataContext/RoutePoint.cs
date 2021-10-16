using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class RoutePoint
    {
        public RoutePoint()
        {
            ChildrenPoints = new List<RoutePoint>();
        }

        public int Id { get; set; }
        public Position PointPostion { get; set; }
        public List<RoutePoint> ChildrenPoints { get; set; }
        public RoutePoint ParentPoint { get; set; }
    }
}