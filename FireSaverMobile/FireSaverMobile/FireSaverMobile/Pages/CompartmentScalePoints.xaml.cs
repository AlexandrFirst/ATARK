using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.MapRenderer;
using FireSaverMobile.Models;
using FireSaverMobile.Popups.PopupNotification;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using FireSaverMobile.Helpers;

namespace FireSaverMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompartmentScalePoints : ContentPage
    {
        private IEvacuationService evacuationService;
        private IScalePointService scalePointService;

        public ObservableCollection<ScalePointDto> ScalePoints { get; set; } = new ObservableCollection<ScalePointDto>();

        private int selectedPoint = -1;
        private CancellationTokenSource cts;

        public CompartmentScalePoints(int compartmentId)
        {
            InitializeComponent();

            evacuationService = TinyIOC.Container.Resolve<IEvacuationService>();
            scalePointService = TinyIOC.Container.Resolve<IScalePointService>();

            Task.Run(async () =>
            {
                await Create();
                var evacuationPlan = await evacuationService.GetEvacPlan(compartmentId);
                if (evacuationPlan == null)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(evacuationPlan.Url))
                {
                    loadingIndicator.IsRunning = true;
                    Thread.Sleep(1000);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitMap(evacuationPlan.Url);


                        foreach (var point in evacuationPlan.ScaleModel.ScalePoints)
                        {
                            PlacePoint(point.MapPosition, point.Id);
                            ScalePoints.Add(point);
                        }

                        BindingContext = this;

                    });
                    loadingIndicator.IsRunning = false;
                }
                else 
                {
                    await PopupNavigation.Instance.PushAsync(new PopupNotificationView("No plan is loaded", MessageType.Warning));
                }
            });
        }


        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }


        private void CenterMapBtn_Cliked(object sender, EventArgs e)
        {
            webView.Eval(string.Format("centerMap()"));
        }

        private void SelectPointClick_btn(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var pointIdToSelect = btn.CommandParameter;
            selectedPoint = Convert.ToInt32(pointIdToSelect);
            selectPoint(selectedPoint);
        }

        private async void UpdateSelectedPoint(object sender, EventArgs e)
        {
            if (selectedPoint == -1) 
            {
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Select point!", MessageType.Error));
                return;
            }

            loadingIndicator.IsRunning = true;
            cts = new CancellationTokenSource();
            var userLocation = await LocationSyncer.GetCurrentLocation(cts);
            var result = await scalePointService.UpdateScalepointWorldPosition(
                new Position() 
                {
                    Latitude = userLocation.Latitude.ToString(),
                    Longtitude = userLocation.Longitude.ToString()
                },
                selectedPoint);

            loadingIndicator.IsRunning = false;

            if(result)
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("World position is updated", MessageType.Notification));
            else
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Try again", MessageType.Warning));
        }

        private void selectPoint(int pointId) 
        {
            webView.Eval(string.Format($"selectPoint({pointId})"));
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

        private void InitMap(string imageUrl)
        {
            webView.Eval(string.Format($"initMap('{imageUrl}')"));
        }

        private void PlacePoint(Position pointPos, int pointId)
        {
            webView.Eval(string.Format($"placeMarker({pointPos.Latitude}, {pointPos.Longtitude}, {pointId})"));
        }

        
    }
}