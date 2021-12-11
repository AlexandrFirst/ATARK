using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KinderMobile.PopupYesNo
{

    public enum ResponseType 
    {
        Yes,
        No
    }

    class PopupYesActionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public ICommand ActionCommand { get; set; }
        public string Message { get; set; }

        private readonly Action PositiveResponse;
        private readonly bool automaticPopUpClose;

        public PopupYesActionViewModel(Action PositiveAction, string message, bool automaticPopUpClose = false)
        {
            ActionCommand = new Command<object>( async (param) => await Action(param));

            this.PositiveResponse = PositiveAction;

            Message = message;
            this.automaticPopUpClose = automaticPopUpClose;
        }


        async Task Action(object response) 
        {
            ResponseType n_response = (ResponseType)int.Parse(response.ToString());

            switch (n_response) 
            {
                case ResponseType.Yes:
                    PositiveResponse();
                    break;
                default:
                    break;
            }
            if(automaticPopUpClose)
                await PopupNavigation.Instance.PopAsync(true);
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
