using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;

namespace FireSaverApi.Profiles
{
    public class PointProfile : Profile
    {
        public PointProfile()
        {
            CreateMap<Position, PositionDto>().ReverseMap();
            CreateMap<ScalePoint, PointDto>().ReverseMap();
            CreateMap<RoutePoint, RoutePointDto>().ReverseMap();

        }
    }
}