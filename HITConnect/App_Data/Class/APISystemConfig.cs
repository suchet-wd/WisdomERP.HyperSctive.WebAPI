using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyperActive
{
    public class APIResend
    {
        [JsonProperty("ApiNo", Required = Required.Always)] 
        public string ApiNo { get; set; }

        [JsonProperty("DocNo", Required = Required.Always)] 
        public string DocNo { get; set; }
    }

    public class APIGetLog
    {
        [JsonProperty("ApiNo")]
        public string ApiNo { get; set; }

        [JsonProperty("DocNo")]
        public string DocNo { get; set; }

        [JsonProperty("SendDateFrom")]
        public string SendDateFrom { get; set; }

        [JsonProperty("SendDateTo")]
        public string SendDateTo { get; set; }
    }

    public class APIGetPeriod
    {

        [JsonProperty("DateFrom")]
        public string DateFrom { get; set; }

        [JsonProperty("DateTo")]
        public string DateTo { get; set; }
    }
}
