using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using FireSaverMobile.Resx;
using FireSaverMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Pages
{
    public class LanguageKeyValue
    {
        public string Key { get; set; }

        public Func<string> GetModifiedValue { get; set; }
        public string Value
        {
            get
            {
                return GetModifiedValue();
            }
        }
    }


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavFlayoutPage : ContentPage
    {
        public ObservableCollection<FlayoutItemModel> NavElements { get; set; }

        public ObservableCollection<LanguageKeyValue> InterfaceLanguages { get; set; }

        public ICommand ChooseLanguage { get; set; }

        private ILoginService loginService;

        public NavFlayoutPage()
        {
            InitializeComponent();

            LocalizationResourceManager.Current.CurrentCulture = CultureInfo.CurrentCulture;

            loginService = TinyIOC.Container.Resolve<ILoginService>();

            NavElements = new ObservableCollection<FlayoutItemModel>()
            {
                new FlayoutItemModel(){ PageNameLocalized = new LocalizedString(() => AppResources.CurrentRoom), ImageSource = "", DetailPage=new CurrentRoomPage()},
                new FlayoutItemModel(){ PageNameLocalized = new LocalizedString(() => AppResources.Account), ImageSource = "", DetailPage = new AccountPage()}
            };

            InterfaceLanguages = new ObservableCollection<LanguageKeyValue>()
            {
               new LanguageKeyValue() { Key = "en", GetModifiedValue = () => AppResources.English },
               new LanguageKeyValue() { Key = "uk", GetModifiedValue = () => AppResources.Ukrainian }
            };

            this.BindingContext = this;

            Task.Run(async () =>
            {
                var userInfo = await loginService.ReadDataFromStorage();
                Console.WriteLine(userInfo);
                if (userInfo.ResponsibleBuildingId.HasValue && userInfo.ResponsibleBuildingId > -1)
                {
                    NavElements.Add(new FlayoutItemModel()
                    {
                        PageNameLocalized = new LocalizedString(() => AppResources.BuildingData),
                        ImageSource = "",
                        DetailPage = new CompartmentInfosPage(userInfo.ResponsibleBuildingId.Value, Models.CompartmentType.Floor)
                    });
                }
            });
        }

        private async void LogoutBtnClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            await loginService.Logout();
            activityIndicator.IsRunning = false;

            while (NavigationDispetcher.Instance.Navigation.ModalStack.Count > 0)
                await NavigationDispetcher.Instance.Navigation.PopModalAsync();
        }

        private async void languageInterface_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = await Application.Current.MainPage.DisplayActionSheet(
                AppResources.ChangeLanuage,
                null, null,
                InterfaceLanguages.Select(m => m.Value).ToArray());
            if (selectedName == null)
            {
                return;
            }

            var lang = InterfaceLanguages.Where(l => l.Value == selectedName).First() as LanguageKeyValue;

            LocalizationResourceManager.Current.CurrentCulture = new CultureInfo(lang.Key);

            (sender as Button).Text = lang.Value;

        }
    }
}