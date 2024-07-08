using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyperActive
{
    public class APIStationResults
    {
        //[JsonProperty("Authen", Required = Required.Always)]
        //public UserAuthen authen { get; set; }

        [JsonProperty("BoxRfid")]
        public string BoxRfid { get; set; }

        [JsonProperty("BoxBarcode")]
        public string BoxBarcode { get; set; }
        
    }
}
