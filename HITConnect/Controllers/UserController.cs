using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class UserController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "String2" };
        }

        [HttpGet]
        [Route("api/GetToken/")]
        public HttpResponseMessage GetToken(string idRq, string pwdRq)
        {
            string _Qry = "";
            int status = 0;
            string msgError = "";
            string token = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            _Qry += " SELECT V.Pwd AS pwd, V.VanderMailLogIn AS VanderMailLogIn " +
                " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V " +
                " WHERE V.VanderMailLogIn = '" + idRq + "'";
            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

            if (dt != null && dt.Rows.Count == 1)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (pwdRq == WSM.Conn.DB.FuncDecryptDataServer(row["pwd"].ToString()))
                    {
                        _Qry = "INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys (VanderMailLogIn, DataKey) ";
                        _Qry += " VALUES ('" + row["VanderMailLogIn"].ToString() + "', '" + row["pwd"].ToString() + "')";

                        if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                        {
                            token = row["pwd"].ToString();
                            status = 1;
                            msgError = "Successful";
                        }
                        else
                        {
                            _Qry = "UPDATE [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys SET DataKey = '";
                            _Qry += row["pwd"].ToString() + "' WHERE VanderMailLogIn = '" + idRq + "'";
                            if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                            {
                                token = row["pwd"].ToString();
                                status = 1;
                                msgError = "Successful";
                            }
                        }
                        //valid = (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER));
                        //token = valid ? row["pwd"].ToString() : "Already token";
                        //status = valid ? "1" : "2";
                        //msgError = valid ? "Successful" : "Not Successful!!!"; ;
                    }
                    else
                    {
                        token = "";
                        status = 2;
                        msgError = "Password is not correct!!!";
                    }
                }
            }
            else
            {
                token = "";
                status = 2;
                msgError = "Please check User and Password!!!";
            }
            dts.Rows.Add(new Object[] { status, token, msgError });
            jsondata = JsonConvert.SerializeObject(dts);
            //string jsondata = (valid) ? JsonConvert.SerializeObject(dts) : "NOT FOUND";
            return new HttpResponseMessage { Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
        }

        [HttpGet]
        [Route("api/CheckToken/")]
        public HttpResponseMessage CheckToken(string idRq, string pwdRq, string tokenRq, string vencodeRq)
        {
            string _Qry = "";
            string jsondata = "";
            string msgError = "";
            string token = "";
            int status = 0;
            int countdtPO = 0;
            DataTable dt = null;
            DataTable dtPO = new DataTable();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            _Qry += " SELECT V.Pwd, V.VanderMailLogIn, ATK.DataKey " +
                " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V  " +
                " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                " WHERE V.VanderMailLogIn = '" + idRq + "'";
            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

            if (dt != null)
            {
                // REMOVE TOKEN FROM AuthenKeys
                _Qry = "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys WHERE VanderMailLogIn = '" + idRq + "'";

                if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                {
                    _Qry = "";
                    /*  Fields Not Send JSON
                     * [FTDataKey],[FTDateCreate],
                     * ,[StateAcknowledge],[AcknowledgeBy]" +
                        ",[AcknowledgeDate],[AcknowledgeTime],[StateAcknowledgeLock],[StateClose],[T2_Confirm_Ship_Date],[T2_Confirm_Price]" +
                        ",[T2_Confirm_Quantity],[T2_Confirm_OrderNo],[T2_Confirm_PO_Date],[T2_Confirm_By],[Estimatedeldate],[Actualdeldate]" +
                        ",[InvoiceNo],[RcvQty],[RcvDate],[StateRead] 
                     * */

                    _Qry += "SELECT ISNULL(VenderCode, '') AS VenderCode ,ISNULL(VendorName, '') AS VendorName ,ISNULL(VendorLocation, '') AS VendorLocation ,ISNULL(FactoryCode, '') AS FactoryCode ,ISNULL(PONo, '') AS PONo ,ISNULL(PODate, '') AS PODate " +
                        " ,ISNULL(Shipto, '') AS Shipto ,ISNULL(GarmentShipmentDestination, '') AS GarmentShipmentDestination ,ISNULL(MatrClass, '') AS MatrClass ,ISNULL(ItemSeq, 0) AS ItemSeq ,ISNULL(POItemCode, '') AS POItemCode ,ISNULL(MatrCode, '') AS MatrCode ,ISNULL(UPCCOMBOIM, '') AS UPCCOMBOIM ,ISNULL(ContentCode, '') AS ContentCode " +
                        " ,ISNULL(CareCode, '') AS CareCode ,ISNULL(Color, '') AS Color ,ISNULL(ColorName, '') AS ColorName ,ISNULL(GCW, '') AS GCW ,ISNULL(Size, '') AS Size ,ISNULL(SizeMatrix, '') AS SizeMatrix ,ISNULL(Currency, '') AS Currency ,ISNULL(Price, 0) AS Price ,ISNULL(Quantity, 0) AS Quantity ,ISNULL(QtyUnit, '') AS QtyUnit ,ISNULL(DeliveryDate, '') AS DeliveryDate " +
                        " ,ISNULL(Season, '') AS Season ,ISNULL(Custporef, '') AS Custporef ,ISNULL(Buy, '') AS Buy ,ISNULL(BuyNo, '') AS BuyNo ,ISNULL(Category, '') AS Category ,ISNULL(Program, '') AS Program ,ISNULL(SubProgram, '') AS SubProgram ,ISNULL(StyleNo, '') AS StyleNo ,ISNULL(StyleName, '') AS StyleName ,ISNULL(POMatching1, '') AS POMatching1 " +
                        " ,ISNULL(POMatching2, '') AS POMatching2 ,ISNULL(POMatching3, '') AS POMatching3 ,ISNULL(POMatching4, '') AS POMatching4 ,ISNULL(POMatching5, '') AS POMatching5 ,ISNULL(ItemMatching1, '') AS ItemMatching1 ,ISNULL(ItemMatching2, '') AS ItemMatching2 ,ISNULL(ItemMatching3, '') AS ItemMatching3 " +
                        " ,ISNULL(ItemMatching4, '') AS ItemMatching4 ,ISNULL(ItemMatching5, '') AS ItemMatching5 ,ISNULL(Position, '') AS Position ,ISNULL(Type, '') AS Type ,ISNULL(PriceTerm, 0) AS PriceTerm ,ISNULL(PaymentTerm, '') AS PaymentTerm ,ISNULL(Remarkfrommer, '') AS Remarkfrommer ,ISNULL(RemarkForPurchase, '') AS RemarkForPurchase " +
                        " ,ISNULL(InvoiceCmpCode, '') AS InvoiceCmpCode ,ISNULL(CompanyName, '') AS CompanyName ,ISNULL(address1, '') AS address1 ,ISNULL(address2, '') AS address2 ,ISNULL(address3, '') AS address3 ,ISNULL(address4, '') AS address4 ,ISNULL(sysowner, '') AS sysowner ,ISNULL(ZeroInspection, '') AS ZeroInspection ,ISNULL(GarmentShip, '') AS GarmentShip " +
                        " ,ISNULL(OGACDate, '') AS OGACDate ,ISNULL(HITLink, '') AS HITLink ,ISNULL(NIKECustomerPONo, '') AS NIKECustomerPONo ,ISNULL(QRS, 0) AS QRS ,ISNULL(PromoQty, 0) AS PromoQty ,ISNULL(ActualQuantity, 0) AS ActualQuantity ,ISNULL(POUploadDate, '') AS POUploadDate ,ISNULL(POUploadTime, '') AS POUploadTime ,ISNULL(POUploadBy, '') AS POUploadBy " +
                        " ,ISNULL(CountryOfOrigin, '') CountryOfOrigin  ,ISNULL(SaleOrderSaleOrderLine, '') AS SaleOrderSaleOrderLine ,ISNULL(NikeSAPPOPONIKEPOLine, '') AS NikeSAPPOPONIKEPOLine ,ISNULL(NikematerialStyleColorway, '') AS NikematerialStyleColorway ,ISNULL(Modifire1, '') AS Modifire1 ,ISNULL(Modifire2, '') AS Modifire2 " +
                        " ,ISNULL(Modifire3, '') AS Modifire3 ,ISNULL(MPOHZ, '') AS MPOHZ ,ISNULL(ItemDescription, '') AS ItemDescription ,ISNULL(BulkQRSSample, '') AS BulkQRSSample ,ISNULL(ExtraMinQty, 0) AS ExtraMinQty ,ISNULL(EAGSystemUnitPriceERP3, 0) AS EAGSystemUnitPriceERP3 ,ISNULL(CCTotalpage, 0) AS CCTotalpage ,ISNULL(Surcharge, 0) AS Surcharge " +
                        " ,ISNULL(POApproveDate, '') AS POApproveDate ,ISNULL(POIssueDate, '') AS POIssueDate ,ISNULL(CLXorderconfirmationnumber, '') AS CLXorderconfirmationnumber ,ISNULL(FTYMerch, '') AS FTYMerch ,ISNULL(HKMerch, '') AS HKMerch ,ISNULL(ChinaInsertCard, '') AS ChinaInsertCard ,ISNULL(P1pc2in1, '') AS P1pc2in1 ,ISNULL(ReplyCode, '') AS ReplyCode " +
                        " ,ISNULL(WovenLabelSizeLength, '') AS WovenLabelSizeLength ,ISNULL(ArgentinaImportNumber, '') AS ArgentinaImportNumber ,ISNULL(DownFill, '') AS DownFill ,ISNULL(SYS_ID, '') AS SYS_ID ,ISNULL(ChinasizeMatrixtype, '') AS ChinasizeMatrixtype ,ISNULL(EAGERPItemNumber, '') AS EAGERPItemNumber " +
                        " ,ISNULL(CompoundColororCCIMfor2in1CCIM, '') AS CompoundColororCCIMfor2in1CCIM ,ISNULL(CPO, '') AS CPO ,ISNULL(BhasaIndonesiaProductBrand, '') AS BhasaIndonesiaProductBrand ,ISNULL(SAFCODE, '') AS SAFCODE ,ISNULL(Youthorder, '') AS Youthorder ,ISNULL(NFCproduct, '') AS NFCproduct ,ISNULL(NeckneckopeningX2, '') AS NeckneckopeningX2 " +
                        " ,ISNULL(ChestbodywidthX2, '') AS ChestbodywidthX2 ,ISNULL(CenterBackbodylength, '') AS CenterBackbodylength ,ISNULL(WaistwaistrelaxedInseam, '') AS WaistwaistrelaxedInseam ,ISNULL(PackQuantityQTYPERSELLINGUNIT, 0) AS PackQuantityQTYPERSELLINGUNIT ,ISNULL(Fabriccode, 0) AS Fabriccode " +
                        " ,ISNULL(PRODUCTDESCRIPTION, '') AS PRODUCTDESCRIPTION ,ISNULL(PRODUCTCODEDESCRIPTION, '') AS PRODUCTCODEDESCRIPTION ,ISNULL(GARMENTSTYLEFIELD, '') AS GARMENTSTYLEFIELD ,ISNULL(INSEAMSTYLE, '') AS INSEAMSTYLE ,ISNULL(StateFlag, '') AS StateFlag  " +
                        " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[POToVender] AS POV " +
                        " WHERE POV.vendercode = '" + vencodeRq + "'AND POV.StateAcknowledge = 0";
                    dtPO = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                    countdtPO = dtPO.Rows.Count;

                    if (dtPO != null)
                    {
                        // Remove Data from Table POToVender_ACK is StateAcknowledge in Table POToVender = 0
                        _Qry = "DECLARE @TotalRow int =0 " +
                            "DECLARE @TotalEff int = 0 " +
                            "DECLARE @TotalStamp int = 0 " +
                            "DECLARE @User nvarchar(50) = 'Admin' " +
                            "DECLARE @Vender nvarchar(50) = 'Admin' " +
                            "DECLARE @Date varchar(8) = Convert(varchar(10), Getdate(), 111) " +
                            "DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) " +
                            "DECLARE @Message nvarchar(500) = '' ";

                        _Qry += " BEGIN TRANSACTION ";
                        _Qry += " BEGIN TRY ";

                        _Qry += "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK " +
                            " WHERE FTDataKey IN (" +
                            " SELECT DISTINCT q.FTDataKey FROM POToVender q " +
                            " INNER JOIN POToVender_ACK u on (u.PONo = q.PONo AND u.VenderCode = q.VenderCode) " +
                            " WHERE q.StateAcknowledge = 0 )";
                        _Qry += " SELECT @TotalRow=@@ROWCOUNT ";


                        //if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                        //{
                        // COPY DATA POToVender TO POToVender_ACK
                        _Qry += "INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK (" +
                            "[FTDataKey],[FTDateCreate],[VenderCode],[VendorName],[VendorLocation],[FactoryCode],[PONo],[PODate]," +
                            "[Shipto],[GarmentShipmentDestination],[MatrClass],[ItemSeq],[POItemCode],[MatrCode],[UPCCOMBOIM]," +
                            "[ContentCode],[CareCode],[Color],[ColorName],[GCW],[Size],[SizeMatrix],[Currency],[Price],[Quantity]," +
                            "[QtyUnit],[DeliveryDate],[Season],[Custporef],[Buy],[BuyNo],[Category],[Program],[SubProgram],[StyleNo]," +
                            "[StyleName],[POMatching1],[POMatching2],[POMatching3],[POMatching4],[POMatching5],[ItemMatching1]," +
                            "[ItemMatching2],[ItemMatching3],[ItemMatching4],[ItemMatching5],[Position],[Type],[PriceTerm],[PaymentTerm]," +
                            "[Remarkfrommer],[RemarkForPurchase],[InvoiceCmpCode],[CompanyName],[address1],[address2],[address3]," +
                            "[address4],[sysowner],[ZeroInspection],[GarmentShip],[OGACDate],[HITLink],[NIKECustomerPONo],[QRS]," +
                            "[PromoQty],[ActualQuantity],[POUploadDate],[POUploadTime],[POUploadBy],[CountryOfOrigin]," +
                            "[SaleOrderSaleOrderLine],[NikeSAPPOPONIKEPOLine],[NikematerialStyleColorway],[Modifire1],[Modifire2]," +
                            "[Modifire3],[MPOHZ],[ItemDescription],[BulkQRSSample],[ExtraMinQty],[EAGSystemUnitPriceERP3],[CCTotalpage]," +
                            "[Surcharge],[POApproveDate],[POIssueDate],[CLXorderconfirmationnumber],[FTYMerch],[HKMerch]," +
                            "[ChinaInsertCard],[P1pc2in1],[ReplyCode],[WovenLabelSizeLength],[ArgentinaImportNumber],[DownFill],[SYS_ID]," +
                            "[ChinasizeMatrixtype],[EAGERPItemNumber],[CompoundColororCCIMfor2in1CCIM],[CPO],[BhasaIndonesiaProductBrand]," +
                            "[SAFCODE],[Youthorder],[NFCproduct],[NeckneckopeningX2],[ChestbodywidthX2],[CenterBackbodylength]," +
                            "[WaistwaistrelaxedInseam],[PackQuantityQTYPERSELLINGUNIT],[Fabriccode],[PRODUCTDESCRIPTION]," +
                            "[PRODUCTCODEDESCRIPTION],[GARMENTSTYLEFIELD],[INSEAMSTYLE]) ";
                        _Qry += " SELECT ISNULL(FTDataKey, '') AS FTDataKey, ISNULL(FTDateCreate, '') AS FTDateCreate, " +
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
                            "ISNULL(GARMENTSTYLEFIELD, '') AS GARMENTSTYLEFIELD, ISNULL(INSEAMSTYLE, '') AS INSEAMSTYLE " +
                            "FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender AS POV " +
                            "WHERE POV.vendercode = '" + vencodeRq + "' AND POV.StateAcknowledge = 0";

                        _Qry += " SELECT @TotalEff=@@ROWCOUNT ";


                        _Qry += " UPDATE[" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender  SET StateAcknowledge = 1";
                        _Qry += ", AcknowledgeBy = '" + idRq + "' , AcknowledgeDate = " + HITConnect.UFuncs.FormatDateDB +
                            ", AcknowledgeTime = " + HITConnect.UFuncs.FormatTimeDB + " ";
                        _Qry += " WHERE vendercode = '" + vencodeRq + "'AND StateAcknowledge = 0";
                        _Qry += " SELECT @TotalStamp=@@ROWCOUNT ";

                        //_Qry += " SELECT @TotalRow AS TotalRows, @TotalEff AS TotalEffect , @USER AS UserName, @Vender AS Vender , @DATE + @TIME AS DateTime,@Message AS Msg ";
                        _Qry += "IF (@TotalRow = @TotalEff) AND  (@TotalRow = @TotalStamp)";
                        _Qry += "   BEGIN ";
                        _Qry += "       COMMIT TRANSACTION ";
                        _Qry += "   END ";
                        _Qry += "ELSE";
                        _Qry += "   BEGIN ";
                        _Qry += "       set @Message = 'Total Row, Effect and Stamp not equal!!!' ";
                        _Qry += "       ROLLBACK TRANSACTION ";
                        _Qry += "   END ";
                        _Qry += " END TRY ";


                        _Qry += " BEGIN CATCH ";
                        _Qry += "   BEGIN ";
                        _Qry += "       ROLLBACK TRANSACTION ";
                        _Qry += "   END ";
                        _Qry += " END CATCH ";

                        if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                        {

                            token = "";
                            status = 1;
                            msgError = "Successful!!!";

                        }
                        else
                        {
                            token = "";
                            status = 2;
                            msgError = "Process Not Successful!!!";
                        }
                    }
                    else
                    {
                        token = "";
                        status = 1;
                        msgError = "Successful [No new PO]";
                    }
                }
                else
                {
                    token = "";
                    status = 2;
                    msgError = "Cannot Remove Token!!!";
                }
                //string jsondata = (valid) ? JsonConvert.SerializeObject(dtPO) : "NOT FOUND";
                //string jsondata = JsonConvert.SerializeObject(dtPO);
            }
            else
            {
                token = "";
                status = 2;
                msgError = "Token is invalid!!!";
            }

            dts.Rows.Add(new Object[] { status, token, msgError });
            if (status == 1)
            {
                jsondata = JsonConvert.SerializeObject(dtPO);
            }
            else
            {
                jsondata = JsonConvert.SerializeObject(dts);
            }
            return new HttpResponseMessage { Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }



        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
