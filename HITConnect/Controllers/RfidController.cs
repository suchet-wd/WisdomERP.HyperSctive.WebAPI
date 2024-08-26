using Newtonsoft.Json;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace HyperActive.Controllers
{
    public class RfidController : ApiController
    {
        [HttpPost]
        [Route("api/GetRfidInfo/")] // API 4 : GetRfidInfo
        public HttpResponseMessage GetRfidInfo([FromBody] APIRfid value)
        {
            string _Qry = "";
            string jsondata = "";
            //WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));


            // Checking All Barcode
            foreach (APIRfidBarcode rb in value.BundleRfidBarcodeList)
            {
                if (APIRfidBarcode.isParentBundleBarcode(rb) == false)
                {
                    dts.Rows.Add(new Object[] { "1", "Please check Parent Bundle Barcode (" + rb.ParentBundleBarcode + ")" });
                    jsondata = JsonConvert.SerializeObject(dts);
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                    };
                }

                foreach (APIRfidBarcodeDetail bd in rb.BundleBarcode)
                {
                    if (APIRfidBarcode.isBundleBarcode(bd) == false)
                    {
                        dts.Rows.Add(new Object[] { "1", "Please check BundleRfidBarcodeList Details (" + bd.BundleBarcode + ")" });
                        jsondata = JsonConvert.SerializeObject(dts);
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                        };
                    }
                }
            }

            int i = 1;
            int j = 1;

            _Qry += " DECLARE @RecCount int = 0 \n";
            _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) \n";
            _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) \n\n";
            _Qry += " DECLARE @Message nvarchar(500) = '' \n\n";

            _Qry += " BEGIN TRANSACTION \n";
            _Qry += " BEGIN TRY \n";

            foreach (APIRfidBarcode b in value.BundleRfidBarcodeList)
            {
                j++;
                foreach (APIRfidBarcodeDetail bd in b.BundleBarcode)
                {
                    _Qry += "INSERT INTO [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TSMGtoWisdom_Staging \n";
                    _Qry += "(FTInsUser, FDInsDate, FTInsTime, FTBoxRfId, FTBoxBarCode, FTBundleRfId, FTBundleParentBarcode \n";
                    _Qry += ", FTBundleBarcode, FTStateProcess, FTSMTimeStamp, FTJob, FTColorWay, FTSize, FNQty ) \n";
                    _Qry += "VALUES('" + value.authen.id + "', @Date, @Time, \n";
                    _Qry += "'" + value.BoxRfid + "', '" + value.BoxBarcode + "', '" + b.Rfid + "', '" + b.ParentBundleBarcode + "' \n";
                    _Qry += ", '" + bd.BundleBarcode + "', '0', '" + value.TimeStamp + "', '" + bd.Job + "', '" + bd.ColorWay + "' \n";
                    _Qry += ", '" + bd.Size + "', '" + bd.BundleQuantity + "' \n";
                    _Qry += "); \n";
                    _Qry += "SET @RecCount = @RecCount + @@ROWCOUNT \n\n";
                    i++;
                }
            }
            _Qry += " IF @RecCount <> " + i;
            _Qry += "   BEGIN \n";
            _Qry += "       COMMIT TRANSACTION \n";
            _Qry += "   END \n\n";

            _Qry += " ELSE \n";

            _Qry += "   BEGIN \n";
            _Qry += "       ROLLBACK TRANSACTION \n";
            _Qry += "   END \n\n";
            _Qry += " END TRY \n\n";

            _Qry += " BEGIN CATCH \n";
            _Qry += "   BEGIN \n";
            _Qry += "       ROLLBACK TRANSACTION \n";
            _Qry += "   END \n\n";

            _Qry += " END CATCH \n";

            if (new WSM.Conn.SQLConn().ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE))
            {
                dts.Rows.Add(new Object[] { "0", "Save total " + j + " Rfid (Total " + i + " Records) Successful." });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                dts.Rows.Add(new Object[] { "1", "Cannot Save " + j + " Rfid (Total " + i + " Records)!!!" });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
        } // End API 4 : GetRfidInfo

        // -------------------------------------------------------------------------------------------------------------------

        // // Start GetStationInOut API_5 : Station In Out SM -> WSM
        [HttpPost]
        [Route("api/GetStationInOut/")]
        public HttpResponseMessage GetStationInOut([FromBody] APIStationInOut value)
        {
            string _Qry = "";
            string jsondata = "";
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            int i = 1;

            _Qry += " DECLARE @RecCount int = 0 \n";
            _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) \n";
            _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) \n\n";
            _Qry += " DECLARE @Message nvarchar(500) = '' \n\n";

            _Qry += " BEGIN TRANSACTION \n";
            _Qry += " BEGIN TRY \n";

            foreach (string listBoxRfid in value.BoxRfidList)
            {
                _Qry += "INSERT INTO [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TSMGtoWisdom_StationInOut \n";
                _Qry += "(FTInsUser, FDInsDate, FTInsTime, InStation, OutStation, BoxRfid, FDDateTimeStamp ) \n";
                _Qry += "VALUES('', @Date, @Time, \n";
                _Qry += "'" + value.InStation + "', '" + value.OutStation + "', '" + listBoxRfid + "', Getdate() \n";
                _Qry += "); \n";
                _Qry += "SET @RecCount = @RecCount + @@ROWCOUNT \n\n";
                i++;
            }
            _Qry += " IF @RecCount <> " + i;
            _Qry += "   BEGIN \n";
            _Qry += "       COMMIT TRANSACTION \n";
            _Qry += "   END \n\n";

            _Qry += " ELSE \n";

            _Qry += "   BEGIN \n";
            _Qry += "       ROLLBACK TRANSACTION \n";
            _Qry += "   END \n\n";
            _Qry += " END TRY \n\n";

            _Qry += " BEGIN CATCH \n";
            _Qry += "   BEGIN \n";
            _Qry += "       ROLLBACK TRANSACTION \n";
            _Qry += "   END \n\n";

            _Qry += " END CATCH \n";

            if (new WSM.Conn.SQLConn().ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE))
            {
                dts.Rows.Add(new Object[] { "0", "Save total " + i + " Rfid (Total " + i + " Records) Successful." });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                dts.Rows.Add(new Object[] { "1", "Cannot Save " + i + " Rfid (Total " + i + " Records)!!!" });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
        } // End GetStationInOut API_5 : Station In Out


        // -------------------------------------------------------------------------------------------------------------------


        // // Start GetStationInOut API_6.1 : Bundle Update WSM -> SM
        [HttpPost]
        [Route("api/GetBundleUpdate/")]
        public HttpResponseMessage GetBundleUpdate([FromBody] APIBundleUpdate value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            if (value.BundleBarCode == "")
            {
                dts.Rows.Add(new Object[] { 1, "Please send Bundle BarCode for get information!!!" });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API6 @BundleBarCode = '" + value.BundleBarCode + "'";
            try
            {
                docXML = Cnn.GetDataXML(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
                JSONresult = JsonConvert.SerializeObject(docXML);
                //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
                JSONresult = JSONresult.Replace("\"[]\"", "[]");
                JSONresult = JSONresult.Replace("[[],", "[");
                JSONresult = JSONresult.Replace("{\"root\":", "");
                JSONresult = JSONresult.Replace("\"_\",", "\"\"");
                JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
                JSONresult = JSONresult.Replace("}]}", "}]");
                JSONresult = JSONresult.Replace("{\"DefectDetails\":[", "[");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            if (JSONresult.Length > 0)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }
        } // End GetStationInOut API_6.1 : Bundle Update WSM -> SM

        // -------------------------------------------------------------------------------------------------------------------


        // // Start GetStationInOut API_6.2 : All Bundle Update WSM -> SM
        [HttpPost]
        [Route("api/GetBundleUpdatePeriod/")]
        public HttpResponseMessage GetAllBundleUpdate([FromBody] APIBundleUpdatePeriod value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            if (value.StateGetAll == "1")
            {
                _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API6_All ";
                _Qry += "@StateGetAll = '" + value.StateGetAll + "'";
                if (value.DateStart != "" && value.DateEnd != "")
                {
                    _Qry += ", @DateStart = '" + value.DateStart + "'";
                    _Qry += ", @DateEnd = '" + value.DateEnd + "'";
                }
                else if (value.DateStart == "" && value.DateEnd == "9999/99/99")
                {
                    _Qry += ", @DateStart = '" + value.DateStart + "'";
                    _Qry += ", @DateEnd = '" + value.DateEnd + "'";
                }
                else
                {
                    if (value.DateStart == "" && value.DateEnd != "")
                    {
                        dts.Rows.Add(new Object[] { 1, "Start Date Not Found !!!" });
                        JSONresult = JsonConvert.SerializeObject(dts);
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                        };
                    }
                    else if (value.DateStart != "" && value.DateEnd == "")
                    {
                        dts.Rows.Add(new Object[] { 1, "End Date Not Found!!!" });
                        JSONresult = JsonConvert.SerializeObject(dts);
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                        };
                    }
                }

                try
                {
                    docXML = Cnn.GetDataXML(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
                    JSONresult = JsonConvert.SerializeObject(docXML);
                    JSONresult = JSONresult.Replace("\"[]\"", "[]");
                    JSONresult = JSONresult.Replace("[[],", "[");
                    JSONresult = JSONresult.Replace("{\"root\":", "");
                    JSONresult = JSONresult.Replace("\"_\",", "\"\"");
                    JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                    };
                }
            }

            if (JSONresult.Length > 0)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }
        } // End GetStationInOut API_6.2 : All Bundle Update WSM -> SM

        // -------------------------------------------------------------------------------------------------------------------


        // // Start GetStationInOut API_7 : Station Results WSM -> SM
        [HttpPost]
        [Route("api/GetStationResults/")]
        public HttpResponseMessage GetStationResults([FromBody] APIStationResults value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            if (value.BoxRfid == "" && value.BoxBarcode == "")
            {
                dts.Rows.Add(new Object[] { 1, "Please send Box RFID or Box Barcode for get information!!!" });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            if (value.BoxRfid != "")
            {
                _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API7 @BoxRfid = '" + value.BoxRfid + "'";

            }
            if (value.BoxBarcode != "")
            {
                _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API7 @BoxBarcode = '" + value.BoxBarcode + "'";
            }

            try
            {
                docXML = Cnn.GetDataXML(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
                JSONresult = JsonConvert.SerializeObject(docXML);
                //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
                JSONresult = JSONresult.Replace("\"[]\"", "[]");
                JSONresult = JSONresult.Replace("[[],", "[");
                JSONresult = JSONresult.Replace(":\"_", ":\"");
                //JSONresult = JSONresult.Replace("}}", "}");
                JSONresult = JSONresult.Replace("{\"root\":", "");
                JSONresult = JSONresult.Replace("\"_\",", "\"\"");
                JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            if (JSONresult.Length > 0)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }
        } // End GetStationInOut API_7 : Station Results WSM -> SM

        // -------------------------------------------------------------------------------------------------------------------

        // Start GetBoxInfo API_8 : Finish Box Status
        [HttpPost]
        [Route("api/FinishBoxStatus/")]
        public HttpResponseMessage FinishBoxStatus([FromBody] APIBoxInfo value)
        {
            string _Qry = "";
            string jsondata = "";
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            int i = 0;

            _Qry += " DECLARE @RecCount int = 0 \n";
            _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) \n";
            _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) \n\n";
            _Qry += " DECLARE @Message nvarchar(500) = '' \n\n";
            _Qry += " BEGIN TRANSACTION \n";
            _Qry += " BEGIN TRY \n";

            foreach (BoxInfo b in value.BoxRfidList)
            {
                if (b.FinishBoxIsEmpty == true)
                {
                    _Qry += "IF EXISTS( SELECT TOP 1 ss.FTStateClearBox \n";
                    _Qry += "   FROM [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TPROSendSuplDefect AS ss WITH (NOLOCK)  \n";
                    _Qry += "    WHERE ss.FTStateClearBox = 0 AND ";
                    _Qry += "    (ss.FTRFIDNo = '" + b.FinishBoxRfid + "' " + " OR ss.FTBoxNo = '" + b.FinishBoxRfid + "' ";
                    _Qry += "     OR ss.FTBoxNo = '" + b.FinishBoxBarcode + "' OR ss.FTBoxNo = '" + b.FinishBoxBarcode + "') \n";
                    _Qry += ") \n";

                    _Qry += "   BEGIN \n";
                    _Qry += "      UPDATE [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TPROSendSuplDefect \n";
                    _Qry += "      SET FTStateClearBox = 1, FDClearBoxDate =  @Date, FTClearBoxTime = @Time \n";
                    _Qry += "      WHERE FTStateClearBox = 0 AND ";
                    _Qry += "      (FTRFIDNo = '" + b.FinishBoxRfid + "' " + " OR FTRFIDNo = '" + b.FinishBoxRfid + "' ";
                    _Qry += "       OR FTBoxNo = '" + b.FinishBoxBarcode + "' OR FTBoxNo = '" + b.FinishBoxBarcode + "'); \n";
                    _Qry += "\n";
                    _Qry += "      INSERT INTO [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TSMGtoWisdom_FinishBoxStatus \n";
                    _Qry += "      (FTInsUser, FDInsDate, FTInsTime, FinishBoxRfid, FinishBoxBarcode, FinishBoxIsEmpty ) \n";
                    _Qry += "      VALUES('', @Date, @Time, \n";
                    _Qry += "      '" + b.FinishBoxRfid + "', '" + b.FinishBoxBarcode + "', '" + ((b.FinishBoxIsEmpty) ? "1" : "0") + "' \n";
                    _Qry += "      ); \n";
                    _Qry += "   END \n";

                    _Qry += "ELSE \n";

                    _Qry += "   BEGIN \n";
                    _Qry += "      INSERT INTO [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TSMGtoWisdom_FinishBoxStatus \n";
                    _Qry += "      (FTInsUser, FDInsDate, FTInsTime, FinishBoxRfid, FinishBoxBarcode, FinishBoxIsEmpty ) \n";
                    _Qry += "      VALUES('', @Date, @Time, \n";
                    _Qry += "      '" + b.FinishBoxRfid + "', '" + b.FinishBoxBarcode + "', '" + ((b.FinishBoxIsEmpty) ? "1" : "0") + "' \n";
                    _Qry += "      ); \n";
                    _Qry += "   END \n\n";
                    _Qry += "SET @RecCount = @RecCount + @@ROWCOUNT \n\n";
                }
                else
                {
                    _Qry += "INSERT INTO [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TSMGtoWisdom_FinishBoxStatus \n";
                    _Qry += "(FTInsUser, FDInsDate, FTInsTime, FinishBoxRfid, FinishBoxBarcode, FinishBoxIsEmpty ) \n";
                    _Qry += "VALUES('', @Date, @Time, \n";
                    _Qry += "'" + b.FinishBoxRfid + "', '" + b.FinishBoxBarcode + "', '" + ((b.FinishBoxIsEmpty) ? "1" : "0") + "' \n";
                    _Qry += "); \n";
                    _Qry += "SET @RecCount = @RecCount + @@ROWCOUNT \n\n";
                }
                i++;
            }
            _Qry += " IF @RecCount = " + i + "\n";
            _Qry += "   BEGIN \n";
            _Qry += "       COMMIT TRANSACTION \n";
            _Qry += "   END \n\n";

            _Qry += " ELSE \n";

            _Qry += "   BEGIN \n";
            _Qry += "       ROLLBACK TRANSACTION \n";
            _Qry += "   END \n\n";
            _Qry += " END TRY \n\n";

            _Qry += " BEGIN CATCH \n";
            _Qry += "   BEGIN \n";
            _Qry += "       ROLLBACK TRANSACTION \n";
            _Qry += "   END \n\n";

            _Qry += " END CATCH \n";

            if (new WSM.Conn.SQLConn().ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE))
            {
                dts.Rows.Add(new Object[] { "0", "Save total " + i + " Rfid (Total " + i + " Records) Successful." });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                dts.Rows.Add(new Object[] { "1", "Cannot Save " + i + " Rfid (Total " + i + " Records)!!!" });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
        } // End GetBoxInfo API_8 : Finish Box Status


        // -------------------------------------------------------------------------------------------------------------------


        // // Start GetStationInOut API_10 : Out Sourse Status WSM -> SM
        [HttpPost]
        [Route("api/OutSourseStatus/")]
        public HttpResponseMessage OutSourseStatus([FromBody] APIOutSourseStatus value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            if (value.BundleBarcode == "" )
            {
                dts.Rows.Add(new Object[] { 1, "Please send Send Supl Barcode for get information!!!" });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            if (value.BundleBarcode != "")
            {
                _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API10 @BarcodeSendSuplNo = '" + value.BundleBarcode + "'";

            }
            
            try
            {
                docXML = Cnn.GetDataXML(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
                JSONresult = JsonConvert.SerializeObject(docXML);
                //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
                JSONresult = JSONresult.Replace("\"[]\"", "[]");
                JSONresult = JSONresult.Replace("[[],", "[");
                JSONresult = JSONresult.Replace(":\"_", ":\"");
                //JSONresult = JSONresult.Replace("}}", "}");
                JSONresult = JSONresult.Replace("{\"root\":", "");
                JSONresult = JSONresult.Replace("\"_\",", "\"\"");
                JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
                JSONresult = "[" + JSONresult + "]";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            if (JSONresult.Length > 0)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }
        } // End GetStationInOut API_10 : Out Sourse Status WSM -> SM


    } // End Class
} // End namespace