using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Popups.qrScannerProxy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputPopUp : PopupPage
    {
        public InputPopUp(Action<InputResult> OnPopupClose)
        {
            InitializeComponent();
            BindingContext = new InputPopUpViewModel(OnPopupClose);
        }
    }
}