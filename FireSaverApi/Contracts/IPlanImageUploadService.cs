using System.Threading.Tasks;
using FireSaverApi.Dtos.EvacuationPlanDtos;
using Microsoft.AspNetCore.Http;

namespace FireSaverApi.Contracts
{
    public interface IPlanImageUploadService
    {
         Task<PlanUploadResponse> UploadPlanImage(IFormFile planImage);
         Task DeletePlanImage(string planImagePublicId);
    }
}