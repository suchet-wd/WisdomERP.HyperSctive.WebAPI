using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class PurchaseController : ApiController
    {
        private string columnPO2V = " SELECT ISNULL(VenderCode, '') AS Vendor Code ,ISNULL(VendorName, '') AS Vendor Name, " +
            "ISNULL(VendorLocation, '') AS Vendor Location ,ISNULL(FactoryCode, '') AS Factory Code ,ISNULL(PONo, '') AS PO Number, " +
            "ISNULL(PODate, '') AS PO Date, ISNULL(Shipto, '') AS Shipto ,ISNULL(GarmentShipmentDestination, '') AS Garment Shipment Destination, " +
            "ISNULL(MatrClass, '') AS Material Type ,ISNULL(ItemSeq, 0) AS ItemSeq ,ISNULL(POItemCode, '') AS PO ItemCode , " +
            "ISNULL(MatrCode, '') AS Material Code ,ISNULL(UPCCOMBOIM, '') AS UPC(COMBO IM#) ,ISNULL(ContentCode, '') AS Content Code , " +
            "ISNULL(CareCode, '') AS Care Code ,ISNULL(Color, '') AS Color Code ,ISNULL(ColorName, '') AS Color Name ,ISNULL(GCW, '') AS GCW , " +
            "ISNULL(Size, '') AS Size ,ISNULL(SizeMatrix, '') AS SizeMatrix ,ISNULL(Currency, '') AS Currency ,ISNULL(Price, 0) AS Price , " +
            "ISNULL(Quantity, 0) AS Quantity ,ISNULL(QtyUnit, '') AS Unit ,ISNULL(DeliveryDate, '') AS FCTY Need Date, " +
            "ISNULL(Season, '') AS Season ,ISNULL(Custporef, '') AS End Customer PO ,ISNULL(Buy, '') AS Buy Code ,ISNULL(BuyNo, '') AS Buy Month , " +
            "ISNULL(Category, '') AS Category ,ISNULL(Program, '') AS Buy Group ,ISNULL(SubProgram, '') AS Sub Program , " +
            "ISNULL(StyleNo, '') AS Style No ,ISNULL(StyleName, '') AS StyleName ,ISNULL(POMatching1, '') AS POMatching1, " +
            "ISNULL(POMatching2, '') AS PO Matching 2 ,ISNULL(PO Matching 3, '') AS PO Matching 3 ,ISNULL(PO Matching 4, '') AS PO Matching 4, " +
            "ISNULL(POMatching5, '') AS PO Matching 5 ,ISNULL(ItemMatching1, '') AS Item Matching 1, ISNULL(ItemMatching2, '') AS Item Matching 2 , " +
            "ISNULL(ItemMatching3, '') AS ItemMatching3, ISNULL(ItemMatching4, '') AS ItemMatching4 ,ISNULL(ItemMatching5, '') AS Item Matching 5 , " +
            "ISNULL(Position, '') AS Position ,ISNULL(Type, '') AS Type ,ISNULL(PriceTerm, 0) AS Price Term, " +
            "ISNULL(PaymentTerm, '') AS Payment Term ,ISNULL(Remarkfrommer, '') AS Remark From Mer , " +
            "ISNULL(RemarkForPurchase, '') AS Remark For Purchase, ISNULL(InvoiceCmpCode, '') AS InvoiceCmpCode , " +
            "ISNULL(CompanyName, '') AS CompanyName, ISNULL(address1, '') AS Address 1 ,ISNULL(address2, '') AS Address 2, " +
            "ISNULL(address3, '') AS Address 3 ,ISNULL(address4, '') AS Address 4 ,ISNULL(sysowner, '') AS Purchaser , " +
            "ISNULL(ZeroInspection, '') AS Zero Inspection ,ISNULL(GarmentShip, '') AS GAC Date, " +
            "ISNULL(OGACDate, '') AS OGAC Date(MM/YYYY) ,ISNULL(HITLink, '') AS HIT Job No, ISNULL(NIKECustomerPONo, '') AS NIKE Sales Order No , " +
            "ISNULL(QRS, 0) AS QPP/QRS Quantity ,ISNULL(PromoQty, 0) AS Promo Quantity ,ISNULL(ActualQuantity, 0) AS Total Quantity , " +
            "ISNULL(POUploadDate, '') AS PO Upload Date ,ISNULL(POUploadTime, '') AS Upload Time , " +
            "ISNULL(POUploadBy, '') AS POUploadBy ,ISNULL(CountryOfOrigin, '') Country Of Origin (Label)  , " +
            "ISNULL(SaleOrderSaleOrderLine, '') AS SalesOrder Line (VAS), ISNULL(NikeSAPPOPONIKEPOLine, '') AS Nike PO-Line , " +
            "ISNULL(NikematerialStyleColorway, '') AS Nike Style-Colorway ,ISNULL(Modifire1, '') AS Modifire 1 , " +
            "ISNULL(Modifire2, '') AS Modifire 2, ISNULL(Modifire3, '') AS Modifire 3 ,ISNULL(MPOHZ, '') AS MPOHZ , " +
            "ISNULL(ItemDescription, '') AS Item Description ,ISNULL(BulkQRSSample, '') AS Bulk/Sample (Care Label) , " +
            //"ISNULL(ExtraMinQty, 0) AS ExtraMinQty ,ISNULL(EAGSystemUnitPriceERP3, 0) AS EAGSystemUnitPriceERP3 , " +
            "ISNULL(CCTotalpage, 0) AS CC Total page ,ISNULL(Surcharge, 0) AS Surcharge , " +
            //ISNULL(POApproveDate, '') AS POApproveDate , " +
            //"ISNULL(POIssueDate, '') AS POIssueDate ,ISNULL(CLXorderconfirmationnumber, '') AS CLXorderconfirmationnumber , " +
            //"ISNULL(FTYMerch, '') AS FTYMerch ,ISNULL(HKMerch, '') AS HKMerch , " +
            "ISNULL(ChinaInsertCard, '') AS ChinaInsertCard , ISNULL(P1pc2in1, '') AS P1PC 2in1 , " +
            //"ISNULL(ReplyCode, '') AS ReplyCode, " +
            "ISNULL(WovenLabelSizeLength, '') AS Woven Label Size Length (Avery) , " +
            "ISNULL(ArgentinaImportNumber, '') AS Argentina Import Number ,ISNULL(DownFill, '') AS DownFill ,ISNULL(SYS_ID, '') AS SYS_ID , " +
            "ISNULL(ChinasizeMatrixtype, '') AS Chinasize Matrixtype ,ISNULL(EAGERPItemNumber, '') AS EAGERP Item Number , " +
            "ISNULL(CompoundColororCCIMfor2in1CCIM, '') AS Compound Coloror CCI Mfor2in1CCIM ,ISNULL(CPO, '') AS CPO , " +
            "ISNULL(BhasaIndonesiaProductBrand, '') AS Bhasa Indonesia Product Brand ,ISNULL(SAFCODE, '') AS SAFCODE , " +
            "ISNULL(Youthorder, '') AS Youth order  ,ISNULL(NFCproduct, '') AS NFC Product , " +
            "ISNULL(NeckneckopeningX2, '') AS Neck neck opening X2, ISNULL(ChestbodywidthX2, '') AS Chest body width X2 , " +
            "ISNULL(CenterBackbodylength, '') AS Center Back body length ,ISNULL(WaistwaistrelaxedInseam, '') AS Waist waistrelaxed Inseam , " +
            "ISNULL(PackQuantityQTYPERSELLINGUNIT, 0) AS Pack Quantity QTYPERSELLINGUNIT ,ISNULL(Fabriccode, 0) AS Fabric code , " +
            "ISNULL(PRODUCTDESCRIPTION, '') AS PRODUCT DESCRIPTION ," +
            //"ISNULL(PRODUCTCODEDESCRIPTION, '') AS PRODUCTCODEDESCRIPTION , " +
            "ISNULL(GARMENTSTYLEFIELD, '') AS Compound Coloror CCI Mfor2in1CCIM , " +
            "ISNULL(GARMENTSTYLEFIELD, '') AS GARMENT STYLE FIELD ,ISNULL(INSEAMSTYLE, '') AS INSEAM STYLE ";
        //"ISNULL(StateFlag, '') AS StateFlag  ";

        //,[StateAcknowledge],[AcknowledgeBy],[AcknowledgeDate],[AcknowledgeTime],[StateAcknowledgeLock],[StateClose]
        //,[T2_Confirm_Ship_Date],[T2_Confirm_Price],[T2_Confirm_Quantity],[T2_Confirm_OrderNo],[T2_Confirm_PO_Date]
        //,[T2_Confirm_By],[Estimatedeldate],[Actualdeldate],[InvoiceNo],[RcvQty],[RcvDate],[StateRead],[FTStateHasFile]
        //,[FTFileRef],[FNPIQuantity],[FNPINetAmt],[FTAWBNo]
        //" , ISNULL(DATALENGTH(FTFileRef), -1) AS FTFileRef ";
        //SELECT FTFileRef from[DB_VENDER].dbo.POPayment FOR XML PATH(''), BINARY BASE64


        [HttpPost]
        [Route("api/GetPurchaseInfo/")]
        public HttpResponseMessage GetPurchaseInfo([FromBody] UserAuthen value)
        {
            string _Qry = "";
            string jsondata = "";
            string msgError = "";
            int statecheck = 0;
            DataTable dt = null;
            DataTable dtPO = new DataTable();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            // Check id + pwd + vender group
            List<string> _result = UserAuthen.ValidateField(value);
            statecheck = int.Parse(_result[0]);
            msgError = _result[1];
            // End Check id + pwd + vender group

            try
            {
                if (statecheck != 2 && value.token != "")
                {
                    dt = HITConnect.UserAuthen.GetDTUserValidate(Cnn, value);
                    // Delete Old Token from Database
                    UserAuthen.DelAuthenKey(Cnn, value.id);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        /*  Fields Not Send JSON  [FTDataKey],[FTDateCreate],  */

                        _Qry = columnPO2V + " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[POToVender] AS POV " +
                            " INNER JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].VenderCode VC " +
                            " ON VC.Vender = POV.VenderCode " +
                            " WHERE VC.VenderGrp = '" + value.venderGroup + "'AND POV.StateAcknowledge = 0";
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

                            _Qry += columnPO2V + ", 1 AS StateAcknowledge, '" + value.id + "' AS AcknowledgeBy, " +
                                " @DATE AS AcknowledgeDate, @TIME AS AcknowledgeTime, " +
                                " ISNULL(StateAcknowledgeLock, '') AS StateAcknowledgeLock, ISNULL(StateClose, '') AS StateClose, " +
                                " ISNULL( T2_Confirm_Price, 0 ) AS T2_Confirm_Price, ISNULL(T2_Confirm_Quantity, 0) AS T2_Confirm_Quantity, " +
                                " ISNULL(T2_Confirm_OrderNo, '') AS T2_Confirm_OrderNo, ISNULL(T2_Confirm_PO_Date, '') AS T2_Confirm_PO_Date, " +
                                " ISNULL(T2_Confirm_By, '') AS T2_Confirm_By, ISNULL(Estimatedeldate, '') AS Estimatedeldate, " +
                                " ISNULL(Actualdeldate, '') AS Actualdeldate, ISNULL(InvoiceNo, '') AS InvoiceNo, ISNULL(RcvQty, 0) AS RcvQty, " +
                                " ISNULL(RcvDate, '') AS RcvDate, ISNULL(StateRead, '') AS StateRead ";
                            _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender AS POV ";
                            _Qry += " POV INNER JOIN [dbo].VenderCode VC ON VC.Vender = POV.VenderCode ";
                            _Qry += " WHERE VC.VenderGrp = '" + value.venderGroup + "' AND POV.StateAcknowledge = 0 ";

                            _Qry += " SELECT @TotalEff=@@ROWCOUNT ";


                            _Qry += " UPDATE[" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender SET StateAcknowledge = 1, ";
                            _Qry += " AcknowledgeBy = '" + value.id + "' , AcknowledgeDate = @DATE, AcknowledgeTime = @TIME ";
                            _Qry += " WHERE vendercode = '" + value.venderGroup + "' AND StateAcknowledge = 0 ";


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
                    }
                    else
                    {
                        statecheck = 2;
                        msgError = "Please check User authentication!!!";

                    }
                }
                else
                {
                    statecheck = 2;
                    msgError = "Invalid token!!!";
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

            //dts.Rows.Add(new Object[] { statecheck, msgError });

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
        [Route("api/GetPurchaseAckInfo/")]
        public HttpResponseMessage GetPurchaseAckInfo([FromBody] POtoVender value)
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

            // Check id + pwd + vender group
            List<string> _result = UserAuthen.ValidateField(value.authen);
            statecheck = int.Parse(_result[0]);
            msgError = _result[1];
            // End Check id + pwd + vender group

            try
            {
                dt = HITConnect.UserAuthen.GetDTUserValidate(Cnn, value.authen);
                // Delete Old Token from Database
                UserAuthen.DelAuthenKey(Cnn, value.authen.id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    _Qry = columnPO2V + " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK ";

                    if (value.startDate != "" && value.endDate != "")
                    {
                        if (Convert.ToDateTime(value.startDate) <= Convert.ToDateTime(value.endDate))
                        {
                            _Qry += " WHERE PODate BETWEEN '" + value.startDate + "' AND '" + value.endDate + "' ";
                            if (value.PONo != "")
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
                        if (value.PONo != "")
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