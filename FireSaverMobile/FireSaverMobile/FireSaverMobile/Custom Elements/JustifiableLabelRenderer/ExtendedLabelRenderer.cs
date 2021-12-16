using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FireSaverMobile.ExtendedLabelRenderer
{
    public class ExtendedLabelRenderer: Label
    {
        public static readonly BindableProperty JustifyTextProperty =
           BindableProperty.Create(
               propertyName: nameof(JustifyText),
               returnType: typeof(Boolean),
               declaringType: typeof(ExtendedLabelRenderer),
               defaultValue: false,
               defaultBindingMode: BindingMode.OneWay
        );

        public bool JustifyText
        {
            get { return (Boolean)GetValue(JustifyTextProperty); }
            set { SetValue(JustifyTextProperty, value); }
        }
    }
}
