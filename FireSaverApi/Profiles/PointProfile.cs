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
            CreateMap<Point, PointDto>().ReverseMap();
        }
    }
}