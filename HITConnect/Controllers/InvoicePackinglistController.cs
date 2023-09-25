using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class InvoicePackinglistController : ApiController
    {
        [HttpPost]
        [Route("api/InvoicePackinglist/")]
        public HttpResponseMessage InvoicePackinglist(InvoicePackingList value)
        {
            DateTime tempDate;
            string _Qry = "";
            int statecheck = 0;
            string msgError = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            List<Invoice> _PRProblem = new List<Invoice>();
            List<Invoice> _PRPass = new List<Invoice>();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            // Check id + pwd + vender group
            List<string> _result = UserAuthen.ValidateField(value.authen);
            statecheck = int.Parse(_result[0]);
            msgError = _result[1];
            // End Check id + pwd + vender group
            int count = 0;
            int _successCount = 0;
            int _problemCount = 0;
            try
            {
                if (statecheck != 2 && value.authen.token != "")
                {
                    dt = HITConnect.UserAuthen.GetDTUserValidate(Cnn, value.authen);

                    // Delete Old Token from Database
                    UserAuthen.DelAuthenKey(Cnn, value.authen.id);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (Invoice row in value.invoice)
                        {
                            if (row.invdate != "")
                            {
                                if (!DateTime.TryParseExact(row.invdate, "yyyy/MM/dd",
                                    DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                                {
                                    statecheck = 2;
                                    msgError = "Please check Date format 'yyyy/MM/dd' !!! [invdate]";
                                    _PRProblem.Add(row);
                                    continue;
                                }
                                else
                                {
                                    _Qry = " DECLARE @TotalRow int = 0 ";
                                    _Qry += " DECLARE @TotalDel int = 0 ";
                                    _Qry += " DECLARE @Message nvarchar(500) = '' ";

                                    _Qry += " BEGIN TRANSACTION ";
                                    _Qry += " BEGIN TRY ";
                                    _Qry += " SELECT @TotalRow =COUNT(*) FROM [" +
                                        WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPackRoll " +
                                        " WHERE invno = '" + row.invno + "'";

                                    _Qry += " DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPackRoll " +
                                    " WHERE invno = '" + row.invno + "' AND FTStateClose is NULL ";
                                    _Qry += " SELECT @TotalDel = @@ROWCOUNT ";

                                    _Qry += " IF (@TotalRow = @TotalDel) ";
                                    _Qry += "   BEGIN ";
                                    _Qry += "       COMMIT TRANSACTION ";
                                    _Qry += "   END ";
                                    _Qry += " ELSE ";
                                    _Qry += "   BEGIN ";
                                    _Qry += "       set @Message = 'Total Row and Del are not equal!!!' ";
                                    _Qry += "       ROLLBACK TRANSACTION ";
                                    _Qry += "       RAISERROR('Total Row and Del are not equal!!!.',16,1) ";
                                    _Qry += "   END ";
                                    _Qry += " END TRY ";


                                    _Qry += " BEGIN CATCH ";
                                    _Qry += "   BEGIN ";
                                    _Qry += "       ROLLBACK TRANSACTION ";
                                    _Qry += "   END ";
                                    _Qry += " END CATCH ";


                                    if (!Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                                    {
                                        _PRProblem.Add(row);
                                        continue;
                                    }
                                    else
                                    {
                                        string _DataKey = "";
                                        foreach (POPackroll pr in row.poPackRoll)
                                        {
                                            DataTable _dataTable = new DataTable();
                                            string _Qry2 = "SELECT VenderCode FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender AS P WITH(NOLOCK) " +
                                                " WHERE P.PONo = '" + pr.pono + "' AND P.POItemCode = '" + pr.itemno + "' AND  P.Color = '" + pr.colorcode + "' AND  P.Size = '" + pr.size + "'";
                                            try
                                            {
                                                _dataTable = Cnn.GetDataTable(_Qry2, WSM.Conn.DB.DataBaseName.DB_VENDER);
                                                if (_dataTable.Rows.Count == 1)
                                                {
                                                    _DataKey = _dataTable.Rows[0]["VenderCode"].ToString() + "|" + row.invno + "|" + pr.pono + "|";
                                                }
                                                else
                                                {
                                                    _PRProblem.Add(row);
                                                    _problemCount++;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex);
                                            }
                                        }
                                        if (_DataKey.Length > 0)
                                        {
                                            foreach (POPackroll pr in row.poPackRoll)
                                            {
                                                foreach (Roll r in pr.roll)
                                                {
                                                    count++;
                                                    _Qry = " DECLARE @TotalEff int = 0 ";
                                                    _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) ";
                                                    _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) ";
                                                    _Qry += " DECLARE @Message nvarchar(500) = '' ";
                                                    _Qry += " BEGIN TRANSACTION ";
                                                    _Qry += " BEGIN TRY ";

                                                    _Qry += " INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPackRoll (";
                                                    _Qry += "FTInsUser, FDInsDate, FTInsTime, invno, invdate, pono, " +
                                                        "itemno, colorcode, size, colorname, rollno, width, actualwidth, actuallength, actualweight, lotno, barcode, " +
                                                        "clothno, stock, ordertype, ORDERNO, ArticleNo, Composition, StdWt, NetWtKG, GrossWtKG, " +
                                                        "Madein, Shipto, POJobFront, POJobBack, POJobSL, POJobPant, FTDataKey ) VALUES ('" + value.authen.id + "', @Date, @Time, ";
                                                    _Qry += "'" + row.invno + "','" + row.invdate + "','" + pr.pono + "','" + pr.itemno + "','" +
                                                        pr.colorcode + "','" + pr.size + "','" + pr.colorname + "','" + r.rollno + "','" +
                                                        r.width + "','" + r.actualwidth + "','" + r.actuallength + "','" + r.actualweight +
                                                        "','" + r.lotno + "','" + r.barcode + "','" + r.clothno + "','" +
                                                        r.stock + "','" + r.ordertype + "','" + r.ORDERNO + "','" +
                                                        r.ArticleNo + "','" + r.Composition + "','" + r.StdWt + "','" + r.NetWtKG + "','" +
                                                        r.GrossWtKG + "','" + r.Madein + "','" + r.Shipto + "','" + r.POJobFront + "','" +
                                                        r.POJobBack + "','" + r.POJobSL + "','" + r.POJobPant + "', '" + _DataKey + count + "' ) ";
                                                    _Qry += " SELECT @TotalEff = @@ROWCOUNT + @TotalEff ";
                                                    _Qry += " IF (@TotalEff = 1) ";
                                                    _Qry += "   BEGIN ";
                                                    _Qry += "       COMMIT TRANSACTION ";
                                                    _Qry += "   END ";
                                                    _Qry += " ELSE ";
                                                    _Qry += "   BEGIN ";
                                                    _Qry += "       set @Message = 'Total Row, Effect and Stamp not equal!!!' ";
                                                    _Qry += "       ROLLBACK TRANSACTION ";
                                                    _Qry += "       RAISERROR('Total Row, Effect and Stamp not equal!!!.',16,1) ";
                                                    _Qry += "   END ";
                                                    _Qry += " END TRY ";


                                                    _Qry += " BEGIN CATCH ";
                                                    _Qry += "   BEGIN ";
                                                    _Qry += "       ROLLBACK TRANSACTION ";
                                                    _Qry += "   END ";
                                                    _Qry += " END CATCH ";
                                                    if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                                                    {
                                                        statecheck = 1;
                                                        msgError = "Successful";
                                                        _PRPass.Add(row);
                                                        _successCount++;
                                                    }
                                                    else
                                                    {
                                                        statecheck = 2;
                                                        msgError = "Cannot save this PO";
                                                        _PRProblem.Add(row);
                                                        _problemCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        statecheck = 2;
                        msgError = "Please check User authentication!!!";
                    }
                    jsondata = JsonConvert.SerializeObject(dts);
                }
                else
                {
                    statecheck = 2;
                    msgError = "Invalid token!!!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            if (_PRProblem.Count == 0)
            {
                if (statecheck == 2)
                {
                    dts.Rows.Add(new Object[] { statecheck, msgError });
                    jsondata = JsonConvert.SerializeObject(dts);
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    dts.Rows.Add(new Object[] { statecheck, "Total Invoice packinglist accepted: " + _successCount });
                    // + " [Cannot accepted:" + _problemCount + "]" }) ;
                    jsondata = JsonConvert.SerializeObject(dts);
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                    };
                }
            }
            else
            {
                jsondata = JsonConvert.SerializeObject(_PRProblem);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
        }
    }
}