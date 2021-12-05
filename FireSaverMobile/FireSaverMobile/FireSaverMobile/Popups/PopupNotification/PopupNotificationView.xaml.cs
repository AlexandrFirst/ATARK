using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Popups.PopupNotification
{
    public partial class PopupNotificationView : PopupPage
    {
        public PopupNotificationView(string message, MessageType messageType)
        {
            InitializeComponent();
            this.BindingContext = new PopupNotificationViewModel(message, messageType);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}