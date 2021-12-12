using FireSaverMobile.Helpers;
using FireSaverMobile.Pages;
using FireSaverMobile.Resx;
using FireSaverMobile.ViewModels;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace FireSaverMobile
{
    public partial class LoginPage : ContentPage
    {
        public ObservableCollection<LanguageKeyValue> InterfaceLanguages { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginPageViewModel();

            LocalizationResourceManager.Current.CurrentCulture = CultureInfo.CurrentCulture;

            InterfaceLanguages = new ObservableCollection<LanguageKeyValue>()
            {
               new LanguageKeyValue() { Key = "en", GetModifiedValue = () => AppResources.English },
               new LanguageKeyValue() { Key = "uk", GetModifiedValue = () => AppResources.Ukrainian }
            };
        }

        private void showPassword_CheckChanged(object sender, EventArgs e)
        {
            passwordField.IsPassword = !(sender as Plugin.InputKit.Shared.Controls.CheckBox).IsChecked;
        }

        private async void LaguageChangeBtn_click(object sender, EventArgs e)
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
