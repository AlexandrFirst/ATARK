using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;

namespace FireSaverApi.Profiles
{
    public class PointProfile : Profile
    {
        public PointProfile()
        {
            CreateMap<ScalePoint, ScalePointDto>().ReverseMap();

            CreateMap<RoutePoint, RoutePointDto>();
            CreateMap<RoutePointDto, RoutePoint>();
            
            
            CreateMap<PositionDto, string>().ConstructUsing(pDto => new string(pDto.Longtitude + ";" + pDto.Latitude));
            CreateMap<string, PositionDto>().ConstructUsing(pStr => new PositionDto(pStr));
        }
    }
}