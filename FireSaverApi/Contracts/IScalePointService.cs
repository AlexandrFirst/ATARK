using System.Threading.Tasks;
using FireSaverApi.Dtos;


namespace FireSaverApi.Contracts
{
    public interface IScalePointService
    {
        Task<ScalePointDto> AddNewScalePoint(int evacuationPlanId, ScalePointDto inputPoint);
        Task DeleteSinglePoint(int scalePointId);
        Task DeleteAllPoints(int evacuationPlanId);
    }
}