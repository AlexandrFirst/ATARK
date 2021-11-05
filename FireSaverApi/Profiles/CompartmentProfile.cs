using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;

namespace FireSaverApi.Profiles
{
    public class CompartmentProfile : Profile
    {
        public CompartmentProfile()
        {
            CreateMap<FloorDto, Floor>().ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id >= 0))
                                        .ForMember(f => f.Id, memberOptions => memberOptions.Ignore());

            CreateMap<Floor, FloorDto>().ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id >= 0));



            CreateMap<RoomDto, Room>().ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id >= 0))
                                        .ForMember(r => r.Id, memberOptions => memberOptions.Ignore());

            CreateMap<Room, RoomDto>().ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id >= 0));
        }
    }
}