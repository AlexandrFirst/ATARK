using System;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Helpers
{
    public class EvacuationServiceHelper : IEvacuationServiceHelper
    {
        private readonly DatabaseContext dataContext;
        public EvacuationServiceHelper(DatabaseContext dataContext)
        {
            this.dataContext = dataContext;

        }
        public async Task<EvacuationPlan> GetEvacPlanById(int evacPlanId)
        {
            var evacPlan = await dataContext.EvacuationPlans.Include(s => s.ScaleModel).ThenInclude(p => p.ScalePoints).FirstOrDefaultAsync(e => e.Id == evacPlanId);
            if (evacPlan == null)
            {
                throw new Exception("Evac plan is not found");
            }
            return evacPlan;
        }
    }
}