using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Models.BuildingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Pages.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShelterPage : ContentPage
    {
        private readonly ShelterDto shelterDto;

        private ICompartmentEnterService compartmentService;

        public ShelterPage(ShelterDto shelterDto)
        {
            InitializeComponent();
            this.shelterDto = shelterDto;
            compartmentService = TinyIOC.Container.Resolve<ICompartmentEnterService>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Position shelterPosition = new Position(double.Parse(shelterDto.ShelterPosition.Latitude),
                double.Parse(shelterDto.ShelterPosition.Longtitude));

            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                shelterPosition,
                Distance.FromMiles(1)));

            map.Pins.Add(new Pin() { Position = shelterPosition, Address = shelterDto.Address, Label = shelterDto.Address });

            addressLbl.Text = shelterDto.Address;
            capacityLbl.Text = shelterDto.TotalPeople + "/" + shelterDto.Capacity;
            leaveBtn.Clicked += async (e, o) =>
            {
                await compartmentService.LeaveShelter(shelterDto.Id);
                await DisplayAlert("Attention", "Good bye", "Ok");
            };
        }
    }
}