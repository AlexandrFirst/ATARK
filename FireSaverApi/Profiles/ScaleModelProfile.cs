using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.ScaleModel;

namespace FireSaverApi.Profiles
{
    public class ScaleModelProfile:Profile
    {
        public ScaleModelProfile()
        {
            CreateMap<ScaleModel, ScaleModelDto>();
                                                     
            CreateMap<ScaleModelDto, ScaleModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}