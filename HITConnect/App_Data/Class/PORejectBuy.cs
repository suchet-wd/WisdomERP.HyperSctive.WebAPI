using Newtonsoft.Json;

namespace HITConnect
{
    public class PORejectBuy
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("PORejected", Required = Required.Always)]
        public PORejected PORejected { get; set; }
    }

    public class PORejected
    {
        [JsonProperty("PONumber", Required = Required.Always)]
        public string PONo { get; set; }

        [JsonProperty("Item", Required = Required.Always)]
        public string Item { get; set; }

        [JsonProperty("Color", Required = Required.Always)]
        public string Color { get; set; }

        [JsonProperty("Size", Required = Required.Always)]
        public string Size { get; set; }

        [JsonProperty("Remark", Required = Required.Always)]
        public string Remark { get; set; }
    }
}
