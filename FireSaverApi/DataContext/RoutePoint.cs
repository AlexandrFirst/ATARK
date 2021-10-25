using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public enum RoutePointType
    {
        EXIT,
        ADDITIONAL_EXIT,
        BUILDING_EXIT,
        UPSTAIRS_LEFT,
        UPSTAIRS_RIGHT,
        DOWNSTAIRS_LEFT,
        DOWNSTAIRS_RIGHT,
        GATHERING_POINT,
        MAIN_PATH,
        ADDITIONAL_PATH,
        POMPIER,
        EXTINGUISHER,
        FIRE_ALARM,
        EMERGENCY_CALL,
        HYDRANT
    }

    public class RoutePoint : Point
    {
        public RoutePoint()
        {
            ChildrenPoints = new List<RoutePoint>();
        }

        public List<RoutePoint> ChildrenPoints { get; set; }
        public RoutePoint ParentPoint { get; set; }
        public RoutePointType RoutePointType { get; set; }
        public Compartment Compartment { get; set; }
    }
}