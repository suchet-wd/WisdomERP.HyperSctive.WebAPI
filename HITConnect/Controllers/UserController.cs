using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class UserController : ApiController
    {
        public object ScriptManager { get; private set; }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "String2" };
        }

        [HttpGet]
        [Route("api/TestJSON/")]
        public string TestJSONText(string user, string pwd)
        {
            string userId = user;
            string pwd_ = pwd;
            return userId + pwd_;
        }


        [HttpGet]
        [Route("api/GetToken/")]
        public HttpResponseMessage GetToken(string id, string pwd)
        {
            string _Qry = "";
            string status = "";
            string msgError = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            _Qry += " SELECT V.Pwd AS pwd, V.VanderMailLogIn AS VanderMailLogIn FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V " +
                "WHERE V.VanderMailLogIn = '" + id + "'";

            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
            bool valid = false;
            if (dt != null && dt.Rows.Count == 1)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (pwd == WSM.Conn.DB.FuncDecryptDataServer(row["pwd"].ToString()))
                    {
                        _Qry = "INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys (VanderMailLogIn, DataKey) ";
                        _Qry += " VALUES ('" + row["VanderMailLogIn"].ToString() + "', '" + row["pwd"].ToString() + "')";
                        valid = (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER));
                        status = valid ? "1" : "2";
                        msgError = valid ? "Successful" : "Cannot save token"; ;

                        dts.Rows.Add(new Object[] { status, row["pwd"].ToString(), msgError });
                    }
                }
            }

            string jsondata = (valid) ? JsonConvert.SerializeObject(dts) : "NOT FOUND";
            return new HttpResponseMessage { Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
        }

        [HttpGet]
        [Route("api/CheckToken/")]
        public HttpResponseMessage CheckToken(HITConnect.Models.User u)
        {
            DataTable dt = null;
            DataTable dtPO = new DataTable();
            DataTable dts = new DataTable();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            string _Qry = "";
            bool valid = false;

            _Qry += " SELECT V.Pwd, V.VanderMailLogIn, ATK.DataKey FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V  " +
                "LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                "WHERE V.VanderMailLogIn = '" + u.id + "'";
            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);


            // REMOVE TOKEN FROM AuthenKeys
            _Qry = "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys WHERE VanderMailLogIn = '" + u.id + "'";
            valid = (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER));

            if (dt != null)
            {
                _Qry = "";

                /*  Fields Not Send JSON
                 * [FTDataKey],[FTDateCreate],
                 * ,[StateAcknowledge],[AcknowledgeBy]" +
                    ",[AcknowledgeDate],[AcknowledgeTime],[StateAcknowledgeLock],[StateClose],[T2_Confirm_Ship_Date],[T2_Confirm_Price]" +
                    ",[T2_Confirm_Quantity],[T2_Confirm_OrderNo],[T2_Confirm_PO_Date],[T2_Confirm_By],[Estimatedeldate],[Actualdeldate]" +
                    ",[InvoiceNo],[RcvQty],[RcvDate],[StateRead] 
                 * */

                _Qry += "SELECT [VenderCode],[VendorName],[VendorLocation],[FactoryCode],[PONo],[PODate]" +
                    " ,[Shipto],[GarmentShipmentDestination],[MatrClass],[ItemSeq],[POItemCode],[MatrCode],[UPCCOMBOIM],[ContentCode]" +
                    " ,[CareCode],[Color],[ColorName],[GCW],[Size],[SizeMatrix],[Currency],[Price],[Quantity],[QtyUnit],[DeliveryDate]" +
                    " ,[Season],[Custporef],[Buy],[BuyNo],[Category],[Program],[SubProgram],[StyleNo],[StyleName],[POMatching1]" +
                    " ,[POMatching2],[POMatching3],[POMatching4],[POMatching5],[ItemMatching1],[ItemMatching2],[ItemMatching3]" +
                    " ,[ItemMatching4],[ItemMatching5],[Position],[Type],[PriceTerm],[PaymentTerm],[Remarkfrommer],[RemarkForPurchase]" +
                    " ,[InvoiceCmpCode],[CompanyName],[address1],[address2],[address3],[address4],[sysowner],[ZeroInspection],[GarmentShip]" +
                    " ,[OGACDate],[HITLink],[NIKECustomerPONo],[QRS],[PromoQty],[ActualQuantity],[POUploadDate],[POUploadTime],[POUploadBy]" +
                    " ,[CountryOfOrigin],[SaleOrderSaleOrderLine],[NikeSAPPOPONIKEPOLine],[NikematerialStyleColorway],[Modifire1],[Modifire2]" +
                    " ,[Modifire3],[MPOHZ],[ItemDescription],[BulkQRSSample],[ExtraMinQty],[EAGSystemUnitPriceERP3],[CCTotalpage],[Surcharge]" +
                    " ,[POApproveDate],[POIssueDate],[CLXorderconfirmationnumber],[FTYMerch],[HKMerch],[ChinaInsertCard],[P1pc2in1],[ReplyCode]" +
                    " ,[WovenLabelSizeLength],[ArgentinaImportNumber],[DownFill],[SYS_ID],[ChinasizeMatrixtype],[EAGERPItemNumber]" +
                    " ,[CompoundColororCCIMfor2in1CCIM],[CPO],[BhasaIndonesiaProductBrand],[SAFCODE],[Youthorder],[NFCproduct],[NeckneckopeningX2]" +
                    " ,[ChestbodywidthX2],[CenterBackbodylength],[WaistwaistrelaxedInseam],[PackQuantityQTYPERSELLINGUNIT],[Fabriccode]" +
                    " ,[PRODUCTDESCRIPTION],[PRODUCTCODEDESCRIPTION],[GARMENTSTYLEFIELD],[INSEAMSTYLE],[StateFlag] " +
                    " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[POToVender] AS POV " +
                    " WHERE POV.vendercode = '" + u.vencode + "'AND POV.StateAcknowledge = 0";
                dtPO = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                if (dtPO != null)
                {
                    // STAMP StateAcknowledge = 1 AND DATE + TIME
                    _Qry = " UPDATE[" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender  SET StateAcknowledge = 1";
                    _Qry += ", AcknowledgeBy = '" + u.id + "' , AcknowledgeDate = " + HITConnect.UFuncs.FormatDateDB +
                        ", AcknowledgeTime = " + HITConnect.UFuncs.FormatTimeDB + " ";
                    _Qry += " WHERE vendercode = '" + u.vencode + "'";

                    valid = (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER));
                    // END STAMP StateAcknowledge = 1 AND DATE + TIME


                    // COPY DATA POToVender TO POToVender_ACK
                    _Qry = "INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK ([FTDataKey],[FTDateCreate]," +
                        "[VenderCode],[VendorName],[VendorLocation],[FactoryCode],[PONo],[PODate],[Shipto],[GarmentShipmentDestination],[MatrClass]," +
                        "[ItemSeq],[POItemCode],[MatrCode],[UPCCOMBOIM],[ContentCode],[CareCode],[Color],[ColorName],[GCW],[Size],[SizeMatrix],[Currency]," +
                        "[Price],[Quantity],[QtyUnit],[DeliveryDate],[Season],[Custporef],[Buy],[BuyNo],[Category],[Program],[SubProgram],[StyleNo]," +
                        "[StyleName],[POMatching1],[POMatching2],[POMatching3],[POMatching4],[POMatching5],[ItemMatching1],[ItemMatching2],[ItemMatching3]" +
                        ",[ItemMatching4],[ItemMatching5],[Position],[Type],[PriceTerm],[PaymentTerm],[Remarkfrommer],[RemarkForPurchase],[InvoiceCmpCode]," +
                        "[CompanyName],[address1],[address2],[address3],[address4],[sysowner],[ZeroInspection],[GarmentShip],[OGACDate],[HITLink]," +
                        "[NIKECustomerPONo],[QRS],[PromoQty],[ActualQuantity],[POUploadDate],[POUploadTime],[POUploadBy],[CountryOfOrigin]," +
                        "[SaleOrderSaleOrderLine],[NikeSAPPOPONIKEPOLine],[NikematerialStyleColorway],[Modifire1],[Modifire2],[Modifire3],[MPOHZ]," +
                        "[ItemDescription],[BulkQRSSample],[ExtraMinQty],[EAGSystemUnitPriceERP3],[CCTotalpage],[Surcharge],[POApproveDate],[POIssueDate]," +
                        "[CLXorderconfirmationnumber],[FTYMerch],[HKMerch],[ChinaInsertCard],[P1pc2in1],[ReplyCode],[WovenLabelSizeLength]," +
                        "[ArgentinaImportNumber],[DownFill],[SYS_ID],[ChinasizeMatrixtype],[EAGERPItemNumber],[CompoundColororCCIMfor2in1CCIM],[CPO]," +
                        "[BhasaIndonesiaProductBrand],[SAFCODE],[Youthorder],[NFCproduct],[NeckneckopeningX2],[ChestbodywidthX2],[CenterBackbodylength]," +
                        "[WaistwaistrelaxedInseam],[PackQuantityQTYPERSELLINGUNIT],[Fabriccode],[PRODUCTDESCRIPTION],[PRODUCTCODEDESCRIPTION]," +
                        "[GARMENTSTYLEFIELD],[INSEAMSTYLE])";
                    _Qry += " SELECT [FTDataKey],[FTDateCreate],[VenderCode],[VendorName],[VendorLocation],[FactoryCode],[PONo],[PODate],[Shipto],[GarmentShipmentDestination],[MatrClass]," +
                        "[ItemSeq],[POItemCode],[MatrCode],[UPCCOMBOIM],[ContentCode],[CareCode],[Color],[ColorName],[GCW],[Size],[SizeMatrix],[Currency]," +
                        "[Price],[Quantity],[QtyUnit],[DeliveryDate],[Season],[Custporef],[Buy],[BuyNo],[Category],[Program],[SubProgram],[StyleNo]," +
                        "[StyleName],[POMatching1],[POMatching2],[POMatching3],[POMatching4],[POMatching5],[ItemMatching1],[ItemMatching2],[ItemMatching3]" +
                        ",[ItemMatching4],[ItemMatching5],[Position],[Type],[PriceTerm],[PaymentTerm],[Remarkfrommer],[RemarkForPurchase],[InvoiceCmpCode]," +
                        "[CompanyName],[address1],[address2],[address3],[address4],[sysowner],[ZeroInspection],[GarmentShip],[OGACDate],[HITLink]," +
                        "[NIKECustomerPONo],[QRS],[PromoQty],[ActualQuantity],[POUploadDate],[POUploadTime],[POUploadBy],[CountryOfOrigin]," +
                        "[SaleOrderSaleOrderLine],[NikeSAPPOPONIKEPOLine],[NikematerialStyleColorway],[Modifire1],[Modifire2],[Modifire3],[MPOHZ]," +
                        "[ItemDescription],[BulkQRSSample],[ExtraMinQty],[EAGSystemUnitPriceERP3],[CCTotalpage],[Surcharge],[POApproveDate],[POIssueDate]," +
                        "[CLXorderconfirmationnumber],[FTYMerch],[HKMerch],[ChinaInsertCard],[P1pc2in1],[ReplyCode],[WovenLabelSizeLength]," +
                        "[ArgentinaImportNumber],[DownFill],[SYS_ID],[ChinasizeMatrixtype],[EAGERPItemNumber],[CompoundColororCCIMfor2in1CCIM],[CPO]," +
                        "[BhasaIndonesiaProductBrand],[SAFCODE],[Youthorder],[NFCproduct],[NeckneckopeningX2],[ChestbodywidthX2],[CenterBackbodylength]," +
                        "[WaistwaistrelaxedInseam],[PackQuantityQTYPERSELLINGUNIT],[Fabriccode],[PRODUCTDESCRIPTION],[PRODUCTCODEDESCRIPTION]," +
                        "[GARMENTSTYLEFIELD],[INSEAMSTYLE] FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender AS POV " +
                        "WHERE POV.vendercode = '" + u.vencode + "'AND POV.StateAcknowledge = 0";

                    valid = (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER));
                    // END COPY DATA POToVender TO POToVender_ACK

                }
            }
            //string jsondata = (valid) ? JsonConvert.SerializeObject(dtPO) : "NOT FOUND";
            string jsondata = JsonConvert.SerializeObject(dtPO);
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
