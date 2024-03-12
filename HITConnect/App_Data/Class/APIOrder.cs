using Newtonsoft.Json;

namespace HyperActive
{
    public class APIOrder
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("OrderNo", Required = Required.Always)]
        public string OrderNo { get; set; }

    }
}
