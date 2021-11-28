using FireSaverApi.DataContext;
using FireSaverApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Helpers
{
    public static class BackupHelper
    {
        public static void BackupDB(System.Data.Entity.DbContext dbContext, BackupModel backupModel)
        {
            // var query = @$"BACKUP DATABASE {backupModel.DbName} TO DISK = '{backupModel.BackupDir}\{backupModel.DbName}.bak'";
            // dbContext.Database.ExecuteSqlRaw(query);

            string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
            dbContext.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, backupModel.DbName, $"{backupModel.BackupDir}\\{backupModel.DbName}.bak"));
        }

        public static void RestoreDB(DatabaseContext dbContext, BackupModel backupModel)
        {
            var query = @$"RESTORE DATABASE {backupModel.DbName} FROM DISK = '{backupModel.BackupDir}\{backupModel.DbName}.bak'";
            dbContext.Database.ExecuteSqlRaw(query);
        }
    }
}