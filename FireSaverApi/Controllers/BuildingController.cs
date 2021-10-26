using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.Dtos.BuildingDtos;
using FireSaverApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            this.buildingService = buildingService;
        }

        [HttpPost("newbuilding")]
        public async Task<IActionResult> AddNewBuilding([FromBody] NewBuildingDto newBuildingDto)
        {
            var response = await buildingService.AddNewBuilding(newBuildingDto);

            return Ok(response);
        }


        public async Task<IActionResult> RemoveBuilding()
        {

            //remove Iots -> compartmet set null
            //remove floors -> according compartmnets deleted
            //for responsible uses builing is set to null
            //if floors can't be deleted(as there are some people on that floors, building can't be deleted)
            return Ok();
        }

        [HttpPost("adduser/{userId}/{buildingId}")]
        public async Task<IActionResult> AddResponsibleUser(int userId, int buildingId)
        {
            var alteredBuilding = await buildingService.AddResponsibleUser(userId, buildingId);
            return Ok(alteredBuilding);
        }

        [HttpPost("removeuser/{userId}")]
        public async Task<IActionResult> RemoveResponsibleUser(int userId)
        {
            var alteredBuilding = await buildingService.RemoveResponsibleUser(userId);
            return Ok(alteredBuilding);
        }

        public async Task<IActionResult> UpdateBuildingInfo(int buildingId, [FromBody] NewBuildingDto newBuildingInfo)
        {
            var alteredBuilding = await buildingService.UpdateBuildingInfo(buildingId, newBuildingInfo);
            return Ok(alteredBuilding);
        }
    }
}