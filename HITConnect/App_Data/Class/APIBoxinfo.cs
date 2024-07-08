using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HyperActive
{
    public class APIBoxInfo         // For API 8 : Finish Box Status
    {
        [JsonProperty("BoxInfo", Required = Required.Always)]
        public List<BoxInfo> BoxRfidList { get; set; }

    }

    public class BoxInfo            // For API 8 : Finish Box Status
    {
        [JsonProperty("FinishBoxRfid")]
        public string FinishBoxRfid { get; set; }

        [JsonProperty("FinishBoxBarcode")]
        public string FinishBoxBarcode { get; set; }

        [JsonProperty("FinishBoxIsEmpty")]
        public bool FinishBoxIsEmpty { get; set; }

    }
}
