using Newtonsoft.Json;
using System.Collections.Generic;

namespace HITConnect
{
    public class PerformaInvoice
    {
        [JsonProperty("Authen")]
        public UserAuthen authen { get; set; }
        
        public List<PI> pi { get; set; }
    }

    public class PDF2PerformaInvoice
    {
        [JsonProperty("Authen")]
        public UserAuthen authen { get; set; }

        [JsonProperty("PINo")]
        public string PINo { get; set; }

        [JsonProperty("pdfFile")]
        public string pdfFile { get; set; }
    }

    public class PI
    {
        [JsonProperty("FTDocNo")]
        public string FTDocNo { get; set; }

        [JsonProperty("FTDocDate")]
        public string FTDocDate { get; set; }

        [JsonProperty("FTStateHasFile")]
        public string FTStateHasFile { get; set; }

        [JsonProperty("FTFileRef")]
        public string FTFileRef { get; set; }

        [JsonProperty("InvoiceNo")]
        public string InvoiceNo { get; set; }

        [JsonProperty("FNPIQuantity")]
        public double FNPIQuantity { get; set; }

        [JsonProperty("FNPINetAmt")]
        public double FNPINetAmt { get; set; }

        [JsonProperty("FTAWBNo")]
        public string FTAWBNo { get; set; }

        [JsonProperty("PO")]
        public List<PO> po { get; set; }
    }

    public class PO
    {
        [JsonProperty("PONo")]
        public string PONo { get; set; }

        [JsonProperty("POItemCode")]
        public string POItemCode { get; set; }

        [JsonProperty("Color")]
        public string Color { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("FNPOQty")]
        public double FNPOQty { get; set; }

        [JsonProperty("FNSeq")]
        public int FNSeq { get; set; }

        [JsonProperty("FNDocType")]
        public int FNDocType { get; set; }

        [JsonProperty("T2_Confirm_Ship_Date")]
        public string T2_Confirm_Ship_Date { get; set; }

        [JsonProperty("T2_Confirm_Price")]
        public double T2_Confirm_Price { get; set; }

        [JsonProperty("T2_Confirm_Quantity")]
        public double T2_Confirm_Quantity { get; set; }

        [JsonProperty("T2_Confirm_OrderNo")]
        public string T2_Confirm_OrderNo { get; set; }

        [JsonProperty("T2_Confirm_PO_Date")]
        public string T2_Confirm_PO_Date { get; set; }

        [JsonProperty("T2_Confirm_By")]
        public string T2_Confirm_By { get; set; }

        [JsonProperty("T2_Confirm_Note")]
        public string T2_Confirm_Note { get; set; }

        [JsonProperty("Estimatedeldate")]
        public string Estimatedeldate { get; set; }

        [JsonProperty("Actualdeldate")]
        public string Actualdeldate { get; set; }

        [JsonProperty("RcvQty")]
        public int RcvQty { get; set; }

        [JsonProperty("RcvDate")]
        public string RcvDate { get; set; }

        /* For Import Main System
        [JsonProperty("StateRead")]
        public string StateRead { get; set; }

        [JsonProperty("ReadDate")]
        public string ReadDate { get; set; }

        [JsonProperty("ReadTime")]
        public string ReadTime { get; set; }
        */
    
    }
}
