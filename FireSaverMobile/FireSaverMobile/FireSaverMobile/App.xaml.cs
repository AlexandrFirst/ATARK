using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using FireSaverMobile.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            TinyIOC.InitContainer();


            MainPage = new LoginPage();
            NavigationDispetcher.Instance.Initialize(MainPage.Navigation);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
