using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Models;

namespace FireSaverApi.Services.PointServices
{
    public class ProjectService : ILocationService
    {
        public ProjectService(DatabaseContext dataContext, IMapper mapper)
        {
            
        }
        public Task<LocationPointModel> CalculateLocationModel(int compartmentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PositionDto> ImgToWorldPostion(PositionDto worldPostion, int compartmentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PositionDto> WorldToImgPostion(PositionDto worldPostion, int compartmentId)
        {
            throw new System.NotImplementedException();
        }
    }
}