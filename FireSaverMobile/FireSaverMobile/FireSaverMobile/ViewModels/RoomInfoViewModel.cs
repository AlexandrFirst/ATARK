using FireSaverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.ViewModels
{
    public class RoomInfoViewModel : CompartmentBaseViewModel
    {
        public RoomInfoViewModel(int baseId) : base(baseId)
        {
        }

        protected override async Task<List<CompartmentDto>> GetCompartmentFromBase(int floorId)
        {
            var roomInfos = await evacuationService.GetRoomsByFloorId(floorId);
            return roomInfos;
        }
    }
}
