using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HITConnect
{
    public class UserAuthen
    {
        [JsonProperty("id", Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty("pwd", Required = Required.Always)]
        public string pwd { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }

        public static List<string> ValidateField(UserAuthen value)
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
            }
            List<string> _result = new List<string>();
            _result.Add(statecheck);
            _result.Add(msgError);
            return _result;
        }

        public static DataTable GetDTUserValidate(WSM.Conn.SQLConn Cnn, UserAuthen value)
        {
            DataTable _dataTable = new DataTable();

            string _Qry = "";
            if (value.id != "" &&  value.token == "") 
            {
                _Qry = " SELECT V.Pwd AS pwd, V.VenderMailLogIn AS VenderMailLogIn , VUP.VenderGrp AS VenderGrp " +
                    " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V WITH ( NOLOCK ) " +
                    " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK WITH ( NOLOCK ) " +
                    " ON ATK.VenderMailLogIn = V.VenderMailLogIn  " +
                    " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUserPermissionCmp VUP WITH ( NOLOCK ) " +
                    " ON V.VenderMailLogIn = VUP.VenderMailLogIn " +
                    " WHERE V.VenderMailLogIn = '" + value.id + "'";
            }
            else if (value.id != "" &&  value.token != "") 
            {
                _Qry = " SELECT V.Pwd AS pwd, V.VenderMailLogIn AS VenderMailLogIn , VUP.VenderGrp AS VenderGrp " +
                    " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V WITH ( NOLOCK ) " +
                    " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK WITH ( NOLOCK ) " +
                    " ON ATK.VenderMailLogIn = V.VenderMailLogIn  " +
                    " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUserPermissionCmp AS VUP WITH ( NOLOCK ) " +
                    " ON V.VenderMailLogIn = VUP.VenderMailLogIn " +
                    " WHERE V.VenderMailLogIn = '" + value.id + "' AND ATK.DataKey = '" + value.token + "'";
            }
            try
            {
                _dataTable = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return _dataTable;
        }

        public static bool DelAuthenKey(WSM.Conn.SQLConn Cnn, string value)
        {
            string _Qry = "";
            bool _result = false;
            try
            {
                _Qry = " DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys WHERE VenderMailLogIn = '" + value + "'";
                _result = Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return _result;
        }
    }
}
