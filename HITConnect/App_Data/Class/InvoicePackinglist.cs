using Newtonsoft.Json;
using System.Collections.Generic;

namespace HITConnect
{
    public class InvoicePackinglist
    {
        [JsonProperty("Authen")]
        public UserAuthen authen { get; set; }

        [JsonProperty("invoice")]
        public List<Invoice> invoice { get; set; }
    }

    public class Invoice
    {
        [JsonProperty("invno")]
        public string invno { get; set; }

        [JsonProperty("invdate")]
        public string invdate { get; set; }

        [JsonProperty("PackRoll")]
        public List<POPackroll> poPackRoll { get; set; }
    }

    public class POPackroll
    {
        [JsonProperty("pono")]
        public string pono { get; set; }

        [JsonProperty("itemno")]
        public string itemno { get; set; }

        [JsonProperty("colorcode")]
        public string colorcode { get; set; }

        [JsonProperty("size")]
        public string size { get; set; }

        [JsonProperty("colorname")]
        public string colorname { get; set; }

        [JsonProperty("Roll")]
        public List<Roll> roll { get; set; }

    }

    public class Roll
    {
        [JsonProperty("rollno")]
        public string rollno { get; set; }

        [JsonProperty("width")]
        public string width { get; set; }

        [JsonProperty("actualwidth")]
        public string actualwidth { get; set; }

        [JsonProperty("actuallength")]
        public double actuallength { get; set; }

        [JsonProperty("actualweight")]
        public double actualweight { get; set; }

        [JsonProperty("lotno")]
        public string lotno { get; set; }

        [JsonProperty("barcode")]
        public string barcode { get; set; }

        [JsonProperty("clothno")]
        public string clothno { get; set; }

        [JsonProperty("stock")]
        public string stock { get; set; }

        [JsonProperty("packno")]
        public string packno { get; set; }

        [JsonProperty("packtype")]
        public string packtype { get; set; }

        [JsonProperty("ordertype")]
        public string ordertype { get; set; }

        [JsonProperty("ORDERNO")]
        public string ORDERNO { get; set; }

        [JsonProperty("ArticleNo")]
        public string ArticleNo { get; set; }

        [JsonProperty("Composition")]
        public string Composition { get; set; }

        [JsonProperty("StdWt")]
        public double StdWt { get; set; }

        [JsonProperty("NetWtKG")]
        public double NetWtKG { get; set; }

        [JsonProperty("GrossWtKG")]
        public double GrossWtKG { get; set; }

        [JsonProperty("Madein")]
        public string Madein { get; set; }

        [JsonProperty("Shipto")]
        public string Shipto { get; set; }

    }
}
