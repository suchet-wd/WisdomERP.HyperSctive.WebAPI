using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Web.Http;

namespace HyperActive.Controllers
{
    public class SystemController : ApiController
    {
        [HttpPost]
        [Route("api/ResetSendAPI/")]
        public HttpResponseMessage ResetSendAPI([FromBody] APIResend value)
        {
            string JSONresult = "";
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            try
            {
                WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
                dts.Rows.Add((ResetSendAPI(Cnn, value) == true) ? new Object[] { 0, "Successful" } : new Object[] { 1, "Unsuccessful !!!" });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            } // End Try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dts.Rows.Add(new Object[] { 1, "Error !!!" });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            } // End catch
        } // End ResendAPI

        // ---------------------------------------------------------------------------------------------------

        public static bool ResetSendAPI(WSM.Conn.SQLConn Cnn, APIResend value)
        {
            bool _state = false;
            if (value.ApiNo != "" && value.DocNo != "")
            {
                string _Qry = "";
                _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].[dbo].[SP_SET_Resend_Hyperconvert_API] ";
                _Qry += "@FTApiNo = '" + value.ApiNo + "', @FTDocumentNo = '" + value.DocNo + "'";
                try
                {
                    _state = (Cnn.ExeTransaction(_Qry) == 0) ? true : false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return _state;
        } // Function ResendAPI

        // ---------------------------------------------------------------------------------------------------

        // Start CheckAPIsStatus
        [HttpPost]
        [Route("api/CheckAPIsStatus/")]
        public HttpResponseMessage CheckAPIsStatus()
        {
            //string _Qry = "";
            string JSONresult = "";
            DataTable dts = new DataTable();
            dts.Columns.Add("APINo", typeof(string));
            dts.Columns.Add("APIUrl", typeof(string));
            dts.Columns.Add("StatusConnection", typeof(string));
            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                string[] apiurls = WSM.Conn.DB.APIURL.Split(';');
                int i = 1;
                foreach (string apiurl in apiurls)
                {
                    try
                    {
                        Ping pingSender = new Ping();
                        PingReply reply = pingSender.Send(apiurl);
                        if (reply.Status == IPStatus.Success)
                        {
                            dts.Rows.Add(new Object[] { "API" + i, apiurl, "OK" });
                        }
                    }
                    catch (Exception ex)
                    {
                        dts.Rows.Add(new Object[] { "API" + i, apiurl, "Connection Error!!!" });
                    }
                    //Console.WriteLine(reply);
                    i++;
                }

                JSONresult = JsonConvert.SerializeObject(dts);

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
            }
            catch (System.Net.WebException ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };

            }
        } // End CheckAPIsStatus

        // Start CheckIPStatus
        [HttpPost]
        [Route("api/CheckIPStatus/")]
        public HttpResponseMessage CheckIPStatus()
        {
            string JSONresult = "";
            DataTable dts = new DataTable();
            dts.Columns.Add("APINo", typeof(string));
            dts.Columns.Add("APIUrl", typeof(string));
            dts.Columns.Add("StatusConnection", typeof(string));

            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                string[] apiips = WSM.Conn.DB.APIIP.Split(';');
                int i = 1;
                foreach (string apiip in apiips)
                {
                    try
                    {
                        Ping pingSender = new Ping();
                        PingReply reply = pingSender.Send(apiip);
                        if (reply.Status == IPStatus.Success)
                        {
                            dts.Rows.Add(new Object[] { "API" + i, apiip, "OK" });
                        }
                    }
                    catch (Exception ex)
                    {
                        dts.Rows.Add(new Object[] { "API" + i, apiip, "Connection Error!!!" });
                    }
                    //Console.WriteLine(reply);
                    i++;
                }

                JSONresult = JsonConvert.SerializeObject(dts);

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

            }
            catch (System.Net.WebException ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };

            }
        } // End CheckIPStatus

    } //class
} // namespace