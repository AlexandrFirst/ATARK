using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
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


            string backupName = string.Format($"{backupModel.DbName}Z{DateTime.Now.Ticks}");

            var query = @$"BACKUP DATABASE {backupModel.DbName} TO DISK = '{basePath}\Backup\{backupName}.bak'";
            string connectionString = GetConnectionString(basePath);
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                var cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }

            return Ok(new ServerResponse() { Message = "Database is backed up" });
        }

        [HttpGet("restore/{backupId}")]
        public async Task<IActionResult> Restore(long backupId)
        {
            string backupBaseName = backupModel.DbName;
            string basePath = Directory.GetCurrentDirectory();
            string backupName = $"{backupBaseName}Z{backupId}";

            if (System.IO.File.Exists(@$"{basePath}\Backup\{backupName}.bak"))
            {
                // string mdf_name = string.Format($"{backupBaseName}Z{backupId}");
                // string ldf_name = string.Format($"{backupBaseName}Z{backupId}_log");
                string mdf_name = "FireSaverDbFinalTRefactored1";
                string ldf_name = "FireSaverDbFinalTRefactored1_log";

                LoadDB(mdf_name, ldf_name, "tempBD");
                return Ok(new ServerResponse() { Message = "Database is restored" });
            }
            else
            {
                return BadRequest(new ServerResponse() { Message = "Backup is not found" });
            }
        }

        [HttpGet("allRestorations")]
        public async Task<IActionResult> GetAllRestorations()
        {
            string basePath = Directory.GetCurrentDirectory();
            List<string> restorationIds = new List<string>();
            string[] allRestorationFileNames = Directory.GetFiles(@$"{basePath}\Backup", "*.bak");
            string regexString = "^(.+)Z(.+)\\.bak$";
            Regex regex = new Regex(regexString);
            foreach (string filename in allRestorationFileNames)
            {
                Match m = regex.Match(filename);
                if (m.Success)
                {
                    Group g = m.Groups[2];
                    CaptureCollection c = g.Captures;
                    var id = c[0].ToString();
                    restorationIds.Add(id);
                }
            }
            return Ok(restorationIds);
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

        [HttpDelete("deleteRestoration/{backupId}")]
        public async Task<IActionResult> DeleteRestoration(string backupId)
        {
            string basePath = Directory.GetCurrentDirectory();
            string backupBaseName = backupModel.DbName;
            string backupName = @$"{basePath}\Backup\{backupBaseName}Z{backupId}.bak";
            if (System.IO.File.Exists(backupName))
            {
                System.IO.File.Delete(backupName);
                return Ok(new ServerResponse() { Message = "Backup is deleted" });
            }

            return BadRequest(new ServerResponse() { Message = "Can't delete the file" });
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

        private bool LoadDB(string orig_mdf, string orig_ldf, string new_database_name)
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