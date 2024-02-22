using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class APICutting
    {
        [JsonProperty("PooNo")]
        public string PooNo { get; set; }

    }
}