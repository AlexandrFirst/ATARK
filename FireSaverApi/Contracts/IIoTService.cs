using System.Threading.Tasks;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.IoTDtos;
using FireSaverApi.Helpers;

namespace FireSaverApi.Contracts
{
    public interface IIoTService
    {
        Task AddIoTToDB(NewIoTDto newIot);
        Task DeleteIoTFromDB(string IoTIdentifier);

        Task AddIoTToCompartment(int compartmentId, string IotIdentifier);
        Task RemoveIoTFromCompartment(int compartmentId, string IotIdentifier);
        
        Task UpdateIoTPostion(string IotIdentifier, PositionDto newPos);
        Task<MyHttpContext> GetIotContext(int iotId);
    }
}