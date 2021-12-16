using FireSaverMobile.Contracts;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models.BuildingModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Services
{
    public class BuildingService : BaseHttpService, IBuildingService
    {
        public async Task<CommonBuildingDto> GetBuildingInfo(int compartmentId)
        {
            var buildingInfo = await client.GetRequest<CommonBuildingDto>($"http://{serverAddr}/Building/infoByCompId/{compartmentId}");
            return buildingInfo;
        }
    }
}
