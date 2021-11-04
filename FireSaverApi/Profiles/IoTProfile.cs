using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.IoTDtos;

namespace FireSaverApi.Profiles
{
    public class IoTProfile:Profile
    {
        public IoTProfile()
        {
            CreateMap<IoTDataInfo, IoT>();
        }
    }
}