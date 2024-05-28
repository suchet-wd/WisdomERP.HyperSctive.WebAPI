using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyperActive
{
    public class APIStationInOut
    {
        //[JsonProperty("Authen", Required = Required.Always)]
        //public UserAuthen authen { get; set; }

        [JsonProperty("InStation")]
        public string InStation { get; set; }

        [JsonProperty("OutStation")]
        public string OutStation { get; set; }

        [JsonProperty("BoxRfidList")]
        public List<string> BoxRfidList { get; set; }

        [JsonProperty("TimeStamp")]
        public string TimeStamp { get; set; }

    }
}
