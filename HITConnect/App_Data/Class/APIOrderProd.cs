using Newtonsoft.Json;
using System;
using System.Data;

namespace HyperActive
{
    public class APIOrderProd
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("OrderNo", Required = Required.Always)]
        public string OrderNo { get; set; }

        public static bool isOrderProdNo(string value)
        {
            DataTable dt = new DataTable();

            string _Qry = "SELECT o.FTOrderNo, o.FTOrderProdNo \n\n"; //,cmp.FTCmpCode
            _Qry += "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd  AS o WITH ( NOLOCK ) \n\n";
            _Qry += "WHERE o.FTOrderNo <> '' \n";

            if (value != null)
            {
                _Qry += " \n AND  o.FTOrderNo= '" + value + "'";
            }

            try
            {
                WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (dt.Rows.Count > 0) ? true : false;
        }
    }
}

//_Qry += "OUTER APPLY (SELECT * " +
//    "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd_MarkMain AS m WITH (NOLOCK ) " +
//    "WHERE o.FTOrderProdNo = m.FTOrderProdNo ) AS m   \n\n";

//_Qry += "OUTER APPLY (SELECT cmp.FTCmpCode FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCmp AS cmp WITH (NOLOCK ) " +
//    "WHERE cmp.FNHSysCmpId = m.FNHSysCmpId) AS cmp \n\n";