using System;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly AppSettings appSettings;
        private readonly ICompartmentHelper compartmentHelper;

        public IoTService(DatabaseContext dataContext,
                            IBuildingHelper buildingHelper,
                            ICompartmentHelper compartmentHelper,
                            IMapper mapper,
                            IOptions<AppSettings> appSettings)
        {
            this.compartmentHelper = compartmentHelper;
            this.mapper = mapper;
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

        public async Task<AuthResponseDto> LoginIot(LoginIoTDto loginIoTDto)
        {
            var iot = await dataContext.IoTs.FirstOrDefaultAsync(i => i.IotIdentifier == loginIoTDto.Identifier);
            if (iot == null)
            {
                throw new Exception("Can't login iot");
            }

            var token = generateJwtToken(iot);
            return new AuthResponseDto()
            {
                Token = token,
                UserId = iot.Id
            };
        }

        private string generateJwtToken(IoT iot)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim("id", iot.Id.ToString()),
                     new Claim("type", "iot"),
                     new Claim(ClaimTypes.Role, UserRole.AUTHORIZED_USER)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
    }

}