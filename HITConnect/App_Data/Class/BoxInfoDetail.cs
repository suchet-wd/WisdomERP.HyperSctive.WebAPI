using Newtonsoft.Json;
using System.Collections.Generic;

namespace HyperActive
{
    public class BoxInfoDetail
    {
        [JsonProperty("FinishBoxRfid")]
        public string FinishBoxRfid { get; set; }

        [JsonProperty("FinishBoxBarcode")]
        public string FinishBoxBarcode { get; set; }

        [JsonProperty("FinishBoxIsEmpty")]
        public bool FinishBoxIsEmpty { get; set; }

        //public List<OrderProdDetails> OrderProdDetails { get; set; }

    }
}
