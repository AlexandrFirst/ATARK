using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;

namespace FireSaverApi.Helpers
{
    public static class BuildingCRUDHelper
    {
        public static async Task<bool> isUserResponsobleForBuildingOrAdmin(IUserHelper userHelper, MyHttpContext currentUserContext, int buildingId)
        {
            var userContext = currentUserContext;
            var currentUserId = userContext.Id;
            var user = await userHelper.GetUserById(currentUserId);

            if (isUserResponsibleForBuilding(user, buildingId) || userContext.RolesList.Contains(UserRoleName.ADMIN))
            {
                return true;
            }
            return false;
        }

        private static bool isUserResponsibleForBuilding(User user, int buildingId)
        {
            return user.ResponsibleForBuilding != null && user.ResponsibleForBuilding.Id == buildingId;
        }
    }
}