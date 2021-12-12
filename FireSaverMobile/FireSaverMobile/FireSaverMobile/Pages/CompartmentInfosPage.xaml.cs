using FireSaverMobile.Models;
using FireSaverMobile.Resx;
using FireSaverMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompartmentInfosPage : ContentPage
    {
        public CompartmentInfosPage(int baseCompartmentId, CompartmentType compartmentType)
        {
            InitializeComponent();
            if (compartmentType == CompartmentType.Floor)
            {
                this.BindingContext = new FloorInfoViewModel(baseCompartmentId);

                (BindingContext as FloorInfoViewModel).OnGettingRoomInfos += async (sender, param) =>
                 {
                     var selectedFloorId = (param as CompartmentInfoEventArgs).CompartmentId;

                     LocalizedString localizedTitle = new LocalizedString(() => AppResources.Room);

                     await Navigation.PushAsync(new CompartmentInfosPage(selectedFloorId, CompartmentType.Room) { Title = localizedTitle.Localized });
                 };

                (BindingContext as FloorInfoViewModel).OnGettingCurrentRoomEvacPlan += async (sender, param) =>
                 {
                     var selectedFloorId = (param as CompartmentInfoEventArgs).CompartmentId;
                     await Navigation.PushAsync(new CompartmentScalePoints(selectedFloorId) { Title = string.Format(AppResources.FloorTitle, selectedFloorId) });
                 };
            }
            else if (compartmentType == CompartmentType.Room)
            {
                this.BindingContext = new RoomInfoViewModel(baseCompartmentId);

                (BindingContext as RoomInfoViewModel).OnGettingCurrentRoomEvacPlan += async (sender, param) =>
                {
                    var selectedRoomId = (param as CompartmentInfoEventArgs).CompartmentId;
                    await Navigation.PushAsync(new CompartmentScalePoints(selectedRoomId) { Title = string.Format(AppResources.FloorTitle, selectedRoomId) });
                };
            }

            compartmentList.ItemSelected += (sender, args) => compartmentList.SelectedItem = null;
        }

        private void EvacPlanBtn_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            (this.BindingContext as CompartmentBaseViewModel).GetEvacuationPlanCommand.Execute(btn.CommandParameter);
        }

        private void RoomVisitBtn_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            (this.BindingContext as FloorInfoViewModel).GoToRoomInfos.Execute(btn.CommandParameter);
        }
    }
}