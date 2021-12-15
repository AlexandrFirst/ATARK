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
    public partial class MainNavPage : FlyoutPage
    {
        public MainNavPage()
        {
            InitializeComponent();
            flyout.listView.ItemSelected += navItemSelected;
        }

        private void navItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var model = e.SelectedItem as FlayoutItemModel;

            if (model != null)
            {
                Detail = new NavigationPage(model.DetailPage);
                flyout.listView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}