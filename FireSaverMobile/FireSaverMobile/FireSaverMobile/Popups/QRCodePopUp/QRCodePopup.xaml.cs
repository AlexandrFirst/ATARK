using FireSaverMobile.Models.QRModel;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace FireSaverMobile.Popups.QRCodePopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRCodePopup : PopupPage
    {
        ZXingBarcodeImageView barcode;
        public QRCodePopup(QrModel qrModel)
        {
            InitializeComponent();

            barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingBarcodeImageView",
            };

            barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            barcode.BarcodeOptions.Width = 400;
            barcode.BarcodeOptions.Height = 400;
            barcode.BarcodeOptions.Margin = 10;
            barcode.BarcodeValue = JsonConvert.SerializeObject(qrModel); ;

            content.Children.Add(barcode);

        }
    }
}