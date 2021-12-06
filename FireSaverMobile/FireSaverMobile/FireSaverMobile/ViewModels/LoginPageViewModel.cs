using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using FireSaverMobile.Pages;
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
    public class LoginPageViewModel : BaseViewModel
    {
        private ILoginService loginService;

        public ICommand AuthCommand { get; set; }
        public ICommand AuthGuestCommand { get; set; }

        private bool isBusy = false;
        public bool IsBusy { get { return isBusy; } set { SetValue(ref isBusy, value); } }


        private string mailValue;
        public string MailValue
        {
            get { return mailValue; }
            set
            {
                SetValue(ref mailValue, value);
            }
        }

        private string passValue;
        public string PassValue
        {
            get { return passValue; }
            set
            {
                SetValue(ref passValue, value);
            }
        }


        private AuthentificationInput authInput = new AuthentificationInput();
        public AuthentificationInput AuthInput
        {
            get
            {
                return authInput;
            }
            set
            {
                SetValue(ref authInput, value);
            }
        }

        public LoginPageViewModel()
        {
            loginService = TinyIOC.Container.Resolve<ILoginService>();

            AuthCommand = new Command<AuthentificationInput>(async (data) =>
            {
                var authResponse = await loginService.AuthUser(data);
                await processAuthResponse(authResponse);
            });

            AuthGuestCommand = new Command(async () =>
            {
                var authResponse = await loginService.AuthGuest();
                await processAuthResponse(authResponse);
            });

            Task.Run(async () =>
            {

                await loadSavedUserData();

            });
        }

        private async Task loadSavedUserData()
        {
            var userResponseData = await loginService.ReadDataFromStorage();
            if (userResponseData.UserId == -1)
            {
                return;
            }

            IsBusy = true;
            var tokenValidity = await loginService.CheckTokenValidity();
            IsBusy = false;
            if (tokenValidity)
            {
                await processAuthResponse(userResponseData);
            }
            else
            {
                loginService.ClearStorage();
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Please, login again", MessageType.Warning));
            }
        }

        private async Task processAuthResponse(AuthentificationResponse authResponse)
        {
            if (authResponse != null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                  {
                      await NavigationDispetcher.Instance.Navigation.PushModalAsync(new MainNavPage());
                  });

                //TODO: push main page with navigation
                //await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Registration is done", MessageType.Notification));
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Invalid password or mail", MessageType.Error));
            }
        }
    }
}
