using System.Threading.Tasks;
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
        private readonly IUserContextService userContextService;
        private readonly IUserHelper userHelper;

        public BuildingController(IBuildingService buildingService,
                                  IUserContextService userContextService,
                                  IUserHelper userHelper)
        {
            this.userContextService = userContextService;
            this.userHelper = userHelper;
            this.buildingService = buildingService;
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

        [HttpGet("adduser/{userId}/{buildingId}")]
        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        public async Task<IActionResult> AddResponsibleUser(int userId, int buildingId)
        {
            var userContext = userContextService.GetUserContext();

            if (await BuildingCRUDHelper.isUserResponsobleForBuildingOrAdmin(userHelper, userContext, buildingId))
            {
                var alteredBuilding = await buildingService.AddResponsibleUser(userId, buildingId);
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


    }
}