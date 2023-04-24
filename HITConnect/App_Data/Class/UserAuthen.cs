using Newtonsoft.Json;
using System;
using System.Data;
using System.Reflection;
using WSM.Conn;

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

        [JsonProperty("venderGroup")]
        public string venderGroup { get; set; }


        public static DataTable GetDTUserValidate(WSM.Conn.SQLConn Cnn, UserAuthen value)
        {
            try
            {
                string _Qry = "";
                if (value.id != "" && value.venderGroup != "")
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
                //DelAuthenKey(Cnn, value.id);
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
            return Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER); ;
        }
    }
}
