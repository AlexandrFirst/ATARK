using FireSaverMobile.Models.QRModel;
using Newtonsoft.Json;
using QRCoder;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireSaverMobile.Popups.QRCodePopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRCodePopup : PopupPage
    {
        public QRCodePopup(QrModel qrModel)
        {
            InitializeComponent();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            var textToCode = JsonConvert.SerializeObject(qrModel);

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(textToCode, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qRCode.GetGraphic(20);

            Image image = new Image();
            image.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
            image.Aspect = Aspect.Fill;

            content.Children.Add(image);

        }
    }
}