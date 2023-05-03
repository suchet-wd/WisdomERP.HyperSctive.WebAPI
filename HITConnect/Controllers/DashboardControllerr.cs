using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class DashboardController : ApiController
    {
        [HttpPost]
        [Route("api/VenderEveluate/")]
        public HttpResponseMessage VenderEveluate(string value)
        {
            string _Qry = "";
            string jsondata = "";
            DataTable dt = new DataTable();
            DataTable dts = new DataTable();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            _Qry = " SELECT SUM(500000) AS TotalPI, Sum(150000) AS TotalSentToAcc, Sum(150000) AS TotalAccConfirm" +
                " , Sum(200000)  AS TotalPayment, Currency, VenderCode FROM POToVender WITH(NOLOCK) WHERE ";
            _Qry += (value != "") ? ("(VenderCode = '" + value.ToString() + "') AND ") : "";
            _Qry += " Right(PODate, 4) + '/' + Left(Right(PODate, 7), 2) + '/' + Left(PODate, 2) >= " +
                " Convert(nvarchar(10), DATeadd(YEAR, -1, Getdate()), 111) GROUP BY Currency,VenderCode";
            try
            {
                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                jsondata = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (jsondata != "")
            {
                //jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
        }

        [HttpPost]
        [Route("api/QuantityOntime/")]
        public HttpResponseMessage QuantityOntime(string value)
        {
            string _Qry = "";
            string jsondata = "";
            DataTable dt = new DataTable();
            DataTable dts = new DataTable();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            _Qry = " SELECT SUM(500) AS TotalPO, Sum(150) AS TotalOntimeDeliveryed, " +
                " Sum(500 - 150) AS TotalWaitDelivery, Sum(20)  AS TotalOverDeliveryed, " +
                " POItemCode AS ItemCode, QtyUnit AS Unit, VenderCode FROM POToVender WITH(NOLOCK) " +
                " WHERE ";
            _Qry += (value != "") ? ("(VenderCode = '" + value.ToString() + "') AND ") : "";
            _Qry += "Right(PODate, 4) + '/' + Left(Right(PODate, 7), 2) + '/' + Left(PODate, 2) >= " +
                " Convert(nvarchar(10), DATeadd(YEAR, -1, Getdate()), 111) " +
                " group by POItemCode,QtyUnit,VenderCode";

            try
            {
                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                jsondata += JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (jsondata != "")
            {
                //jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
        }

        [HttpPost]
        [Route("api/VenderPayment/")]
        public HttpResponseMessage VenderPayment(string value)
        {
            string _Qry = "";
            string jsondata = "";
            DataTable dt = new DataTable();
            DataTable dts = new DataTable();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            _Qry = " SELECT SUM(500) AS TotalPO, Sum(150) AS TotalOntimeDeliveryed, " +
                " Sum(500 - 150) AS TotalWaitDelivery, Sum(20)  AS TotalOverDeliveryed, " +
                " POItemCode AS ItemCode, QtyUnit AS Unit, VenderCode FROM POToVender WITH(NOLOCK) " +
                " WHERE ";
            _Qry += (value != "") ? ("(VenderCode = '" + value.ToString() + "') AND ") : "";
            _Qry += "Right(PODate, 4) + '/' + Left(Right(PODate, 7), 2) + '/' + Left(PODate, 2) >= " +
                " Convert(nvarchar(10), DATeadd(YEAR, -1, Getdate()), 111) " +
                " group by POItemCode,QtyUnit,VenderCode";

            try
            {
                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                jsondata += JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (jsondata != "")
            {
                //jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
        }
    }
}
