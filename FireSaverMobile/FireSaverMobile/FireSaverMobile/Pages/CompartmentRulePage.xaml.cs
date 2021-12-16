using FireSaverMobile.Models.QRModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompartmentRulePage : ContentPage
    {
        private readonly QrModel qrModel;

        public CompartmentRulePage(string rules, QrModel qrModel)
        {
            InitializeComponent();
            RulesContent.Text = rules;
            this.qrModel = qrModel;
        }

        private async void EnterRoomClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new CompartmentTestPage(qrModel));
        }
    }
}