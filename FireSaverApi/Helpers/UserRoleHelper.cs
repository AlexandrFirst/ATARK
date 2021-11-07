using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Helpers
{
    public class UserRoleHelper : IUserRoleHelper
    {
        private readonly DatabaseContext databaseContext;

        public UserRoleHelper(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<UserRole> GetRoleByName(string roleName)
        {
            var role = await databaseContext.UserRoles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                throw new System.Exception("Role is not found");
            }
            return role;
        }

        public async Task<List<UserRole>> AddUserRoles(params string[] roles)
        {
            List<UserRole> newUserRoles = new List<UserRole>();

            foreach (var role in roles)
            {
                newUserRoles.Add(new UserRole()
                {
                    Name = role
                });
            }

            await databaseContext.UserRoles.AddRangeAsync(newUserRoles);
            await databaseContext.SaveChangesAsync();

            return newUserRoles;
        }


    }
}