using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Models;

namespace FireSaverApi.Contracts
{
    public interface ILocationService
    {
        Task<LocationPointModel> CalculateLocationModel();
        Task<PositionDto> WorldToImgPostion(PositionDto worldPostion);
        Task<PositionDto> ImgToWorldPostion(PositionDto worldPostion);
    }
}