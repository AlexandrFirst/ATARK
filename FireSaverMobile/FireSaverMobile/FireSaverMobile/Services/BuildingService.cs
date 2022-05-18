using FireSaverMobile.Contracts;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using FireSaverMobile.Models.BuildingModels;
using FireSaverMobile.Models.GoogleApiResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Services
{
    public class BuildingService : BaseHttpService, IBuildingService
    {
        public async Task<CommonBuildingDto> GetBuildingInfoByBuildingId(int buildingId)
        {
            var buildingInfo = await client.GetRequest<CommonBuildingDto>($"http://{serverAddr}/Building/info/{buildingId}");
            return buildingInfo;
        }

        public async Task<CommonBuildingDto> GetBuildingInfoByCompartmentId(int compartmentId)
        {
            var buildingInfo = await client.GetRequest<CommonBuildingDto>($"http://{serverAddr}/Building/infoByCompId/{compartmentId}");
            return buildingInfo;
        }

        public async Task<GoogleApiRouteResponse> GetRouteToShelter(Position origin, Position dest)
        {
            var shelterRoute = await client.GetRequest<GoogleApiRouteResponse>(
                $"https://maps.googleapis.com/maps/api/directions/json?destination={dest.Latitude},{dest.Longtitude}&" +
                $"origin={origin.Latitude},{origin.Longtitude}&" +
                $"key={APIKeyStorage.googleKey}");
            return shelterRoute;
        }
    }
}
