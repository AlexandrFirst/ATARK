using FireSaverMobile.Models.QRModel;
using FireSaverMobile.Models.TestCompartmentModels;
using FireSaverMobile.ViewModels;
using KinderMobile.PopupYesNo;
using Rg.Plugins.Popup.Services;
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
    public partial class CompartmentTestPage : ContentPage
    {
        private readonly QrModel qrModel;

        public CompartmentTestPage(QrModel qrModel)
        {
            InitializeComponent();
            this.qrModel = qrModel;

            var context = new CompartmentTestPageViewModel();

            context.OnCompartmentSuccessEntering += async (sender, args) =>
              {
                  await PopupNavigation.Instance.PushAsync(new PopupYesActionView(async () =>
                  {
                      await Navigation.PopAsync();
                  }, "You successfully entered compartment", true));
              };

            context.OnTestFailed += async (sender, args) =>
            {
                await PopupNavigation.Instance.PushAsync(new PopupYesActionView(async () =>
                {
                    await Navigation.PopAsync();
                }, "Test is failed. Try again later", true));
            };
            this.BindingContext = context;
        }

        protected async override void OnAppearing()
        {
            var context = this.BindingContext as CompartmentTestPageViewModel;
            await context.InitTest(qrModel);

            base.OnAppearing();
        }

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            var selectedCheckBox = (sender as Plugin.InputKit.Shared.Controls.CheckBox);

            var contextModel = this.BindingContext as CompartmentTestPageViewModel;

            var selectedValue = selectedCheckBox.CommandParameter;

            if (selectedCheckBox.IsChecked)
            {
                contextModel.AddAnswerToQuestion.Execute(selectedValue);
            }
            else 
            {
                contextModel.RemoveAnswerFromQuestion.Execute(selectedValue);
            }
        }
    }
}