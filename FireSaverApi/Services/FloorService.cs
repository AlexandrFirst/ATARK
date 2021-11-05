using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class FloorService : CompartmentService<FloorDto, Floor, Building>
    {
        public FloorService(DatabaseContext databaseContext, IMapper mapper)
                    : base(databaseContext, mapper)
        {
        }

        protected override async Task<Building> GetBaseCompartment(int baseCompartmentId)
        {
            var building = await databaseContext.Buildings.FirstOrDefaultAsync(b => b.Id == baseCompartmentId);
            if (building == null)
            {
                throw new System.Exception("building is not found");
            }

            return building;
        }

        protected override async Task<Floor> GetCompartmentById(int CompartmentId)
        {
            var compartment = await databaseContext.Floors.Include(u => u.InboundUsers).Include(i => i.Iots).FirstOrDefaultAsync(b => b.Id == CompartmentId);
            if (compartment == null)
            {
                throw new System.Exception("floor is not found");
            }
            return compartment;
        }
    }
}