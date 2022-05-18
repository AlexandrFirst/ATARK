using FireSaverMobile.Models;
using FireSaverMobile.Models.BuildingModels;
using FireSaverMobile.Models.GoogleApiResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Contracts
{
    public interface IBuildingService
    {
        Task<CommonBuildingDto> GetBuildingInfoByCompartmentId(int compartmentId);
        Task<CommonBuildingDto> GetBuildingInfoByBuildingId(int buildingId);
        Task<GoogleApiRouteResponse> GetRouteToShelter(Position origin, Position dest);
    }
}
