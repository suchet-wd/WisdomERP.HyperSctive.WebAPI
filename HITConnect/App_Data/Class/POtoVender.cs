using Newtonsoft.Json;

namespace HITConnect
{
    public class POtoVender
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("FromDate")]
        public string startDate { get; set; }

        [JsonProperty("ToDate")]
        public string endDate { get; set; }

        [JsonProperty("PONumber")]
        public string PONo { get; set; }
    }
}
