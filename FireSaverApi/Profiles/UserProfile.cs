using System.Linq;
using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Helpers;

namespace FireSaverApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDto, User>();

            CreateMap<User, UserInfoDto>()
                   .ForMember(userInfo => userInfo.RolesList,
                   memberOptions => memberOptions.MapFrom(user => user.RolesList.Select(r => r.Name)));


            CreateMap<UserInfoDto, User>()
                    .ForMember(userInfo => userInfo.Id, memberOptions => memberOptions.Ignore())
                    .ForMember(userInfo => userInfo.Mail, memberOptions => memberOptions.Ignore())
                    .ForMember(userInfo => userInfo.RolesList, memberOptions => memberOptions.Ignore())
                    .ForMember(UserInfoDto => UserInfoDto.LastSeenBuildingPosition, memberOptions => memberOptions.Ignore());

            CreateMap<UserInfoDto, MyHttpContext>();
        }
    }
}