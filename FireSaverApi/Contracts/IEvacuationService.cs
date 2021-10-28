using System.Threading.Tasks;
using FireSaverApi.Dtos.EvacuationPlanDtos;

namespace FireSaverApi.Contracts
{
    public interface IEvacuationService
    {
         Task<EvacuationPlanDto> addEvacuationPlanToCompartment(int compartmentId, EvacuationPlanDto plan);
         Task<EvacuationPlanDto> changeEvacuationPlanToCompartment(int EvacuationPlanId, EvacuationPlanDto plan);
         Task<EvacuationPlanDto> removeEvacuationPlanToCompartment(int compartmentId);
         Task<EvacuationPlanDto> getEvacuationPlanToCompartment(int compartmentId);
    }
}