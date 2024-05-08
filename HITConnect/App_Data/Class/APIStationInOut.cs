using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyperActive
{
    public class APIStationInOut
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("BundleRfid")]
        public string BundleRfid { get; set; }

        [JsonProperty("BundleQuantity")]
        public string BundleQuantity { get; set; }

        [JsonProperty("TimeStamp")]
        public string TimeStamp { get; set; }

    }
}
