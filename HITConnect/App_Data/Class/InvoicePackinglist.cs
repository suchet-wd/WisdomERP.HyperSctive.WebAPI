using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace HITConnect
{
    public class InvoicePackingList
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
        [JsonProperty("PONumber", Required = Required.Always)]
        public string pono { get; set; }

        [JsonProperty("ItemNo", Required = Required.Always)]
        public string itemno { get; set; }

        [JsonProperty("ColorCode", Required = Required.Always)]
        public string colorcode { get; set; }

        [JsonProperty("Size", Required = Required.Always)]
        public string size { get; set; }

        [JsonProperty("ColorName")]
        public string colorname { get; set; }

        [JsonProperty("Roll")]
        public List<Roll> roll { get; set; }

    }

    public class Roll
    {
        [JsonProperty("RollNo")]
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

        [JsonProperty("ClothNo")]
        public string clothno { get; set; }

        [JsonProperty("Stock")]
        public string stock { get; set; }

        [JsonProperty("MaterialType")]
        public string ordertype { get; set; }

        [JsonProperty("ORDERNO")]
        public string ORDERNO { get; set; }

        [JsonProperty("ArticleNo")]
        public string ArticleNo { get; set; }


        [JsonProperty("NetWeightKG")]
        public double NetWtKG { get; set; }

        [JsonProperty("GrossWeightKG")]
        public double GrossWtKG { get; set; }

        [JsonProperty("VendorCountryOfOrigin")]
        public string Madein { get; set; }

        [JsonProperty("Composition")]
        public string Composition { get; set; }
        
        [JsonProperty("StandardWeight")]
        public string StdWt { get; set; }
        
        [JsonProperty("ShipFrom")]
        public string Shipto { get; set; }

        [JsonProperty("POFront")]
        public string POJobFront { get; set; }

        [JsonProperty("POBack")]
        public string POJobBack { get; set; }

        [JsonProperty("POSL")]
        public string POJobSL { get; set; }

        [JsonProperty("POPant")]
        public string POJobPant { get; set; }

        /*[JsonProperty("packno")]
        public string packno { get; set; }

        [JsonProperty("packtype")]
        public string packtype { get; set; }*/
        /*
        [JsonProperty("FTDateCreate")]
        public string FTDateCreate { get; set; }

        [JsonProperty("FTStateClose")]
        public string FTStateClose { get; set; }

        [JsonProperty("FTStateCloseDate")]
        public string FTStateCloseDate { get; set; }

        [JsonProperty("FTStateCloseBy")]
        public string FTStateCloseBy { get; set; }

        [JsonProperty("FTVenderCode")]
        public string FTVenderCode { get; set; }
        */
    }
}
