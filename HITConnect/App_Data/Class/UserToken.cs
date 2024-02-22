using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class UserToken
    {
        [JsonProperty("id", Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty("pwd", Required = Required.Always)]
        public string pwd { get; set; }

        public static List<string> ValidateField(UserToken value)
        {
            DataTable _dataTable = null;
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
                    try
                    {
                        WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
                        string _Qry = "SELECT V.VenderMailLogIn AS VenderMailLogIn ,V.Pwd AS pwd, ATK.VenderMailLogIn ";
                        _Qry += "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.VenderUser AS V WITH(NOLOCK) ";
                        _Qry += "LEFT JOIN [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.AuthenKeys AS ATK WITH(NOLOCK )  ON ATK.VenderMailLogIn = V.VenderMailLogIn ";
                        _Qry += "WHERE V.VenderMailLogIn = '" + value.id + "' AND V.Pwd = '" + value.id + "'";
                        //_dataTable = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return null;
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
            DataTable _dataTable = null;
            try
            {
                string _Qry = "";
                if (value.id != "")// && value.venderGroup != "")
                {
                    _Qry = "SELECT DISTINCT V.Pwd AS pwd, V.VenderMailLogIn, ATK.VenderMailLogIn AS ATK " +
                        //--, P.VenderGrp AS VenderGrp
                        "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.VenderUser AS V WITH(NOLOCK) " +
                        "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.AuthenKeys WITH (NOLOCK ) " +
                        "WHERE VenderMailLogIn = V.VenderMailLogIn) AS ATK " +
                        "OUTER APPLY(SELECT VenderGrp FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.VenderUserPermissionCmp WITH (NOLOCK ) " +
                        "WHERE VenderMailLogIn = V.VenderMailLogIn) AS P " +
                        "WHERE V.VenderMailLogIn = '" + value.id + "'";
                    //_dataTable = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                }
                else
                {
                    return _dataTable;
                }
                return _dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
