using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FireSaverMobile.MapRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(FireSaverMobile.Droid.MapRenderer.BaseUrl))]
namespace FireSaverMobile.Droid.MapRenderer
{
    public class BaseUrl : IBaseUrl
    {
        public string Get() { return "file:///android_asset/"; }
    }
}