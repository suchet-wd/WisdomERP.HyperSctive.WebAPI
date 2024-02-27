using Newtonsoft.Json;

namespace HyperActive
{
    public class CuttingPartRoute
    {
        [JsonProperty("Station")]
        public string Station { get; set; }

        [JsonProperty("FactoryNo")]
        public string FactoryNo { get; set; }

    }
}
