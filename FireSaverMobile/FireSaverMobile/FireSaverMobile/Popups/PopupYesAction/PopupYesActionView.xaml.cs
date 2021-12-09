using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KinderMobile.PopupYesNo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupYesActionView : PopupPage
    {
        public PopupYesActionView(Action PositiveAction, string message)
        {
            InitializeComponent();
            this.BindingContext = new PopupYesActionViewModel(PositiveAction, message);
        }
    }
}