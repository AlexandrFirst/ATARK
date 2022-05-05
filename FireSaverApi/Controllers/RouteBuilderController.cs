using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Helpers;
using FireSaverApi.Models;
using FireSaverApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteBuilderController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserContextService userContextService;
        private readonly IUserHelper userHelper;
        private readonly CompartmentDataStorage compartmentDataStorage;
        private readonly DatabaseContext databaseContext;

        public RouteBuilderController(IMapper mapper,
                                        IUserContextService userContextService,
                                        IUserHelper userHelper,
                                        CompartmentDataStorage compartmentDataStorage,
                                        DatabaseContext databaseContext)
        {
            this.mapper = mapper;
            this.userContextService = userContextService;
            this.userHelper = userHelper;
            this.compartmentDataStorage = compartmentDataStorage;
            this.databaseContext = databaseContext;
        }

        [Authorize]
        [HttpPost("addBlockePoint/{radius}")]
        public async Task<IActionResult> AddBlockedPoint([FromBody] PositionDto coords, int radius)
        {
            var userId = userContextService.GetUserContext().Id;
            var user = await userHelper.GetUserById(userId);

            compartmentDataStorage.AddBlockPointToCompartment(user.CurrentCompartment.Id, new Common.BlockedPoint()
            { X = (int)coords.Latitude, Y = (int)coords.Longtitude, Radius = radius });

            return Ok(new ServerResponse() { Message = "Point is added" });
        }

        [Authorize(Roles = new string[] { UserRoleName.ADMIN })]
        [HttpPost("addExitPoint/{compartmentId}")]
        public async Task<IActionResult> AddExitToCompartment([FromBody] PositionDto coords, int compartmentId)
        {
            var compartment = await databaseContext.Compartment.FirstOrDefaultAsync(c => c.Id == compartmentId);
            if (compartment == null)
            {
                return NotFound(new ServerResponse() { Message = "Compartment is not found" });
            }

            Point point = mapper.Map<Point>(coords);
            ExitPoint exitPoint = point as ExitPoint;

            compartment.ExitPoints.Add(exitPoint);
            await databaseContext.SaveChangesAsync();
            return Ok(exitPoint);
        }

        [Authorize(Roles = new string[] { UserRoleName.ADMIN })]
        [HttpDelete("removeExitPoint/{pointId}")]
        public async Task<IActionResult> RemoveExitPoint(int pointId)
        {
            var point = await databaseContext.ExitPoints.FirstOrDefaultAsync(e => e.Id == pointId);

            if (point == null)
                return NotFound(new ServerResponse() { Message = "Point is not found" });

            databaseContext.Remove(point);
            await databaseContext.SaveChangesAsync();
            return Ok(new ServerResponse() { Message = "Point is deleted" });
        }

    }
}