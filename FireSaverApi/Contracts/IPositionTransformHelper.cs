using System.Threading.Tasks;
using FireSaverApi.Dtos;

namespace FireSaverApi.Contracts
{
    public interface IPositionTransformHelper
    {
         Task<PositionDto> ConvertImgToWorldPos(PositionDto inputPosition);
         Task<PositionDto> ConvertWorldToImgPos(PositionDto inputPosition);
    }
}