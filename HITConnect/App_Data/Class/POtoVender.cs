using Newtonsoft.Json;

namespace HITConnect
{
    public class POtoVender
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("FromPODate")]
        public string startDate { get; set; }

        [JsonProperty("ToPODate")]
        public string endDate { get; set; }

        [JsonProperty("PONumber")]
        public string PONo { get; set; }

        [JsonProperty("FromAckDate")]
        public string startACK { get; set; }

        [JsonProperty("ToAckDate")]
        public string endACK { get; set; }

        [JsonProperty("AcknowledgeBy")]
        public string AcknowledgeBy { get; set; }
    }
}
