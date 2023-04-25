using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class PaymentController : ApiController
    {
        private string columnPOPayment = "SELECT ISNULL(PONo, '') AS PONo, ISNULL(PayType, '') AS PayType, ISNULL(PaymentTerm, '') AS PaymentTerm, " +
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


        [HttpPost]
        [Route("api/GetPayment/")]
        public HttpResponseMessage GetPayment(Payment value)
        {
            string _Qry = "";
            int statecheck = 0;
            string msgError = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            // Check id + pwd + vender group
            List<string> _result = UserAuthen.ValidateField(value.authen);
            statecheck = int.Parse(_result[0]);
            msgError = _result[1];
            // End Check id + pwd + vender group

            try
            {
                if (statecheck != 2)
                {
                    dt = HITConnect.UserAuthen.GetDTUserValidate(Cnn, value.authen);

                    // Delete Old Token from Database
                    UserAuthen.DelAuthenKey(Cnn, value.authen.id);

                    if (dt != null && dt.Rows.Count > 0)
                    {

                        _Qry = columnPOPayment + " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPayment ";

                        if (value.startDate != null && value.endDate != null)
                        {
                            if (Convert.ToDateTime(value.startDate) <= Convert.ToDateTime(value.endDate))
                            {
                                _Qry += " WHERE PaymentDate BETWEEN '" + value.startDate + "' AND '" + value.endDate + "' ";
                                if (value.PI != null && value.PO == null)
                                {
                                    _Qry += " AND PINo = '" + value.PI + "' ";
                                }
                                if (value.PI == null && value.PO != null)
                                {
                                    _Qry += " AND PONo = '" + value.PO + "'";
                                }
                            }
                            else
                            {
                                statecheck = 2;
                                msgError = "Start Date must be lower than End Date";
                            }

                        }
                        else if (value.startDate == "" || value.endDate == "")
                        {
                            if (value.PI != "" && value.PO == "")
                            {
                                _Qry += " WHERE PINo = '" + value.PI + "' ";
                            }
                            if (value.PO != "" && value.PI == "")
                            {
                                _Qry += " WHERE PONo = '" + value.PO + "'";
                            }
                            if (value.PO != "" && value.PI != "")
                            {
                                _Qry += " WHERE PONo = '" + value.PO + "'  OR PINo = '" + value.PI + "' ";
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
                        msgError = "Please check User authentication!!!";
                    }

                    jsondata = JsonConvert.SerializeObject(dts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (statecheck == 1)
            {
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
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + statecheck +
                    (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + msgError + (char)34 + "}",
                    System.Text.Encoding.UTF8, "application/json")
                };
            }
        }
    }
}