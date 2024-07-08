using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HyperActive
{
    public class APIRfidBarcode
    {
        [JsonProperty("Rfid")]
        public string Rfid { get; set; }

        [JsonProperty("ParentBundleBarcode")]
        public string ParentBundleBarcode { get; set; }

        [JsonProperty("BundleBarcode")]
        public List<APIRfidBarcodeDetail> BundleBarcode { get; set; }
        //public List<string> BundleBarcode { get; set; }


        public static bool isParentBundleBarcode(APIRfidBarcode value)
        {
            bool state = false;
            if (value.BundleBarcode.ToString() != "" && value.ParentBundleBarcode.ToString() != "")
            {
                string _Qry = "SELECT TOP 1 bd.FTBarcodeBundleNo as 'ParentBundleBarcode' \n";
                _Qry += "FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODTBundle_Detail AS bd WITH (NOLOCK) \n";
                _Qry += "WHERE bd.FTBarcodeBundleNo = '" + value.ParentBundleBarcode + "' \n";
                try
                {
                    state = (new WSM.Conn.SQLConn().GetField(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION, "") != "") ? true : false;
                    if (state == false)
                    {
                        _Qry = "SELECT TOP 1 bp.FTBarcodeBundleNo as 'ParentBundleBarcode' \n";
                        _Qry += "FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODTBundle_Part AS bp WITH (NOLOCK) \n";
                        _Qry += "WHERE bp.FTBarcodeBundleNo = '" + value.ParentBundleBarcode + "' \n";
                        //_Qry += "AND bp.FTBarcodePartNo = '" + value.BundleBarcode + "'";
                        state = (new WSM.Conn.SQLConn().GetField(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION, "") != "") ? true : false;
                    }
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

        public static bool isBundleBarcode(APIRfidBarcodeDetail value)
        {
            bool state = false;
            if (value.BundleBarcode != "")
            {
                //string _Qry = "SELECT TOP 1 ss.FTBarcodeBundleNo as 'BundleBarcode' " +
                //    "FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODBarcode_SendSupl AS ss WITH (NOLOCK)\n";
                //_Qry += "WHERE ss.FTBarcodeSendSuplNo  = '" + value + "'";
                string _Qry = "SELECT TOP 1 ss.FTBarcodeBundleNo as 'BundleBarcode' \n";

                _Qry += "FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODBarcode_SendSupl AS ss WITH(NOLOCK) \n";
                _Qry += "OUTER APPLY(SELECT TOP 1 * FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODTOrderProd AS p WITH(NOLOCK) \n";
                _Qry += "WHERE p.FTOrderProdNo = ss.FTOrderProdNo) as p \n";
                _Qry += "OUTER APPLY(SELECT TOP 1 * FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODTBundle AS b WITH (NOLOCK) \n";
                _Qry += "WHERE b.FTBarcodeBundleNo = ss.FTBarcodeBundleNo ) as b \n";
                _Qry += "WHERE ss.FTBarcodeSendSuplNo = '" + value.BundleBarcode + "' AND p.FTOrderNo = '" + value.Job + "'  \n";
                _Qry += "AND b.FTColorway = '" + value.ColorWay + "' AND b.FTSizeBreakDown = '" + value.Size + "'";

                try
                {
                    state = (new WSM.Conn.SQLConn().GetField(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION, "") != "") ? true : false;

                    if (state == true)
                    {
                        return state;
                    }
                    else
                    {
                        _Qry = "SELECT TOP 1 a.FTOrderProdNo FROM " + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + ".dbo.TPRODTBundle_Part AS a WITH (NOLOCK) ";
                        _Qry += "WHERE  a.FTBarcodePartNo = '" + value.BundleBarcode + "'";

                        state = (new WSM.Conn.SQLConn().GetField(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION, "") != "") ? true : false;
                    }
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