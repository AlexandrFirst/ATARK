using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
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
        private readonly IMapper mapper;
        private readonly IRoutebuilderService routeBuilder;

        public RouteBuilderController( IMapper mapper,
                                        IRoutebuilderService routeBuilder)
        {
            this.mapper = mapper;
            this.routeBuilder = routeBuilder;
        }

        [HttpPost("newpoint")]
        public async Task<IActionResult> SetNewPoint([FromBody] NewRoutePointDto routePoint)
        {
            var newPoint = await routeBuilder.SetNewPoint(routePoint);
            return Ok(mapper.Map<RoutePointDto>(newPoint));
        }

        [HttpPost("{compartmentId}/newroute")]
        public async Task<IActionResult> SetNewRoute(int compartmentId, [FromBody] NewRoutePointDto routePoint)
        {
            var newPoint = await routeBuilder.SetNewRoute(compartmentId, routePoint);
            return Ok(mapper.Map<RoutePointDto>(newPoint));
        }

        [HttpDelete("routepoint/{routePointId}")]
        public async Task<IActionResult> DeletePoint(int routePointId)
        {
            var deletePointOutputDto = await routeBuilder.DeletePoint(routePointId);
            return Ok(deletePointOutputDto);
        }

        [HttpGet("routepoint/{routePointId}")]
        public async Task<IActionResult> GetRoutePoint(int routePointId)
        {
            var retrievingPoint = await routeBuilder.GetRoutePoint(routePointId);
            var pointToReturn = mapper.Map<RoutePointDto>(retrievingPoint);
            return Ok(pointToReturn);
        }

        [HttpDelete("route/{routeId}")]
        public async Task<IActionResult> DeleteRoute(int routeId)
        {
            await routeBuilder.DeleteRoute(routeId);

            return Ok();
        }

        [HttpGet("route/{routePointId}")]
        public async Task<IActionResult> GetRoute(int routePointId)
        {
            var initPoint = await routeBuilder.GetAllRoute(routePointId);
            return Ok(mapper.Map<RoutePointDto>(initPoint));
        }

        [HttpPut("updateMapPos")]
        public async Task<IActionResult> UpdateRoutePointPos([FromBody] RoutePointDto updatingRoutePoint)
        {
            var pointToUpdate = await routeBuilder.UpdateRoutePointPos(updatingRoutePoint);
            return Ok(pointToUpdate);
        }
    }
}