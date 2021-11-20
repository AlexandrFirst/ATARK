using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.BuildingDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.Pagination;
using FireSaverApi.Models;
using FireSaverApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = new string[] { UserRoleName.ADMIN })]
    public class AdminController : ControllerBase
    {
        private readonly BackupModel backupModel;
        private readonly DatabaseContext databaseContext;
        private readonly IBuildingService buildingService;

        public AdminController(DatabaseContext databaseContext,
                                IOptions<BackupModel> backupModel,
                                IBuildingService buildingService)
        {
            this.databaseContext = databaseContext;
            this.buildingService = buildingService;
            this.backupModel = backupModel.Value;
        }

        [HttpGet("backup")]
        public async Task<IActionResult> Backup()
        {
            BackupHelper.BackupDB(databaseContext, backupModel);

            return Ok(new ServerResponse() { Message = "Database is backed up" });
        }

        [HttpGet("restore")]
        public async Task<IActionResult> Restore()
        {
            BackupHelper.RestoreDB(databaseContext, backupModel);

            return Ok(new ServerResponse() { Message = "Database is restored" });
        }

        [HttpGet("allBuildingsInfo")]
        public async Task<IActionResult> GetAllBuildingInfo([FromQuery] BuildingFilterParams buildingParams)
        {
            var pagedListResult = await buildingService.GetAllBuildings(buildingParams);

            Response.AddPagination(pagedListResult.CurrentPage,
                                   pagedListResult.PageSize,
                                   pagedListResult.TotalCount,
                                   pagedListResult.TotalPages);

            var response = (List<BuildingInfoDto>)pagedListResult;
            return Ok(response);
        }
    }
}