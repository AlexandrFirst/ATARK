using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FireSaverMobile.ViewModels
{
    public class FloorInfoViewModel : CompartmentBaseViewModel
    {
        public ICommand GoToRoomInfos { get; set; }

        public EventHandler OnGettingRoomInfos;

        public FloorInfoViewModel(int baseId):base(baseId)
        {
            GoToRoomInfos = new Command<int>(flooId =>
            {
                OnGettingRoomInfos.Invoke(null, new CompartmentInfoEventArgs() { CompartmentId = flooId});
            });
        }

        protected override async Task<List<CompartmentDto>> GetCompartmentFromBase(int buildingId)
        {
            var floorInfos = await evacuationService.GetFloorsByBuildingId(buildingId);
            return floorInfos;
        }
    }
}
