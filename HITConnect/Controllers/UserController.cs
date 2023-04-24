using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class UserController : ApiController
    {
        /*
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "String2" };
        }
        */

        [HttpPost]
        [Route("api/GetToken/")]
        public HttpResponseMessage GetToken([FromBody] UserAuthen value)
        {
            string _Qry = "";
            int statecheck = 0;
            string msgError = "";
            string token = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("id", typeof(string));
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            // Delete Old Token from Database
            UserAuthen.DelAuthenKey(Cnn, value.id);
            if (value.id == "")
            {
                statecheck = 2;
                msgError = "Please check ID!!!";
            }
            else
            {
                if (value.pwd == "")
                {
                    statecheck = 2;
                    msgError = "Please check password!!!";
                }
                else
                {
                    if (value.venderGroup == "")
                    {
                        statecheck = 2;
                        msgError = "Please check Vender Group!!!";
                    }
                }
            }

            try
            {
                if (statecheck != 2)
                {
                    dt = HITConnect.UserAuthen.GetDTUserValidate(Cnn, value);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (value.pwd == WSM.Conn.DB.FuncDecryptDataServer(row["pwd"].ToString()))
                            {
                                _Qry = "INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys (VanderMailLogIn, DataKey) ";
                                _Qry += " VALUES ('" + row["VanderMailLogIn"].ToString() + "', NEWID())";

                                if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                                {
                                    statecheck = 1;
                                    msgError = "Successful";
                                }
                                _Qry = "SELECT DataKey FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys " +
                                    " WHERE VanderMailLogIn = '" + value.id + "'";
                                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                                foreach (DataRow r in dt.Rows)
                                {
                                    token = r["DataKey"].ToString();
                                }
                            }
                            else
                            {
                                token = "";
                                statecheck = 2;
                                msgError = "Password is not correct!!!";
                            }
                        }
                    }
                    else
                    {
                        token = "";
                        statecheck = 2;
                        msgError = "Please check User authentication!!!";

                    }
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
            dts.Rows.Add(new Object[] { value.id, statecheck, token, msgError });
            jsondata = JsonConvert.SerializeObject(dts);

            return (statecheck == 1) ?
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                } :
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + statecheck + (char)34 + "," +
                    (char)34 + "Refer" + (char)34 + ": " + (char)34 + msgError + (char)34 + "}",
                    System.Text.Encoding.UTF8, "application/json")
                };
        }
    }
}
