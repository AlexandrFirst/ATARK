using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using FireSaverMobile.Resx;
namespace FireSaverMobile.ViewModels
{
    public class NavFlayoutViewModel : BaseViewModel
    {
        public LocalizedString CurrentLanguage { get; }

        public ICommand ChangeLanguageCommand { get; }

        public NavFlayoutViewModel()
        {

            LocalizationResourceManager.Current.CurrentCulture = CultureInfo.CurrentCulture;

            ChangeLanguageCommand = new Command<string>((lang) =>
            {
                LocalizationResourceManager.Current.CurrentCulture = new CultureInfo(lang);
            });
        }
    }
}
