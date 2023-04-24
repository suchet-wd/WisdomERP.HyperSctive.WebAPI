using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class PO2VController : ApiController
    {
        private string columnPO2V = " SELECT ISNULL(VenderCode, '') AS VenderCode ,ISNULL(VendorName, '') AS VendorName, " +
            "ISNULL(VendorLocation, '') AS VendorLocation ,ISNULL(FactoryCode, '') AS FactoryCode ,ISNULL(PONo, '') AS PONo, " +
            "ISNULL(PODate, '') AS PODate, ISNULL(Shipto, '') AS Shipto ,ISNULL(GarmentShipmentDestination, '') AS GarmentShipmentDestination, " +
            "ISNULL(MatrClass, '') AS MatrClass ,ISNULL(ItemSeq, 0) AS ItemSeq ,ISNULL(POItemCode, '') AS POItemCode , " +
            "ISNULL(MatrCode, '') AS MatrCode ,ISNULL(UPCCOMBOIM, '') AS UPCCOMBOIM ,ISNULL(ContentCode, '') AS ContentCode , " +
            "ISNULL(CareCode, '') AS CareCode ,ISNULL(Color, '') AS Color ,ISNULL(ColorName, '') AS ColorName ,ISNULL(GCW, '') AS GCW , " +
            "ISNULL(Size, '') AS Size ,ISNULL(SizeMatrix, '') AS SizeMatrix ,ISNULL(Currency, '') AS Currency ,ISNULL(Price, 0) AS Price , " +
            "ISNULL(Quantity, 0) AS Quantity ,ISNULL(QtyUnit, '') AS QtyUnit ,ISNULL(DeliveryDate, '') AS DeliveryDate, " +
            "ISNULL(Season, '') AS Season ,ISNULL(Custporef, '') AS Custporef ,ISNULL(Buy, '') AS Buy ,ISNULL(BuyNo, '') AS BuyNo , " +
            "ISNULL(Category, '') AS Category ,ISNULL(Program, '') AS Program ,ISNULL(SubProgram, '') AS SubProgram , " +
            "ISNULL(StyleNo, '') AS StyleNo ,ISNULL(StyleName, '') AS StyleName ,ISNULL(POMatching1, '') AS POMatching1, " +
            "ISNULL(POMatching2, '') AS POMatching2 ,ISNULL(POMatching3, '') AS POMatching3 ,ISNULL(POMatching4, '') AS POMatching4, " +
            "ISNULL(POMatching5, '') AS POMatching5 ,ISNULL(ItemMatching1, '') AS ItemMatching1, ISNULL(ItemMatching2, '') AS ItemMatching2 , " +
            "ISNULL(ItemMatching3, '') AS ItemMatching3, ISNULL(ItemMatching4, '') AS ItemMatching4 ,ISNULL(ItemMatching5, '') AS ItemMatching5 , " +
            "ISNULL(Position, '') AS Position ,ISNULL(Type, '') AS Type ,ISNULL(PriceTerm, 0) AS PriceTerm, " +
            "ISNULL(PaymentTerm, '') AS PaymentTerm ,ISNULL(Remarkfrommer, '') AS Remarkfrommer , " +
            "ISNULL(RemarkForPurchase, '') AS RemarkForPurchase, ISNULL(InvoiceCmpCode, '') AS InvoiceCmpCode , " +
            "ISNULL(CompanyName, '') AS CompanyName, ISNULL(address1, '') AS address1 ,ISNULL(address2, '') AS address2, " +
            "ISNULL(address3, '') AS address3 ,ISNULL(address4, '') AS address4 ,ISNULL(sysowner, '') AS sysowner , " +
            "ISNULL(ZeroInspection, '') AS ZeroInspection ,ISNULL(GarmentShip, '') AS GarmentShip, " +
            "ISNULL(OGACDate, '') AS OGACDate ,ISNULL(HITLink, '') AS HITLink, ISNULL(NIKECustomerPONo, '') AS NIKECustomerPONo , " +
            "ISNULL(QRS, 0) AS QRS ,ISNULL(PromoQty, 0) AS PromoQty ,ISNULL(ActualQuantity, 0) AS ActualQuantity , " +
            "ISNULL(POUploadDate, '') AS POUploadDate ,ISNULL(POUploadTime, '') AS POUploadTime , " +
            "ISNULL(POUploadBy, '') AS POUploadBy ,ISNULL(CountryOfOrigin, '') CountryOfOrigin  , " +
            "ISNULL(SaleOrderSaleOrderLine, '') AS SaleOrderSaleOrderLine, ISNULL(NikeSAPPOPONIKEPOLine, '') AS NikeSAPPOPONIKEPOLine , " +
            "ISNULL(NikematerialStyleColorway, '') AS NikematerialStyleColorway ,ISNULL(Modifire1, '') AS Modifire1 , " +
            "ISNULL(Modifire2, '') AS Modifire2, ISNULL(Modifire3, '') AS Modifire3 ,ISNULL(MPOHZ, '') AS MPOHZ , " +
            "ISNULL(ItemDescription, '') AS ItemDescription ,ISNULL(BulkQRSSample, '') AS BulkQRSSample , " +
            "ISNULL(ExtraMinQty, 0) AS ExtraMinQty ,ISNULL(EAGSystemUnitPriceERP3, 0) AS EAGSystemUnitPriceERP3 , " +
            "ISNULL(CCTotalpage, 0) AS CCTotalpage ,ISNULL(Surcharge, 0) AS Surcharge ,ISNULL(POApproveDate, '') AS POApproveDate , " +
            "ISNULL(POIssueDate, '') AS POIssueDate ,ISNULL(CLXorderconfirmationnumber, '') AS CLXorderconfirmationnumber , " +
            "ISNULL(FTYMerch, '') AS FTYMerch ,ISNULL(HKMerch, '') AS HKMerch ,ISNULL(ChinaInsertCard, '') AS ChinaInsertCard , " +
            "ISNULL(P1pc2in1, '') AS P1pc2in1 ,ISNULL(ReplyCode, '') AS ReplyCode, ISNULL(WovenLabelSizeLength, '') AS WovenLabelSizeLength , " +
            "ISNULL(ArgentinaImportNumber, '') AS ArgentinaImportNumber ,ISNULL(DownFill, '') AS DownFill ,ISNULL(SYS_ID, '') AS SYS_ID , " +
            "ISNULL(ChinasizeMatrixtype, '') AS ChinasizeMatrixtype ,ISNULL(EAGERPItemNumber, '') AS EAGERPItemNumber , " +
            "ISNULL(CompoundColororCCIMfor2in1CCIM, '') AS CompoundColororCCIMfor2in1CCIM ,ISNULL(CPO, '') AS CPO , " +
            "ISNULL(BhasaIndonesiaProductBrand, '') AS BhasaIndonesiaProductBrand ,ISNULL(SAFCODE, '') AS SAFCODE , " +
            "ISNULL(Youthorder, '') AS Youthorder ,ISNULL(NFCproduct, '') AS NFCproduct , " +
            "ISNULL(NeckneckopeningX2, '') AS NeckneckopeningX2, ISNULL(ChestbodywidthX2, '') AS ChestbodywidthX2 , " +
            "ISNULL(CenterBackbodylength, '') AS CenterBackbodylength ,ISNULL(WaistwaistrelaxedInseam, '') AS WaistwaistrelaxedInseam , " +
            "ISNULL(PackQuantityQTYPERSELLINGUNIT, 0) AS PackQuantityQTYPERSELLINGUNIT ,ISNULL(Fabriccode, 0) AS Fabriccode , " +
            "ISNULL(PRODUCTDESCRIPTION, '') AS PRODUCTDESCRIPTION ,ISNULL(PRODUCTCODEDESCRIPTION, '') AS PRODUCTCODEDESCRIPTION , " +
            "ISNULL(GARMENTSTYLEFIELD, '') AS GARMENTSTYLEFIELD ,ISNULL(INSEAMSTYLE, '') AS INSEAMSTYLE , " +
            "ISNULL(StateFlag, '') AS StateFlag  ";

        //,[StateAcknowledge],[AcknowledgeBy],[AcknowledgeDate],[AcknowledgeTime],[StateAcknowledgeLock],[StateClose]
        //,[T2_Confirm_Ship_Date],[T2_Confirm_Price],[T2_Confirm_Quantity],[T2_Confirm_OrderNo],[T2_Confirm_PO_Date]
        //,[T2_Confirm_By],[Estimatedeldate],[Actualdeldate],[InvoiceNo],[RcvQty],[RcvDate],[StateRead],[FTStateHasFile]
        //,[FTFileRef],[FNPIQuantity],[FNPINetAmt],[FTAWBNo]
        //" , ISNULL(DATALENGTH(FTFileRef), -1) AS FTFileRef ";
        //SELECT FTFileRef from[DB_VENDER].dbo.POPayment FOR XML PATH(''), BINARY BASE64


        [HttpPost]
        [Route("api/SetPOtoVender/")]
        public HttpResponseMessage SetPOtoVender([FromBody] UserAuthen value)
        {
            string _Qry = "";
            string jsondata = "";
            string msgError = "";
            int statecheck = 0;
            DataTable dt = null;
            DataTable dts = new DataTable();
            DataTable dtPO = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            if (value.id == "")
            {
                statecheck = 2;
                msgError = "Please check ID!!!";
            }
            else
            {
                if (value.pwd == "")
                {
                    statecheck = 2;
                    msgError = "Please check password!!!";
                }
                else
                {
                    if (value.venderCode == "")
                    {
                        statecheck = 2;
                        msgError = "Please check vender code!!!";
                    }
                }
            }
            try
            {
                if (statecheck != 2)
                {
                    dt = HITConnect.UserAuthen.GetDTUserValidate(Cnn, value);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //if (dtPO != null)
                        //{

                        /*  Fields Not Send JSON  [FTDataKey],[FTDateCreate],  */

                        _Qry = columnPO2V + " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[POToVender] AS POV " +
                            " WHERE POV.vendercode = '" + value.venderCode + "'AND POV.StateAcknowledge = 0";
                        dtPO = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                        if (dtPO != null)
                        {
                            // Remove Data from Table POToVender_ACK is StateAcknowledge in Table POToVender = 0
                            _Qry = "  DECLARE @TotalRow int = 0 ";
                            _Qry += " DECLARE @TotalEff int = 0 ";
                            _Qry += " DECLARE @TotalStamp int = 0 ";
                            _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) ";
                            _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) ";
                            _Qry += " DECLARE @Message nvarchar(500) = '' ";

                            _Qry += " BEGIN TRANSACTION ";
                            _Qry += " BEGIN TRY ";

                            _Qry += "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK " +
                                " WHERE FTDataKey IN (" +
                                " SELECT DISTINCT q.FTDataKey FROM POToVender q " +
                                " INNER JOIN POToVender_ACK u on (u.PONo = q.PONo AND u.VenderCode = q.VenderCode) " +
                                " WHERE q.StateAcknowledge = 0 )";
                            _Qry += " SELECT @TotalRow=@@ROWCOUNT ";


                            // COPY DATA POToVender TO POToVender_ACK
                            _Qry += "INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK ( " +
                                " [FTDataKey],[FTDateCreate],[VenderCode],[VendorName],[VendorLocation],[FactoryCode],[PONo],[PODate], " +
                                " [Shipto],[GarmentShipmentDestination],[MatrClass],[ItemSeq],[POItemCode],[MatrCode],[UPCCOMBOIM], " +
                                " [ContentCode],[CareCode],[Color],[ColorName],[GCW],[Size],[SizeMatrix],[Currency],[Price],[Quantity], " +
                                " [QtyUnit],[DeliveryDate],[Season],[Custporef],[Buy],[BuyNo],[Category],[Program],[SubProgram],[StyleNo], " +
                                " [StyleName],[POMatching1],[POMatching2],[POMatching3],[POMatching4],[POMatching5],[ItemMatching1], " +
                                " [ItemMatching2],[ItemMatching3],[ItemMatching4],[ItemMatching5],[Position],[Type],[PriceTerm],[PaymentTerm], " +
                                " [Remarkfrommer],[RemarkForPurchase],[InvoiceCmpCode],[CompanyName],[address1],[address2],[address3], " +
                                " [address4],[sysowner],[ZeroInspection],[GarmentShip],[OGACDate],[HITLink],[NIKECustomerPONo],[QRS], " +
                                " [PromoQty],[ActualQuantity],[POUploadDate],[POUploadTime],[POUploadBy],[CountryOfOrigin], " +
                                " [SaleOrderSaleOrderLine],[NikeSAPPOPONIKEPOLine],[NikematerialStyleColorway],[Modifire1],[Modifire2], " +
                                " [Modifire3],[MPOHZ],[ItemDescription],[BulkQRSSample],[ExtraMinQty],[EAGSystemUnitPriceERP3],[CCTotalpage], " +
                                " [Surcharge],[POApproveDate],[POIssueDate],[CLXorderconfirmationnumber],[FTYMerch],[HKMerch], " +
                                " [ChinaInsertCard],[P1pc2in1],[ReplyCode],[WovenLabelSizeLength],[ArgentinaImportNumber],[DownFill],[SYS_ID], " +
                                " [ChinasizeMatrixtype],[EAGERPItemNumber],[CompoundColororCCIMfor2in1CCIM],[CPO],[BhasaIndonesiaProductBrand], " +
                                " [SAFCODE],[Youthorder],[NFCproduct],[NeckneckopeningX2],[ChestbodywidthX2],[CenterBackbodylength], " +
                                " [WaistwaistrelaxedInseam],[PackQuantityQTYPERSELLINGUNIT],[Fabriccode],[PRODUCTDESCRIPTION], " +
                                " [PRODUCTCODEDESCRIPTION],[GARMENTSTYLEFIELD],[INSEAMSTYLE], " +

                                " [StateAcknowledge],[AcknowledgeBy],[AcknowledgeDate],[AcknowledgeTime], " +
                                " [StateAcknowledgeLock],[StateClose],[T2_Confirm_Price],[T2_Confirm_Quantity],[T2_Confirm_OrderNo], " +
                                " [T2_Confirm_PO_Date],[T2_Confirm_By],[Estimatedeldate],[Actualdeldate],[InvoiceNo],[RcvQty],[RcvDate],[StateRead] ) ";
                            /*_Qry += " SELECT ISNULL(FTDataKey, '') AS FTDataKey, ISNULL(FTDateCreate, '') AS FTDateCreate, " +
                                "ISNULL(VenderCode, ''), ISNULL(VendorName, '') AS VendorName, ISNULL(VendorLocation, '') AS VendorLocation, " +
                                "ISNULL(FactoryCode, '') AS FactoryCode, ISNULL(PONo, '') AS PONo, ISNULL(PODate, '') AS PODate, " +
                                "ISNULL(Shipto, '') AS Shipto, ISNULL(GarmentShipmentDestination, '') AS GarmentShipmentDestination," +
                                "ISNULL(MatrClass, '') AS MatrClass, ISNULL(ItemSeq, 0) AS ItemSeq,ISNULL(POItemCode, '') AS POItemCode," +
                                "ISNULL(MatrCode, '') AS MatrCode,ISNULL(UPCCOMBOIM, '') AS UPCCOMBOIM,ISNULL(ContentCode, '') AS ContentCode," +
                                "ISNULL(CareCode, '') AS CareCode, ISNULL(Color, '') AS Color, ISNULL(ColorName, '') AS ColorName, " +
                                "ISNULL(GCW, '') AS GCW, ISNULL(Size, '') AS Size, ISNULL(SizeMatrix, '') AS SizeMatrix, " +
                                "ISNULL(Currency, '') AS Currency, ISNULL(Price, 0) AS Price, ISNULL(Quantity, 0) AS Quantity," +
                                "ISNULL(QtyUnit, '') AS QtyUnit, ISNULL(DeliveryDate, '') AS DeliveryDate, ISNULL(Season, '') AS Season, " +
                                "ISNULL(Custporef, ''),ISNULL(Buy, ''),ISNULL(BuyNo, ''), ISNULL(Category, '') AS Category, " +
                                "ISNULL(Program, '') AS Program, ISNULL(SubProgram, '') AS SubProgram, ISNULL(StyleNo, '')," +
                                "ISNULL(StyleName, ''),ISNULL(POMatching1, ''), ISNULL(POMatching2, ''),ISNULL(POMatching3, '') AS POMatching3, " +
                                "ISNULL(POMatching4, ''),ISNULL(POMatching5, '') AS POMatching5, ISNULL(ItemMatching1, '') AS ItemMatching1, " +
                                "ISNULL(ItemMatching2, '') AS ItemMatching2,ISNULL(ItemMatching3, '') AS ItemMatching3, " +
                                "ISNULL(ItemMatching4, '') AS ItemMatching4, ISNULL(ItemMatching5, '') AS ItemMatching5, " +
                                "ISNULL(Position, '') AS Position, ISNULL(Type, '') AS Type, ISNULL(PriceTerm, ''),ISNULL(PaymentTerm, ''), " +
                                "ISNULL(Remarkfrommer, '') AS Remarkfrommer, ISNULL(RemarkForPurchase, '') AS RemarkForPurchase," +
                                "ISNULL(InvoiceCmpCode, '') AS InvoiceCmpCode, ISNULL(CompanyName, '') AS CompanyName, " +
                                "ISNULL(address1, '') AS address1, ISNULL(address2, '') AS address2, ISNULL(address3, '') AS address3, " +
                                "ISNULL(address4, '') AS address4, ISNULL(sysowner, '') AS sysowner, ISNULL(ZeroInspection, '') AS ZeroInspection, " +
                                "ISNULL(GarmentShip, '') AS GarmentShip, ISNULL(OGACDate, '') AS OGACDate, ISNULL(HITLink, '') AS HITLink, " +
                                "ISNULL(NIKECustomerPONo, '') AS NIKECustomerPONo, ISNULL(QRS, 0) AS QRS, ISNULL(PromoQty, 0) AS PromoQty, " +
                                "ISNULL(ActualQuantity, 0) AS ActualQuantity, ISNULL(POUploadDate, '') AS POUploadDate, " +
                                "ISNULL(POUploadTime, '') AS POUploadTime, ISNULL(POUploadBy, '') AS POUploadBy, " +
                                "ISNULL(CountryOfOrigin, '') AS CountryOfOrigin, ISNULL(SaleOrderSaleOrderLine, '') AS SaleOrderSaleOrderLine," +
                                "ISNULL(NikeSAPPOPONIKEPOLine, '') AS NikeSAPPOPONIKEPOLine, " +
                                "ISNULL(NikematerialStyleColorway, '') AS NikematerialStyleColorway, ISNULL(Modifire1, '') AS Modifire1, " +
                                "ISNULL(Modifire2, '') AS Modifire2, ISNULL(Modifire3, '') AS Modifire3, ISNULL(MPOHZ, '') AS MPOHZ, " +
                                "ISNULL(ItemDescription, '') AS ItemDescription, ISNULL(BulkQRSSample, '') AS BulkQRSSample, " +
                                "ISNULL(ExtraMinQty, 0) AS ExtraMinQty, ISNULL(EAGSystemUnitPriceERP3, 0) AS EAGSystemUnitPriceERP3, " +
                                "ISNULL(CCTotalpage, 0) AS CCTotalpage, ISNULL(Surcharge, 0) AS Surcharge, ISNULL(POApproveDate, '') AS POApproveDate," +
                                "ISNULL(POIssueDate, '') AS POIssueDate,ISNULL(CLXorderconfirmationnumber, '') AS CLXorderconfirmationnumber, " +
                                "ISNULL(FTYMerch, '') AS FTYMerch,ISNULL(HKMerch, '') AS HKMerch, ISNULL(ChinaInsertCard, '') AS ChinaInsertCard," +
                                "ISNULL(P1pc2in1, '') AS P1pc2in1,ISNULL(ReplyCode, '') AS ReplyCode, " +
                                "ISNULL(WovenLabelSizeLength, '') AS WovenLabelSizeLength, ISNULL(ArgentinaImportNumber, '') AS ArgentinaImportNumber, " +
                                "ISNULL(DownFill, '') AS DownFill, ISNULL(SYS_ID, '') AS SYS_ID, ISNULL(ChinasizeMatrixtype, '') AS ChinasizeMatrixtype, " +
                                "ISNULL(EAGERPItemNumber, '') AS EAGERPItemNumber,ISNULL(CompoundColororCCIMfor2in1CCIM, '') AS CompoundColororCCIMfor2in1CCIM," +
                                "ISNULL(CPO, '') AS CPO,ISNULL(BhasaIndonesiaProductBrand, '') AS BhasaIndonesiaProductBrand, ISNULL(SAFCODE, '') AS SAFCODE, " +
                                "ISNULL(Youthorder, '') AS Youthorder, ISNULL(NFCproduct, '') AS NFCproduct, ISNULL(NeckneckopeningX2, '') AS NeckneckopeningX2, " +
                                "ISNULL(ChestbodywidthX2, '') AS ChestbodywidthX2, ISNULL(CenterBackbodylength, '') AS CenterBackbodylength, " +
                                "ISNULL(WaistwaistrelaxedInseam, '') AS WaistwaistrelaxedInseam, " +
                                "ISNULL(PackQuantityQTYPERSELLINGUNIT, 0) AS PackQuantityQTYPERSELLINGUNIT, ISNULL(Fabriccode, '') AS Fabriccode, " +
                                "ISNULL(PRODUCTDESCRIPTION, '') AS PRODUCTDESCRIPTION, ISNULL(PRODUCTCODEDESCRIPTION, '') AS PRODUCTCODEDESCRIPTION," +
                                "ISNULL(GARMENTSTYLEFIELD, '') AS GARMENTSTYLEFIELD, ISNULL(INSEAMSTYLE, '') AS INSEAMSTYLE, " +
                                // Update Stamp in POToVender_ACK
                                "1 AS StateAcknowledge, '" + value.id + "' AS AcknowledgeBy, @DATE AS AcknowledgeDate, @TIME AS AcknowledgeTime, " +
                                "ISNULL(StateAcknowledgeLock, '') AS StateAcknowledgeLock, ISNULL(StateClose, '') AS StateClose, " +
                                "ISNULL( T2_Confirm_Price, 0 ) AS T2_Confirm_Price, ISNULL(T2_Confirm_Quantity, 0) AS T2_Confirm_Quantity, " +
                                "ISNULL(T2_Confirm_OrderNo, '') AS T2_Confirm_OrderNo, ISNULL(T2_Confirm_PO_Date, '') AS T2_Confirm_PO_Date, " +
                                "ISNULL(T2_Confirm_By, '') AS T2_Confirm_By, ISNULL(Estimatedeldate, '') AS Estimatedeldate, " +
                                "ISNULL(Actualdeldate, '') AS Actualdeldate, ISNULL(InvoiceNo, '') AS InvoiceNo, ISNULL(RcvQty, 0) AS RcvQty, " +
                                "ISNULL(RcvDate, '') AS RcvDate, ISNULL(StateRead, '') AS StateRead " +
                                "FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender AS POV " +
                                "WHERE POV.vendercode = '" + value.venderCode + "' AND POV.StateAcknowledge = 0";
                            */
                            _Qry += columnPO2V + ", 1 AS StateAcknowledge, '" + value.id + "' AS AcknowledgeBy, " +
                                " @DATE AS AcknowledgeDate, @TIME AS AcknowledgeTime, " +
                                " ISNULL(StateAcknowledgeLock, '') AS StateAcknowledgeLock, ISNULL(StateClose, '') AS StateClose, " +
                                " ISNULL( T2_Confirm_Price, 0 ) AS T2_Confirm_Price, ISNULL(T2_Confirm_Quantity, 0) AS T2_Confirm_Quantity, " +
                                " ISNULL(T2_Confirm_OrderNo, '') AS T2_Confirm_OrderNo, ISNULL(T2_Confirm_PO_Date, '') AS T2_Confirm_PO_Date, " +
                                " ISNULL(T2_Confirm_By, '') AS T2_Confirm_By, ISNULL(Estimatedeldate, '') AS Estimatedeldate, " +
                                " ISNULL(Actualdeldate, '') AS Actualdeldate, ISNULL(InvoiceNo, '') AS InvoiceNo, ISNULL(RcvQty, 0) AS RcvQty, " +
                                " ISNULL(RcvDate, '') AS RcvDate, ISNULL(StateRead, '') AS StateRead ";
                            _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender AS POV ";
                            _Qry += " WHERE POV.vendercode = '" + value.venderCode + "' AND POV.StateAcknowledge = 0 ";

                            _Qry += " SELECT @TotalEff=@@ROWCOUNT ";


                            _Qry += " UPDATE[" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender SET StateAcknowledge = 1, ";
                            _Qry += " AcknowledgeBy = '" + value.id + "' , AcknowledgeDate = @DATE, AcknowledgeTime = @TIME ";
                            _Qry += " WHERE vendercode = '" + value.venderCode + "' AND StateAcknowledge = 0 ";


                            _Qry += " SELECT @TotalStamp=@@ROWCOUNT ";

                            _Qry += " IF (@TotalEff = @TotalStamp) ";
                            _Qry += "   BEGIN ";
                            _Qry += "       COMMIT TRANSACTION ";
                            _Qry += "   END ";
                            _Qry += " ELSE ";
                            _Qry += "   BEGIN ";
                            _Qry += "       set @Message = 'Total Row, Effect and Stamp not equal!!!' ";
                            _Qry += "       ROLLBACK TRANSACTION ";
                            _Qry += "       RAISERROR('Total Row, Effect and Stamp not equal!!!.',16,1) ";
                            _Qry += "   END ";
                            _Qry += " END TRY ";


                            _Qry += " BEGIN CATCH ";
                            _Qry += "   BEGIN ";
                            _Qry += "       ROLLBACK TRANSACTION ";
                            _Qry += "   END ";
                            _Qry += " END CATCH ";

                            if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                            {
                                statecheck = 1;
                                msgError = "Successful!!!";
                            }
                            else
                            {
                                statecheck = 2;
                                msgError = "Process Not Successful!!!";
                            }
                        }
                        else
                        {
                            statecheck = 2;
                            msgError = "Please check connection!!!";
                        }
                        //}
                        //else
                        //{
                        //    statecheck = 2;
                        //    msgError = "Token / Vender Group is invalid!!!";
                        //}
                    }
                    else
                    {
                        statecheck = 2;
                        msgError = "Please check User authentication!!!";
                        UserAuthen.DelAuthenKey(Cnn, value.id);
                    }
                }
                else
                {
                    UserAuthen.DelAuthenKey(Cnn, value.id);
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + statecheck +
                    (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + ex.Message + (char)34 + "}",
                    System.Text.Encoding.UTF8, "application/json")
                };
            }

            dts.Rows.Add(new Object[] { statecheck, msgError });
            if (statecheck == 1)
            {
                jsondata = JsonConvert.SerializeObject(dtPO);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + statecheck +
                    (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + msgError + (char)34 + "}",
                    System.Text.Encoding.UTF8, "application/json")
                };
            }
        }

        [HttpPost]
        [Route("api/GetPO2VenACK/")]
        public HttpResponseMessage GetPO2VenACK(POtoVender value)
        {
            string _Qry = "";
            int statecheck = 0;
            string msgError = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            try
            {
                dt = HITConnect.UserAuthen.GetDTUserValidate(Cnn, value.authen);
                if (dt != null && dt.Rows.Count > 0)
                {
                    _Qry = columnPO2V + " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK ";

                    if (value.startDate != "" && value.endDate != "")
                    {
                        if (Convert.ToDateTime(value.startDate) <= Convert.ToDateTime(value.endDate))
                        {
                            _Qry += " WHERE PODate BETWEEN '" + value.startDate + "' AND '" + value.endDate + "' ";
                            /*if (getRq.PI != null && getRq.PO == null)
                            {
                                _Qry += " AND PINo = '" + getRq.PI + "' ";
                            }*/
                            if (value.PONo != "") //getRq.PI == null &&
                            {
                                _Qry += " AND PONo = '" + value.PONo + "'";
                            }
                        }
                        else
                        {
                            statecheck = 2;
                            msgError = "Start Date must be lower than End Date";
                        }

                    }
                    else if (value.startDate == "" || value.endDate == "")
                    {
                        /*if (getRq.PI != null && getRq.PONo == null)
                        {
                            _Qry += " WHERE PINo = '" + getRq.PI + "' ";
                        }*/

                        /*if (getRq.PONo != null && getRq.PI != null)
                        {
                            _Qry += " WHERE PONo = '" + getRq.PONo + "'  OR PINo = '" + getRq.PI + "' ";
                        }*/
                        if (value.PONo != "")// && getRq.PI == null)
                        {
                            _Qry += " WHERE PONo = '" + value.PONo + "'";
                        }
                        else
                        {
                            statecheck = 2;
                            msgError = "Please check start date and end date!!!";
                        }
                    }

                    if (statecheck == 0)
                    {
                        dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                        statecheck = 1;
                        msgError = "Successful";
                    }
                    /*
                    if (statecheck != 2)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (Convert.ToBase64String((byte[])row["FTFileRef"]) != "-1")
                            {
                                row["FTFileRef"] = Convert.ToBase64String((byte[])row["FTFileRef"]);
                            }
                            else
                            {
                                row["FTFileRef"] = "";
                            }
                        }
                        dts = dt;
                    }
                    else
                    {
                        dts.Rows.Add(new Object[] { statecheck, msgError });
                    }*/
                }
                else
                {
                    statecheck = 2;
                    msgError = "Please check User authentication!!!";
                }
                //jsondata = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (statecheck == 1)
            {
                jsondata = JsonConvert.SerializeObject(dt);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                //jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + statecheck +
                    (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + msgError + (char)34 + "}",
                    System.Text.Encoding.UTF8, "application/json")
                };
            }
        }
    }
}