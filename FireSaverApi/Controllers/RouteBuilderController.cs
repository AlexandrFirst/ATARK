using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteBuilderController : ControllerBase
    {
        private readonly DatabaseContext context;
        private readonly IMapper mapper;

        public RouteBuilderController(DatabaseContext context,
                                        IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // [HttpPost("newpoint")]
        // public async Task<IActionResult> SetNewPoint([FromBody] NewRoutePointDto routePoint)
        // {
        //     if (routePoint.ParentRoutePointId is null)
        //         throw new System.Exception("Can't set new point without parent point");

        //     var parentPoint = await context.RoutePoints.FirstOrDefaultAsync(p => p.Id == routePoint.ParentRoutePointId);
        //     if (parentPoint is null)
        //         throw new System.Exception("Can't find parent point");

        //     var newPoint = new RoutePoint()
        //     {
        //         MapPosition = mapper.Map<Position>(routePoint.PointPostion)
        //     };
        //     await context.RoutePoints.AddAsync(newPoint);

        //     newPoint.ParentPoint = parentPoint;
        //     await context.SaveChangesAsync();

        //     return Ok(mapper.Map<RoutePointDto>(newPoint));
        // }

        // [HttpPost("newroute")]
        // public async Task<IActionResult> SetNewRoute([FromBody] NewRoutePointDto routePoint)
        // {
        //     var pointForCurrentMap = await context.RoutePoints.ToListAsync();
        //     if (pointForCurrentMap.Count != 0)
        //         throw new System.Exception("One map can contain one route only");

        //     var newPoint = new RoutePoint()
        //     {
        //         MapPosition = mapper.Map<Position>(routePoint.PointPostion)
        //     };
        //     await context.RoutePoints.AddAsync(newPoint);
        //     await context.SaveChangesAsync();

        //     return Ok(mapper.Map<RoutePointDto>(newPoint));
        // }

        // [HttpDelete("routepoint/{routePointId}")]
        // public async Task<IActionResult> DeletePoints(int routePointId)
        // {
        //     var deletingPoint = await context.RoutePoints.Include(c => c.ChildrenPoints)
        //                                                 .Include(p => p.ParentPoint)
        //                                                 .FirstOrDefaultAsync(i => i.Id == routePointId);

        //     DeletePointOutputDto deletePointOutputDto = new DeletePointOutputDto();

        //     if (deletingPoint == null)
        //         throw new System.Exception("Can't find deleting point");

        //     if (deletingPoint.ParentPoint != null && deletingPoint.ChildrenPoints.Count < 2)
        //     {
        //         var childPoint = deletingPoint.ChildrenPoints.FirstOrDefault();
        //         if (childPoint != null)
        //         {
        //             childPoint.ParentPoint = deletingPoint.ParentPoint;
        //             deletePointOutputDto.Point2Id = childPoint.Id;
        //         }

        //         deletePointOutputDto.Point1Id = deletingPoint.ParentPoint.Id;
        //         context.RoutePoints.Remove(deletingPoint);

        //     }
        //     else if (deletingPoint.ParentPoint == null && deletingPoint.ChildrenPoints.Count <= 2)
        //     {
        //         var firstChild = deletingPoint.ChildrenPoints.FirstOrDefault();
        //         var secondChild = deletingPoint.ChildrenPoints.ElementAtOrDefault(1);

        //         if (firstChild != null)
        //         {
        //             firstChild.ParentPoint = null;
        //             if (secondChild != null)
        //             {
        //                 secondChild.ParentPoint = firstChild;
        //                 deletePointOutputDto.Point2Id = secondChild.Id;
        //             }
        //             deletePointOutputDto.Point1Id = firstChild.Id;
        //             firstChild.ParentPoint = null;
        //         }
        //         context.RoutePoints.Remove(deletingPoint);
        //     }
        //     else if (deletingPoint.ChildrenPoints.Count > 2)
        //     {
        //         throw new System.Exception("You can delete points with two or less connections or delete the whole bunch");
        //     }


        //     await context.SaveChangesAsync();

        //     return Ok(deletePointOutputDto);
        // }

        // [HttpGet("routepoint/{routePointId}")]
        // public async Task<IActionResult> GetRoutePoint(int routePointId)
        // {
        //     var retrievingPoint = await context.RoutePoints.Include(c => c.ChildrenPoints)
        //                                                             .Include(p => p.ParentPoint)
        //                                                             .Include(p => p.MapPosition)
        //                                                             .FirstOrDefaultAsync(i => i.Id == routePointId);
        //     if (retrievingPoint == null)
        //         throw new System.Exception("Can't find deleting point");

        //     retrievingPoint.ChildrenPoints = await GetAllChildrenPoints(retrievingPoint.ChildrenPoints);
        //     var pointToReturn = mapper.Map<RoutePointDto>(retrievingPoint);
        //     return Ok(pointToReturn);
        // }

        // [HttpDelete("route")]
        // public async Task<IActionResult> DeleteRoute()
        // {
        //     var listPoint = await context.RoutePoints.ToListAsync();
        //     if (listPoint.Count == 0)
        //         throw new System.Exception("there is no point to delete");

        //     var initPoint = await context.RoutePoints.Include(p => p.ParentPoint).Include(p => p.MapPosition).Include(c => c.ChildrenPoints).FirstOrDefaultAsync(p => p.ParentPoint == null);
        //     if (initPoint is null)
        //         throw new System.Exception("can't find init point");

        //     initPoint.ChildrenPoints = await GetAllChildrenPoints(initPoint.ChildrenPoints, true);
        //     context.RoutePoints.Remove(initPoint);
        //     await context.SaveChangesAsync();

        //     return Ok();
        // }

        // [HttpGet("route")]
        // public async Task<IActionResult> GetRoute()
        // {
        //     var initPoint = await context.RoutePoints.Include(p => p.ParentPoint)
        //                                             .Include(c => c.ChildrenPoints)
        //                                             .Include(c => c.MapPosition)
        //                                             .FirstOrDefaultAsync(p => p.ParentPoint == null);
        //     if (initPoint == null)
        //         throw new System.Exception("Something went wrong. Try again");

        //     initPoint.ChildrenPoints = await GetAllChildrenPoints(initPoint.ChildrenPoints);

        //     return Ok(mapper.Map<RoutePointDto>(initPoint));
        // }

        // [HttpPost("updateMapPos")]
        // public async Task<IActionResult> UpdateRoutePointPos([FromBody] RoutePointDto updatingRoutePoint)
        // {
        //     var pointToUpdate = await context.RoutePoints.Include(p => p.MapPosition)
        //                                                 .FirstOrDefaultAsync(d => d.Id == updatingRoutePoint.Id);

        //     if (pointToUpdate == null)
        //     {
        //         throw new System.Exception("Can't find route point");
        //     }

        //     pointToUpdate.MapPosition = mapper.Map<Position>(updatingRoutePoint.PointPostion);

        //     await context.SaveChangesAsync();

        //     return Ok(pointToUpdate);
        // }

        // [HttpGet("buildRoute/{pointid1}/{pointid2}")]
        // public async Task<IActionResult> GetRouteBetweenPoints(int pointid1, int pointid2)
        // {
        //     var point1 = context.RoutePoints.Any(p => p.Id == pointid1);

        //     var point2 = context.RoutePoints.Any(p => p.Id == pointid2);


        //     if ((!point1) || (!point2))
        //         throw new System.Exception("Entered invalid points");

        //     var route = await BuildRoute(pointid1, pointid2);
        //     route.ParentPoint = null;
        //     var routeToReturn = mapper.Map<RoutePointDto>(route);
        //     return Ok(routeToReturn);
        // }

        // private async Task<RoutePoint> BuildRoute(int fromId, int toId, int fromChildId = -1)
        // {
        //     RoutePoint currentPoint = await context.RoutePoints.Include(p => p.ChildrenPoints)
        //                                         .Include(p => p.ParentPoint)
        //                                         .Include(p => p.MapPosition)
        //                                         .FirstOrDefaultAsync(p => p.Id == fromId);

        //     if (currentPoint.Id == toId)
        //         return currentPoint;
        //     else if (currentPoint.ChildrenPoints.Count > (fromChildId == -1 ? 0 : 1))//have we come from child -> 1; not -> -1
        //     {
        //         //TODO: make map tp hoave more than one route.
        //         foreach (var point in currentPoint.ChildrenPoints)
        //         {
        //             if (fromChildId > -1 && fromChildId == point.Id)
        //                 continue;
        //             var observedPoint = await BuildRoute(point.Id, toId);
        //             if (observedPoint != null)
        //             {
        //                 currentPoint.ChildrenPoints = new List<RoutePoint>(){
        //                     observedPoint
        //                 };
        //                 return currentPoint;
        //             }
        //         }
        //         return null;
        //     }
        //     else if (currentPoint.ParentPoint != null)
        //     {
        //         var parentPoint = await BuildRoute(currentPoint.ParentPoint.Id, toId, currentPoint.Id);
        //         currentPoint.ChildrenPoints = new List<RoutePoint>(){
        //             parentPoint
        //         };
        //     }

        //     return currentPoint;
        // }

        // private async Task<List<RoutePoint>> GetAllChildrenPoints(List<RoutePoint> parentChildernPoints, bool isDeleteMode = false)
        // {
        //     if (parentChildernPoints.Count == 0)
        //     {
        //         return new List<RoutePoint>();
        //     }

        //     List<RoutePoint> allChildren = new List<RoutePoint>();
        //     foreach (var point in parentChildernPoints)
        //     {
        //         var children = await context.RoutePoints.Include(c => c.ChildrenPoints)
        //                                                 .Include(p => p.ParentPoint)
        //                                                 .Include(c => c.MapPosition)
        //                                                 .FirstOrDefaultAsync(p => p.Id == point.Id);
        //         point.ChildrenPoints = await GetAllChildrenPoints(children.ChildrenPoints, isDeleteMode);

        //         System.Console.WriteLine(point.ChildrenPoints.Count);
        //         if (isDeleteMode)
        //             context.RoutePoints.Remove(point);
        //     }

        //     return parentChildernPoints;
        // }
    }
}