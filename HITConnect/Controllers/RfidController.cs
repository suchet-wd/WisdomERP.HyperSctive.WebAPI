using Newtonsoft.Json;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HyperActive.Controllers
{
    public class RfidController : ApiController
    {
        [HttpPost]
        [Route("api/GetRfidInfo/")]
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
                if (APIRfidBarcode.isParentBundleBarcode(rb.ParentBundleBarcode) == false)
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
        } // End GetRfidInfo


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
        } // End GetStationInOut


        [HttpPost]
        [Route("api/GetBoxInfo/")]
        public HttpResponseMessage GetBoxInfo([FromBody] APIBoxInfo value)
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
                _Qry += "IF EXISTS( \n   SELECT TOP 1 fb.FinishBoxIsEmpty \n";
                _Qry += "   FROM [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TSMGtoWisdom_FinishBoxStatus AS fb WITH (NOLOCK) \n";
                _Qry += "   WHERE fb.FinishBoxRfid = '" + b.FinishBoxRfid + "' AND FinishBoxBarcode = '" + b.FinishBoxBarcode + "' \n";
                _Qry += ") \n";
                
                _Qry += "   BEGIN \n";
                _Qry += "      UPDATE [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TSMGtoWisdom_FinishBoxStatus ";
                _Qry += "      SET FinishBoxIsEmpty = '" + ((b.FinishBoxIsEmpty) ? "1" : "0") + "' \n";
                _Qry += "      WHERE FinishBoxRfid = '" + b.FinishBoxRfid + "' AND FinishBoxBarcode = '" + b.FinishBoxBarcode + "' \n";
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
                i++;
            }
            _Qry += " IF @RecCount = " + i;
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
        } // End GetBoxInfo

    } // End Class
} // End namespace