using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models.QRModel
{
    public enum QRCodeType { CompartmentCode, UserCode, Undefined }

    public class QrModel
    {
        [JsonProperty("userId")]
        public int? UserId = null;

        [JsonProperty("buildingId")]
        public int? BuildingId = null;

        [JsonProperty("IOTId")]
        public string IOTId = null;

        [JsonProperty("compatrmentId")]
        public int? CompatrmentId = null;

        [JsonProperty("QrCodeType")]
        public QRCodeType QrCodeType = QRCodeType.Undefined;
    }
}
