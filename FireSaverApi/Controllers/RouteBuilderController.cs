using System.Collections.Generic;
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
            if (deletingPoint == null)
                throw new System.Exception("Can't find deleting point");

            deletingPoint.ChildrenPoints = await GetAllChildrenPoints(deletingPoint.ChildrenPoints);

            context.RoutePoints.Remove(deletingPoint);
            await context.SaveChangesAsync();

            return Ok("route points are deleted");
        }

        [HttpDelete("route")]
        public async Task<IActionResult> DeleteRoute(int routePointId)
        {
            var initPoint = await context.RoutePoints.Include(p => p.ParentPoint).Include(c => c.ChildrenPoints).FirstOrDefaultAsync(p => p.ParentPoint == null);
            if (initPoint == null)
                throw new System.Exception("Something went wrong. Try again");

            initPoint.ChildrenPoints = await GetAllChildrenPoints(initPoint.ChildrenPoints);
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

        private async Task<List<RoutePoint>> GetAllChildrenPoints(List<RoutePoint> parentChildernPoints)
        {
            if (parentChildernPoints.Count == 0)
            {
                return new List<RoutePoint>();
            }

            List<RoutePoint> allChildren = new List<RoutePoint>();
            foreach (var point in parentChildernPoints)
            {
                var children = await context.RoutePoints.Include(c => c.ChildrenPoints).FirstOrDefaultAsync(p => p.Id == point.Id);
                point.ChildrenPoints.AddRange(await GetAllChildrenPoints(children.ChildrenPoints));
            }

            return parentChildernPoints;
        }

    }
}