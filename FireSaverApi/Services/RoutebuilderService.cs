using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class RoutebuilderService : IRoutebuilderService
    {

        private readonly DatabaseContext context;
        private readonly IMapper mapper;
        private readonly ICompartmentHelper compartmentHelper;

        public RoutebuilderService(DatabaseContext context,
                                   IMapper mapper,
                                   ICompartmentHelper compartmentHelper)
        {
            this.compartmentHelper = compartmentHelper;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<RoutePoint> SetNewPoint(NewRoutePointDto routePoint)
        {
            if (routePoint.ParentRoutePointId is null)
                throw new System.Exception("Can't set new point without parent point");

            var parentPoint = await context.RoutePoints.Include(c => c.Compartment)
                                                       .FirstOrDefaultAsync(p => p.Id == routePoint.ParentRoutePointId);
            if (parentPoint is null)
                throw new System.Exception("Can't find parent point");

            var newPoint = new RoutePoint()
            {
                MapPosition = mapper.Map<string>(routePoint.PointPostion),
                RoutePointType = RoutePointType.MAIN_PATH,
                Compartment = parentPoint.Compartment
            };
            await context.RoutePoints.AddAsync(newPoint);

            newPoint.ParentPoint = parentPoint;
            await context.SaveChangesAsync();

            return newPoint;
        }

        public async Task<RoutePoint> SetNewRoute(int compartmentId, NewRoutePointDto routePoint)
        {
            var compartmentForCurrentPoint = await compartmentHelper.GetCompartmentById(compartmentId);

            if (compartmentForCurrentPoint.RoutePoints.Count() > 1)
                throw new System.Exception("Only one rooute available for the plan");

            if (routePoint.ParentRoutePointId != null)
                throw new System.Exception("Root of the path can't have a parent");

            var newPoint = new RoutePoint()
            {
                MapPosition = mapper.Map<string>(routePoint.PointPostion),
                RoutePointType = RoutePointType.MAIN_PATH,
                Compartment = compartmentForCurrentPoint
            };
            await context.RoutePoints.AddAsync(newPoint);
            await context.SaveChangesAsync();

            return newPoint;
        }

        public async Task<DeletePointOutputDto> DeletePoint(int routePointId)
        {
            var deletingPoint = await context.RoutePoints.Include(c => c.ChildrenPoints)
                                                      .Include(p => p.ParentPoint)
                                                      .FirstOrDefaultAsync(i => i.Id == routePointId);

            DeletePointOutputDto deletePointOutputDto = new DeletePointOutputDto();

            if (deletingPoint == null)
                throw new System.Exception("Can't find deleting point");

            if (deletingPoint.ParentPoint != null && deletingPoint.ChildrenPoints.Count < 2)
            {
                var childPoint = deletingPoint.ChildrenPoints.FirstOrDefault();
                if (childPoint != null)
                {
                    childPoint.ParentPoint = deletingPoint.ParentPoint;
                    deletePointOutputDto.Point2Id = childPoint.Id;
                }

                deletePointOutputDto.Point1Id = deletingPoint.ParentPoint.Id;
                context.RoutePoints.Remove(deletingPoint);

            }
            else if (deletingPoint.ParentPoint == null && deletingPoint.ChildrenPoints.Count <= 2)
            {
                var firstChild = deletingPoint.ChildrenPoints.FirstOrDefault();
                var secondChild = deletingPoint.ChildrenPoints.ElementAtOrDefault(1);

                if (firstChild != null)
                {
                    firstChild.ParentPoint = null;
                    if (secondChild != null)
                    {
                        secondChild.ParentPoint = firstChild;
                        deletePointOutputDto.Point2Id = secondChild.Id;
                    }
                    deletePointOutputDto.Point1Id = firstChild.Id;
                    firstChild.ParentPoint = null;
                }
                context.RoutePoints.Remove(deletingPoint);
            }
            else if (deletingPoint.ChildrenPoints.Count > 2)
            {
                throw new System.Exception("You can delete points with two or less connections or delete the whole bunch");
            }

            await context.SaveChangesAsync();


            return deletePointOutputDto;
        }

        public async Task<RoutePoint> GetRootPointForRoutePoint(int routePointId)
        {
            var currentRoute = await context.RoutePoints.Include(r => r.ParentPoint).FirstOrDefaultAsync(r => r.Id == routePointId);

            return await GetParentPointForCurrentpoint(currentRoute);
        }

        async Task<RoutePoint> GetParentPointForCurrentpoint(RoutePoint currentRoutePoint)
        {
            if (currentRoutePoint.ParentPoint == null)
                return currentRoutePoint;

            return await GetParentPointForCurrentpoint(currentRoutePoint.ParentPoint);
        }

        public async Task<RoutePoint> GetRoutePoint(int routePointId)
        {
            var retrievingPoint = await context.RoutePoints.Include(c => c.ChildrenPoints)
                                                                  .FirstOrDefaultAsync(i => i.Id == routePointId);
            if (retrievingPoint == null)
                throw new System.Exception("Can't find deleting point");

            retrievingPoint.ChildrenPoints = await GetAllChildrenPoints(retrievingPoint.ChildrenPoints);
            return retrievingPoint;
        }

        public async Task DeleteRoute(int rootRoutePointId)
        {
            var listPoint = await context.RoutePoints.ToListAsync();
            if (listPoint.Count == 0)
                throw new System.Exception("there is no point to delete");

            var initPoint = await context.RoutePoints.Include(p => p.ParentPoint)
                                                     .Include(p => p.MapPosition)
                                                     .Include(c => c.ChildrenPoints)
                                                     .FirstOrDefaultAsync(p => p.ParentPoint == null &&
                                                                               p.Id == rootRoutePointId);
            if (initPoint is null)
                throw new System.Exception("can't find init point");

            initPoint.ChildrenPoints = await GetAllChildrenPoints(initPoint.ChildrenPoints, true);
            context.RoutePoints.Remove(initPoint);
            await context.SaveChangesAsync();
        }

        public async Task<RoutePoint> GetAllRoute(int rootRoutePointId)
        {
            var initPoint = await context.RoutePoints.Include(p => p.ParentPoint)
                                                   .Include(c => c.ChildrenPoints)
                                                   .FirstOrDefaultAsync(p => p.ParentPoint == null &&
                                                                             p.Id == rootRoutePointId);
            if (initPoint == null)
                throw new System.Exception("Something went wrong. Try again");

            initPoint.ChildrenPoints = await GetAllChildrenPoints(initPoint.ChildrenPoints);

            return initPoint;
        }

        public async Task<RoutePoint> UpdateRoutePointPos(RoutePointDto updatingRoutePoint)
        {
            var pointToUpdate = await context.RoutePoints.FirstOrDefaultAsync(d => d.Id == updatingRoutePoint.Id);

            if (pointToUpdate == null)
            {
                throw new System.Exception("Can't find route point");
            }

            //mapper.Map(pointToUpdate, updatingRoutePoint);

            pointToUpdate.MapPosition = mapper.Map<string>(updatingRoutePoint.MapPosition);
            context.Update(pointToUpdate);

            pointToUpdate.RoutePointType = updatingRoutePoint.RoutePointType;

            context.Update(pointToUpdate);
            await context.SaveChangesAsync();

            return pointToUpdate;
        }

        public async Task<RoutePoint> GetAllRoutForCompartment(int compartmentId)
        {
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);

            if (compartment.RoutePoints.Count > 0)
                return await GetAllRoute(compartment.RoutePoints[0].Id);
            else
                throw new System.Exception("No route for compartment found");
        }

        public async Task<RoutePoint> GetRouteBetweenPoints(int pointid1, int pointid2)
        {
            var point1 = context.RoutePoints.Any(p => p.Id == pointid1);

            var point2 = context.RoutePoints.Any(p => p.Id == pointid2);


            if ((!point1) || (!point2))
                throw new System.Exception("Entered invalid points");

            var route = await BuildRoute(pointid1, pointid2);
            route.ParentPoint = null;

            return route;
        }

        private async Task<List<RoutePoint>> GetAllChildrenPoints(List<RoutePoint> parentChildernPoints, bool isDeleteMode = false)
        {
            if (parentChildernPoints.Count == 0)
            {
                return new List<RoutePoint>();
            }

            List<RoutePoint> allChildren = new List<RoutePoint>();
            foreach (var point in parentChildernPoints)
            {
                var children = await context.RoutePoints.Include(c => c.ChildrenPoints)
                                                        .Include(p => p.ParentPoint)
                                                        .FirstOrDefaultAsync(p => p.Id == point.Id);
                point.ChildrenPoints = await GetAllChildrenPoints(children.ChildrenPoints, isDeleteMode);

                System.Console.WriteLine(point.ChildrenPoints.Count);
                if (isDeleteMode)
                    context.RoutePoints.Remove(point);
            }

            return parentChildernPoints;
        }

        private async Task<RoutePoint> BuildRoute(int fromId, int toId, int fromChildId = -1)
        {
            RoutePoint currentPoint = await context.RoutePoints.Include(p => p.ChildrenPoints)
                                                .Include(p => p.ParentPoint)
                                                .FirstOrDefaultAsync(p => p.Id == fromId);

            if (currentPoint.Id == toId)
            {
                currentPoint.ChildrenPoints = null;
                return currentPoint;
            }
            else if (currentPoint.ChildrenPoints.Count > (fromChildId == -1 ? 0 : 1))//have we come from child -> 1; not -> -1
            {
                //TODO: make map tp hoave more than one route.

                RoutePoint backed_point = null;

                foreach (var point in currentPoint.ChildrenPoints)
                {
                    if (fromChildId > -1 && fromChildId == point.Id)
                        continue;

                    var observedPoint = await BuildRoute(point.Id, toId);
                    if (observedPoint != null)
                    {
                        currentPoint.ChildrenPoints = new List<RoutePoint>(){
                            observedPoint
                        };

                        backed_point = observedPoint;
                        if (backed_point.IsBlocked)
                            continue;

                        return currentPoint;
                    }
                }
                return backed_point;
            }
            else if (currentPoint.ParentPoint != null)
            {
                var parentPoint = await BuildRoute(currentPoint.ParentPoint.Id, toId, currentPoint.Id);
                currentPoint.ChildrenPoints = new List<RoutePoint>(){
                    parentPoint
                };
            }

            return currentPoint;
        }

        public async Task BlockRoutePoint(int pointId)
        {
            var point = await context.RoutePoints.FirstOrDefaultAsync(p => p.Id == pointId);
            if (point != null)
            {
                point.IsBlocked = true;
                context.Update(point);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new System.Exception("Route point is not found");
            }
        }
    }
}