using FireSaverMobile.Helpers;
using FireSaverMobile.MapRenderer;
using FireSaverMobile.Models;
using FireSaverMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentRoomPage : ContentPage
    {
        private CurrentCompartmentViewModel viewModel;
        private bool isCreated = false;

        public CurrentRoomPage()
        {
            InitializeComponent();

            viewModel = new CurrentCompartmentViewModel(Navigation);


            viewModel.OnEvacPlanRecieved += (object sender, EvacuationPlanDto evacPlan) =>
            {

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!isCreated)
                    {
                        await Create();
                        Thread.Sleep(1000);
                        await InitMap(evacPlan.Url, evacPlan.Width, evacPlan.Height);
                    }
                    else 
                    {
                        ChangeMap(evacPlan.Url);
                    }

                    isCreated = true;
                });
            };

            this.BindingContext = viewModel;
        }


        protected override async void OnAppearing()
        {
            var bindingContext = this.BindingContext as CurrentCompartmentViewModel;

            bindingContext.OnEvacPlanRecieved += (object sender, EvacuationPlanDto evacPlan) =>
            {

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!isCreated)
                        await Create();
                    Thread.Sleep(1000);
                    await InitMap(evacPlan.Url, evacPlan.Width, evacPlan.Height);

                    isCreated = true;
                });
            };

            if (bindingContext != null)
            {
                bindingContext.IsBusy = true;
                var isInited = await bindingContext.InitInfo();
                if(isInited)
                    await bindingContext.InitInfo();
                bindingContext.IsBusy = false;
            }



            base.OnAppearing();

        }

        private void RefreshBtn_clicked(object sender, EventArgs e)
        {
            var model = this.BindingContext as CurrentCompartmentViewModel;
            model.RefreshInfo.Execute(null);
        }

        private void TelephoneNumberClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            PhoneDialer.Open(button.CommandParameter.ToString());
        }

        private async Task InitMap(string imageUrl, int width, int height)
        {
            var res = await webView.EvaluateJavaScriptAsync(string.Format($"initMap('{imageUrl}', {width}, {height})"));
            Console.WriteLine(res);
        }

        private async Task Create()
        {
            var source = new HtmlWebViewSource();
            source.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("FireSaverMobile.index.html");
            StreamReader reader = null;
            if (stream != null)
            {
                try
                {
                    reader = new StreamReader(stream);
                    source.Html = reader.ReadToEnd();
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Dispose();
                    }
                }
                webView.Source = source;
            }
            else
            {
                await DisplayAlert("Map error", "Try again", "Ok");
            }
        }

        private async void EnterOtherRoomBtn_click(object sender, EventArgs e)
        {
            viewModel.GoToRulePage.Execute(this.Navigation);
        }

        private void ChangeMap(string url)
        {
            webView.Eval("clearMap()");
            Thread.Sleep(1000);
            webView.Eval($"setMap('{url}')");
        }
    }
}