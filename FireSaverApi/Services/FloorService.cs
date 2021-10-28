using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class FloorService : CompartmentCRUDService<FloorDto, Floor, Building>
    {
        public FloorService(DatabaseContext databaseContext, IMapper mapper)
                    : base(databaseContext, mapper)
        {
        }

        public override async Task<Building> GetBaseCompartment(int baseCompartmentId)
        {
            var building = await databaseContext.Buildings.FirstOrDefaultAsync(b => b.Id == baseCompartmentId);
            if (building == null)
            {
                throw new System.Exception("building is not found");
            }

            return building;
        }

        public override async Task<Floor> GetCompartmentById(int CompartmentId)
        {
            var compartment = await databaseContext.Floors.FirstOrDefaultAsync(b => b.Id == CompartmentId);
            return compartment;
        }
    }
}