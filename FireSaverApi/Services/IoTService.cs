using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.IoTDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.ExceptionHandler.CustomExceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FireSaverApi.Services
{
    public class IoTService : IIoTService, IIoTHelper
    {
        private readonly DatabaseContext dataContext;
        private readonly IBuildingHelper buildingHelper;
        private readonly IMapper mapper;
        private readonly ISocketService sockerService;
        private readonly AppSettings appSettings;
        private readonly ICompartmentHelper compartmentHelper;

        public IoTService(DatabaseContext dataContext,
                            IBuildingHelper buildingHelper,
                            ICompartmentHelper compartmentHelper,
                            IMapper mapper,
                            IOptions<AppSettings> appSettings,
                            ISocketService sockerService)
        {
            this.compartmentHelper = compartmentHelper;
            this.mapper = mapper;
            this.sockerService = sockerService;
            this.appSettings = appSettings.Value;
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
            var iot = await dataContext.IoTs.Include(p => p.MapPosition).Include(c => c.Compartment).FirstOrDefaultAsync(i => i.IotIdentifier == IotIdentifier);
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
            
            if(!compartment.Iots.Any(i => i.IotIdentifier == IotIdentifier))
                throw new Exception("iot doesn't belong to compartment");
                
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

        public async Task<IoT> UpdateIoTPostion(string IotIdentifier, PositionDto newPos)
        {
            var iot = await GetIoTById(IotIdentifier);
            var pos = mapper.Map<Position>(newPos);
            iot.MapPosition = pos;
            dataContext.Update(iot);
            await dataContext.SaveChangesAsync();
            return iot;
        }

        public async Task<AuthResponseDto> LoginIot(LoginIoTDto loginIoTDto)
        {
            var iot = await dataContext.IoTs.FirstOrDefaultAsync(i => i.IotIdentifier == loginIoTDto.Identifier);
            if (iot == null)
            {
                throw new Exception("Can't login iot");
            }

            var token = TokenGenerator.generateJwtToken(iot.Id, TokenGenerator.IoTJWTType, UserRole.AUTHORIZED_USER, appSettings.Secret);
            return new AuthResponseDto()
            {
                Token = token,
                UserId = iot.Id
            };
        }

        public async Task<MyHttpContext> GetIotContext(int iotId)
        {
            try
            {
                var iot = await dataContext.IoTs.FirstOrDefaultAsync(i => i.Id == iotId);
                if (iot == null)
                    throw new IotNotFoundException();
                return new MyHttpContext()
                {
                    Id = iot.Id
                };
            }
            catch (IotNotFoundException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AnalizeIoTDataInfo(string iotId, IoTDataInfo dataInfo)
        {
            var iot = await GetIoTById(iotId);

            iot = mapper.Map<IoT>(dataInfo);
            dataContext.Update(iot);

            if (dataInfo.LastRecordedAmmoniaLevel > 10)  //FIXME: check values when to add real iot
            {
                var compartmentId = iot.Compartment.Id;
                var buildingId = await FindBuildingWithCompartmentId(compartmentId);
                await sockerService.SetAlarmForBuilding(buildingId);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<int> FindBuildingWithCompartmentId(int compartmentid)
        {
            var building = await dataContext.Buildings.Include(b => b.Floors).FirstOrDefaultAsync(b => b.Floors.Any(f => f.Id == compartmentid));
            if (building != null)
            {
                return building.Id;
            }

            building = await dataContext.Buildings.Include(b => b.Floors).ThenInclude(f => f.Rooms).FirstOrDefaultAsync(b => b.Floors.Any(f => f.Rooms.Any(r => r.Id == compartmentid)));
            if (building != null)
            {
                return building.Id;
            }

            throw new Exception("Error while building seraching");
        }

    }

}