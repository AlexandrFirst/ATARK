using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Helpers
{
    public class CompartmentHelper : ICompartmentHelper
    {
        private readonly DatabaseContext databaseContext;

        public CompartmentHelper(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<Compartment> GetCompartmentById(int compartmentId)
        {
            Compartment compartment = await databaseContext.Compartment.Include(c => c.EvacuationPlan)
                                                                       .Include(t => t.CompartmentTest)
                                                                       .ThenInclude(q => q.Questions)
                                                                       .FirstOrDefaultAsync(c => c.Id == compartmentId);
            if(compartment == null)
            {
                throw new System.Exception("Compartment is not found");
            }

            return compartment;
        }
    }
}