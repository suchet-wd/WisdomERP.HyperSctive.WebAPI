using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class CuttingPartRoute
    {
        [JsonProperty("Station")]
        public string Station { get; set; }

        [JsonProperty("FactoryNo")]
        public string FactoryNo { get; set; }

    }
}
