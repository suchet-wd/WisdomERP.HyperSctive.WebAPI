using Newtonsoft.Json;

namespace HyperActive
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
