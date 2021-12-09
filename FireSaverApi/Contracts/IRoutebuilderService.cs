using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Helpers;

namespace FireSaverApi.Contracts
{
    public interface IRoutebuilderService
    {
        Task<RoutePoint> SetNewPoint(NewRoutePointDto routePoint);
        Task<RoutePoint> SetNewRoute(int compartmentId, NewRoutePointDto routePoint);
        Task<DeletePointOutputDto> DeletePoint(int routePointId);
        Task<RoutePoint> GetRoutePoint(int routePointId);
        Task DeleteRoute(int rootRoutePointId);
        Task<RoutePoint> GetAllRoute(int rootRoutePointId);
        Task<RoutePoint> UpdateRoutePointPos(RoutePointDto updatingRoutePoint);
        Task<RoutePoint> GetRouteBetweenPoints(int pointid1, int pointid2);
        Task<RoutePoint> GetRootPointForRoutePoint(int routePointId);
        Task<RoutePoint> GetAllRoutForCompartment(int compartmentId);
        Task BlockRoutePoint(int pointId);
    }
}