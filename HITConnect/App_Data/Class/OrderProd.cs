using Newtonsoft.Json;
using System.Collections.Generic;

namespace HyperActive
{
    public class OrderProd
    {
        [JsonProperty("OrderProdNo")]
        public string OrderProdNo { get; set; }

        [JsonProperty("CustomerPO")]
        public string CustomerPO { get; set; }

        [JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [JsonProperty("Job")]
        public string Job { get; set; }

        [JsonProperty("SubJobNo")]
        public string SubJobNo { get; set; }

        [JsonProperty("OrderProdDetails")]
        public List<OrderProdDetails> OrderProdDetails { get; set; }

    }
}
