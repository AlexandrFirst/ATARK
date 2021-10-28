using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;

namespace FireSaverApi.Profiles
{
    public class CompartmentProfile : Profile
    {
        public CompartmentProfile()
        {
            CreateMap<FloorDto, Floor>().ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id >= 0)).ReverseMap();
            CreateMap<RoomDto, Room>().ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id >= 0)).ReverseMap();
        }
    }
}