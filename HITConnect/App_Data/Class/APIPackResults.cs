using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyperActive
{
    //public class APIPackResults
    //{
    //    [JsonProperty("BundleBarCode", Required = Required.Always)]
    //    public string BundleBarCode { get; set; }
    //}

    public class APIPackResults
    {
        [JsonProperty("StateGetAll", Required = Required.Always)]
        public string StateGetAll { get; set; }

        [JsonProperty("DateStart")]
        public string DateStart { get; set; }
        // Option Start Date (GetAll "DateStart": "")

        [JsonProperty("DateEnd", Required = Required.Always)]
        public string DateEnd { get; set; }
        // Option End Date (GetAll "DateEnd": "9999/99/99")
    }
}
