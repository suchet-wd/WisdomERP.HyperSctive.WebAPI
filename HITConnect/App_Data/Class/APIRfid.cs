using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HyperActive
{
    public class APIRfid
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("BoxRfid", Required = Required.Always)]
        public string BoxRfid { get; set; }

        [JsonProperty("BoxBarcode", Required = Required.Always)]
        public string BoxBarcode { get; set; }

        [JsonProperty("BundleRfidBarcodeList", Required = Required.Always)]
        public List<APIRfidBarcode> BundleRfidBarcodeList { get; set; }

        [JsonProperty("TimeStamp", Required = Required.Always)]
        public DateTime TimeStamp { get; set; }


    }
}
