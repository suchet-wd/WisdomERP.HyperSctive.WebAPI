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

        [JsonProperty("PONumber")]
        public string PONo { get; set; }

        [JsonProperty("pdfFile")]
        public string pdfFile { get; set; }
    }

    public class PI
    {
        //
        //StateExport
        //StateExportDate
        //POPrice
        //T2_Confirm_Amount
        [JsonProperty("PINumber")]
        public string FTDocNo { get; set; }

        [JsonProperty("PIDate")]
        public string FTDocDate { get; set; }

        [JsonProperty("PIConfirmDeliveryDate")]
        public string Estimatedeldate { get; set; }

        [JsonProperty("ConfirmQuantity")]
        public double FNPIQuantity { get; set; }

        [JsonProperty("ConfirmMOQQuantity")]
        public double T2_Confirm_MOQQuantity { get; set; }

        [JsonProperty("TotalP/IQty")]
        public double TotalPIQty { get; set; }

        [JsonProperty("P/ISurChargeAmount")]
        public double Doc_Surcharge_Amount { get; set; }
        
        [JsonProperty("TotalP/IAmount")]
        public double TotalPIAmount { get; set; }

        [JsonProperty("InvoiceNo")]
        public string InvoiceNo { get; set; }

        [JsonProperty("AWBNo")]
        public string FTAWBNo { get; set; }

        [JsonProperty("ConfirmPrice")]
        public double ConfirmPrice { get; set; }
        
        [JsonProperty("ConfirmShipDate")]
        public string T2_Confirm_Ship_Date { get; set; }

        [JsonProperty("ConfirmShipQuantity")]
        public double T2_Confirm_ShipQuantity { get; set; }

        [JsonProperty("ActualDeliveryDate")]
        public string Actualdeldate { get; set; }

        [JsonProperty("ConfirmShipDate(2nd)")]
        public string T2_Confirm_Ship_Date2 { get; set; }

        [JsonProperty("ConfirmShipQuantity(2nd)")]
        public double T2_Confirm_ShipQuantity2 { get; set; }

        [JsonProperty("ActualDeliveryDate(2nd)")]
        public string Actualdeldate2 { get; set; }

        [JsonProperty("FNPINetAmt")]
        public double FNPINetAmt { get; set; }
        //[JsonProperty("ConfirmBy")]
        //public string T2_Confirm_By { get; set; }

        [JsonProperty("Memo")]
        public string T2_Confirm_Note { get; set; }

        //[JsonProperty("FTStateHasFile")]
        //public string FTStateHasFile { get; set; }

        [JsonProperty("FilePDF")]
        public string FTFileRef { get; set; }

        [JsonProperty("PO")]
        public List<PO> po { get; set; }

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

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("POQty")]
        public double FNPOQty { get; set; }

        [JsonProperty("Seq")]
        public int FNSeq { get; set; }

        [JsonProperty("DocType")]
        public int FNDocType { get; set; }

        //[JsonProperty("ConfirmOrderNo")]
        //public string T2_Confirm_OrderNo { get; set; }

        [JsonProperty("ConfirmPrice")]
        public double T2_Confirm_Price { get; set; }

        [JsonProperty("ConfirmQuantity")]
        public double T2_Confirm_Quantity { get; set; }

        //[JsonProperty("RcvQty")]
        //public int RcvQty { get; set; }

        //[JsonProperty("RcvDate")]
        //public string RcvDate { get; set; }

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
