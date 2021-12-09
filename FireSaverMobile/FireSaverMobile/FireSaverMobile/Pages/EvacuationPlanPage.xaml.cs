using FireSaverMobile.Models;
using FireSaverMobile.Models.PointModels;
using FireSaverMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        bool isMapInited = false;

        int currentSelectedPoint = 0;
        bool hasExit = false;

        public EvacuationPlanPage()
        {
            InitializeComponent();
            evacPlanModel = new EvacuationPlanViewModel();

            evacPlanModel.OnEvacuationPlanInit += (EvacuationPlanDto evacPlanDto, RoutePointDto startPoint) =>
            {
                ClearAllPoints();
                ClearAllLines();
                if (!isMapInited)
                    InitMap(evacPlanDto.Url);
                else
                    ChangeMap(evacPlanDto.Url);

                InitEvacPoints(startPoint);


                SelectPoint(routePointIds[currentSelectedPoint]);

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


            this.BindingContext = evacPlanModel;
        }



        private void InitEvacPoints(RoutePointDto rootPoint)
        {
            routePointIds.Add(rootPoint.Id);
            PlacePoint(rootPoint.Id, rootPoint.MapPosition);
            foreach (var point in rootPoint.ChildrenPoints)
            {
                if (point.RoutePointType == RoutePointType.EXIT)
                    hasExit = true;
                InitEvacPoints(point);
                PlaceLine(rootPoint.MapPosition, point.MapPosition, point.Id);
                linesBetweenPoints.Add(point.Id);
            }

            return;
        }

        private void InitMap(string mapUrl)
        {
            webView.Eval($"initMap('{mapUrl}')");
        }

        private void PlacePoint(int pointId, Position pointPos, string color = "#ff7800")
        {
            webView.Eval($"placeMarker({pointPos.Latitude}, {pointPos.Longtitude}, {pointId}, '{color}')");
        }

        private void PlaceLine(Position from, Position to, int lineId)
        {
            webView.Eval($"newLine({from.Latitude},{from.Longtitude}, {to.Latitude}, {to.Longtitude}, '#DC143C', {lineId})");
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
            evacPlanModel.BlockSelectedPoint.Execute(routePointIds[currentSelectedPoint]);
        }
    }
}