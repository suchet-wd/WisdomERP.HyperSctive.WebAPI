using Newtonsoft.Json;

namespace HyperActive
{
    public class UserAuthen
    {
        [JsonProperty("id", Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty("pwd", Required = Required.Always)]
        public string pwd { get; set; }

        [JsonProperty("token", Required = Required.Always)]
        public string token { get; set; }
    }

}