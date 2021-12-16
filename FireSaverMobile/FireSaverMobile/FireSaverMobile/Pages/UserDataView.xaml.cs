using FireSaverMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDataView : ContentView
    {
        public UserDataView()
        {
            InitializeComponent();
            BindingContext = new UserDataViewModel();
            //this.LayoutChanged += (s, e) =>
            //   {
                  
            //   };
        }


    }
}