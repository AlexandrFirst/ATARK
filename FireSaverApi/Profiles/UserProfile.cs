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
                    .ForMember(user => user.RolesList,
                    memberOptions => memberOptions.MapFrom(userInfo => userInfo.RolesList.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList<string>()));


            CreateMap<UserInfoDto, User>()
                    .ForMember(userInfo => userInfo.RolesList,
                    memberOptions => memberOptions.MapFrom(user => string.Join(',', user.RolesList)));
        
            CreateMap<UserInfoDto, UserContextInfo>();
        }
    }
}