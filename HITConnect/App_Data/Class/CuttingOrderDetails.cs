using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class CuttingOrderDetails
    {
        //TPRODTOrderProd_MarkMain - FTOrderProdNo
        [JsonProperty("OrderProdNo")]
        public string OrderProdNo { get; set; }

        [JsonProperty("TableNo")]
        public string TableNo { get; set; }

        [JsonProperty("NumberOfLayers")]
        public string NumberOfLayers { get; set; }


        [JsonProperty("PartDetail")]
        public List<CuttingPartDetails> PartDetail { get; set; }

    }
}
