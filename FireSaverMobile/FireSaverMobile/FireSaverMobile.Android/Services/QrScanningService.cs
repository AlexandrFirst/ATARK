using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FireSaverMobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


[assembly: Dependency(typeof(FireSaverMobile.Droid.Services.QrScanningService))]
namespace FireSaverMobile.Droid.Services
{
    public class QrScanningService : IQrScaninngService
    {
        public async Task<string> ScanAsync()
        {
            return "";
        }
    }
}