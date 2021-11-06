using System.Linq;
using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.TestDtos;

namespace FireSaverApi.Profiles
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<Question, QuestionInputDto>()
                    .ForMember(q_dto => q_dto.AnswearsList,
                               memberOption => memberOption.MapFrom(q => q.AnswearsList
                                                                          .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                                                                          .ToList<string>()))
                    .ForMember(q_dto => q_dto.PossibleAnswears,
                               memberOption => memberOption.MapFrom(q => q.PossibleAnswears
                                                                          .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                                                                          .ToList<string>()));

            CreateMap<QuestionInputDto, Question>()
                    .ForMember(q => q.AnswearsList,
                               memberOption => memberOption.MapFrom(q_dto => string.Join(',', q_dto.AnswearsList)))
                    .ForMember(q => q.PossibleAnswears,
                               memberOption => memberOption.MapFrom(q_dto => string.Join(',', q_dto.PossibleAnswears)));


            CreateMap<Question, QuestionOutputDto>()
                    .ForMember(q_dto => q_dto.PossibleAnswears,
                               memberOption => memberOption.MapFrom(q => q.PossibleAnswears
                                                                        .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                                                                        .ToList<string>()));


            CreateMap<Test, TestInputDto>().ReverseMap();
            CreateMap<Test, TestOutputDto>().ReverseMap();

        }
    }
}