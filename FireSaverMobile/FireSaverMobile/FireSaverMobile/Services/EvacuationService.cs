using FireSaverMobile.Contracts;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Services
{
    public class EvacuationService : BaseHttpService, IEvacuationService
    {
        public async Task<EvacuationPlanDto> GetEvacPlan(int compartmentId)
        {
            var evacPlan = await client.GetRequest<EvacuationPlanDto>($"http://{serverAddr}/EvacuationPlan/{compartmentId}");
            return evacPlan;
        }

        public async Task<List<CompartmentDto>> GetFloorsByBuildingId(int buildingId)
        {
            var buildingInfo = await client.GetRequest<BuildingInfoDto>($"http://{serverAddr}/Building/info/{buildingId}");
            if (buildingInfo == null)
                return new List<CompartmentDto>();

            List<CompartmentDto> floorsInBuilding = buildingInfo.Floors;
            return floorsInBuilding;

        }

        public async Task<List<CompartmentDto>> GetRoomsByFloorId(int floorId)
        {
            var floorInfo = await client.GetRequest<CompartmentDto>($"http://{serverAddr}/Floor/{floorId}");
            if (floorInfo == null)
                return new List<CompartmentDto>();

            List<CompartmentDto> roomsOnFloor = floorInfo.Rooms;
            return roomsOnFloor;
        }
    }
}
