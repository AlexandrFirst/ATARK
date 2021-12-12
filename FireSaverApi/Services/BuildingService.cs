using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.BuildingDtos;
using FireSaverApi.Dtos.CompartmentDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.ExceptionHandler.CustomExceptions;
using FireSaverApi.Helpers.Pagination;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class BuildingService : IBuildingService, IBuildingHelper
    {
        private readonly DatabaseContext context;
        private readonly IMapper mapper;
        private readonly IUserHelper userHelper;
        private readonly IUserContextService userContextService;
        private readonly ICompartmentHelper compartmentHelper;

        public BuildingService(DatabaseContext context,
                                IMapper mapper,
                                IUserHelper userHelper,
                                IUserContextService userContextService,
                                ICompartmentHelper compartmentHelper)
        {
            this.userHelper = userHelper;
            this.userContextService = userContextService;
            this.compartmentHelper = compartmentHelper;
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

        public async Task<BuildingInfoDto> RemoveResponsibleUser(int userId, int buildingId)
        {

            var user = await userHelper.GetUserById(userId);

            if (user.ResponsibleForBuilding.Id != buildingId)
            {
                throw new System.Exception("Ypu can delete only resposible users from your building");
            }

            user.ResponsibleForBuilding = null;

            await context.SaveChangesAsync();

            var building = await GetBuildingById(buildingId);
            return mapper.Map<BuildingInfoDto>(building);
        }

        public async Task<BuildingInfoDto> UpdateBuildingCenter(int buildingId, BuildingCenterDto buildingCenter)
        {
            var updatedBuilding = await GetBuildingById(buildingId);

            mapper.Map(buildingCenter, updatedBuilding);

            context.Buildings.Update(updatedBuilding);
            await context.SaveChangesAsync();

            return mapper.Map<BuildingInfoDto>(updatedBuilding);
        }

        public async Task<BuildingInfoDto> UpdateBuildingInfo(int buildingId, NewBuildingDto newBuildingInfo)
        {

            var updatedBuilding = await GetBuildingById(buildingId);

            mapper.Map(newBuildingInfo, updatedBuilding);

            context.Buildings.Update(updatedBuilding);
            await context.SaveChangesAsync();

            return mapper.Map<BuildingInfoDto>(updatedBuilding);

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

        public async Task<PagedList<BuildingInfoDto>> GetAllBuildings(BuildingFilterParams buildingFilter)
        {
            var allBuildings = await context.Buildings.ToListAsync();
            if (buildingFilter.BuildingId > 0)
            {
                allBuildings = allBuildings.Where(b => b.Id == buildingFilter.BuildingId).ToList();
            }

            if (!string.IsNullOrEmpty(buildingFilter.Address))
            {
                allBuildings = allBuildings.Where(b => b.Address.ToLower().Contains(buildingFilter.Address)).ToList();
            }

            var allBuildingsDto = mapper.Map<List<BuildingInfoDto>>(allBuildings);


            return PagedList<BuildingInfoDto>.CreateAsync(allBuildingsDto, buildingFilter.PageNumber, buildingFilter.PageSize);
        }

        public async Task ReleaseAllBlockedPoints(int buildingId)
        {
            var buildingCompartments = await GetAllBuildingCompartment(buildingId);

            var compartmentsId = buildingCompartments.Select(c => c.Id);

            var compartmentRoutePoints = await context.RoutePoints.Include(c => c.Compartment)
                .Where(p => compartmentsId.Contains(p.Compartment.Id)).ToListAsync();

            foreach (var point in compartmentRoutePoints)
            {
                point.IsBlocked = false;
            }
            context.RoutePoints.UpdateRange(compartmentRoutePoints);
            await context.SaveChangesAsync();
        }

        private async Task<List<Compartment>> GetAllBuildingCompartment(int buildingId)
        {
            var buidling = await GetBuildingById(buildingId);
            List<Compartment> allCompartments = new List<Compartment>();

            foreach (var floor in buidling.Floors)
            {
                allCompartments.Add(floor);
                foreach (var room in floor.Rooms)
                {
                    allCompartments.Add(room);
                }
            }
            return allCompartments;
        }

        public async Task<Building> GetBuildingByCompartment(int compartmentId)
        {
            var compartment = await context.Compartment.FirstOrDefaultAsync(c => c.Id == compartmentId);
            if (compartment == null)
                throw new System.Exception("Can't determine building");

            var compFloor = compartment as Floor;
            var compRoom = compartment as Room;

            if (compFloor != null)
            {
                var f_compartment = await context.Floors.Include(b => b.BuildingWithThisFloor).ThenInclude(u => u.ResponsibleUsers)
                    .FirstOrDefaultAsync(c => c.Id == compartmentId);

                return f_compartment.BuildingWithThisFloor;
            }
            else
            {
                var r_compartment = await context.Rooms.Include(f => f.RoomFloor).ThenInclude(b => b.BuildingWithThisFloor).ThenInclude(u => u.ResponsibleUsers)
                    .FirstOrDefaultAsync(r => r.Id == compartmentId);
                return r_compartment.RoomFloor.BuildingWithThisFloor;
            }




        }

        public async Task<CompartmentCommonInfo> GetCompartmentById(int compartmentId)
        {
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            return mapper.Map<CompartmentCommonInfo>(compartment);
        }
    }

}