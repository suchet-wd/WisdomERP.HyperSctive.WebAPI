using Newtonsoft.Json;
using System.Collections.Generic;

namespace HITConnect
{
    public class InvoicePackinglist
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("Invoice")]
        public List<Invoice> invoice { get; set; }
    }

    public class Invoice
    {
        [JsonProperty("InvoiceNo", Required = Required.Always)]
        public string invno { get; set; }

        [JsonProperty("InvoiceDate", Required = Required.Always)]
        public string invdate { get; set; }

        [JsonProperty("PackRoll")]
        public List<POPackroll> poPackRoll { get; set; }
    }

    public class POPackroll
    {
        [JsonProperty("PONumber")]
        public string pono { get; set; }

        [JsonProperty("ItemNo")]
        public string itemno { get; set; }

        [JsonProperty("ColorCode")]
        public string colorcode { get; set; }

        [JsonProperty("Size")]
        public string size { get; set; }

        [JsonProperty("ColorName")]
        public string colorname { get; set; }

        [JsonProperty("Roll")]
        public List<Roll> roll { get; set; }

    }

    public class Roll
    {
        [JsonProperty("Roll No")]
        public string rollno { get; set; }

        [JsonProperty("Width")]
        public string width { get; set; }

        [JsonProperty("ActualWidth")]
        public string actualwidth { get; set; }

        [JsonProperty("ActualLength")]
        public double actuallength { get; set; }

        [JsonProperty("ActualWeight")]
        public double actualweight { get; set; }

        [JsonProperty("LotNo")]
        public string lotno { get; set; }

        [JsonProperty("Barcode")]
        public string barcode { get; set; }

        [JsonProperty("Cloth No")]
        public string clothno { get; set; }

        [JsonProperty("Stock")]
        public string stock { get; set; }

        [JsonProperty("PackingNo")]
        public string packno { get; set; }

        [JsonProperty("PackingType")]
        public string packtype { get; set; }

        [JsonProperty("MaterialType")]
        public string ordertype { get; set; }

        [JsonProperty("ORDERNO")]
        public string ORDERNO { get; set; }

        [JsonProperty("ArticleNo")]
        public string ArticleNo { get; set; }

        [JsonProperty("Composition")]
        public string Composition { get; set; }

        [JsonProperty("StandardWeight")]
        public double StdWt { get; set; }

        [JsonProperty("NetWeightKG")]
        public double NetWtKG { get; set; }

        [JsonProperty("GrossWeightKG")]
        public double GrossWtKG { get; set; }

        [JsonProperty("VendorCountryOfOrigin")]
        public string Madein { get; set; }

        [JsonProperty("ShipFrom")]
        public string Shipto { get; set; }

        [JsonProperty("UploadDate")]
        public string UploadDate { get; set; }

    }
}
