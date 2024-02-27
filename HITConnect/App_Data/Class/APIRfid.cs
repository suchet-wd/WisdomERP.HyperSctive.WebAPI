using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class APIRfid
    {
        [JsonProperty("BoxRfid")]
        public string BoxRfid { get; set; }

        [JsonProperty("BoxBarcode")]
        public string BoxBarcode { get; set; }

        [JsonProperty("BundleRfidBarcodeList")]
        public List<APIRfidBarcode> BundleRfidBarcodeList { get; set; }

        [JsonProperty("TimeStamp")]
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
