using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class APIRfidBarcode
    {
        [JsonProperty("Rfid")]
        public string Rfid { get; set; }

        [JsonProperty("ParentBundleBarcode")]
        public string ParentBundleBarcode { get; set; }
        //{
        //    get { return ParentBundleBarcode; }   // get method
        //    set { ParentBundleBarcode = value; }  // set method
        //}

        [JsonProperty("BundleBarcode")]
        public List<string> BundleBarcode { get; set; }


        public static bool isParentBundleBarcode(string value)
        {
            bool state = false;
            if (value != "")
            {
                string _Qry = "SELECT TOP 1 bd.FTBarcodeBundleNo as 'ParentBundleBarcode' FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODTBundle_Detail AS bd WITH (NOLOCK) \n";
                _Qry += "WHERE bd.FTBarcodeBundleNo = '" + value + "'";
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