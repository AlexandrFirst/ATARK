using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.BuildingDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.Pagination;
using FireSaverApi.Models;
using FireSaverApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
            string basePath = Directory.GetCurrentDirectory();


            var query = @$"BACKUP DATABASE {backupModel.DbName} TO DISK = '{basePath}\Backup\{backupModel.DbName}.bak'";
            string connectionString = GetConnectionString(basePath);
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                var cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }

            return Ok(new ServerResponse() { Message = "Database is backed up" });
        }

        [HttpGet("restore")]
        public async Task<IActionResult> Restore()
        {
            LoadDB(@"FireSaverDbFinalTRefactored1", @"FireSaverDbFinalTRefactored1_log", "tempBD");

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

        [HttpGet("checkRights")]
        public async Task<IActionResult> CheckAdminRights()
        {
            return Ok(new ServerResponse() { Message = "Ok" });
        }

        private string GetConnectionString(string basePath)
        {
            var builder = new ConfigurationBuilder()
          .SetBasePath(basePath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("FireSaverDbConnectionString");
            return connectionString;
        }



        private bool LoadDB(
    string orig_mdf,
    string orig_ldf,
    string new_database_name)
        {
            //use RESTORE FILELISTONLY FROM DISK = 'D:\nure\ATARK\FireSaver\FireSaverApi\Backup\FireSaverDbFinalTRefactored1.bak' to see restoring mdf and ldf

            string basePath = Directory.GetCurrentDirectory();
            if (!System.IO.File.Exists(@$"{basePath}\Backup\{backupModel.DbName}.bak"))
            {
                return false;
            }
            string connectionString = GetConnectionString(basePath);

            var database_dir = System.IO.Path.GetTempPath();

            var temp_mdf = $"{database_dir}{new_database_name}.mdf";
            var temp_ldf = $"{database_dir}{new_database_name}.ldf";
            var query = @$"RESTORE DATABASE {new_database_name} FROM DISK = '{basePath}\Backup\{backupModel.DbName}.bak' WITH MOVE '{orig_mdf}' TO '{temp_mdf}', MOVE '{orig_ldf}' TO '{temp_ldf}', REPLACE;";

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
    }
}