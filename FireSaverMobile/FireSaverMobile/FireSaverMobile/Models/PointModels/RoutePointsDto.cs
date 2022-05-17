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
    public class RoutePointsDto
    {
        public double DangerFactor { get; set; }
        public List<Position> RoutePoints { get; set; }
    }
}
