using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class OrderProdDetailsRatio
    {
        [JsonProperty("Colorway")]
        public string Colorway { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

    }
}
