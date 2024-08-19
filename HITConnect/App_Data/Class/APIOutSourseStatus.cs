using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyperActive
{
    public class APIOutSourseStatus
    {
        //[JsonProperty("Authen", Required = Required.Always)]
        //public UserAuthen authen { get; set; }

        [JsonProperty("BundleBarcode")]
        public string BundleBarcode { get; set; }
        
    }
}
