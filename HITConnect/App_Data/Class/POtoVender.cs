using Newtonsoft.Json;

namespace HITConnect
{
    public class POtoVender
    {
        [JsonProperty("Authen")]
        public UserAuthen authen { get; set; }

        [JsonProperty("startDate")]
        public string startDate { get; set; }

        [JsonProperty("endDate")]
        public string endDate { get; set; }

        [JsonProperty("PONo")]
        public string PONo { get; set; }
    }
}
