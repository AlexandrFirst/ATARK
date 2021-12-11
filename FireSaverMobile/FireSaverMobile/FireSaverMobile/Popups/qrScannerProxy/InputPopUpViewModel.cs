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
        public string IotId { get; set; }
    }

    public class InputPopUpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string compartmentIdText = "";
        public string CompartmentIdText
        {
            get { return compartmentIdText; }
            set
            {
                compartmentIdText = value;
                OnPropertyChanged("CompartmentIdText");
            }
        }

        private string ioTIdText = "";
        private readonly Action<InputResult> onPopupClose;

        public string IoTIdText
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
                CompartmentId = Convert.ToInt32(compartmentIdText),
                IotId = compartmentIdText
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
