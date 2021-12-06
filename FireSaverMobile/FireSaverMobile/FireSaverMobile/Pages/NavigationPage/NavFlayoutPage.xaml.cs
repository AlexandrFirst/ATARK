using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavFlayoutPage : ContentPage
    {
        public ObservableCollection<FlayoutItemModel> NavElements { get; set; }


        private Dictionary<string, string> interfaceLanguages = new Dictionary<string, string>()
            { { "English", "English"}, {"Ukrainean", "Ukrainean"} };

        private ILoginService loginService;

        public NavFlayoutPage()
        {
            InitializeComponent();
            loginService = TinyIOC.Container.Resolve<ILoginService>();

            NavElements = new ObservableCollection<FlayoutItemModel>()
            {
                new FlayoutItemModel(){ PageName = "Scan QR", ImageSource = "", DetailPage=new QRScanPage()},
                new FlayoutItemModel(){ PageName = "Account", ImageSource = "", DetailPage = new AccountPage()}
            };



            this.BindingContext = this;

            foreach (string lang in interfaceLanguages.Keys)
            {
                languageInterface.Items.Add(lang);
            }
            languageInterface.SelectedItem = interfaceLanguages["English"];



            Task.Run(async () =>
            {
                var userInfo = await loginService.ReadDataFromStorage();
                if (userInfo.ResponsibleBuildingId.HasValue && userInfo.ResponsibleBuildingId > -1)
                {
                    NavElements.Add(new FlayoutItemModel()
                    {
                        PageName = "Building data",
                        ImageSource = "",
                        DetailPage = new BuildingInfoPage(userInfo.ResponsibleBuildingId.Value)
                    });
                }
            });
            

        }

        private async void LogoutBtnClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            await loginService.Logout();
            activityIndicator.IsRunning = false;

            while(NavigationDispetcher.Instance.Navigation.ModalStack.Count > 0)
                await NavigationDispetcher.Instance.Navigation.PopModalAsync();
        }
    }
}