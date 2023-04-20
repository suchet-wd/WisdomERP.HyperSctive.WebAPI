using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HITConnect
{
    public class Payment
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("pwd")]
        public string pwd { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("venderCode")]
        public string venderCode { get; set; }

        [JsonProperty("venderGroup")]
        public string venderGroup { get; set; }
        
        [JsonProperty("startDate")]
        public string startDate { get; set; }

        [JsonProperty("endDate")]
        public string endDate { get; set; }

        [JsonProperty("PI")]
        public string PI { get; set; }

        [JsonProperty("PO")]
        public string PO { get; set; }
    }
}
