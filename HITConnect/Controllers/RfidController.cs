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

                foreach (string s in rb.BundleBarcode)
                {
                    if (APIRfidBarcode.isBundleBarcode(s) == false)
                    {
                        dts.Rows.Add(new Object[] { "1", "Please check Bundle Barcode (" + s + ")" });
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
                foreach (string s in b.BundleBarcode)
                {
                    _Qry += "INSERT INTO [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TSMGtoWisdom_Staging \n";
                    _Qry += "(FTInsUser, FDInsDate, FTInsTime, FTBoxRfId, FTBoxBarCode, FTBundleRfId, FTBundleParentBarcode, FTBundleBarcode, FTStateProcess, FTSMTimeStamp) \n";
                    _Qry += "VALUES('" + value.authen.id + "', @Date, @Time, ";
                    _Qry += "'" + value.BoxRfid + "', '" + value.BoxBarcode + "', '" + b.Rfid + "', '" + b.ParentBundleBarcode + "', '" + s + "', '0', '" + value.TimeStamp + "'";
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
    } // End Class
} // End namespace