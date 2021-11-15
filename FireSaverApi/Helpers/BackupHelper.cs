using FireSaverApi.DataContext;
using FireSaverApi.Models;
using Microsoft.EntityFrameworkCore; 

namespace FireSaverApi.Helpers
{
    public static class BackupHelper
    {
        public static void BackupDB(DatabaseContext dbContext, BackupModel backupModel)
        {
            var query = @$"BACKUP DATABASE {backupModel.DbName} TO DISK = '{backupModel.BackupDir}\{backupModel.DbName}.bak'";
            dbContext.Database.ExecuteSqlRaw(query);
        }

        public static void RestoreDB(DatabaseContext dbContext, BackupModel backupModel)
        {
            var query = @$"RESTORE DATABASE {backupModel.DbName} FROM DISK = '{backupModel.BackupDir}\{backupModel.DbName}.bak'";
            dbContext.Database.ExecuteSqlRaw(query);
        }
    }
}