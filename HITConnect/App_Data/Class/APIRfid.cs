using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class APIRfid
    {
        [JsonProperty("BoxRfid")]
        public string BoxRfid { get; set; }

        [JsonProperty("BoxBarcode")]
        public string BoxBarcode { get; set; }

        [JsonProperty("BundleRfidBarcodeList")]
        public List<APIRfidBarcode> BundleRfidBarcodeList { get; set; }

        [JsonProperty("TimeStamp")]
        public DateTime TimeStamp { get; set; }

    }
}
