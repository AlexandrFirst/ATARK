using FireSaverMobile.Models;
using FireSaverMobile.Models.PointModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Contracts
{
    public interface IEvacuationService
    {
        Task<EvacuationPlanDto> GetEvacPlan(int compartmentId);
        Task<List<CompartmentDto>> GetFloorsByBuildingId(int buildingId);
        Task<List<CompartmentDto>> GetRoomsByFloorId(int floorId);
        Task<List<EvacuationPlanDto>> GetEvacuationPlansFromCompartment();
        Task<RoutePointsDto> BuildCompartmentEvacRouteForUser();
    }
}
