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

        [JsonProperty("BundleBarcode")]
        public List<string> BundleBarcode { get; set; }


        public static bool isParentBundleBarcode(string value)
        {
            bool state = false;
            if (value != "")
            {
                DataTable dt = new DataTable();

                string _Qry = "SELECT *  FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrder AS od WITH(NOLOCK) \n\n";

                if (value != null)
                {
                    _Qry += " \n WHERE c.FTCmpCode = '" + value + "' \n";
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