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
    public class PaymentController : ApiController
    {
        private string columnList = "SELECT ISNULL(PONo, '') AS PONo, ISNULL(PayType, '') AS PayType, ISNULL(PaymentTerm, '') AS PaymentTerm, " +
            " ISNULL(PaymentDate, '') AS PaymentDate, ISNULL(LCNo, '') AS LCNo, ISNULL(PINo, '') AS PINo, ISNULL(PIDate, '') AS PIDate, " +
            " ISNULL(RcvPIDate, '') AS RcvPIDate, ISNULL(PISuplCFMDeliveryDate, '') AS PISuplCFMDeliveryDate, ISNULL(InvoiceNo, '') AS InvoiceNo, " +
            " ISNULL(InvoiceDate, '') AS InvoiceDate, ISNULL(PurchaseDate, '') AS PurchaseDate, ISNULL(PurchaseBy, '') AS PurchaseBy, " +
            " ISNULL(SupplierCode, '') AS SupplierCode, ISNULL(SupplierName, '') AS SupplierName, ISNULL(Currency, '') AS Currency, " +
            " ISNULL(DeliveryDate, '') AS DeliveryDate, ISNULL(Company, '') AS Company, ISNULL(Buy, '') AS Buy, ISNULL(POUnit, '') AS POUnit, " +
            " ISNULL(POQty, 0) AS POQty, ISNULL(POAmount, 0) AS POAmount, ISNULL(POOutstandingQty, 0) AS POOutstandingQty, " +
            " ISNULL(Revised, 0) AS Revised, ISNULL(RevisedDate, '') AS RevisedDate, ISNULL(RevisedBy, '') AS RevisedBy, " +
            " ISNULL(SentDocToAccDate, '') AS SentDocToAccDate, ISNULL(FinishbalancePaymentDate, '') AS FinishbalancePaymentDate, " +
            " ISNULL(PIQuantity, 0) AS PIQuantity, ISNULL(PINetAmt, 0) AS PINetAmt, ISNULL(PIDocCNAmt, 0) AS PIDocCNAmt, " +
            " ISNULL(PIDocDNAmt, 0) AS PIDocDNAmt, ISNULL(PIDocSurchargeAmt, 0) AS PIDocSurchargeAmt, ISNULL(PIDocNetAmt, 0) AS PIDocNetAmt, " +
            " ISNULL(Note, '') AS Note, ISNULL(FTStateClose, '') AS FTStateClose, ISNULL(FTStateHasFile, '') AS FTStateHasFile " +
            " , ISNULL(DATALENGTH(FTFileRef), -1) AS FTFileRef ";

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "String2" };
        }

        [HttpGet]
        [Route("api/CheckPayment/")]
        public HttpResponseMessage CheckPayment(string idRq, string pwdRq, string tokenRq, string startRq = null, string endRq = null, string PIRq = null, string PORq = null)
        {
            string _Qry = "";
            int status = 0;
            string msgError = "";
            string token = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            _Qry += " SELECT V.Pwd, V.VanderMailLogIn, ATK.DataKey " +
                " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V  " +
                " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                " WHERE V.VanderMailLogIn = '" + idRq + "'";
            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

            if (dt != null)
            { // REMOVE TOKEN FROM AuthenKeys
                _Qry = "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys WHERE VanderMailLogIn = '" + idRq + "'";

                if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                {
                    _Qry = columnList + " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPayment ";

                    if (startRq != null && endRq != null)
                    {
                        _Qry += " WHERE PaymentDate BETWEEN '" + startRq + "' AND '" + endRq + "' ";
                        if (PIRq != null && PORq == null)
                        {
                            _Qry += " AND PINo = '" + PIRq + "' ";
                        }
                        if (PORq != null && PIRq == null)
                        {
                            _Qry += " AND PONo = '" + PORq + "'";
                        }
                    }
                    else if (startRq == null || endRq == null)
                    {
                        if (PIRq != null && PORq == null)
                        {
                            _Qry += " WHERE PINo = '" + PIRq + "' ";
                        }
                        if (PORq != null && PIRq == null)
                        {
                            _Qry += " WHERE PONo = '" + PORq + "'";
                        }
                        if (PORq != null && PIRq != null)
                        {
                            _Qry += " WHERE PONo = '" + PORq + "'  OR PINo = '" + PIRq + "' ";
                        }
                        else
                        {
                            token = "";
                            status = 2;
                            msgError = "Please check start date and end date!!!";
                        }
                    }

                    if (status == 0)
                        dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                    if (status != 2)
                    {
                        dts = dt;
                        token = "";
                        status = 1;
                        msgError = "Successful";
                    }
                    else
                    {
                        dts.Rows.Add(new Object[] { status, token, msgError });
                    }
                }
                else
                {
                    token = "";
                    status = 2;
                    msgError = "Token is invalid!!!";
                }
            }
            else
            {
                token = "";
                status = 2;
                msgError = "Please check Username and Password";
            }

            jsondata = JsonConvert.SerializeObject(dts);
            return new HttpResponseMessage { Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
        }

        /*
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }



        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        */

        /*
         * 
         * NOT USE by Chet
         * 
         * 
        [HttpGet]
        [Route("api/CheckPO/")]
        public HttpResponseMessage CheckPO(string idRq, string pwdRq, string tokenRq, string PORq, int? startRq = null, int? endRq = null)
        {
            string _Qry = "";
            int status = 0;
            string msgError = "";
            string token = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            _Qry += " SELECT V.Pwd, V.VanderMailLogIn, ATK.DataKey " +
                " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V  " +
                " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                " WHERE V.VanderMailLogIn = '" + idRq + "'";
            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

            if (dt != null)
            { // REMOVE TOKEN FROM AuthenKeys
                _Qry = "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys WHERE VanderMailLogIn = '" + idRq + "'";

                if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                {
                    _Qry = columnList + " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPayment " +
                        " WHERE PONo = '" + PORq + "'";
                    dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                    if (dt != null)
                    {
                        dts = dt;
                        token = "";
                        status = 1;
                        msgError = "Successful";
                    }
                    else
                    {
                        token = "";
                        status = 2;
                        msgError = "This PO Number is not found!!!";
                    }
                }
                else
                {
                    token = "";
                    status = 2;
                    msgError = "Token is invalid!!!";
                }
            }
            if (status != 1)
                dts.Rows.Add(new Object[] { status, token, msgError });
            jsondata = JsonConvert.SerializeObject(dts);
            return new HttpResponseMessage { Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
        }


        [HttpGet]
        [Route("api/CheckPI/")]
        public HttpResponseMessage CheckPI(string idRq, string pwdRq, string tokenRq, string PIRq, int? startRq = null, int? endRq = null)
        {
            string _Qry = "";
            int status = 0;
            string msgError = "";
            string token = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            _Qry += " SELECT V.Pwd, V.VanderMailLogIn, ATK.DataKey " +
                " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V  " +
                " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                " WHERE V.VanderMailLogIn = '" + idRq + "'";
            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

            if (dt != null)
            { // REMOVE TOKEN FROM AuthenKeys
                _Qry = "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys WHERE VanderMailLogIn = '" + idRq + "'";

                if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                {
                    _Qry = columnList + " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPayment " +
                        " WHERE PINo = '" + PIRq + "'";
                    dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                    if (dt != null)
                    {
                        dts = dt;
                        token = "";
                        status = 1;
                        msgError = "Successful";
                    }
                    else
                    {
                        token = "";
                        status = 2;
                        msgError = "This PI Number is not found!!!";
                    }
                }
                else
                {
                    token = "";
                    status = 2;
                    msgError = "Token is invalid!!!";
                }
            }
            if (status != 1)
                dts.Rows.Add(new Object[] { status, token, msgError });
            jsondata = JsonConvert.SerializeObject(dts);
            return new HttpResponseMessage { Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
        }
        */
    }
}
