using FireSaverMobile.Models.BuildingModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Contracts
{
    public interface IBuildingService
    {
        Task<CommonBuildingDto> GetBuildingInfo(int compartmentId);
    }
}
