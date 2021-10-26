using System.Threading.Tasks;
using FireSaverApi.Dtos.BuildingDtos;

namespace FireSaverApi.Contracts
{
    public interface IBuildingService
    {
        Task<BuildingInfoDto> AddNewBuilding(NewBuildingDto newBuilding);
        Task<BuildingInfoDto> AddResponsibleUser(int userId, int buildingId);
        Task<BuildingInfoDto> RemoveResponsibleUser(int userId);
        Task<BuildingInfoDto> UpdateBuildingInfo(int buildingId, NewBuildingDto newBuildingInfo);
    }
}