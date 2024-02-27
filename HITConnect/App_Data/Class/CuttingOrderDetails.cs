using Newtonsoft.Json;
using System.Collections.Generic;

namespace HyperActive
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
