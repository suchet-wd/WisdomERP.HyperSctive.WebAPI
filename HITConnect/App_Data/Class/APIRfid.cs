using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HyperActive
{
    public class APIRfid
    {
        [JsonProperty("Authen", Required = Required.Always)]
        public UserAuthen authen { get; set; }

        [JsonProperty("BoxRfid", Required = Required.Always)]
        public string BoxRfid { get; set; }

        [JsonProperty("BoxBarcode", Required = Required.Always)]
        public string BoxBarcode { get; set; }

        [JsonProperty("BundleRfidBarcodeList", Required = Required.Always)]
        public List<APIRfidBarcode> BundleRfidBarcodeList { get; set; }

        [JsonProperty("TimeStamp", Required = Required.Always)]
        public DateTime TimeStamp { get; set; }

        //public static bool isBoxBarcode(string value)
        //{
        //    DataTable dt = new DataTable();

        //    string _Qry = "SELECT o.FTOrderNo, o.FTOrderProdNo \n"; //,cmp.FTCmpCode
        //    _Qry += " \n FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd  AS o WITH ( NOLOCK ) WHERE o.FTOrderNo <> '' \n";

        //    if (value != null)
        //    {
        //        _Qry += " \n AND  o.FTOrderNo= '" + value + "'";
        //    }

        //    try
        //    {
        //        WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
        //        dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //    return (dt.Rows.Count > 0) ? true : false;
        //}

    }
}
