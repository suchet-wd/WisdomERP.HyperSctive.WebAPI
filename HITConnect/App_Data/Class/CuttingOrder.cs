using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class CuttingOrder
    {
        [JsonProperty("Job")]
        public string Job { get; set; }

        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; }

        [JsonProperty("CustomerPo")]
        public string CustomerPo { get; set; }

        [JsonProperty("JobProductionNo")]
        public string JobProductionNo { get; set; }

        [JsonProperty("SubJobNo")]
        public string SubJobNo { get; set; }

        [JsonProperty("GacDate")]
        public string GacDate { get; set; }

        [JsonProperty("PoLineItem")]
        public string PoLineItem { get; set; }

        [JsonProperty("OrderProdDetails")]
        public List<CuttingOrderDetails> OrderProdDetails { get; set; }


        public static bool isPooNo(string poono)
        {
            DataTable _dataTable = new DataTable();
            bool state = false;
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            //string _Qry = " SELECT  *  FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTBundle_Detail AS bd WITH (NOLOCK) ";
            
            if (poono != "")
            {
                //If EXISTS()
                string _Qry = " SELECT TOP 1 bd.FTLayCutNo  FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTBundle_Detail AS bd WITH (NOLOCK) WHERE bd.FTLayCutNo = '" + poono + "' \n";
                //_Qry += " BEGIN select 'T' AS 'Result' END ELSE BEGIN select 'F' AS 'Result' END";
                try
                {
                    _dataTable = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION);
                    if (_dataTable.Rows.Count > 0)
                    {
                        state = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return state;
        }

    }
}
