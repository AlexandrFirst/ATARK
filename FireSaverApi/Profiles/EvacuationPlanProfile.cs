using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.EvacuationPlanDtos;

namespace FireSaverApi.Profiles
{
    public class EvacuationPlanProfile:Profile
    {
        public EvacuationPlanProfile()
        {
            CreateMap<EvacuationPlan, EvacuationPlanDto>().ReverseMap();
        }
    }
}