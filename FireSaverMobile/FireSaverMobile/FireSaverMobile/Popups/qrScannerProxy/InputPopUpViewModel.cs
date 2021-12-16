using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FireSaverMobile.Popups.qrScannerProxy
{
    public class InputResult
    {
        public int CompartmentId { get; set; }
        public int? IotId { get; set; }
    }

    public class InputPopUpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int compartmentIdText = 0;
        public int CompartmentIdText
        {
            get { return compartmentIdText; }
            set
            {
                compartmentIdText = value;
                OnPropertyChanged("CompartmentIdText");
            }
        }

        private int? ioTIdText = null;
        private readonly Action<InputResult> onPopupClose;

        public int? IoTIdText
        {
            get { return ioTIdText; }
            set
            {
                ioTIdText = value;
                OnPropertyChanged("IoTIdText");
            }
        }

        public ICommand ApplyInput { get; set; }

        public InputPopUpViewModel(Action<InputResult> OnPopupClose)
        {
            ApplyInput = new Command(async () => await Action());
            onPopupClose = OnPopupClose;
        }

        async Task Action()
        {

            onPopupClose(new InputResult() 
            {
                CompartmentId = compartmentIdText,
                IotId = ioTIdText
            });

            while (PopupNavigation.Instance.PopupStack.Count > 0)
                await PopupNavigation.Instance.PopAsync(true);
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
