using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using FireSaverMobile.Pages;
using FireSaverMobile.Resx;
using System;
using Xamarin.CommunityToolkit.Helpers;
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

            LocalizationResourceManager.Current.PropertyChanged += (sender, e) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);

            MainPage = new ShelterRoutePage();
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
