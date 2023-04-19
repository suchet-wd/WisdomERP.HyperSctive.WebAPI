using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class POPackRoll : ApiController
    {
        private string columnList = "SELECT ISNULL(FTDataKey, '') AS FTDataKey, ISNULL(invno, '') AS invno, ISNULL(invdate, '') AS invdate, " +
            "ISNULL(pono, '') AS pono, ISNULL(itemno, '') AS itemno, ISNULL(colorcode, '') AS colorcode, ISNULL(size, '') AS size, " +
            "ISNULL(colorname, '') AS colorname, ISNULL(rollno, '') AS rollno, ISNULL(width, '') AS width, ISNULL(actualwidth, '') AS actualwidth, " +
            "ISNULL(actuallength, 0) AS actuallength, ISNULL(actualweight, 0) AS actualweight, ISNULL(potno, '') AS potno, " +
            "ISNULL(barcode, '') AS barcode, ISNULL(clothno, '') AS clothno, ISNULL(stock, '') AS stock, ISNULL(packno, '') AS packno, " +
            "ISNULL(packtype, '') AS packtype, ISNULL(ordertype, '') AS ordertype, ISNULL(ORDERNO, '') AS ORDERNO, ISNULL(ArticleNo, '') AS ArticleNo, " +
            "ISNULL(Composition, '') AS Composition, ISNULL(StdWt, 0) AS StdWt, ISNULL(NetWtKG, 0) AS NetWtKG, ISNULL(GrossWtKG, 0) AS GrossWtKG, " +
            "ISNULL(Madein, '') AS Madein, ISNULL(Shipto, '') AS Shipto, ISNULL(FTDateCreate, '') AS FTDateCreate, ISNULL(FTStateClose, '') AS FTStateClose, " +
            "ISNULL(FTStateCloseDate, '') AS FTStateCloseDate, ISNULL(FTStateCloseBy, '') AS FTStateCloseBy, ISNULL(FTVanderCode, '') AS FTVanderCode " +
            "FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[POPackRoll] ";
        
        private string columnList2 = "SELECT [FTDataKey],[invno],[invdate],[pono],[itemno],[colorcode],[size],[colorname],[rollno],[width]," +
            "[actualwidth],[actuallength],[actualweight],[potno],[barcode],[clothno],[stock],[packno],[packtype],[ordertype],[ORDERNO]," +
            "[ArticleNo],[Composition],[StdWt],[NetWtKG],[GrossWtKG],[Madein],[Shipto],[FTDateCreate],[FTStateClose],[FTStateCloseDate]," +
            "[FTStateCloseBy],[FTVanderCode] FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[POPackRoll] ";

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "String2" };
        }

        [HttpPost]
        [Route("api/CheckPOPackRoll/")]
        public HttpResponseMessage CheckPOPackRoll(string id)
        {
            string _Qry = "";
            int statecheck = 0;
            string msgError = "";
            //string token = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            //dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            try
            {
                //token = "";
                WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

                _Qry += " SELECT V.Pwd, V.VanderMailLogIn, ATK.DataKey " +
                    " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V  " +
                    " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                    " WHERE V.VanderMailLogIn = '" + id + "'";
                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                if (dt != null)
                {
                    
                }
                else
                {
                    statecheck = 2;
                    msgError = "Please check Username and Password";
                }

                jsondata = JsonConvert.SerializeObject(dts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //return new HttpResponseMessage { Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
            if (statecheck == 1)
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
                //return new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + "1" + (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + "" + (char)34 + "}", System.Text.Encoding.UTF8, "application/json") };
            }
            else
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.NotAcceptable, Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + "0" + (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + msgError + (char)34 + "}", System.Text.Encoding.UTF8, "application/json") };
            }
        }
    }
}
