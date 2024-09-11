using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace HyperActive.Controllers
{
    public class PackController : ApiController
    {

        // Start GetBoxInfo API_9 : Pack Results
        [HttpPost]
        [Route("api/GetPackResults/")]
        public HttpResponseMessage GetPackResults([FromBody] APIPackResults value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API9 ";
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
                JSONresult = JSONresult.Replace("\"_\",", "\"\",");
                JSONresult = JSONresult.Replace("{\"PackResults\":[", "[");
                JSONresult = JSONresult.Replace("]}", "]");
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
        } // End GetBoxInfo API_9 : 


        // Start GetBoxInfo API_9.2 : Pack Results GetPackResultsByPeriod
        [HttpPost]
        [Route("api/GetPackResultsByPeriod/")]
        public HttpResponseMessage GetPackResultsByPeriod([FromBody] APIPackResults value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API9byDate ";
            //_Qry += "@StateGetAll = '" + value.StateGetAll + "'";
            if (value.DateStart != "" && value.DateEnd != "")
            {
                _Qry += "@DateStart = '" + value.DateStart + "'";
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
                else if (value.DateStart == "" && value.DateEnd == "")
                {
                    dts.Rows.Add(new Object[] { 1, "Start & End Date Not Found!!!" });
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
                //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
                //JSONresult = JSONresult.Replace("\"BundleInfo\":[\"[]\",{", "\"BundleInfo\":[{");
                JSONresult = JSONresult.Replace("\"[]\"", "[]");
                //JSONresult = JSONresult.Replace("[[],", "[");
                JSONresult = JSONresult.Replace(":\"_", ":\"");
                JSONresult = JSONresult.Replace(":[[],{\"", ":[{\"");
                //JSONresult = JSONresult.Replace("}}", "}");
                JSONresult = JSONresult.Replace("{\"root\":", "");
                JSONresult = JSONresult.Replace("\"_\",", "\"\",");
                JSONresult = JSONresult.Replace("{\"PackResults\":[", "[");
                //JSONresult = JSONresult.Replace("]}", "]");
                JSONresult = JSONresult.Substring(0, JSONresult.Length - 2);
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
        } // End GetBoxInfo API_9.2 : Pack Results GetPackResultsByPeriod


        // Start GetBoxInfo API_9.3 : Pack Results GetPackResultsByState2
        [HttpPost]
        [Route("api/GetAPI9byWaitState/")]
        public HttpResponseMessage GetAPI9byWaitState([FromBody] APIPackResults value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API9byState2 ";
            //_Qry += "@StateGetAll = '" + value.StateGetAll + "'";
            if (value.DateStart != "" && value.DateEnd != "")
            {
                _Qry += "@DateStart = '" + value.DateStart + "'";
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
                else if (value.DateStart == "" && value.DateEnd == "")
                {
                    dts.Rows.Add(new Object[] { 1, "Start & End Date Not Found!!!" });
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
                JSONresult = JSONresult.Replace(":\"_", ":\"");
                JSONresult = JSONresult.Replace(":[[],{\"", ":[{\"");
                JSONresult = JSONresult.Replace("{\"root\":", "");
                JSONresult = JSONresult.Replace("\"_\",", "\"\",");
                JSONresult = JSONresult.Replace("{\"PackResults\":[", "[");
                JSONresult = JSONresult.Substring(0, JSONresult.Length - 2);
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
        } // End GetBoxInfo API_9 : Pack Results GetPackResultsByState2
    }
}
