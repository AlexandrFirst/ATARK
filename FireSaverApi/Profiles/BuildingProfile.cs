using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.BuildingDtos;

namespace FireSaverApi.Profiles
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile()
        {
            CreateMap<Building, BuildingInfoDto>().ReverseMap();
            CreateMap<NewBuildingDto, Building>().ReverseMap();
            CreateMap<BuildingCenterDto, Building>().ReverseMap();
        }
    }
}