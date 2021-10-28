using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.ScaleModel;

namespace FireSaverApi.Profiles
{
    public class ScaleModelProfile:Profile
    {
        public ScaleModelProfile()
        {
            CreateMap<ScaleModel, ScaleModelDto>()
            .ConstructUsing(src => new ScaleModelDto(src.CoordToPixelCoef,
                                                     src.PixelToXoordCoef, 
                                                     src.MinDistanceDifferenceLatitudeCoef, 
                                                     src.MinDistanceDifferenceLongtitudeCoef));
                                                     
            CreateMap<ScaleModelDto, ScaleModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}