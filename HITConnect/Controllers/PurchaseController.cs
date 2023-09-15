using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class PurchaseController : ApiController
    {
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
                        _Qry = " exec [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[USP_PULLPOACK] @User = '" + value.id + "' ";
                        dtPO = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                        /*  Fields Not Send JSON  */
                        if (dtPO.Rows.Count > 0)
                        {
                            /* ------------------  Remove Column ------------------ */
                            dtPO.Columns.Remove("FTDataKey");
                            dtPO.Columns.Remove("FTDateCreate");
                            dtPO.Columns.Remove("ExtraMinQty");
                            dtPO.Columns.Remove("EAGSystemUnitPriceERP3");
                            dtPO.Columns.Remove("POApproveDate");
                            dtPO.Columns.Remove("POIssueDate");
                            dtPO.Columns.Remove("CLXorderconfirmationnumber");
                            dtPO.Columns.Remove("FTYMerch");
                            dtPO.Columns.Remove("HKMerch");
                            dtPO.Columns.Remove("ReplyCode");
                            dtPO.Columns.Remove("StateAcknowledge");
                            dtPO.Columns.Remove("AcknowledgeBy");
                            dtPO.Columns.Remove("AcknowledgeDate");
                            dtPO.Columns.Remove("AcknowledgeTime");
                            dtPO.Columns.Remove("StateAcknowledgeLock");
                            dtPO.Columns.Remove("StateClose");
                            dtPO.Columns.Remove("T2_Confirm_Ship_Date");
                            dtPO.Columns.Remove("T2_Confirm_Price");
                            dtPO.Columns.Remove("T2_Confirm_Quantity");
                            dtPO.Columns.Remove("T2_Confirm_OrderNo");
                            dtPO.Columns.Remove("T2_Confirm_PO_Date");
                            dtPO.Columns.Remove("T2_Confirm_By");
                            dtPO.Columns.Remove("Estimatedeldate");
                            dtPO.Columns.Remove("Actualdeldate");
                            dtPO.Columns.Remove("RcvQty");
                            dtPO.Columns.Remove("RcvDate");
                            dtPO.Columns.Remove("StateRead");
                            dtPO.Columns.Remove("UPCCOMBOIM");
                            dtPO.Columns.Remove("ContentCode");
                            dtPO.Columns.Remove("CareCode");
                            dtPO.Columns.Remove("SizeMatrix");
                            dtPO.Columns.Remove("Quantity");
                            dtPO.Columns.Remove("GarmentShip");
                            dtPO.Columns.Remove("CountryOfOrigin");
                            dtPO.Columns.Remove("SaleOrderSaleOrderLine");
                            dtPO.Columns.Remove("NikeSAPPOPONIKEPOLine");
                            dtPO.Columns.Remove("NikematerialStyleColorway");
                            dtPO.Columns.Remove("Modifire1");
                            dtPO.Columns.Remove("Modifire2");
                            dtPO.Columns.Remove("Modifire3");
                            dtPO.Columns.Remove("MPOHZ");
                            dtPO.Columns.Remove("CCTotalpage");
                            dtPO.Columns.Remove("ChinaInsertCard");
                            dtPO.Columns.Remove("P1pc2in1");
                            dtPO.Columns.Remove("WovenLabelSizeLength");
                            dtPO.Columns.Remove("ArgentinaImportNumber");
                            dtPO.Columns.Remove("DownFill");
                            dtPO.Columns.Remove("SYS_ID");
                            dtPO.Columns.Remove("ChinasizeMatrixtype");
                            dtPO.Columns.Remove("EAGERPItemNumber");
                            dtPO.Columns.Remove("CompoundColororCCIMfor2in1CCIM");
                            dtPO.Columns.Remove("CPO");
                            dtPO.Columns.Remove("BhasaIndonesiaProductBrand");
                            dtPO.Columns.Remove("SAFCODE");
                            dtPO.Columns.Remove("Youthorder");
                            dtPO.Columns.Remove("NFCproduct");
                            dtPO.Columns.Remove("NeckneckopeningX2");
                            dtPO.Columns.Remove("ChestbodywidthX2");
                            dtPO.Columns.Remove("CenterBackbodylength");
                            dtPO.Columns.Remove("WaistwaistrelaxedInseam");
                            dtPO.Columns.Remove("PackQuantityQTYPERSELLINGUNIT");
                            dtPO.Columns.Remove("Fabriccode");
                            dtPO.Columns.Remove("PRODUCTDESCRIPTION");
                            dtPO.Columns.Remove("PRODUCTCODEDESCRIPTION");
                            dtPO.Columns.Remove("GARMENTSTYLEFIELD");
                            dtPO.Columns.Remove("INSEAMSTYLE");
                            dtPO.Columns.Remove("InvoiceNo");
                            dtPO.Columns.Remove("T2_Confirm_ShipQuantity");
                            dtPO.Columns.Remove("T2_Confirm_Ship_Date2");
                            dtPO.Columns.Remove("T2_Confirm_ShipQuantity2");
                            dtPO.Columns.Remove("Actualdeldate2");
                            dtPO.Columns.Remove("StateRejectedBuy");
                            dtPO.Columns.Remove("RejectedBuyBy");
                            dtPO.Columns.Remove("RejectedBuyDate");
                            dtPO.Columns.Remove("RejectedBuyTime");
                            dtPO.Columns.Remove("T2_Confirm_MOQQuantity");
                            dtPO.Columns.Remove("RejectedBuyNote");
                            dtPO.Columns.Remove("StateMailNotify");
                            dtPO.Columns.Remove("InvoiceCmpCode");


                            /* ------------------  Rename Column ------------------ */


                            dtPO.Columns["PONo"].ColumnName = "PONumber";
                            dtPO.Columns["MatrClass"].ColumnName = "MaterialType";
                            dtPO.Columns["MatrCode"].ColumnName = "MaterialCode";
                            dtPO.Columns["Color"].ColumnName = "ColorCode";
                            dtPO.Columns["QtyUnit"].ColumnName = "Unit";
                            dtPO.Columns["DeliveryDate"].ColumnName = "FCTYNeedDate";
                            dtPO.Columns["Custporef"].ColumnName = "EndCustomerPO";
                            dtPO.Columns["Buy"].ColumnName = "BuyMonth";
                            dtPO.Columns["BuyNo"].ColumnName = "BuyCode";
                            dtPO.Columns["Program"].ColumnName = "BuyGroup";
                            dtPO.Columns["SubProgram"].ColumnName = "SubProgram";
                            dtPO.Columns["Remarkfrommer"].ColumnName = "RemarkFromMer";
                            dtPO.Columns["address1"].ColumnName = "Address1";
                            dtPO.Columns["address2"].ColumnName = "Address2";
                            dtPO.Columns["address3"].ColumnName = "Address3";
                            dtPO.Columns["address4"].ColumnName = "Address4";
                            dtPO.Columns["sysowner"].ColumnName = "Purchaser";
                            dtPO.Columns["sysownername"].ColumnName = "PurchaserName";
                            dtPO.Columns["sysownermail"].ColumnName = "PurchaserEMail";
                            dtPO.Columns["ZeroInspection"].ColumnName = "ZeroInspection";
                            dtPO.Columns["OGACDate"].ColumnName = "GACDate";
                            dtPO.Columns["HITLink"].ColumnName = "HITJobNo";
                            dtPO.Columns["NIKECustomerPONo"].ColumnName = "NIKESalesOrderNo";
                            dtPO.Columns["QRS"].ColumnName = "QPP/QRSQuantity";
                            dtPO.Columns["PromoQty"].ColumnName = "PromoQuantity";
                            dtPO.Columns["BulkQRSSample"].ColumnName = "BulkQuantity";
                            dtPO.Columns["ActualQuantity"].ColumnName = "TotalPOQuantity";
                            dtPO.Columns["POUploadTime"].ColumnName = "UploadTime";
                            dtPO.Columns["POUploadBy"].ColumnName = "UploadBy";
                            dtPO.Columns["MOQ"].ColumnName = "MOQQuantity";

                            //dtPO.Columns["VenderCode"].ColumnName = "VendorCode";
                            //dtPO.Columns["VendorName"].ColumnName = "VendorName";
                            //dtPO.Columns["VendorLocation"].ColumnName = "VendorLocation";
                            //dtPO.Columns["FactoryCode"].ColumnName = "FactoryCode";
                            //dtPO.Columns["PODate"].ColumnName = "PODate";
                            //dtPO.Columns["Shipto"].ColumnName = "Shipto";
                            //dtPO.Columns["GarmentShipmentDestination"].ColumnName = "GarmentShipmentDestination";
                            //dtPO.Columns["POItemCode"].ColumnName = "POItemCode";
                            //dtPO.Columns["ColorName"].ColumnName = "ColorName";
                            //dtPO.Columns["GCW"].ColumnName = "GCW";
                            //dtPO.Columns["Size"].ColumnName = "Size";
                            //dtPO.Columns["Currency"].ColumnName = "Currency";
                            //dtPO.Columns["Price"].ColumnName = "Price";
                            //dtPO.Columns["Season"].ColumnName = "Season";
                            //dtPO.Columns["Category"].ColumnName = "Category";
                            //dtPO.Columns["StyleNo"].ColumnName = "StyleNo";
                            //dtPO.Columns["StyleName"].ColumnName = "StyleName";
                            //dtPO.Columns["POMatching1"].ColumnName = "POMatching1";
                            //dtPO.Columns["POMatching2"].ColumnName = "POMatching2";
                            //dtPO.Columns["POMatching3"].ColumnName = "POMatching3";
                            //dtPO.Columns["POMatching4"].ColumnName = "POMatching4";
                            //dtPO.Columns["POMatching5"].ColumnName = "POMatching5";
                            //dtPO.Columns["ItemMatching1"].ColumnName = "ItemMatching1";
                            //dtPO.Columns["ItemMatching2"].ColumnName = "ItemMatching2";
                            //dtPO.Columns["ItemMatching3"].ColumnName = "ItemMatching3";
                            //dtPO.Columns["ItemMatching4"].ColumnName = "ItemMatching4";
                            //dtPO.Columns["ItemMatching5"].ColumnName = "ItemMatching5";
                            //dtPO.Columns["Position"].ColumnName = "Position";
                            //dtPO.Columns["Type"].ColumnName = "Type";
                            //dtPO.Columns["PriceTerm"].ColumnName = "PriceTerm";
                            //dtPO.Columns["PaymentTerm"].ColumnName = "PaymentTerm";
                            //dtPO.Columns["RemarkForPurchase"].ColumnName = "RemarkForPurchase";
                            //dtPO.Columns["InvoiceCmpCode"].ColumnName = "InvoiceCmpCode";
                            //dtPO.Columns["CompanyName"].ColumnName = "CompanyName";
                            //dtPO.Columns["POUploadDate"].ColumnName = "POUploadDate";
                            //dtPO.Columns["ItemDescription"].ColumnName = "ItemDescription";
                            //dtPO.Columns["Surcharge"].ColumnName = "Surcharge";
                        }
                        else
                        {
                            statecheck = 2;
                            msgError = "No new Data!!!";
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

            if (dtPO.Rows.Count > 0)
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
                //jsondata = JsonConvert.SerializeObject(dtPO);
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
            string columnPO2V = " SELECT ISNULL(VenderCode, '') AS 'VendorCode' ,ISNULL(VendorName, '') AS 'VendorName', " +
            "ISNULL(VendorLocation, '') AS 'VendorLocation' ,ISNULL(FactoryCode, '') AS 'FactoryCode' ," +
            "ISNULL(PONo, '') AS 'PONumber', RIGHT(PODate,4)+'/'+SUBSTRING(PODate,4,2)+'/'+LEFT(PODate,2)  AS 'PODate', " +
            "ISNULL(Shipto, '') AS 'Shipto', ISNULL(GarmentShipmentDestination, '') AS 'Garment Shipment Destination', " +
            "ISNULL(MatrClass, '') AS 'MaterialType' , ISNULL(POItemCode, '') AS 'POItemCode' , ISNULL(MatrCode, '') AS 'MaterialCode' ," +
            "ISNULL(ItemSeq, 0) AS 'ItemSeq', ISNULL(Color, '') AS 'Color Code' ," +
            "ISNULL(ColorName, '') AS 'Color Name' ,ISNULL(GCW, '') AS 'GCW' , ISNULL(Size, '') AS 'Size' ," +
            "ISNULL(Currency, '') AS 'Currency', ISNULL(Price, 0) AS 'Price' , ISNULL(QtyUnit, '') AS 'Unit' ," +
            "RIGHT ( DeliveryDate, 4 )+ '/' + SUBSTRING( DeliveryDate, 4, 2 )+ '/' + LEFT ( DeliveryDate, 2 ) AS 'FCTYNeedDate',ISNULL(Season, '') AS 'Season' ," +
            "ISNULL(Custporef, '') AS 'EndCustomerPO' ,ISNULL(Buy, '') AS 'BuyCode' ," +
            "ISNULL(BuyNo, '') AS 'BuyMonth' , ISNULL(Category, '') AS 'Category' ," +
            "ISNULL(Program, '') AS 'BuyGroup' ,ISNULL(SubProgram, '') AS 'SubProgram' , " +
            "ISNULL(StyleNo, '') AS 'StyleNo' ,ISNULL(StyleName, '') AS 'StyleName' ," +
            "ISNULL(POMatching1, '') AS 'POMatching1', ISNULL(POMatching2, '') AS 'POMatching2' ," +
            "ISNULL(POMatching3, '') AS 'POMatching3' ,ISNULL(POMatching4, '') AS 'POMatching4', " +
            "ISNULL(POMatching5, '') AS 'POMatching5' ,ISNULL(ItemMatching1, '') AS 'ItemMatching1', " +
            "ISNULL(ItemMatching2, '') AS 'Item Matching2' , ISNULL(ItemMatching3, '') AS 'ItemMatching3', " +
            "ISNULL(ItemMatching4, '') AS 'ItemMatching4' ,ISNULL(ItemMatching5, '') AS 'Item Matching 5' , " +
            "ISNULL(Position, '') AS 'Position' ,ISNULL(Type, '') AS 'Type' ,ISNULL(PriceTerm, 0) AS 'Price Term', " +
            "ISNULL(PaymentTerm, '') AS 'Payment Term' ,ISNULL(Remarkfrommer, '') AS 'Remark From Mer' , " +
            "ISNULL(RemarkForPurchase, '') AS 'Remark For Purchase', ISNULL(InvoiceCmpCode, '') AS 'InvoiceCmpCode', " +
            "ISNULL(CompanyName, '') AS 'CompanyName', ISNULL(address1, '') AS 'Address 1' ," +
            "ISNULL(address2, '') AS 'Address 2', ISNULL(address3, '') AS 'Address 3' ," +
            "ISNULL(address4, '') AS 'Address 4' ,ISNULL(sysowner, '') AS 'Purchaser' , " +
            "ISNULL(ZeroInspection, '') AS 'Zero Inspection' ,ISNULL(GarmentShip, '') AS 'GAC Date', " +
            "ISNULL(HITLink, '') AS 'HIT Job No', ISNULL(NIKECustomerPONo, '') AS 'NIKE Sales Order No' , ISNULL(QRS, 0) AS 'QPP/QRS Quantity', " +
            "ISNULL(BulkQRSSample, 0) AS 'BulkQuantity', " +
            "ISNULL(PromoQty, 0) AS 'PromoQuantity' ,ISNULL(ActualQuantity, 0) AS 'TotalQuantity' , " +
            "RIGHT ( POUploadDate, 4 )+ '/' + SUBSTRING( POUploadDate, 4, 2 )+ '/' + LEFT ( POUploadDate, 2 )  AS 'POUploadDate' ,ISNULL(POUploadTime, '') AS 'UploadTime' , " +
            "ISNULL(POUploadBy, '') AS 'POUploadBy' , ISNULL(ItemDescription, '') AS 'ItemDescription' , ISNULL(Surcharge, 0) AS 'Surcharge' ";
            //"ISNULL(SizeMatrix, '') AS 'SizeMatrix' , ISNULL(Quantity, 0) AS 'Quantity' ," +
            //"ISNULL(CountryOfOrigin, '') AS 'Country Of Origin (Label)' , "ISNULL(SaleOrderSaleOrderLine, '') AS 'SalesOrder Line (VAS)', " +
            //"ISNULL(NikeSAPPOPONIKEPOLine, '') AS 'Nike PO-Line' , " +
            //"ISNULL(NikematerialStyleColorway, '') AS 'Nike Style-Colorway' ,ISNULL(Modifire1, '') AS 'Modifire 1' , " +
            //"ISNULL(Modifire2, '') AS 'Modifire 2', ISNULL(Modifire3, '') AS 'Modifire 3' ," +
            //"ISNULL(MPOHZ, '') AS 'MPOHZ' , " +
            //"ISNULL(BulkQRSSample, '') AS 'Bulk/Sample (Care Label)' ,ISNULL(CCTotalpage, 0) AS 'CC Total page' ," +
            //"ISNULL(ChinaInsertCard, '') AS 'ChinaInsertCard' , " +
            //"ISNULL(P1pc2in1, '') AS 'P1PC 2in1' , ISNULL(WovenLabelSizeLength, '') AS 'Woven Label Size Length (Avery)' , " +
            //"ISNULL(ArgentinaImportNumber, '') AS 'Argentina Import Number' ,ISNULL(DownFill, '') AS 'DownFill' ," +
            //"ISNULL(SYS_ID, '') AS 'SYS_ID' , ISNULL(ChinasizeMatrixtype, '') AS 'Chinasize Matrixtype' ," +
            //"ISNULL(EAGERPItemNumber, '') AS 'EAGERP Item Number' , " +
            //"ISNULL(CompoundColororCCIMfor2in1CCIM, '') AS 'Compound Color CCI Mfor2in1CCIM' ," +
            //"ISNULL(CPO, '') AS 'CPO' , ISNULL(BhasaIndonesiaProductBrand, '') AS 'Bhasa Indonesia Product Brand' ," +
            //"ISNULL(SAFCODE, '') AS 'SAFCODE' , ISNULL(Youthorder, '') AS 'Youth order'  ," +
            //"ISNULL(NFCproduct, '') AS 'NFC Product' , ISNULL(NeckneckopeningX2, '') AS 'Neck neck opening X2', " +
            //"ISNULL(ChestbodywidthX2, '') AS 'Chest body width X2' , " +
            //"ISNULL(CenterBackbodylength, '') AS 'Center Back body length' ," +
            //"ISNULL(WaistwaistrelaxedInseam, '') AS 'Waist waistrelaxed Inseam' , " +
            //"ISNULL(PackQuantityQTYPERSELLINGUNIT, 0) AS 'Pack Quantity QTYPERSELLINGUNIT' ," +
            //"ISNULL(Fabriccode, 0) AS 'Fabric code' ,ISNULL(PRODUCTDESCRIPTION, '') AS 'PRODUCT DESCRIPTION' ," +
            //"ISNULL(GARMENTSTYLEFIELD, '') AS 'Compound Color CCI Mfor2in1CCIM' , " +
            //"ISNULL(GARMENTSTYLEFIELD, '') AS 'GARMENT STYLE FIELD' ,ISNULL(INSEAMSTYLE, '') AS 'INSEAM STYLE' ";
            //"ISNULL(UPCCOMBOIM, '') AS 'UPC(COMBO IM#)' ,ISNULL(ContentCode, '') AS 'Content Code' ," +
            //"ISNULL(CareCode, '') AS 'Care Code' , ISNULL(OGACDate, '') AS 'OGAC Date(MM/YYYY)' ," +
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
                    _Qry = columnPO2V + " FROM " + WSM.Conn.DB.DataBaseName.DB_VENDER + ".dbo.POToVender_ACK AS P WITH (NOLOCK)" +
                        "WHERE P.VenderCode IN (SELECT Vender FROM " + WSM.Conn.DB.DataBaseName.DB_VENDER + ".dbo.VenderUserPermissionCmp AS V WITH (NOLOCK) " +
                        "OUTER APPLY(SELECT* FROM " + WSM.Conn.DB.DataBaseName.DB_VENDER + ".dbo.VenderCode AS C WITH (NOLOCK) " +
                        "WHERE V.VenderGrp = C.VenderGrp) AS A WHERE V.VenderMailLogIn = '"+ value.authen.id + "') ";
                    string _QryCondition = "";
                    if (value.startDate != "" && value.endDate != "")
                    {
                        if (Convert.ToDateTime(value.startDate) <= Convert.ToDateTime(value.endDate))
                        {
                            //_QryCondition = (_QryCondition.Length > 0) ? (" AND " + _QryCondition) : _QryCondition;
                            _QryCondition += " AND (RIGHT(PODate,4)+'/'+SUBSTRING(PODate,4,2)+'/'+LEFT(PODate,2)) >= '" + value.startDate + "' " +
                                " AND (RIGHT(PODate,4)+'/'+SUBSTRING(PODate,4,2)+'/'+LEFT(PODate,2)) <= '" + value.endDate + "' ";
                        }
                        else
                        {
                            statecheck = 2;
                            msgError = "Start Date must be lower than End Date";
                        }

                    }
                    if (value.startACK != "" && value.endACK != "")
                    {
                        if (Convert.ToDateTime(value.startACK) <= Convert.ToDateTime(value.endACK))
                        {
                            //_QryCondition = (_QryCondition.Length > 0) ? (" AND " + _QryCondition) : _QryCondition;

                            _QryCondition += " AND (RIGHT(AcknowledgeDate,4)+'/'+SUBSTRING(AcknowledgeDate,4,2)+'/'+LEFT(AcknowledgeDate,2)) >= '" + value.startACK + "' " +
                                " AND (RIGHT(AcknowledgeDate,4)+'/'+SUBSTRING(AcknowledgeDate,4,2)+'/'+LEFT(AcknowledgeDate,2)) <= '" + value.endACK + "' ";
                        }
                        else
                        {
                            statecheck = 2;
                            msgError = "Start Date must be lower than End Date";
                        }
                    }
                    if (value.PONo != "")
                    {
                        //_QryCondition = (_QryCondition.Length > 0) ? (" AND " + _QryCondition) : _QryCondition;
                        _QryCondition += " AND PONo = '" + value.PONo + "'";
                    }
                    if (value.AcknowledgeBy != "")
                    {
                        //_QryCondition = (_QryCondition.Length > 0) ? (" AND " + _QryCondition) : _QryCondition;
                        _QryCondition += " AND AcknowledgeBy = '" + value.AcknowledgeBy + "'";
                    }

                    if (statecheck == 0)
                    {
                        _Qry = (_QryCondition.Length > 0) ? (_Qry +  _QryCondition) : _Qry;
                        dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                        statecheck = 1;
                        msgError = "Successful";
                    }

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

        [HttpPost]
        [Route("api/RejectBuy/")]
        public HttpResponseMessage RejectBuy([FromBody] PORejectBuy value)
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
                    if (value.PORejected != null)
                    {
                        //STamp StateRejectedBuy = 1, RejectedBuyBy = '', RejectedBuyDate = '', RejectedBuyTime + Reject Note
                        //StateSendMailRejected = '1', StateSendMailRejectedComplete = '0'
                        _Qry = " declare @Date varchar(10) =Convert(varchar(10),Getdate(),103) " +
                        " declare @Time varchar(8) = Convert(varchar(8), Getdate(), 114)" +
                     " UPDATE " + WSM.Conn.DB.DataBaseName.DB_VENDER + ".dbo.POToVender_ACK SET StateSendMailRejected = '1', StateSendMailRejectedComplete = '0', " +
                        " StateRejectedBuy = 1, RejectedBuyBy = '" + value.authen.id + "', RejectedBuyDate = @Date, RejectedBuyTime = @Time ";
                        _Qry += "WHERE PONo = '" + value.PORejected.PONo + "' AND POItemCode = '" + value.PORejected.Item + "' AND Color ='" +
                            value.PORejected.Color + "' AND Size = '" + value.PORejected.Size + "' ";

                        // Send Mail Reject Buy
                        _Qry += "exec [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[USP_MAILNOTIFY_REJECTBUY] @User = '" + value.authen.id + "' ";

                        dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                        statecheck = 1;
                        msgError = "Successful";
                    }
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
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
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

/*
 * private string columnPO2V = "SELECT FTDataKey, FTDateCreate, VenderCode, VendorName, VendorLocation, FactoryCode, " +
            "A.PONo, PODate, Shipto, GarmentShipmentDestination, MatrClass, ItemSeq, POItemCode, MatrCode, UPCCOMBOIM, " +
            "ContentCode, CareCode, Color, ColorName, GCW, Size, SizeMatrix, Currency, Price, Quantity, QtyUnit, " +
            "DeliveryDate, Season, Custporef, Buy, BuyNo, Category, Program, SubProgram, StyleNo, StyleName, " +
            "POMatching1, POMatching2, POMatching3, POMatching4, POMatching5, ItemMatching1, ItemMatching2, " +
            "ItemMatching3, ItemMatching4, ItemMatching5, Position, Type, PriceTerm, PaymentTerm, Remarkfrommer, " +
            "RemarkForPurchase, InvoiceCmpCode, CompanyName, address1, address2, address3, address4, sysowner, " +
            "ZeroInspection, GarmentShip, OGACDate, HITLink, NIKECustomerPONo, QRS, PromoQty, ActualQuantity, " +
            "POUploadDate, POUploadTime, POUploadBy, CountryOfOrigin, SaleOrderSaleOrderLine, NikeSAPPOPONIKEPOLine, " +
            "NikematerialStyleColorway, Modifire1, Modifire2, Modifire3, MPOHZ, ItemDescription, BulkQRSSample, " +
            "ExtraMinQty, EAGSystemUnitPriceERP3, CCTotalpage, Surcharge, POApproveDate, POIssueDate, " +
            "CLXorderconfirmationnumber, FTYMerch, HKMerch, ChinaInsertCard, P1pc2in1, ReplyCode, WovenLabelSizeLength, " +
            "ArgentinaImportNumber, DownFill, SYS_ID, ChinasizeMatrixtype, EAGERPItemNumber, " +
            "CompoundColororCCIMfor2in1CCIM, CPO, BhasaIndonesiaProductBrand, SAFCODE, Youthorder, NFCproduct, " +
            "NeckneckopeningX2, ChestbodywidthX2, CenterBackbodylength, WaistwaistrelaxedInseam, " +
            "PackQuantityQTYPERSELLINGUNIT, Fabriccode, PRODUCTDESCRIPTION, PRODUCTCODEDESCRIPTION, GARMENTSTYLEFIELD, " +
            "INSEAMSTYLE, StateFlag, StateAcknowledge, AcknowledgeBy, AcknowledgeDate, AcknowledgeTime, " +
            "StateAcknowledgeLock, StateClose, x.T2_Confirm_Ship_Date, x.T2_Confirm_Price, x.T2_Confirm_Quantity, " +
            "x.T2_Confirm_OrderNo, x.T2_Confirm_PO_Date, x.T2_Confirm_By, x.T2_Confirm_Note, x.Estimatedeldate, " +
            "x.Actualdeldate, x.InvoiceNo, ISNULL(R.RQuantity,0) as RcvQty, ISNULL(INVXC.ReceiveDate ,'') RcvDate, " +
            "StateRead, X.FTStateHasFile, X.FTFileRef";
        //"ISNULL(StateFlag, '') AS StateFlag  ";
        
        //,[StateAcknowledge],[AcknowledgeBy],[AcknowledgeDate],[AcknowledgeTime],[StateAcknowledgeLock],[StateClose]
        //,[T2_Confirm_Ship_Date],[T2_Confirm_Price],[T2_Confirm_Quantity],[T2_Confirm_OrderNo],[T2_Confirm_PO_Date]
        //,[T2_Confirm_By],[Estimatedeldate],[Actualdeldate],[InvoiceNo],[RcvQty],[RcvDate],[StateRead],[FTStateHasFile]
        //,[FTFileRef],[FNPIQuantity],[FNPINetAmt],[FTAWBNo]
        //" , ISNULL(DATALENGTH(FTFileRef), -1) AS FTFileRef ";
        //SELECT FTFileRef from[DB_VENDER].dbo.POPayment FOR XML PATH(''), BINARY BASE64

/*
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
                            _Qry += "INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK " +
                                //" [FTDataKey],[FTDateCreate]," +
                                " (FTDataKey, FTDateCreate, VenderCode, VendorName, VendorLocation, FactoryCode, " +
                                "PONo, PODate, Shipto, GarmentShipmentDestination, MatrClass, ItemSeq, POItemCode, " +
                                "MatrCode, UPCCOMBOIM, ContentCode, CareCode, Color, ColorName, GCW, Size, " +
                                "SizeMatrix, Currency, Price, Quantity, QtyUnit, DeliveryDate, Season, Custporef, " +
                                "Buy, BuyNo, Category, Program, SubProgram, StyleNo, StyleName, POMatching1, " +
                                "POMatching2, POMatching3, POMatching4, POMatching5, ItemMatching1, ItemMatching2, " +
                                "ItemMatching3, ItemMatching4, ItemMatching5, Position, Type, PriceTerm, PaymentTerm, " +
                                "Remarkfrommer, RemarkForPurchase, InvoiceCmpCode, CompanyName, address1, address2, " +
                                "address3, address4, sysowner, ZeroInspection, GarmentShip, OGACDate, HITLink, " +
                                "NIKECustomerPONo, QRS, PromoQty, ActualQuantity, POUploadDate, POUploadTime, " +
                                "POUploadBy, CountryOfOrigin, SaleOrderSaleOrderLine, NikeSAPPOPONIKEPOLine, " +
                                "NikematerialStyleColorway, Modifire1, Modifire2, Modifire3, MPOHZ, ItemDescription, " +
                                "BulkQRSSample, ExtraMinQty, EAGSystemUnitPriceERP3, CCTotalpage, Surcharge, " +
                                "POApproveDate, POIssueDate, CLXorderconfirmationnumber, FTYMerch, HKMerch, " +
                                "ChinaInsertCard, P1pc2in1, ReplyCode, WovenLabelSizeLength, ArgentinaImportNumber, " +
                                "DownFill, SYS_ID, ChinasizeMatrixtype, EAGERPItemNumber, CompoundColororCCIMfor2in1CCIM, " +
                                "CPO, BhasaIndonesiaProductBrand, SAFCODE, Youthorder, NFCproduct, NeckneckopeningX2, " +
                                "ChestbodywidthX2, CenterBackbodylength, WaistwaistrelaxedInseam, PackQuantityQTYPERSELLINGUNIT, " +
                                "Fabriccode, PRODUCTDESCRIPTION, PRODUCTCODEDESCRIPTION, GARMENTSTYLEFIELD, INSEAMSTYLE, StateFlag, " +
                                "StateAcknowledge, AcknowledgeBy, AcknowledgeDate, AcknowledgeTime, StateAcknowledgeLock, " +
                                "StateClose, T2_Confirm_Ship_Date, T2_Confirm_Price, T2_Confirm_Quantity, T2_Confirm_OrderNo, " +
                                "T2_Confirm_PO_Date, T2_Confirm_By,T2_Confirm_Note, Estimatedeldate, Actualdeldate, InvoiceNo, " +
                                "RcvQty, RcvDate, StateRead, FTStateHasFile, FTFileRef) ";

                            _Qry += columnPO2V + ", 1 AS 'StateAcknowledge', '" + value.id + "' AS 'AcknowledgeBy', " +
                                " @DATE AS 'AcknowledgeDate', @TIME AS 'AcknowledgeTime', " +
                                " ISNULL(StateAcknowledgeLock, '') AS 'StateAcknowledgeLock', ISNULL(StateClose, '') AS 'StateClose', " +
                                " ISNULL( T2_Confirm_Price, 0 ) AS 'T2_Confirm_Price', ISNULL(T2_Confirm_Quantity, 0) AS 'T2_Confirm_Quantity', " +
                                " ISNULL(T2_Confirm_OrderNo, '') AS 'T2_Confirm_OrderNo', ISNULL(T2_Confirm_PO_Date, '') AS 'T2_Confirm_PO_Date', " +
                                " ISNULL(T2_Confirm_By, '') AS 'T2_Confirm_By', ISNULL(Estimatedeldate, '') AS 'Estimatedeldate', " +
                                " ISNULL(Actualdeldate, '') AS 'Actualdeldate', ISNULL(InvoiceNo, '') AS 'InvoiceNo', ISNULL(RcvQty, 0) AS 'RcvQty', " +
                                " ISNULL(RcvDate, '') AS 'RcvDate', ISNULL(StateRead, '') AS 'StateRead' ";
                            _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender AS POV ";
                            _Qry += " INNER JOIN [dbo].VenderCode VC ON VC.Vender = POV.VenderCode ";
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
                        */
/* if (dtPO.Rows.Count > 0)
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
*/


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