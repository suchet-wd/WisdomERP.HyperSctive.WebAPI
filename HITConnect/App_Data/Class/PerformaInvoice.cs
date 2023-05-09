using Newtonsoft.Json;
using System.Collections.Generic;

namespace HITConnect
{
    public class PerformaInvoice
    {
        [JsonProperty("Authen")]
        public UserAuthen authen { get; set; }

        [JsonProperty("PI")]
        public List<PI> pi { get; set; }
    }

    public class PDF2PerformaInvoice
    {
        [JsonProperty("Authen")]
        public UserAuthen authen { get; set; }

        [JsonProperty("PINumber")]
        public string PINo { get; set; }

        [JsonProperty("pdfFile")]
        public string pdfFile { get; set; }
    }

    public class PI
    {
        [JsonProperty("PINumber")]
        public string FTDocNo { get; set; }

        [JsonProperty("PIDate")]
        public string FTDocDate { get; set; }

        [JsonProperty("PIConfirmDeliveryDate")]
        public string Estimatedeldate { get; set; }

        [JsonProperty("TotalPIQty")]
        public double FNPIQuantity { get; set; }

        [JsonProperty("TotalPIAmt")]
        public double FNPINetAmt { get; set; }

        [JsonProperty("ConfirmShipDate")]
        public string T2_Confirm_Ship_Date { get; set; }

        [JsonProperty("ActualDeliveryDate")]
        public string Actualdeldate { get; set; }

        [JsonProperty("ConfirmPrice")]
        public double T2_Confirm_Price { get; set; }

        [JsonProperty("ConfirmQuantity")]
        public double T2_Confirm_Quantity { get; set; }

        [JsonProperty("InvoiceNo")]
        public string InvoiceNo { get; set; }

        [JsonProperty("AWBNo")]
        public string FTAWBNo { get; set; }

        [JsonProperty("ConfirmBy")]
        public string T2_Confirm_By { get; set; }

        [JsonProperty("Remark")]
        public string T2_Confirm_Note { get; set; }

        [JsonProperty("PO")]
        public List<PO> po { get; set; }

        /// 

        //[JsonProperty("FTStateHasFile")]
        //public string FTStateHasFile { get; set; }

        [JsonProperty("FTFileRef")]
        public string FTFileRef { get; set; }

        
    }

    public class PO
    {
        [JsonProperty("PONumber")]
        public string PONo { get; set; }

        [JsonProperty("PODate")]
        public string T2_Confirm_PO_Date { get; set; }

        [JsonProperty("POItemCode")]
        public string POItemCode { get; set; }

        [JsonProperty("ColorCode")]
        public string Color { get; set; }


        // 

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("POQty")]
        public double FNPOQty { get; set; }

        [JsonProperty("Seq")]
        public int FNSeq { get; set; }

        [JsonProperty("DocType")]
        public int FNDocType { get; set; }

        [JsonProperty("ConfirmOrderNo")]
        public string T2_Confirm_OrderNo { get; set; }

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
