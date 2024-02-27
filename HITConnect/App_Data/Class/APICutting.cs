using Newtonsoft.Json;

namespace HyperConvert
{
    public class APICutting
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("PooNo", Required = Required.Always)]
        public string PooNo { get; set; }

    }
}