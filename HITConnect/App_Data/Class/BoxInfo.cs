using Newtonsoft.Json;
using System.Collections.Generic;

namespace HyperActive
{
    public class BoxInfo
    {
        [JsonProperty("FinishBoxRfid")]
        public string FinishBoxRfid { get; set; }

        [JsonProperty("FinishBoxBarcode")]
        public string FinishBoxBarcode { get; set; }

        [JsonProperty("FinishBoxIsEmpty")]
        public bool FinishBoxIsEmpty { get; set; }

        //[JsonProperty("BoxInfo")]
        //public List<BoxInfoDetail> BoxInfoDetail { get; set; }
    }
}
