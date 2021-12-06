using AutoMapper;
using FireSaverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Helpers
{
    public class MapperContainer
    {
        private static IMapper userMap;
        public static IMapper UserMap { 
            get 
            {
                if (userMap == null)
                    userMap = CreateUserMapp();
                return userMap;
            } 
        }

        private static IMapper CreateUserMapp()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserInfoDto, UserInfo>().ConstructUsing(dto => new UserInfo(dto.Id,
                    dto.Mail,
                    dto.LastSeenBuildingPosition,
                    dto.Name,
                    dto.Surname,
                    dto.Patronymic,
                    dto.TelephoneNumber,
                    dto.DOB));
            });
            return mapperConfiguration.CreateMapper();
        }
    }
}
