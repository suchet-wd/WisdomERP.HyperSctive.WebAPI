﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class OrderDetails
    {
        [JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [JsonProperty("JobProductionNo")]
        public string JobProductionNo { get; set; }

        [JsonProperty("SubjobNo")]
        public string SubjobNo { get; set; }

        [JsonProperty("GacDate")]
        public string GacDate { get; set; }

        [JsonProperty("PoLineItem")]
        public string PoLineItem { get; set; }

        [JsonProperty("Colorway")]
        public string Colorway { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("SubJobQuantity")]
        public int SubJobQuantity { get; set; }

        [JsonProperty("AssortQuantity")]
        public List<string> AssortQuantity { get; set; }

        [JsonProperty("BoxQuantity")]
        public int BoxQuantity { get; set; }

        [JsonProperty("PackType")]
        public string PackType { get; set; }

        [JsonProperty("PackRatio")]
        public List<OrderProdDetailsRatio> PackRatio { get; set; }
    }
}
