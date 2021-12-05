using System.Threading.Tasks;
using FireSaverApi.DataContext;
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
        
        Task<IotNewPositionDto> UpdateIoTPostion(string IotIdentifier, PositionDto newPos);
        Task<MyHttpContext> GetIotContext(int iotId);
        Task<AuthResponseDto> LoginIot(LoginIoTDto loginIoTDto);

        Task AnalizeIoTDataInfo(string iotId, IoTDataInfo dataInfo);
        Task<int> FindBuildingWithCompartmentId(int compartmentid);
    }
}