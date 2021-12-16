using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace FireSaverMobile.Pages
{
    public class FlayoutItemModel
    {
        public LocalizedString PageNameLocalized { get; set; }
        public string PageName { get; set; }
        public string ImageSource { get; set; }
        public Page DetailPage { get; set; }
    }
}
