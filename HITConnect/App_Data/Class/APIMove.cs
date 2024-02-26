using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class APIMove
    {
        [JsonProperty("InStation")]
        public string InStation { get; set; }

        [JsonProperty("OutStation")]
        public string OutStation { get; set; }

        [JsonProperty("BoxRfidList")]
        public List<string> BoxRfidList { get; set; }

        [JsonProperty("TimeStamp")]
        public DateTime TimeStamp { get; set; }

    }
}
