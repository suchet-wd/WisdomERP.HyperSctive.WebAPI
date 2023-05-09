using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HITConnect
{
    public class UserToken
    {
        [JsonProperty("id", Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty("pwd", Required = Required.Always)]
        public string pwd { get; set; }

        [JsonProperty("venderGroup", Required = Required.Always)]
        public string venderGroup { get; set; }

        public static List<string> ValidateField(UserToken value)
        {
            string msgError = "";
            string statecheck = "0";
            if (value.id == "")
            {
                statecheck = "2";
                msgError = "Please check ID!!!";
            }
            else
            {
                if (value.pwd == "")
                {
                    statecheck = "2";
                    msgError = "Please check password!!!";
                }
                else
                {
                    if (value.venderGroup == "")
                    {
                        statecheck = "2";
                        msgError = "Please check Vender Group!!!";
                    }
                }
            }
            List<string> _result = new List<string>();
            _result.Add(statecheck);
            _result.Add(msgError);
            return _result;
        }

        public static DataTable GetDTUserValidate(WSM.Conn.SQLConn Cnn, UserToken value)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string _Qry = "";
                if (value.id != "" && value.venderGroup != "")
                {
                    _Qry = " SELECT V.Pwd AS pwd, V.VenderMailLogIn AS VenderMailLogIn , VUP.VenderGrp AS VenderGrp " +
                        " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V " +
                        " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VenderMailLogIn = V.VenderMailLogIn  " +
                        " LEFT JOIN VenderUserPermissionCmp VUP ON V.VenderMailLogIn = VUP.VenderMailLogIn " +
                        " WHERE V.VenderMailLogIn = '" + value.id + "' AND VUP.VenderGrp = '" + value.venderGroup + "'";
                }
                else
                {
                    return null;
                }
                return Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
