using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class PerformaInvoiceController : ApiController
    {
        /*
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "String2" };
        }
        */

        [HttpPost]
        [Route("api/PerformaInvoiceInfo/")]
        public HttpResponseMessage PerformaInvoiceInfo([FromBody] PerformaInvoice value)
        {
            List<PI> _PIProblem = new List<PI>();
            List<PI> _PIPass = new List<PI>();
            string _Qry = "";
            int statecheck = 0;
            string msgError = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            // Check id + pwd + vender group
            List<string> _result = UserAuthen.ValidateField(value.authen);
            statecheck = int.Parse(_result[0]);
            msgError = _result[1];
            // End Check id + pwd + vender group

            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            try
            {
                if (statecheck != 2 && value.authen.token != "")
                {
                    dt = HITConnect.UserAuthen.GetDTUserValidate(Cnn, value.authen);

                    // Delete Old Token from Database
                    UserAuthen.DelAuthenKey(Cnn, value.authen.id);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        // Verify Format Date "yyyy/MM/dd" and PO Quantity
                        foreach (PI _pi in value.pi)
                        {
                            statecheck = 0;
                            DateTime tempDate;
                            if (_pi.FTDocDate != "")
                            {
                                if (!DateTime.TryParseExact(_pi.FTDocDate, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                                {
                                    statecheck = 2;
                                    msgError = "Please check Date format 'yyyy/MM/dd' !!! [FTDocDate]";
                                }
                            }
                            if (_pi.T2_Confirm_Ship_Date != "")
                            {
                                if (!DateTime.TryParseExact(_pi.T2_Confirm_Ship_Date, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                                {
                                    statecheck = 2;
                                    msgError = "Please check Date format 'yyyy/MM/dd' !!! [T2_Confirm_Ship_Date]";
                                    break;
                                }
                            }
                            if (_pi.Actualdeldate != "")
                            {
                                if (!DateTime.TryParseExact(_pi.Actualdeldate, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                                {
                                    statecheck = 2;
                                    msgError = "Please check Date format 'yyyy/MM/dd' !!! [Actualdeldate]";
                                    break;
                                }
                            }
                            if (_pi.Estimatedeldate != "")
                            {
                                if (!DateTime.TryParseExact(_pi.Estimatedeldate, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                                {
                                    statecheck = 2;
                                    msgError = "Please check Date format 'yyyy/MM/dd' !!! [Estimatedeldate]";
                                    break;
                                }
                            }
                            double _PIQuantity = 0;
                            foreach (PO _po in _pi.po)
                            {
                                _PIQuantity = +_po.FNPOQty;
                                if (_po.RcvDate != "")
                                {
                                    if (!DateTime.TryParseExact(_po.RcvDate, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                                    {
                                        statecheck = 2;
                                        msgError = "Please check Date format 'yyyy/MM/dd' !!! [RcvDate]";
                                        break;
                                    }
                                }
                                if (_po.T2_Confirm_PO_Date != "")
                                {
                                    if (!DateTime.TryParseExact(_po.T2_Confirm_PO_Date, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                                    {
                                        statecheck = 2;
                                        msgError = "Please check Date format 'yyyy/MM/dd' !!! [T2_Confirm_PO_Date]";
                                        break;
                                    }
                                }

                            }

                            if (_pi.FNPIQuantity != _PIQuantity)
                            {
                                statecheck = 2;
                                msgError = "Please check PO Quantity!!!";
                            }


                            if (statecheck == 2)
                            {
                                _PIProblem.Add(_pi);
                            }
                            else
                            {
                                _PIPass.Add(_pi);
                            }
                        }
                        // End Verify Format Date "yyyy/MM/dd" and PO Quantity

                        int count = 0;
                        if (_PIPass.Count > 0)
                        {
                            try
                            {
                                foreach (PI _pi in _PIPass)
                                {
                                    int seq = 1;
                                    foreach (PO _po in _pi.po)
                                    {
                                        _Qry = "  DECLARE @TotalRow int = 0 ";
                                        _Qry += " DECLARE @TotalEff int = 0 ";
                                        _Qry += " DECLARE @TotalUpdate int = 0 ";
                                        _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) ";
                                        _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) ";
                                        _Qry += " DECLARE @Message nvarchar(500) = '' ";

                                        _Qry += " BEGIN TRANSACTION ";
                                        _Qry += " BEGIN TRY ";

                                        /*_Qry += " DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                                            " WHERE FTDocNo = '" + _pi.FTDocNo + "' AND PONo = '" + _po.PONo + "' ";*/
                                        
                                        _Qry += "IF (select count(1) FROM [DB_VENDER].dbo.POToVenderConfirm  WHERE FTDocNo = '" +
                                            _pi.FTDocNo + "' AND PONo = '" + _po.PONo + "' ) > 0";
                                        _Qry += " BEGIN ";
                                        
                                        _Qry += " UPDATE [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm ";
                                        //[FTFileRef],[Estimatedeldate]
                                        string StateHasFile = (_pi.FTFileRef.Length > '0') ? "1" : "0";
                                        _Qry += " SET FTUpdUser = '" + value.authen.id + "', FDUpdDate = @Date, FTUpdTime = @Time, PONo = '" + _po.PONo + "', POItemCode = '" + _po.POItemCode + "', Color = '" +
                                            _po.Color + "', Size = '" + _po.Size + "', FNPOQty = " + _po.FNPOQty + ", FNSeq = " + seq++ + ", FNDocType = " + _po.FNDocType + ", FTDocNo = '" + _pi.FTDocNo + "', FTDocDate = '" +
                                            _pi.FTDocDate + "', T2_Confirm_Ship_Date = '" + _pi.T2_Confirm_Ship_Date + "', T2_Confirm_Price = " + _pi.T2_Confirm_Price + ", T2_Confirm_Quantity = " +
                                            _pi.T2_Confirm_Quantity + ", T2_Confirm_OrderNo = '" + _po.T2_Confirm_OrderNo + "', T2_Confirm_PO_Date = '" + _po.T2_Confirm_PO_Date + "', T2_Confirm_By = '" +
                                            _pi.T2_Confirm_By + "', T2_Confirm_Note = '" + _pi.T2_Confirm_Note + "', Actualdeldate = '" + //_pi.Estimatedeldate + "','" +
                                            _pi.Actualdeldate + "', RcvQty = " + _po.RcvQty + ", RcvDate = '" + _po.RcvDate + "', FTStateHasFile = '" + StateHasFile + "', InvoiceNo = '" + _pi.InvoiceNo + "', FNPINetAmt = " +
                                            _pi.FNPINetAmt + ", FNPIQuantity = " + _pi.FNPIQuantity + ", FTAWBNo = '" + _pi.FTAWBNo + "' " +
                                            " WHERE FTDocNo = '" + _pi.FTDocNo + "' AND PONo = '" + _po.PONo + "'";
                                        _Qry += " SELECT @TotalEff=@@ROWCOUNT ";
                                        //_pi.FTFileRef + "', '" +
                                        _Qry += " END ELSE BEGIN ";
                                        
                                        _Qry += " INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                                        "([FTInsUser], [FDInsDate], [FTInsTime], [PONo], [POItemCode], " +
                                        "[Color], [Size], [FNPOQty], [FNSeq], [FNDocType], [FTDocNo], " +
                                        "[FTDocDate], [T2_Confirm_Ship_Date], [T2_Confirm_Price], " +
                                        "[T2_Confirm_Quantity], [T2_Confirm_OrderNo], [T2_Confirm_PO_Date], " +
                                        "[T2_Confirm_By], [T2_Confirm_Note], [Estimatedeldate], " +
                                        "[Actualdeldate], [RcvQty], [RcvDate], [FTStateHasFile], [InvoiceNo]," +
                                        "[FNPINetAmt], [FNPIQuantity], [FTAWBNo])";
                                        //[FTFileRef],
                                        _Qry += " VALUES ('" + value.authen.id + "', @Date, @Time, '" + _po.PONo + "', '" + _po.POItemCode + "', '" +
                                            _po.Color + "', '" + _po.Size + "', " + _po.FNPOQty + ", " + seq++ + "," + _po.FNDocType + ",'" + _pi.FTDocNo + "','" +
                                            _pi.FTDocDate + "', '" + _pi.T2_Confirm_Ship_Date + "', " + _pi.T2_Confirm_Price + "," +
                                            _pi.T2_Confirm_Quantity + ", '" + _po.T2_Confirm_OrderNo + "','" + _po.T2_Confirm_PO_Date + "','" +
                                            _pi.T2_Confirm_By + "','" + _pi.T2_Confirm_Note + "', '" + _pi.Estimatedeldate + "','" +
                                            _pi.Actualdeldate + "'," + _po.RcvQty + ", '" + _po.RcvDate + "','" + StateHasFile + "','" + _pi.InvoiceNo + "', " +
                                            _pi.FNPINetAmt + ", " + _pi.FNPIQuantity + ", '" + _pi.FTAWBNo + "' ) ";
                                        //_pi.FTFileRef + "', '" +
                                        _Qry += " SELECT @TotalEff=@@ROWCOUNT ";

                                        _Qry += " END ";

                                        _Qry += " UPDATE [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK SET " +
                                            " [T2_Confirm_Ship_Date] = '" + _pi.T2_Confirm_Ship_Date + "', [T2_Confirm_Price] = '" + _pi.T2_Confirm_Price + "', " +
                                            " [T2_Confirm_Quantity] = '" + _pi.T2_Confirm_Quantity + "', [T2_Confirm_OrderNo] = '" + _po.T2_Confirm_OrderNo + "', " +
                                            " [T2_Confirm_PO_Date] = '" + _po.T2_Confirm_PO_Date + "', " + " [T2_Confirm_By]= '" + _pi.T2_Confirm_By + "', " +
                                            " [Estimatedeldate]= '" + _pi.FTDocDate + "', " + " [Actualdeldate]= '" + _pi.FTDocDate + "', " +
                                            " [RcvQty]= '" + _po.RcvQty + "', " + " [RcvDate]= '" + _po.RcvDate + "', " +
                                            " [FTStateHasFile] = '" + StateHasFile + "', " + "[InvoiceNo] = '" + _pi.InvoiceNo + "', " +
                                            " [FNPINetAmt] = '" + _pi.FNPINetAmt + "', " + " [FNPIQuantity]= '" + _pi.FNPIQuantity + "', " +
                                            " [FTAWBNo] = '" + _pi.FTAWBNo + "'" +
                                            " WHERE PONo = '" + _po.PONo + "' AND POItemCode = '" + _po.POItemCode + "' AND Color = '" + _po.Color + "'";

                                        _Qry += " SELECT @TotalUpdate=@@ROWCOUNT ";

                                        //_Qry += " SELECT @TotalRow AS TotalRows, @TotalEff AS TotalEffect , @USER AS UserName, @Vender AS Vender , @DATE + @TIME AS DateTime,@Message AS Msg ";
                                        _Qry += "IF (@TotalEff = @TotalUpdate)";
                                        _Qry += "   BEGIN ";
                                        _Qry += "       COMMIT TRANSACTION ";
                                        _Qry += "   END ";
                                        _Qry += "ELSE";
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
                                            count++;
                                            statecheck = 1;
                                            msgError = "Successful";
                                        }
                                        else
                                        {
                                            statecheck = 2;
                                            msgError = "Cannot save this PO";
                                            _PIProblem.Add(_pi);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        msgError = "Number PI are accepted is  " + count + " PI";
                    }
                    else
                    {
                        statecheck = 2;
                        msgError = "Please check User authentication!!!";
                    }
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

            if (_PIProblem.Count > 0)
            {
                jsondata = JsonConvert.SerializeObject(_PIProblem);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                dts.Rows.Add(new Object[] { statecheck, msgError });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
        }



        [HttpPost]
        [Route("api/PDF2PerformaInvoice/")]
        public HttpResponseMessage PDF2PerformaInvoice([FromBody] PDF2PerformaInvoice value)
        {
            string _Qry = "";
            string jsondata = "";
            string msgError = "";
            int statecheck = 0;
            DataTable dt = null;
            DataTable dts = new DataTable();
            DataTable dtPO = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            // Check id + pwd + vender group
            List<string> _result = UserAuthen.ValidateField(value.authen);
            statecheck = int.Parse(_result[0]);
            msgError = _result[1];
            // End Check id + pwd + vender group

            if (value.PINo == "")
            {
                statecheck = 2;
                msgError = "Please check PI number!!!";
            }
            if (statecheck != 2 && value.authen.token != "")
            {
                dt = HITConnect.UserAuthen.GetDTUserValidate(Cnn, value.authen);

                // Delete Old Token from Database
                UserAuthen.DelAuthenKey(Cnn, value.authen.id);

                if (dt != null && dt.Rows.Count > 0)
                {
                    try
                    {
                        _Qry = "  DECLARE @TotalEff int = 0 ";
                        _Qry += " DECLARE @TotalEff_ACK int = 0 ";
                        _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) ";
                        _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) ";
                        _Qry += " DECLARE @Message nvarchar(500) = '' ";

                        _Qry += " BEGIN TRANSACTION ";
                        _Qry += " BEGIN TRY ";
                        _Qry += " UPDATE [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                            " SET FTUpdUser = '" + value.authen.id + "', FDUpdDate = @Date, FTUpdTime = @Time, " +
                            " FTFileRef = CAST('" + value.pdfFile + "' AS varbinary) WHERE PINo = '" + value.PINo + "' ";
                        _Qry += " SELECT @TotalEff=@@ROWCOUNT ";

                        _Qry += " UPDATE [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK " +
                            " SET FTUpdUser = '" + value.authen.id + "', FDUpdDate = @Date, FTUpdTime = @Time, " +
                            " FTFileRef = CAST('" + value.pdfFile + "' AS varbinary) WHERE PINo = '" + value.PINo + "' ";

                        _Qry += " SELECT @TotalEff_ACK=@@ROWCOUNT ";

                        _Qry += " IF (@TotalEff = @TotalEff_ACK) ";
                        _Qry += "   BEGIN ";
                        _Qry += "       COMMIT TRANSACTION ";
                        _Qry += "   END ";
                        _Qry += " ELSE ";
                        _Qry += "   BEGIN ";
                        _Qry += "       set @Message = 'Total Row, Effect are not equal!!!' ";
                        _Qry += "       ROLLBACK TRANSACTION ";
                        _Qry += "       RAISERROR('Total Row, Effect are not equal!!!.',16,1) ";
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
                        }
                        else
                        {
                            statecheck = 2;
                            msgError = "Cannot save PDF file";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    statecheck = 2;
                    msgError = "Please check User authentication!!!";
                }
            }
            else
            {
                statecheck = 2;
                msgError = "Invalid token!!!";
            }

            if (statecheck == 1)
            {
                //jsondata = JsonConvert.SerializeObject(dtPO);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                //jsondata = JsonConvert.SerializeObject(dts);
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