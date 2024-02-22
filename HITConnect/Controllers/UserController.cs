using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HyperConvert.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("api/GetToken/")]
        public HttpResponseMessage GetToken([FromBody] UserToken value)
        {
            int statecheck = 0;
            string msgError = "";
            string token = "";
            DataTable dts = new DataTable();
            dts.Columns.Add("id", typeof(string));
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            

            // Check id + pwd + vender group
            List<string> _result = UserToken.ValidateField(value);
            statecheck = int.Parse(_result[0]);
            msgError = _result[1];
            // End Check id + pwd + vender group

            try
            {
                if (statecheck != 2)
                {
                    WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
                    DataTable dt = HyperConvert.UserToken.GetDTUserValidate(Cnn, value);

                    // Delete Old Token from Database
                    UserAuthen.DelAuthenKey(Cnn, value.id);

                    // Save ID & Token to DB then send JSON file
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (value.pwd == WSM.Conn.DB.FuncDecryptDataServer(row["pwd"].ToString()))
                            {
                                string _Qry = "";
                                _Qry = "INSERT INTO [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.AuthenKeys (VenderMailLogIn, DataKey) ";
                                _Qry += " VALUES ('" + row["VenderMailLogIn"].ToString() + "', NEWID())";

                                if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.HITECH_MERCHAN))
                                {
                                    statecheck = 1;
                                    msgError = "Successful";
                                }
                                _Qry = "SELECT DataKey FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.AuthenKeys WITH ( NOLOCK ) " +
                                    " WHERE VenderMailLogIn = '" + value.id + "'";
                                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_MERCHAN);
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
            string jsondata = JsonConvert.SerializeObject(dts);

            return (statecheck == 1) ?
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                } :
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + statecheck +
                    (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + msgError + (char)34 + "}",
                    System.Text.Encoding.UTF8, "application/json")
                };
        }
    }
}
