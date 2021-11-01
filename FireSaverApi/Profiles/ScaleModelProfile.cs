using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.ScaleModel;
using FireSaverApi.Models;

namespace FireSaverApi.Profiles
{
    public class ScaleModelProfile:Profile
    {
        public ScaleModelProfile()
        {
            CreateMap<ScaleModel, ScaleModelDto>();
            CreateMap<ScaleModel, LocationPointModel>().ReverseMap();
                                                     
            CreateMap<ScaleModelDto, ScaleModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}