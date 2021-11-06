using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.Dtos.EvacuationPlanDtos;
using Microsoft.AspNetCore.Http;

namespace FireSaverApi.Contracts
{
    public interface IEvacuationService
    {
         Task<EvacuationPlanDto> addEvacuationPlanToCompartment(int compartmentId, IFormFile planImage);
         Task<EvacuationPlanDto> changeEvacuationPlanOfCompartment(int compartmentId, IFormFile planImage);
         Task RemoveEvacuationPlanOfCompartment(int compartmentId);
         Task<EvacuationPlanDto> GetEvacuationPlanOfCompartment(int compartmentId);
         Task<List<EvacuationPlanDto>> GetEvacuationPlansFromCompartmentByUserId(int userId);
         Task<List<EvacuationPlanDto>> GetEvacuationPlansFromCompartmentByCompartmentId(int compartmentId);
    }
}