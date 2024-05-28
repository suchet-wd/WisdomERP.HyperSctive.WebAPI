using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyperActive
{
    public class APIBoxInfo
    {
        //[JsonProperty("Authen", Required = Required.Always)]
        //public UserAuthen authen { get; set; }

        [JsonProperty("BoxInfo")]
        public List<BoxInfo> BoxRfidList { get; set; }

        //[JsonProperty("TimeStamp")]
        //public string TimeStamp { get; set; }

    }
}
