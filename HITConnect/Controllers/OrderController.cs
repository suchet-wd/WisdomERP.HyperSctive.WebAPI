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
    public class ProductionController : ApiController
    {
        [HttpPost]
        [Route("api/GetOrderInfo/")]
        public HttpResponseMessage GetOrderInfo([FromBody] APIOrder value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            List<string> _result = UserController.ValidateField(value.authen);
            if (_result.Count != 0)
            {
                dts.Rows.Add(new Object[] { 1, _result[1].ToString() });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            if (UserController.CheckUserPermission(Cnn, value.authen) == true)
            {
                _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API1 '" + value.OrderNo + "'";
                try
                {
                    if (UserController.CheckUserPermission(Cnn, value.authen) == true)
                    {

                    }
                    docXML = Cnn.GetDataXML(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
                    JSONresult = JsonConvert.SerializeObject(docXML);
                    //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented
                    JSONresult = JSONresult.Replace("\"[]\"", "[]");
                    JSONresult = JSONresult.Replace("[[],", "[");
                    JSONresult = JSONresult.Replace(":\"_", ":\"");
                    //JSONresult = JSONresult.Replace("}}", "}");
                    JSONresult = JSONresult.Replace("{\"root\":", "");
                    JSONresult = JSONresult.Replace("\"_\",", "");
                    JSONresult = JSONresult.Replace("AssortQuantity\":{", "AssortQuantity\":[{");
                    JSONresult = JSONresult.Replace("},\"ProductOperation", "}],\"ProductOperation");
                    JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                dts.Rows.Add(new Object[] { 1, "Please check User authentication!!!" });
                JSONresult = JsonConvert.SerializeObject(dts);
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
        } // End API GetOrderInfo


        [HttpPost]
        [Route("api/GetOrderDetailInfo/")]
        public HttpResponseMessage GetOrderDetailInfo([FromBody] APIOrder value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            List<string> _result = UserController.ValidateField(value.authen);
            if (_result.Count != 0)
            {
                dts.Rows.Add(new Object[] { 1, _result[1].ToString() });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            if (UserController.CheckUserPermission(Cnn, value.authen) == true)
            {
                _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API2 '" + value.OrderNo + "'";
                try
                {
                    docXML = Cnn.GetDataXML(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
                    JSONresult = JsonConvert.SerializeObject(docXML);
                    JSONresult = JSONresult.Replace("PartDetail\":[\"[]\",{", "PartDetail\":[{");
                    JSONresult = JSONresult.Replace("PooDetail\":[\"[]\",", "PooDetail\":[");
                    JSONresult = JSONresult.Replace("\"SpreadingRatio\":[\"[]\",", "\"SpreadingRatio\":[");
                    JSONresult = JSONresult.Replace("\"\",", "");
                    JSONresult = JSONresult.Replace("{\"root\":", "");
                    JSONresult = JSONresult.Replace("\"Route\":[\"[]\",{\"Station", "\"Route\":[{\"Station");
                    JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                dts.Rows.Add(new Object[] { 1, "Please check User authentication!!!" });
                JSONresult = JsonConvert.SerializeObject(dts);
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
        } // End API GetOrderDetailInfo
    }
}