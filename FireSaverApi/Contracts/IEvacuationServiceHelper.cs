using System.Threading.Tasks;
using FireSaverApi.DataContext;

namespace FireSaverApi.Contracts
{
    public interface IEvacuationServiceHelper
    {
         Task<EvacuationPlan> GetEvacPlanById(int evacPlanId);
    }
}