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
    public class CuttingController : ApiController
    {
        [HttpPost]
        [Route("api/GetCuttingInfo/")]
        public HttpResponseMessage GetCuttingInfo([FromBody] APICutting value)
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
                if (CuttingOrder.isPooNo(value.PooNo))
                {
                    //// Start API 3:  Bundle Info
                    _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API3 " + value.PooNo;
                    docXML = WSM.Conn.DB.GetDataXML(_Qry);

                    JSONresult = JsonConvert.SerializeObject(docXML);
                    JSONresult = JSONresult.Replace("\"\",", "");
                    JSONresult = JSONresult.Replace("\"[]\"", "[]");
                    JSONresult = JSONresult.Replace("[[],", "[");
                    JSONresult = JSONresult.Replace("\"_", "\"");
                    JSONresult = JSONresult.Replace("}}", "}");
                    JSONresult = JSONresult.Replace("{\"root\":", "");

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
                        Console.WriteLine("Error API No #3 DocNo = " + value.PooNo + "!!!");
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                        };

                    }
                    //Console.WriteLine("End API#3");
                }
                else
                {
                    Console.WriteLine("Error API No #3 DocNo = " + value.PooNo + "!!!");
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                    };
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
        } // End API GetCuttingInfo
    }
}
