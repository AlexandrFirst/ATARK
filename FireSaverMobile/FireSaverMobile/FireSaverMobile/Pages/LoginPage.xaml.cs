using FireSaverMobile.Helpers;
using FireSaverMobile.ViewModels;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FireSaverMobile
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginPageViewModel();
        }

        private void showPassword_CheckChanged(object sender, EventArgs e)
        {
            passwordField.IsPassword = !(sender as Plugin.InputKit.Shared.Controls.CheckBox).IsChecked;
        }
    }
}
