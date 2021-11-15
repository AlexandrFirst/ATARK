using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Helpers;
using FireSaverApi.Models;
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
        public AdminController(DatabaseContext databaseContext, IOptions<BackupModel> backupModel)
        {
            this.databaseContext = databaseContext;
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
    }
}