using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HyperActive
{
    public class APIRfidBarcodeDetail
    {
        [JsonProperty("Job")]
        public string Job { get; set; }

        [JsonProperty("ColorWay")]
        public string ColorWay { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("BundleBarcode")]
        public string BundleBarcode { get; set; }

        [JsonProperty("BundleQuantity")]
        public string BundleQuantity { get; set; }


        public static bool isParentBundleBarcode(string value)
        {
            bool state = false;
            if (value != "")
            {
                string _Qry = "SELECT TOP 1 bd.FTBarcodeBundleNo as 'ParentBundleBarcode' FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODTBundle_Detail AS bd WITH (NOLOCK) \n";
                _Qry += "WHERE bd.FTBarcodeBundleNo = '" + value + "'";
                try
                {
                    state = (new WSM.Conn.SQLConn().GetField(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION, "") != "") ? true : false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                state = false;
            }
            return state; // (DateTime.Compare(tempDateStart, tempDateEnd) <= 1) ? true : false;
        }

        public static bool isBundleBarcode(string value)
        {
            bool state = false;
            if (value != "")
            {
                string _Qry = "SELECT TOP 1 ss.FTBarcodeBundleNo as 'BundleBarcode' FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODBarcode_SendSupl AS ss WITH (NOLOCK)\n";
                _Qry += "WHERE ss.FTBarcodeSendSuplNo  = '" + value + "'";
                //If HI.Conn.SQLConn.GetField(_Qry, Conn.DB.DataBaseName.DB_MERCHAN, "") = "" Then
                try
                {
                    state = (new WSM.Conn.SQLConn().GetField(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION, "") != "") ? true : false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                state = false;
            }
            return state; // (DateTime.Compare(tempDateStart, tempDateEnd) <= 1) ? true : false;
        }

    } // End Class
} //End namespace