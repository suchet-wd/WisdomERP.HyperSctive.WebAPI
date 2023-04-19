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
        //" , ISNULL(DATALENGTH(FTFileRef), -1) AS FTFileRef ";
        //SELECT FTFileRef from[DB_VENDER].dbo.POPayment FOR XML PATH(''), BINARY BASE64

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "String2" };
        }


        //public HttpResponseMessage CheckPayment(string idRq, string pwdRq, string tokenRq, string startRq = null, string endRq = null, string PIRq = null, string PORq = null)

        [HttpPost]
        [Route("api/CheckPayment/")]
        public HttpResponseMessage CheckPayment(HITConnect.Payment pmRq)
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

            //token = "";
            try
            {

                WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

                _Qry += " SELECT V.Pwd, V.VanderMailLogIn, ATK.DataKey " +
                    " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V  " +
                    " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                    " WHERE V.VanderMailLogIn = '" + pmRq.id + "'";
                dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                if (dt != null)
                { // REMOVE TOKEN FROM AuthenKeys
                    _Qry = "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys WHERE VanderMailLogIn = '" + pmRq.id + "'";

                    if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                    {

                        _Qry = columnList + " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPayment ";

                        if (pmRq.startDate != null && pmRq.endDate != null)
                        {
                            if (Convert.ToDateTime(pmRq.startDate) <= Convert.ToDateTime(pmRq.endDate))
                            {
                                _Qry += " WHERE PaymentDate BETWEEN '" + pmRq.startDate + "' AND '" + pmRq.endDate + "' ";
                                if (pmRq.PI != null && pmRq.PO == null)
                                {
                                    _Qry += " AND PINo = '" + pmRq.PI + "' ";
                                }
                                if (pmRq.PI == null && pmRq.PO != null)
                                {
                                    _Qry += " AND PONo = '" + pmRq.PO + "'";
                                }
                            } else
                            {
                                statecheck = 2;
                                msgError = "Start Date must be lower than End Date";
                            }
                            
                        }
                        else if (pmRq.startDate == null || pmRq.endDate == null)
                        {
                            if (pmRq.PI != null && pmRq.PO == null)
                            {
                                _Qry += " WHERE PINo = '" + pmRq.PI + "' ";
                            }
                            if (pmRq.PO != null && pmRq.PI == null)
                            {
                                _Qry += " WHERE PONo = '" + pmRq.PO + "'";
                            }
                            if (pmRq.PO != null && pmRq.PI != null)
                            {
                                _Qry += " WHERE PONo = '" + pmRq.PO + "'  OR PINo = '" + pmRq.PI + "' ";
                            }
                            else
                            {
                                statecheck = 2;
                                msgError = "Please check start date and end date!!!";
                            }
                        }

                        if (statecheck == 0)
                            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                        if (statecheck != 2)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if (Convert.ToBase64String((byte[])row["FTFileRef"]) != "-1")
                                {
                                    row["FTFileRef"] = Convert.ToBase64String((byte[])row["FTFileRef"]);
                                }
                                else
                                {
                                    row["FTFileRef"] = "";
                                }
                            }
                            dts = dt;

                            statecheck = 1;
                            msgError = "Successful";
                        }
                        else
                        {
                            dts.Rows.Add(new Object[] { statecheck, msgError });
                        }
                    }
                    else
                    {
                        statecheck = 2;
                        msgError = "Token is invalid!!!";
                    }
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
