using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;

namespace FireSaverApi.Profiles
{
    public class CompartmentProfile : Profile
    {
        public CompartmentProfile()
        {
            CreateMap<FloorDto, Floor>().ReverseMap();
        }
    }
}