﻿using AutoMapper;
using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using FireSaverMobile.Popups.PopupNotification;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FireSaverMobile.ViewModels
{
    public class UserDataViewModel : BaseViewModel
    {
        private UserInfo userInfo = null;
        public UserInfo UserInfo
        {
            get { return userInfo; }
            set
            {
                userInfo = value;
                OnPropertyChanged("UserInfo");
            }
        }

        public ICommand UpdateUserInfo { get; set; }


        private IUserService userService;
        private ILoginService loginService;

        private IMapper userMap;


        public UserDataViewModel()
        {
            userService = TinyIOC.Container.Resolve<IUserService>();
            loginService = TinyIOC.Container.Resolve<ILoginService>();
            userMap = MapperContainer.UserMap;

            Task.Run(async () =>
            {
                var authUserInfo = await loginService.ReadDataFromStorage();

                var userInfoDto = await userService.GetUserInfoById(authUserInfo.UserId);

                try
                {
                    //UserInfo = userInfoDto;
                    //OnPropertyChanged("UserInfo");
                    UserInfo = new UserInfo(userInfoDto.Id, userInfoDto.Mail,
                        userInfoDto.LastSeenBuildingPosition, userInfoDto.Name, userInfoDto.Surname,
                        userInfoDto.Patronymic, userInfoDto.TelephoneNumber, userInfoDto.DOB);

                    //UserInfo = userMap.Map<UserInfo>(userInfoDto);
                    //UserInfo.Name = userInfoDto.Name;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            });

            UpdateUserInfo = new Command(async () =>
            {
                if (userInfo == null)
                {
                    await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Data is loading", MessageType.Warning));
                    return;
                }

                var updatedUser = await userService.UpdateUserInfo(userInfo);
                if (updatedUser != null)
                {
                    UserInfo = new UserInfo(updatedUser.Id, updatedUser.Mail,
                       updatedUser.LastSeenBuildingPosition, updatedUser.Name, updatedUser.Surname,
                       updatedUser.Patronymic, updatedUser.TelephoneNumber, updatedUser.DOB);
                    await PopupNavigation.Instance.PushAsync(new PopupNotificationView("User data is updated", MessageType.Notification));
                }
                else 
                {
                    await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Something went wromg! Try again", MessageType.Error));
                }
            });
        }
    }
}
