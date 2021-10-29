using System.Threading.Tasks;
using FireSaverApi.Dtos.EvacuationPlanDtos;
using Microsoft.AspNetCore.Http;

namespace FireSaverApi.Contracts
{
    public interface IEvacuationService
    {
         Task<EvacuationPlanDto> addEvacuationPlanToCompartment(int compartmentId, IFormFile planImage);
         Task<EvacuationPlanDto> changeEvacuationPlanToCompartment(int compartmentId, IFormFile planImage);
         Task removeEvacuationPlanToCompartment(int compartmentId);
         Task<EvacuationPlanDto> getEvacuationPlanToCompartment(int compartmentId);
    }
}