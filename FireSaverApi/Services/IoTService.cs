using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.IoTDtos;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class IoTService : IIoTService, IIoTHelper
    {
        private readonly DatabaseContext dataContext;
        private readonly IBuildingHelper buildingHelper;
        private readonly IMapper mapper;
        private readonly ICompartmentHelper compartmentHelper;

        public IoTService(DatabaseContext dataContext,
                            IBuildingHelper buildingHelper,
                            ICompartmentHelper compartmentHelper,
                            IMapper mapper)
        {
            this.compartmentHelper = compartmentHelper;
            this.mapper = mapper;
            this.buildingHelper = buildingHelper;
            this.dataContext = dataContext;
        }
        public async Task AddIoTToDB(NewIoTDto newIot)
        {
            var iot = mapper.Map<IoT>(newIot);
            await dataContext.AddAsync(iot);
            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteIoTFromDB(string IoTIdentifier)
        {
            var iotToDelete = await dataContext.IoTs.FirstOrDefaultAsync(i => i.IotIdentifier == IoTIdentifier);
            if (iotToDelete == null)
            {
                throw new System.Exception("Can't delete iot from db");
            }

            dataContext.Remove(iotToDelete);
            await dataContext.SaveChangesAsync();
        }

        public async Task<IoT> GetIoTById(string IotIdentifier)
        {
            var iot = await dataContext.IoTs.Include(p => p.MapPosition).FirstOrDefaultAsync(i => i.IotIdentifier == IotIdentifier);
            if (iot == null)
            {
                throw new System.Exception("iot is not found");
            }
            return iot;
        }

        public async Task RemoveIoTFromCompartment(int compartmentId, string IotIdentifier)
        {
            var iot = await GetIoTById(IotIdentifier);
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            compartment.Iots.Remove(iot);
            dataContext.Update(compartment);
            await dataContext.SaveChangesAsync();
        }

        public async Task AddIoTToCompartment(int compartmentId, string IotIdentifier)
        {
            var iot = await GetIoTById(IotIdentifier);
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            compartment.Iots.Add(iot);
            dataContext.Update(compartment);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateIoTPostion(string IotIdentifier, PositionDto newPos)
        {
            var iot = await GetIoTById(IotIdentifier);
            var pos = mapper.Map<Position>(newPos);
            iot.MapPosition = pos;
            dataContext.Update(iot);
            await dataContext.SaveChangesAsync();
        }
    }
}