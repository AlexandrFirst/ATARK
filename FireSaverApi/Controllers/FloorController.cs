using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class FloorController : ControllerBase
    {
        private readonly ICompartmentService<FloorDto, Floor> floorService;
        private readonly IMapper mapper;
        private readonly IUserContextService userContextService;
        private readonly IUserHelper userHelper;

        public FloorController(ICompartmentService<FloorDto, Floor> floorService,
                               IMapper mapper,
                               IUserContextService userContextService,
                               IUserHelper userHelper)
        {
            this.floorService = floorService;
            this.mapper = mapper;
            this.userContextService = userContextService;
            this.userHelper = userHelper;
        }


        [HttpPost("addFloorToBuilding/{buildingId}")]
        [Authorize(Roles = new string[] { UserRole.ADMIN, UserRole.AUTHORIZED_USER })]
        public async Task<IActionResult> AddFloorToBuilding(int buildingId, [FromBody] FloorDto newFloorDto)
        {
            var userContext = userContextService.GetUserContext();

            if (await BuildingCRUDHelper.isUserResponsobleForBuildingOrAdmin(userHelper, userContext, buildingId))
            {
                var result = await floorService.AddCompartment(buildingId, newFloorDto);
                var resultToDto = mapper.Map<FloorDto>(result);

                return Ok(resultToDto);
            }

            return BadRequest(new ServerResponse() { Message = "Only responsible users can add floor to building" });
        }

        [HttpPut("changeFloorInfo/{floorId}")]
        [Authorize(Roles = new string[] { UserRole.ADMIN, UserRole.AUTHORIZED_USER })]
        public async Task<IActionResult> ChangeFloorInfo(int floorId, [FromBody] FloorDto newFloorDto)
        {
            var result = await floorService.ChangeCompartmentInfo(floorId, newFloorDto);
            var resultToDto = mapper.Map<FloorDto>(result);
            return Ok(resultToDto);
        }

        [HttpDelete("{floorId}")]
        [Authorize(Roles = new string[] { UserRole.ADMIN, UserRole.AUTHORIZED_USER })]
        public async Task<IActionResult> RemoveFloorFromBuilding(int floorId)
        {
            await floorService.DeleteCompartment(floorId);
            return Ok(new ServerResponse()
            {
                Message = "Floor is deleted"
            });
        }

        [HttpGet("{floorId}")]
        public async Task<IActionResult> GetFloorInfo(int floorId)
        {
            var result = await floorService.GetCompartmentInfo(floorId);
            var resultToDto = mapper.Map<FloorDto>(result);
            return Ok(resultToDto);
        }
    }
}