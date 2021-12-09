using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FireSaverMobile.Models
{

    public enum CompartmentType { Floor, Room }

    public class CompartmentInfoEventArgs : EventArgs
    {
        public int CompartmentId { get; set; }
    }

    public abstract class CompartmentBaseViewModel : BaseViewModel
    {


        protected IEvacuationService evacuationService;

        public EventHandler OnGettingCurrentRoomEvacPlan;

        public ICommand GetEvacuationPlanCommand { get; set; }


        public ObservableCollection<CompartmentDto> CompartmentInfos { get; set; } = new ObservableCollection<CompartmentDto>();


        public CompartmentBaseViewModel(int baseId)
        {
            evacuationService = TinyIOC.Container.Resolve<IEvacuationService>();

            GetEvacuationPlanCommand = new Command<int>(currentCompartmentId =>
            {
                OnGettingCurrentRoomEvacPlan.Invoke(null, new CompartmentInfoEventArgs() { CompartmentId = currentCompartmentId });
            });

            Task.Run(async () =>
            {
                var compartmentInfos = await GetCompartmentFromBase(baseId);

                foreach (var compartment in compartmentInfos)
                {
                    CompartmentInfos.Add(compartment);
                }
            });
        }

        protected abstract Task<List<CompartmentDto>> GetCompartmentFromBase(int baseCompartmentId);
    }
}
