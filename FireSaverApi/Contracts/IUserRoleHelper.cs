using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.DataContext;

namespace FireSaverApi.Contracts
{
    public interface IUserRoleHelper
    {
         Task<UserRole> GetRoleByName(string roleName);
         Task<List<UserRole>> AddUserRoles(params string[] roles);
    }
}