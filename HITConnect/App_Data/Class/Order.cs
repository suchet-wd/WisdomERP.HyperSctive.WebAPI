using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WSM.Conn;

namespace HyperConvert
{
    public class Order
    {
        [JsonProperty("Job")]
        public string Job { get; set; }

        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; }

        [JsonProperty("CustomerPo")]
        public string CustomerPo { get; set; }

        [JsonProperty("Note")]
        public string Note { get; set; }

        [JsonProperty("OrderDetails")]
        public List<OrderDetails> OrderDetails { get; set; }


        public static DataTable getOrder()
        {
            DataTable _dataTable = new DataTable();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            string _Qry = " SELECT TOP 1 *  FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrder AS od WITH ( NOLOCK ) ";

            try
            {
                _dataTable = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_MERCHAN);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return _dataTable;
        }

    }
}
