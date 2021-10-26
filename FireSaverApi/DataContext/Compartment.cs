using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class Compartment
    {
        public Compartment()
        {
            // Iots = new List<IoT>();
            // RoutePoints = new List<RoutePoint>();
            //InboundUsers = new List<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public EvacuationPlan EvacuationPlan { get; set; }
        public string SafetyRules { get; set; }
        //public Test CompartmentTest { get; set; }
        //public IList<IoT> Iots { get; set; }
       // public IList<RoutePoint> RoutePoints { get; set; }
        //public IList<User> InboundUsers { get; set; }
    }
}