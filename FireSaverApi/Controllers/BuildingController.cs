using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.BuildingDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService buildingService;
        private readonly IBuildingHelper buildingHelper;
        private readonly IUserContextService userContextService;
        private readonly IUserHelper userHelper;
        private readonly IMapper mapper;

        public BuildingController(IBuildingService buildingService,
                                  IBuildingHelper buildingHelper,
                                  IUserContextService userContextService,
                                  IUserHelper userHelper,
                                  IMapper mapper)
        {
            this.userContextService = userContextService;
            this.userHelper = userHelper;
            this.mapper = mapper;
            this.buildingService = buildingService;
            this.buildingHelper = buildingHelper;
        }

        [Authorize(Roles = new string[] { UserRoleName.ADMIN })]
        [HttpPost("newbuilding")]
        public async Task<IActionResult> AddNewBuilding([FromBody] NewBuildingDto newBuildingDto)
        {
            var response = await buildingService.AddNewBuilding(newBuildingDto);

            return Ok(response);
        }

        [HttpDelete("{buildingId}")]
        [Authorize(Roles = new string[] { UserRoleName.ADMIN })]
        public async Task<IActionResult> RemoveBuilding(int buildingId)
        {
            await buildingService.DeleteBuilding(buildingId);

            return Ok(new ServerResponse() { Message = "Building is deleted successfully" });
        }

        [HttpGet("info/{buildingId}")]
        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        public async Task<IActionResult> GetBuildingInfoById(int buildingId)
        {
            var buildingInfo = await buildingHelper.GetBuildingById(buildingId);
            var buildingInfoDto = mapper.Map<BuildingInfoDto>(buildingInfo);

            return Ok(buildingInfoDto);
        }

        [HttpGet("infoByCompId/{compartmentId}")]
        public async Task<IActionResult> GetBuildingInfoByCompartmentId(int compartmentId)
        {
            var buildingInfo = await buildingHelper.GetBuildingByCompartment(compartmentId);
            var buildingInfoDto = mapper.Map<BuildingInfoDto>(buildingInfo);

            return Ok(buildingInfoDto);
        }

        [HttpGet("adduser/{userMail}/{buildingId}")]
        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        public async Task<IActionResult> AddResponsibleUser(string userMail, int buildingId)
        {
            var userContext = userContextService.GetUserContext();

            if (await BuildingCRUDHelper.isUserResponsobleForBuildingOrAdmin(userHelper, userContext, buildingId))
            {
                var user = await userHelper.GetUserByMail(userMail);
                var alteredBuilding = await buildingService.AddResponsibleUser(user.Id, buildingId);
                return Ok(alteredBuilding);
            }

            return BadRequest(new ServerResponse() { Message = "Only responsible users can add new responsible users" });
        }

        [HttpDelete("removeuser/{userId}")]
        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        public async Task<IActionResult> RemoveResponsibleUser(int userId)
        {
            var userContext = userContextService.GetUserContext();
            int currentUserId = userContext.Id;

            var user = await userHelper.GetUserById(userId);
            var requestorUser = await userHelper.GetUserById(currentUserId);

            if (user.ResponsibleForBuilding == null)
            {
                return BadRequest(new ServerResponse() { Message = "This user is not responsible" });
            }

            if (await BuildingCRUDHelper.isUserResponsobleForBuildingOrAdmin(userHelper, userContext, user.ResponsibleForBuilding.Id))
            {
                if (currentUserId == userId)
                {
                    return BadRequest(new ServerResponse() { Message = "You can't remove yourself" });
                }
                var alteredBuilding = await buildingService.RemoveResponsibleUser(userId, user.ResponsibleForBuilding.Id);
                return Ok(alteredBuilding);
            }

            return BadRequest(new ServerResponse() { Message = "Only responsible users can remove responsible users" });

        }

        [HttpPut("updateBuilding/{buildingId}")]
        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        public async Task<IActionResult> UpdateBuildingInfo(int buildingId, [FromBody] NewBuildingDto newBuildingInfo)
        {
            var userContext = userContextService.GetUserContext();

            if (await BuildingCRUDHelper.isUserResponsobleForBuildingOrAdmin(userHelper, userContext, buildingId))
            {
                var alteredBuilding = await buildingService.UpdateBuildingInfo(buildingId, newBuildingInfo);
                return Ok(alteredBuilding);
            }

            return BadRequest(new ServerResponse() { Message = "Only responsible users can update building info" });
        }

        [HttpPut("setBuildingCenter/{buildingId}")]
        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        public async Task<IActionResult> SetBuildingCenter(int buildingId, [FromBody] BuildingCenterDto buildingCentre)
        {
            var userContext = userContextService.GetUserContext();

            if (await BuildingCRUDHelper.isUserResponsobleForBuildingOrAdmin(userHelper, userContext, buildingId))
            {
                BuildingInfoDto alteredBuilding = await buildingService.UpdateBuildingCenter(buildingId, buildingCentre);
                return Ok(alteredBuilding);
            }
            return BadRequest(new ServerResponse() { Message = "Only responsible users can set building center" });
        }

        [HttpGet("compartment/{compartmentId}")]
        [Authorize]
        public async Task<IActionResult> GetCompartmentInfo(int compartmentId)
        {
            var compartment = await buildingService.GetCompartmentById(compartmentId);
            return Ok(compartment);
        }
    }
}