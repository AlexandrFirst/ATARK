using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models.PointModels
{
    public enum RoutePointType
    {
        EXIT,
        ADDITIONAL_EXIT,
        BUILDING_EXIT,
        UPSTAIRS_LEFT,
        ADDITIONAL_PATH,
        UPSTAIRS_RIGHT,
        DOWNSTAIRS_LEFT,
        DOWNSTAIRS_RIGHT,
        MAIN_PATH,
        GATHERING_POINT,
        POMPIER,
        EXTINGUISHER,
        FIRE_ALARM,
        EMERGENCY_CALL,
        HYDRANT
    }
    public class RoutePointDto
    {
        public int Id { get; set; }
        public Position MapPosition { get; set; }
        public List<RoutePointDto> ChildrenPoints { get; set; }
        public RoutePointDto ParentPoint { get; set; }
        public RoutePointType RoutePointType { get; set; }
    }
}
