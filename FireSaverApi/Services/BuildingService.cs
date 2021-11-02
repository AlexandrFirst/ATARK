using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.BuildingDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.ExceptionHandler.CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class BuildingService : IBuildingService, IBuildingHelper
    {
        private readonly DatabaseContext context;
        private readonly IMapper mapper;
        private readonly IUserHelper userHelper;
        private readonly IUserContextService userContextService;

        public BuildingService(DatabaseContext context,
                                IMapper mapper,
                                IUserHelper userHelper,
                                IUserContextService userContextService)
        {
            this.userHelper = userHelper;
            this.userContextService = userContextService;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<BuildingInfoDto> AddNewBuilding(NewBuildingDto newBuilding)
        {
            var building = mapper.Map<Building>(newBuilding);
            await context.Buildings.AddAsync(building);
            await context.SaveChangesAsync();

            return mapper.Map<BuildingInfoDto>(building);
        }

        public async Task<BuildingInfoDto> AddResponsibleUser(int userId, int buildingId)
        {
            var user = await userHelper.GetUserById(userId);
            if (user.ResponsibleForBuilding == null)
            {
                var building = await GetBuildingById(buildingId);
                building.ResponsibleUsers.Add(user);
                await context.SaveChangesAsync();

                return mapper.Map<BuildingInfoDto>(building);
            }
            else
            {
                throw new UserIsAlredyResponsibleForBuilding();
            }
        }

        public async Task DeleteBuilding(int buildingId)
        {
            var building = await GetBuildingById(buildingId);
            foreach (var floor in building.Floors)
            {
                foreach (var room in floor.Rooms)
                {
                    if (room.InboundUsers.Count > 0)
                    {
                        throw new DeleteBuildingException();
                    }
                }
            }

            context.Buildings.Remove(building);
            await context.SaveChangesAsync();
        }





        public async Task<BuildingInfoDto> RemoveResponsibleUser(int userId)
        {
            var user = await userHelper.GetUserById(userId);
            user.ResponsibleForBuilding = null;

            var building = await GetBuildingById(user.ResponsibleForBuilding.Id);

            await context.SaveChangesAsync();
            return mapper.Map<BuildingInfoDto>(building);
        }

        public async Task<BuildingInfoDto> UpdateBuildingCenter(int buildingId, BuildingCenterDto buildingCenter)
        {
            var updatedBuilding = await GetBuildingById(buildingId);
            updatedBuilding = mapper.Map<Building>(buildingCenter);
            context.Buildings.Update(updatedBuilding);
            await context.SaveChangesAsync();

            return mapper.Map<BuildingInfoDto>(updatedBuilding);
        }

        public async Task<BuildingInfoDto> UpdateBuildingInfo(int buildingId, NewBuildingDto newBuildingInfo)
        {

            var updatedBuilding = await GetBuildingById(buildingId);


            if (CanUserUpdateBuildingInfo(updatedBuilding))
            {
                updatedBuilding = mapper.Map<Building>(newBuildingInfo);
                context.Buildings.Update(updatedBuilding);
                await context.SaveChangesAsync();

                return mapper.Map<BuildingInfoDto>(updatedBuilding);
            }
            else
            {
                throw new System.Exception("You have no rights to modify this building");
            }

        }

        private bool CanUserUpdateBuildingInfo(Building building)
        {
            HttpUserContext userContext = userContextService.GetUserContext();
            return building.ResponsibleUsers.Any(u => u.Id == userContext.Id) ||
                    userContext.RolesList.Contains(UserRole.ADMIN);
        }

        public async Task<Building> GetBuildingById(int buildingId)
        {
            var building = await context.Buildings.Include(b => b.ResponsibleUsers)
                                                   .Include(f => f.Floors)
                                                   .ThenInclude(r => r.Rooms)
                                                   .ThenInclude(u => u.InboundUsers)
                                                   .FirstOrDefaultAsync(b => b.Id == buildingId);

            if (building == null)
                throw new System.Exception("Building is not found by id");

            return building;
        }

    }
}