using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class APIOrder
    {
        [JsonProperty("OrderNo")]
        public string OrderNo { get; set; }

        //[JsonProperty("Company")]
        //public string Company { get; set; }

        //[JsonProperty("OrderStartDate")]
        //public string OrderStartDate { get; set; }

        //[JsonProperty("OrderEndDate")]
        //public string OrderEndDate { get; set; }

        //[JsonProperty("ShipmentStartDate")]
        //public string ShipmentStartDate { get; set; }

        //[JsonProperty("ShipmentEndDate")]
        //public string ShipmentEndDate { get; set; }

        public static bool checkDateFormat(string value)
        {
            bool state = false;
            DateTime tempDate;
            if (value != "")
            {
                state = (!DateTime.TryParseExact(value, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate)) ? true : false;
            }
            return state;
        }

        public static bool checkPeriodFormat(string vStart, string vEnd)
        {
            DateTime tempDateStart, tempDateEnd;
            DateTime.TryParseExact(vStart, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDateStart);
            DateTime.TryParseExact(vEnd, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDateEnd);

            return (DateTime.Compare(tempDateStart, tempDateEnd) <= 1) ? true : false;
        }


        public static bool checkCompany(string value)
        {
            DataTable dt = new DataTable();

            string _Qry = "SELECT DISTINCT od.FNHSysCmpId, c.FTCmpCode,c.FTCmpNameEN, c.FTCmpNameTH \n";
            _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrder AS od WITH(NOLOCK) \n\n";

            _Qry += " OUTER APPLY(SELECT c.FTCmpCode, c.FTCmpNameEN, c.FTCmpNameTH \n";
            _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCmp AS c WITH (NOLOCK) \n\n";

             _Qry += " WHERE c.FNHSysCmpId = od.FNHSysCmpId) AS c \n";

            if (value != null)
            {
                _Qry += " \n WHERE c.FTCmpCode = '" + value + "' \n";
            }

            try
            {
                WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_MERCHAN);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (dt.Rows.Count > 0) ? true : false;
        }


        public static bool checkCustomer(string value)
        {
            DataTable dt = new DataTable();

            string _Qry = "SELECT DISTINCT od.FNHSysCmpId, c.FTCustCode, c.FTCustNameEN, c.FTCustNameTH \n";
            _Qry += " \n FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrder AS od WITH ( NOLOCK ) \n";

            _Qry += " \n OUTER APPLY (SELECT c.FTCustCode,c.FTCustNameEN, c.FTCustNameTH FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCustomer AS c WITH (NOLOCK) \n";
            _Qry += " WHERE c.FNHSysCustId = od.FNHSysCustId) AS c \n\n";

            if (value != null)
            {
                _Qry += " \n WHERE c.FTCustCode = '" + value + "'";
            }

            try
            {
                WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_MERCHAN);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (dt.Rows.Count > 0) ? true : false;
        }
    }
}
