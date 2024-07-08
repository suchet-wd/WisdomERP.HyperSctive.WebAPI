using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyperActive
{
    public class APIBundleUpdate
    {
        [JsonProperty("BundleBarCode", Required = Required.Always)]
        public string BundleBarCode { get; set; }
    }
}
