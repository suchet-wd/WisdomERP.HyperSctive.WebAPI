using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HITConnect
{

    public class UserAuthen
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("pwd")]
        public string pwd { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }

        //[JsonProperty("statecheck")]
        //public string statecheck { get; set; }

        [JsonProperty("venderCode")]
        public string venderCode { get; set; }

        [JsonProperty("venderGroup")]
        public string venderGroup { get; set; }

        public static DataTable GetDTUserValidate(WSM.Conn.SQLConn Cnn, UserAuthen value)
        {
            try
            {
                string _Qry = "";
                if (value.token == "")
                {
                    _Qry = " SELECT V.Pwd AS pwd, V.VanderMailLogIn AS VanderMailLogIn " +
                    " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V " +
                    " WHERE V.VanderMailLogIn = '" + value.id + "'";
                }
                else
                {
                    if (value.token != "" && value.venderCode != "")
                    {
                        _Qry = " SELECT V.Pwd AS Pws, V.VanderMailLogIn AS VanderMailLogIn, ATK.DataKey AS DataKey, V.Vander AS Vander" +
                        " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V  " +
                        " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                        " WHERE V.VanderMailLogIn = '" + value.id + "' AND V.Vander = '" + value.venderCode + "' ";
                    }
                    else if (value.token != "" && value.venderCode == "" && value.venderGroup != "")
                    {
                        _Qry = " SELECT V.Pwd AS pwd, V.VanderMailLogIn AS VanderMailLogIn , VUP.VanderGrp AS VanderGrp " +
                            " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V " +
                            " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                            " LEFT JOIN VenderUserPermissionCmp VUP ON V.VanderMailLogIn = VUP.VanderMailLogIn " +
                            " WHERE V.VanderMailLogIn = '" + value.id + "' AND VUP.VanderGrp = '" + value.venderGroup + "'";
                    }
                    else
                    {
                        return null;
                    }
                    DelAuthenKey(Cnn, value.id);
                }

                return Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static bool DelAuthenKey(WSM.Conn.SQLConn Cnn, string value)
        {
            string _Qry = "";
            try
            {
                _Qry = " DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys WHERE VanderMailLogIn = '" + value + "'";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
        }
    }
}
