using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models.BuildingModels;
using FireSaverMobile.Models.GoogleApiResponse;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShelterRoutePage : ContentPage
    {
        int buildingId = 1002;

        private readonly IBuildingService buildingService;

        public ShelterRoutePage()
        {
            InitializeComponent();
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;

            buildingService = TinyIOC.Container.Resolve<IBuildingService>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var location = await LocationSyncer.GetCurrentLocation(new System.Threading.CancellationTokenSource());
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude),
                                             Xamarin.Forms.GoogleMaps.Distance.FromMiles(1)));

            var buildingInfo = await buildingService.GetBuildingInfoByBuildingId(buildingId);

            if (buildingInfo.Shelters.Count == 0) 
            {
                await DisplayAlert("Attention", "No shelters exists", "OK");
                return;
            }

            ShelterDto destination = null;
            double shelterIndex = Double.MaxValue;

            foreach (var shelter in buildingInfo.Shelters)
            {
                double tempShelterIndex = ((shelter.TotalPeople + 1) / (shelter.Capacity + 1)) * 0.4;
                tempShelterIndex+= shelter.Distance * 0.6;
                if (tempShelterIndex < shelterIndex) 
                {
                    shelterIndex = tempShelterIndex;
                    destination = shelter;
                }
            }

            if (destination == null) 
            {
                await DisplayAlert("Attention", "Don't know where to go", "OK");
                return;
            }

            GoogleApiRouteResponse routeToShelter = await buildingService.GetRouteToShelter(
                new Models.Position()
                {
                    Latitude = buildingInfo.BuildingCenterPosition.Latitude.Replace(',', '.'),
                    Longtitude = buildingInfo.BuildingCenterPosition.Longtitude.Replace(',', '.')
                },
                new Models.Position()
                {
                    Latitude = destination.ShelterPosition.Latitude.Replace(',','.'),
                    Longtitude = destination.ShelterPosition.Longtitude.Replace(',', '.')
                });
            
            if (routeToShelter == null) 
            {
                await DisplayAlert("Attention", "No shelters exists", "OK");
                return;
            }

            List<FireSaverMobile.Models.Position> routePoints = new List<Models.Position>();

            foreach (var route in routeToShelter.routes)
            {
                foreach (var leg in route.legs)
                {
                    foreach (var step in leg.steps)
                    {
                        routePoints.AddRange(DecodePolylinePoints(step.polyline.points));
                    }
                } 
            }

            var routeLine = new Xamarin.Forms.GoogleMaps.Polyline();
            routeLine.StrokeWidth = 10f;
            routeLine.StrokeColor = Color.Blue;

            foreach (var point in routePoints)
            {
                routeLine.Positions.Add(new Position(double.Parse(point.Latitude), double.Parse(point.Longtitude)));
            }
            map.Polylines.Add(routeLine);
        }

        string convertDoubleToString(double value)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            return value.ToString(nfi);
        }

        private List<FireSaverMobile.Models.Position> DecodePolylinePoints(string encodedPoints)
        {
            if (encodedPoints == null || encodedPoints == "") return null;
            List<FireSaverMobile.Models.Position> poly = new List<FireSaverMobile.Models.Position>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    FireSaverMobile.Models.Position p = new FireSaverMobile.Models.Position();
                    p.Latitude = convertDoubleToString(Convert.ToDouble(currentLat) / 100000.0);
                    p.Longtitude = convertDoubleToString(Convert.ToDouble(currentLng) / 100000.0);
                    poly.Add(p);
                }
            }
            catch (Exception ex)
            {
                // logo it
            }
            return poly;
        }
    }
}