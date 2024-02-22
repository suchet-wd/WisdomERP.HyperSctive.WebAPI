using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class CuttingPartDetails
    {
        [JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [JsonProperty("PartNo")]
        public string PartNo { get; set; }

        [JsonProperty("PartType")]
        public string PartType { get; set; }

        [JsonProperty("ColorWay")]
        public string ColorWay { get; set; }

        [JsonProperty("ColorShades")]
        public string ColorShades { get; set; }

        [JsonProperty("Sizw")]
        public string Size { get; set; }


        [JsonProperty("Route")]
        public List<CuttingPartRoute> Route { get; set; }

    }
}
