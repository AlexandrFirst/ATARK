using FireSaverMobile.MapRenderer;
using FireSaverMobile.Models;
using FireSaverMobile.Models.PointModels;
using FireSaverMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EvacuationPlanPage : ContentPage
    {
        EvacuationPlanViewModel evacPlanModel;

        private List<int> routePointIds = new List<int>();
        private List<int> linesBetweenPoints = new List<int>();

        private Position userPosition = null;

        Dictionary<int, Position> routePoints = new Dictionary<int, Position>();

        bool isMapInited = false;

        int currentSelectedPoint = 0;
        bool hasExit = false;

        public EvacuationPlanPage()
        {
            InitializeComponent();
            evacPlanModel = new EvacuationPlanViewModel();
            this.BindingContext = evacPlanModel;


            evacPlanModel.OnEvacuationPlanInit += (EvacuationPlanDto evacPlanDto, RoutePointsDto startPoint) =>
            {
                ClearAllPoints();
                ClearAllLines();
                if (!isMapInited)
                {
                    InitMap(evacPlanDto.Url, evacPlanDto.Width, evacPlanDto.Height);
                    isMapInited = true;
                }
                else
                {
                    ChangeMap(evacPlanDto.Url);
                }

                InitEvacPoints(startPoint);

                if (routePointIds != null && routePointIds.Count > 0)
                {
                    SelectPoint(routePointIds[currentSelectedPoint]);
                }

            };

            evacPlanModel.OnUserPositonChange += (newUserPos) =>
            {
                if (userPosition == null)
                {
                    PlacePoint(-1, newUserPos, "#4B0082");
                }
                else
                {
                    ChangePointPos(newUserPos, -1);
                }

                userPosition = newUserPos;
            };
        }

        protected override async void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Create();
                Thread.Sleep(1000);
            });

            var bindingContext = this.BindingContext as EvacuationPlanViewModel;
            if (bindingContext != null)
                await bindingContext.InitEvacPlans();

            base.OnAppearing();

        }

        protected override bool OnBackButtonPressed()
        {
            var evacViewModel = BindingContext as EvacuationPlanViewModel;
            return evacPlanModel.IsUserReachedExit;
        }

        private void InitEvacPoints(RoutePointsDto root)
        {
            if (root != null)
            {
                routePoints.Clear();

                PlacePoint(root.RoutePoints[0].Id.Value, new Position()
                {
                    Latitude = root.RoutePoints[0].Longtitude,
                    Longtitude = root.RoutePoints[0].Latitude
                });

                routePointIds.Add(root.RoutePoints[0].Id.Value);

                for (int i = 1; i < root.RoutePoints.Count; i++)
                {
                    PlacePoint(root.RoutePoints[i].Id.Value, new Position()
                    {
                        Latitude = root.RoutePoints[i].Longtitude,
                        Longtitude = root.RoutePoints[i].Latitude
                    });

                    routePoints.Add(root.RoutePoints[i].Id.Value, new Position()
                    {
                        Latitude = root.RoutePoints[i].Longtitude,
                        Longtitude = root.RoutePoints[i].Latitude
                    });

                    routePointIds.Add(root.RoutePoints[i].Id.Value);
                    PlaceLine(root.RoutePoints[i - 1].Id.Value, root.RoutePoints[i].Id.Value, root.RoutePoints[i].Id.Value);
                    linesBetweenPoints.Add(root.RoutePoints[i].Id.Value);
                }
            }

            return;
        }


        private async void InitMap(string imageUrl, int width, int height)
        {

            var res = await webView.EvaluateJavaScriptAsync(string.Format($"initMap('{imageUrl}', {width}, {height})"));
            Console.WriteLine(res);
        }

        private void PlacePoint(int pointId, Position pointPos, string color = "#ff7800")
        {
            webView.Eval($"placeMarker({pointPos.Latitude}, {pointPos.Longtitude}, {pointId}, '{color}')");
        }

        private void PlaceLine(int fromId, int toId, int lineId)
        {
            webView.Eval($"newLine({fromId},{toId},'#DC143C', {lineId})");
        }

        private void ClearAllPoints()
        {
            foreach (var pointId in routePointIds)
            {
                webView.Eval($"removePoint({pointId})");
            }
        }

        private void ClearAllLines()
        {
            foreach (var lineId in linesBetweenPoints)
            {
                webView.Eval($"removeLine({lineId})");
            }
        }

        private void ChangeMap(string url)
        {
            webView.Eval("clearMap()");
            Thread.Sleep(1000);
            webView.Eval($"setMap('{url}')");
        }

        private void ChangePointPos(Position newPos, int pointId)
        {
            webView.Eval($"changePointPosition({pointId}, {newPos.Latitude}, {newPos.Longtitude})");
        }

        private void SelectPoint(int pointId)
        {
            webView.Eval($"selectPoint({pointId})");
        }

        private void NextPointBtnClicked(object sender, EventArgs e)
        {
            currentSelectedPoint++;
            currentSelectedPoint %= (routePointIds.Count - 1);

            SelectPoint(routePointIds[currentSelectedPoint]);
        }

        private void PrevPointDtnClicked(object sender, EventArgs e)
        {
            currentSelectedPoint--;
            if (currentSelectedPoint < 0)
                currentSelectedPoint = routePointIds.Count - 1;

            SelectPoint(routePointIds[currentSelectedPoint]);
        }

        private void BlockPointBtnClicked(object sender, EventArgs e)
        {
            Position pointPosition;
            if (routePoints.TryGetValue(routePointIds[currentSelectedPoint], out pointPosition)) 
            {
                evacPlanModel.BlockSelectedPoint.Execute(pointPosition);
            }
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
    }
}