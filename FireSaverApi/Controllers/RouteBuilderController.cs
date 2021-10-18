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

        [HttpPost("newpoint")]
        public async Task<IActionResult> SetNewPoint([FromBody] NewRoutePointDto routePoint)
        {
            if (routePoint.ParentRoutePointId is null)
                throw new System.Exception("Can't set new point without parent point");

            var parentPoint = await context.RoutePoints.FirstOrDefaultAsync(p => p.Id == routePoint.ParentRoutePointId);
            if (parentPoint is null)
                throw new System.Exception("Can't find parent point");

            var newPoint = new RoutePoint()
            {
                PointPostion = mapper.Map<Position>(routePoint.PointPostion)
            };
            await context.RoutePoints.AddAsync(newPoint);

            newPoint.ParentPoint = parentPoint;
            await context.SaveChangesAsync();

            return Ok(mapper.Map<RoutePointDto>(newPoint));
        }

        [HttpPost("newroute")]
        public async Task<IActionResult> SetNewRoute([FromBody] NewRoutePointDto routePoint)
        {
            var pointForCurrentMap = await context.RoutePoints.ToListAsync();
            if (pointForCurrentMap.Count != 0)
                throw new System.Exception("One map can contain one route only");

            var newPoint = new RoutePoint()
            {
                PointPostion = mapper.Map<Position>(routePoint.PointPostion)
            };
            await context.RoutePoints.AddAsync(newPoint);
            await context.SaveChangesAsync();

            return Ok(mapper.Map<RoutePointDto>(newPoint));
        }

        [HttpDelete("routepoint/{routePointId}")]
        public async Task<IActionResult> DeletePoints(int routePointId)
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

            return Ok(deletePointOutputDto);
        }

        [HttpGet("routepoint/{routePointId}")]
        public async Task<IActionResult> GetRoutePoint(int routePointId)
        {
            var retrievingPoint = await context.RoutePoints.Include(c => c.ChildrenPoints)
                                                                    .Include(p => p.ParentPoint)
                                                                    .Include(p => p.PointPostion)
                                                                    .FirstOrDefaultAsync(i => i.Id == routePointId);
            if (retrievingPoint == null)
                throw new System.Exception("Can't find deleting point");

            retrievingPoint.ChildrenPoints = await GetAllChildrenPoints(retrievingPoint.ChildrenPoints);
            var pointToReturn = mapper.Map<RoutePointDto>(retrievingPoint);
            return Ok(pointToReturn);
        }

        [HttpDelete("route")]
        public async Task<IActionResult> DeleteRoute()
        {
            var listPoint = await context.RoutePoints.ToListAsync();
            if (listPoint.Count == 0)
                throw new System.Exception("there is no point to delete");

            var initPoint = await context.RoutePoints.Include(p => p.ParentPoint).Include(p => p.PointPostion).Include(c => c.ChildrenPoints).FirstOrDefaultAsync(p => p.ParentPoint == null);
            if (initPoint is null)
                throw new System.Exception("can't find init point");

            initPoint.ChildrenPoints = await GetAllChildrenPoints(initPoint.ChildrenPoints, true);
            context.RoutePoints.Remove(initPoint);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("route")]
        public async Task<IActionResult> GetRoute()
        {
            var initPoint = await context.RoutePoints.Include(p => p.ParentPoint).Include(c => c.ChildrenPoints).FirstOrDefaultAsync(p => p.ParentPoint == null);
            if (initPoint == null)
                throw new System.Exception("Something went wrong. Try again");

            initPoint.ChildrenPoints = await GetAllChildrenPoints(initPoint.ChildrenPoints);

            return Ok(mapper.Map<RoutePointDto>(initPoint));
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
                var children = await context.RoutePoints.Include(c => c.ChildrenPoints).Include(p => p.ParentPoint).FirstOrDefaultAsync(p => p.Id == point.Id);
                point.ChildrenPoints = await GetAllChildrenPoints(children.ChildrenPoints, isDeleteMode);

                System.Console.WriteLine(point.ChildrenPoints.Count);
                if (isDeleteMode)
                    context.RoutePoints.Remove(point);
            }

            return parentChildernPoints;
        }

    }
}