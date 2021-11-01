using System.Threading.Tasks;
using FireSaverApi.Dtos;

namespace FireSaverApi.Contracts
{
    public interface IPositionTransformHelper
    {
         Task<PositionDto> ConvertImgToWorldPos(PositionDto inputPosition, int compartmentId);
         Task<PositionDto> ConvertWorldToImgPos(PositionDto inputPosition, int compartmentId);
    }
}