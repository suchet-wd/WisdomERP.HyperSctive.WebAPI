using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HITConnect
{
    public class Payment
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("FromDate")]
        public string startDate { get; set; }

        [JsonProperty("ToDate")]
        public string endDate { get; set; }

        [JsonProperty("PINumber")]
        public string PI { get; set; }

        [JsonProperty("PONumber")]
        public string PO { get; set; }
    }
}
