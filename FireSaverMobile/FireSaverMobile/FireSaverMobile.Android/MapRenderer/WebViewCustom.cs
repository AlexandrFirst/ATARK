using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using FireSaverMobile.MapRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(IWebViewCustom), typeof(FireSaverMobile.Droid.MapRenderer.WebViewCustom))]
namespace FireSaverMobile.Droid.MapRenderer
{
#pragma warning disable CS0618
    public class WebViewCustom : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            Control.Settings.JavaScriptEnabled = true;
            Control.Settings.UseWideViewPort = true;
            Control.Settings.LoadWithOverviewMode = true;
            Control.Settings.SaveFormData = true;
            Control.Settings.SetRenderPriority(WebSettings.RenderPriority.High);
            Control.SetLayerType(LayerType.Hardware, null);
            //Control.SetLayerType(LayerType.Software, null);
            Control.SetWebChromeClient(new MyWebClient());
            global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
        }

        public class MyWebClient : WebChromeClient
        {

        }
    }
}