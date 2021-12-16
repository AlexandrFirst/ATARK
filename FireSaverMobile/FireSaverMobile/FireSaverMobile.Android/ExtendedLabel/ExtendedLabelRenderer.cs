using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[
    assembly: ExportRenderer(
    typeof(FireSaverMobile.ExtendedLabelRenderer.ExtendedLabelRenderer),
    typeof(FireSaverMobile.Droid.ExtendedLabel.ExtendedLabelRenderer))
]
namespace FireSaverMobile.Droid.ExtendedLabel
{
    class ExtendedLabelRenderer: Xamarin.Forms.Platform.Android.LabelRenderer
    {
        public ExtendedLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var el = (Element as FireSaverMobile.ExtendedLabelRenderer.ExtendedLabelRenderer);

            if (el != null && el.JustifyText)
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    Control.JustificationMode = Android.Text.JustificationMode.InterWord;
                }

            }
        }
    }
}