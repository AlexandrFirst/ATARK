using AutoMapper;
using FireSaverApi.Common;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;

namespace FireSaverApi.Profiles
{
    public class PointProfile : Profile
    {
        public PointProfile()
        {
            CreateMap<ScalePoint, ScalePointDto>().ReverseMap();
                
            CreateMap<PositionDto, string>().ConstructUsing(pDto => new string(pDto.Longtitude + ";" + pDto.Latitude));
            CreateMap<string, PositionDto>().ConstructUsing(pStr => new PositionDto(pStr));

            CreateMap<Route, RouteDto>();
        }
    }
}