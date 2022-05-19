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

            for (int i = 0; i < buildingInfo.Shelters.Count; i++)
            {
                buildingInfoDto.Shelters[i].TotalPeople = buildingInfo.Shelters[i].Users.Count;
            }

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

        [Authorize]
        [HttpGet("compartment/{compartmentId}")]
        public async Task<IActionResult> GetCompartmentInfo(int compartmentId)
        {
            var compartment = await buildingService.GetCompartmentById(compartmentId);
            return Ok(compartment);
        }

        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        [HttpPost("shelter/{buildingId}")]
        public async Task<IActionResult> AddShelterToCompartment(int buildingId, ShelterDto shelterDto)
        {
            var shelter = await buildingService.AddShelter(buildingId, shelterDto);
            return Ok(shelter);
        }

        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        [HttpPut("shelter/{shelterId}")]
        public async Task<IActionResult> UpdateShelter(int shelterId, ShelterDto shelterDto)
        {
            var shelter = await buildingService.UpdateShelter(shelterId, shelterDto);
            if (shelter == null)
            {
                return NotFound(new ServerResponse() { Message = "No shelter found" });
            }
            return Ok(shelter);
        }

        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        [HttpDelete("shelter/{shelterId}")]
        public async Task<IActionResult> DeleteShelter(int shelterId)
        {
            bool success = await buildingService.RemoveShelter(shelterId);
            if (!success)
            {
                return NotFound(new ServerResponse() { Message = "Shelter is not found" });
            }
            return Ok(new ServerResponse() { Message = "Shelter is deleted" });
        }

        [Authorize]
        [HttpGet("shelter/{shelterId}")]
        public async Task<IActionResult> GetShelterById(int shelterId)
        {
            var shelter = await buildingService.GetShelterInfo(shelterId);
            if (shelter == null)
            {
                return NotFound(new ServerResponse() { Message = "No shelter is found" });
            }
            return Ok(shelter);
        }

        [Authorize]
        [HttpGet("shelter/enter/{shelterId}")]
        public async Task<IActionResult> EnterShelter(int shelterId)
        {
            var contextUser = userContextService.GetUserContext();
            var dbUser = await userHelper.GetUserById(contextUser.Id);

            await buildingService.EnterShelter(shelterId, dbUser);
            return Ok(new ServerResponse() { Message = "Welcome to the shelter" });
        }

        [Authorize]
        [HttpDelete("shelter/leave")]
        public async Task<IActionResult> LeaveShelter()
        {
            var contextUser = userContextService.GetUserContext();
            await buildingService.LeaveShelter(contextUser.Id);
            return Ok(new ServerResponse() { Message = "Good bye" });
        }
    }
}