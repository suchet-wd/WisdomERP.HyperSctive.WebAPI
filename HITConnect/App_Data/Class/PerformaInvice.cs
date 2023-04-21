using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HITConnect
{
    public class PerformaInvice
    {
        [JsonProperty("Authen")]
        public authen authen { get; set; }
        
        public List<PI> pi { get; set; }
    }

    public class authen
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("pwd")]
        public string pwd { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }

        [JsonProperty("venderCode")]
        public string venderCode { get; set; }

        [JsonProperty("venderGroup")]
        public string venderGroup { get; set; }
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

        [JsonProperty("VenderCode")]
        public string VenderCode { get; set; }

        [JsonProperty("VendorName")]
        public string VendorName { get; set; }

        [JsonProperty("VendorLocation")]
        public string VendorLocation { get; set; }

        [JsonProperty("FactoryCode")]
        public string FactoryCode { get; set; }

        [JsonProperty("Season")]
        public string Season { get; set; }

        [JsonProperty("Custporef")]
        public string Custporef { get; set; }

        [JsonProperty("Buy")]
        public string Buy { get; set; }

        [JsonProperty("BuyNo")]
        public string BuyNo { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("Program")]
        public string Program { get; set; }

        [JsonProperty("SubProgram")]
        public string SubProgram { get; set; }

        [JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [JsonProperty("StyleName")]
        public string StyleName { get; set; }

        [JsonProperty("PriceTerm")]
        public string PriceTerm { get; set; }

        [JsonProperty("PaymentTerm")]
        public string PaymentTerm { get; set; }

        [JsonProperty("Remarkfrommer")]
        public string Remarkfrommer { get; set; }

        [JsonProperty("RemarkForPurchase")]
        public string RemarkForPurchase { get; set; }

        [JsonProperty("InvoiceCmpCode")]
        public string InvoiceCmpCode { get; set; }

        [JsonProperty("CompanyName")]
        public string CompanyName { get; set; }

        [JsonProperty("address1")]
        public string address1 { get; set; }

        [JsonProperty("address2")]
        public string address2 { get; set; }

        [JsonProperty("address3")]
        public string address3 { get; set; }

        [JsonProperty("address4")]
        public string address4 { get; set; }

        [JsonProperty("sysowner")]
        public string sysowner { get; set; }

        [JsonProperty("POUploadDate")]
        public string POUploadDate { get; set; }

        [JsonProperty("POUploadTime")]
        public string POUploadTime { get; set; }

        [JsonProperty("POUploadBy")]
        public string POUploadBy { get; set; }

        [JsonProperty("CountryOfOrigin")]
        public string CountryOfOrigin { get; set; }
        
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

        [JsonProperty("PODate")]
        public string PODate { get; set; }

        [JsonProperty("Shipto")]
        public string Shipto { get; set; }

        [JsonProperty("GarmentShipmentDestination")]
        public string GarmentShipmentDestination { get; set; }

        [JsonProperty("MatrClass")]
        public string MatrClass { get; set; }

        [JsonProperty("ItemSeq")]
        public string ItemSeq { get; set; }

        [JsonProperty("MatrCode")]
        public string MatrCode { get; set; }

        [JsonProperty("UPCCOMBOIM")]
        public string UPCCOMBOIM { get; set; }

        [JsonProperty("ContentCode")]
        public string ContentCode { get; set; }

        [JsonProperty("CareCode")]
        public string CareCode { get; set; }

        [JsonProperty("ColorName")]
        public string ColorName { get; set; }

        [JsonProperty("GCW")]
        public string GCW { get; set; }

        [JsonProperty("SizeMatrix")]
        public string SizeMatrix { get; set; }

        [JsonProperty("Price")]
        public string Price { get; set; }

        [JsonProperty("Quantity")]
        public string Quantity { get; set; }

        [JsonProperty("QtyUnit")]
        public string QtyUnit { get; set; }

        [JsonProperty("DeliveryDate")]
        public string DeliveryDate { get; set; }

        [JsonProperty("POMatching1")]
        public string POMatching1 { get; set; }

        [JsonProperty("POMatching2")]
        public string POMatching2 { get; set; }

        [JsonProperty("POMatching3")]
        public string POMatching3 { get; set; }

        [JsonProperty("POMatching4")]
        public string POMatching4 { get; set; }

        [JsonProperty("POMatching5")]
        public string POMatching5 { get; set; }

        [JsonProperty("ItemMatching1")]
        public string ItemMatching1 { get; set; }

        [JsonProperty("ItemMatching2")]
        public string ItemMatching2 { get; set; }

        [JsonProperty("ItemMatching3")]
        public string ItemMatching3 { get; set; }

        [JsonProperty("ItemMatching4")]
        public string ItemMatching4 { get; set; }

        [JsonProperty("ItemMatching5")]
        public string ItemMatching5 { get; set; }

        [JsonProperty("Position")]
        public string Position { get; set; }

        [JsonProperty("ZeroInspection")]
        public string ZeroInspection { get; set; }

        [JsonProperty("GarmentShip")]
        public string GarmentShip { get; set; }

        [JsonProperty("OGACDate")]
        public string OGACDate { get; set; }

        [JsonProperty("HITLink")]
        public string HITLink { get; set; }

        [JsonProperty("NIKECustomerPONo")]
        public string NIKECustomerPONo { get; set; }

        [JsonProperty("QRS")]
        public string QRS { get; set; }

        [JsonProperty("PromoQty")]
        public string PromoQty { get; set; }

        [JsonProperty("ActualQuantity")]
        public string ActualQuantity { get; set; }

        [JsonProperty("SaleOrderSaleOrderLine")]
        public string SaleOrderSaleOrderLine { get; set; }

        [JsonProperty("NikeSAPPOPONIKEPOLine")]
        public string NikeSAPPOPONIKEPOLine { get; set; }

        [JsonProperty("NikematerialStyleColorway")]
        public string NikematerialStyleColorway { get; set; }
        
        [JsonProperty("Modifire1")]
        public string Modifire1 { get; set; }

        [JsonProperty("Modifire2")]
        public string Modifire2 { get; set; }

        [JsonProperty("Modifire3")]
        public string Modifire3 { get; set; }

        [JsonProperty("MPOHZ")]
        public string MPOHZ { get; set; }

        [JsonProperty("ItemDescription")]
        public string ItemDescription { get; set; }

        [JsonProperty("BulkQRSSample")]
        public string BulkQRSSample { get; set; }

        [JsonProperty("ExtraMinQty")]
        public string ExtraMinQty { get; set; }

        [JsonProperty("EAGSystemUnitPriceERP3")]
        public string EAGSystemUnitPriceERP3 { get; set; }

        [JsonProperty("CCTotalpage")]
        public string CCTotalpage { get; set; }

        [JsonProperty("Surcharge")]
        public string Surcharge { get; set; }

        [JsonProperty("POApproveDate")]
        public string POApproveDate { get; set; }

        [JsonProperty("POIssueDate")]
        public string POIssueDate { get; set; }

        [JsonProperty("CLXorderconfirmationnumber")]
        public string CLXorderconfirmationnumber { get; set; }

        [JsonProperty("FTYMerch")]
        public string FTYMerch { get; set; }


        [JsonProperty("HKMerch")]
        public string HKMerch { get; set; }

        [JsonProperty("ChinaInsertCard")]
        public string ChinaInsertCard { get; set; }

        [JsonProperty("P1pc2in1")]
        public string P1pc2in1 { get; set; }

        [JsonProperty("ReplyCode")]
        public string ReplyCode { get; set; }

        [JsonProperty("WovenLabelSizeLength")]
        public string WovenLabelSizeLength { get; set; }

        [JsonProperty("ArgentinaImportNumber")]
        public string ArgentinaImportNumber { get; set; }

        [JsonProperty("DownFill")]
        public string DownFill { get; set; }

        [JsonProperty("SYS_ID")]
        public string SYS_ID { get; set; }

        [JsonProperty("ChinasizeMatrixtype")]
        public string ChinasizeMatrixtype { get; set; }

        [JsonProperty("EAGERPItemNumber")]
        public string EAGERPItemNumber { get; set; }

        [JsonProperty("CompoundColororCCIMfor2in1CCIM")]
        public string CompoundColororCCIMfor2in1CCIM { get; set; }

        [JsonProperty("CPO")]
        public string CPO { get; set; }

        [JsonProperty("BhasaIndonesiaProductBrand")]
        public string BhasaIndonesiaProductBrand { get; set; }

        [JsonProperty("SAFCODE")]
        public string SAFCODE { get; set; }

        [JsonProperty("Youthorder")]
        public string Youthorder { get; set; }

        [JsonProperty("NFCproduct")]
        public string NFCproduct { get; set; }

        [JsonProperty("NeckneckopeningX2")]
        public string NeckneckopeningX2 { get; set; }

        [JsonProperty("ChestbodywidthX2")]
        public string ChestbodywidthX2 { get; set; }

        [JsonProperty("CenterBackbodylength")]
        public string CenterBackbodylength { get; set; }

        [JsonProperty("WaistwaistrelaxedInseam")]
        public string WaistwaistrelaxedInseam { get; set; }

        [JsonProperty("PackQuantityQTYPERSELLINGUNIT")]
        public string PackQuantityQTYPERSELLINGUNIT { get; set; }

        [JsonProperty("Fabriccode")]
        public string Fabriccode { get; set; }

        [JsonProperty("PRODUCTDESCRIPTION")]
        public string PRODUCTDESCRIPTION { get; set; }

        [JsonProperty("PRODUCTCODEDESCRIPTION")]
        public string PRODUCTCODEDESCRIPTION { get; set; }

        [JsonProperty("GARMENTSTYLEFIELD")]
        public string GARMENTSTYLEFIELD { get; set; }


        [JsonProperty("INSEAMSTYLE")]
        public string INSEAMSTYLE { get; set; }


        [JsonProperty("StateFlag")]
        public string StateFlag { get; set; }

        [JsonProperty("StateAcknowledge")]
        public string StateAcknowledge { get; set; }

        [JsonProperty("AcknowledgeBy")]
        public string AcknowledgeBy { get; set; }

        [JsonProperty("AcknowledgeDate")]
        public string AcknowledgeDate { get; set; }

        [JsonProperty("AcknowledgeTime")]
        public string AcknowledgeTime { get; set; }

        [JsonProperty("StateAcknowledgeLock")]
        public string StateAcknowledgeLock { get; set; }

        [JsonProperty("StateClose")]
        public string StateClose { get; set; }

        [JsonProperty("StateRead")]
        public string StateRead { get; set; }

    }
}
