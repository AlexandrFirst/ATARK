using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.DataContext;

namespace FireSaverApi.Contracts
{
    public interface IBuildingHelper
    {
         Task<Building> GetBuildingById(int buildingId);
         Task<Building> GetBuildingByCompartment(int compartment);
    }
}