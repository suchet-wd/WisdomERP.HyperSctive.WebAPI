using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class PIController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "String2" };
        }

        [HttpPost]
        [Route("api/RcPI/")]
        public HttpResponseMessage RcPI(RcPI PIdata)
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

            // Verify Format Date "yyyy/MM/dd"
            foreach (PI _pi in PIdata.pi)
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
                    if (_po.T2_Confirm_Ship_Date != "")
                    {
                        if (!DateTime.TryParseExact(_po.T2_Confirm_Ship_Date, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                        {
                            statecheck = 2;
                            msgError = "Please check Date format 'yyyy/MM/dd' !!! [T2_Confirm_Ship_Date]";
                            break;
                        }
                    }
                    if (_po.Actualdeldate != "")
                    {
                        if (!DateTime.TryParseExact(_po.Actualdeldate, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                        {
                            statecheck = 2;
                            msgError = "Please check Date format 'yyyy/MM/dd' !!! [Actualdeldate]";
                            break;
                        }
                    }
                    if (_po.Estimatedeldate != "")
                    {
                        if (!DateTime.TryParseExact(_po.Estimatedeldate, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                        {
                            statecheck = 2;
                            msgError = "Please check Date format 'yyyy/MM/dd' !!! [Estimatedeldate]";
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
            // End Verify Format Date "yyyy/MM/dd"

            int count = 0;
            if (_PIPass.Count > 0)
            {
                try
                {
                    WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

                    _Qry = " SELECT V.Pwd, V.VanderMailLogIn, ATK.DataKey, VUP.VanderGrp " +
                        " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.VenderUser AS V  " +
                        " LEFT JOIN [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys AS ATK ON ATK.VanderMailLogIn = V.VanderMailLogIn  " +
                        " LEFT JOIN[DB_VENDER].dbo.VenderUserPermissionCmp AS VUP ON VUP.VanderMailLogIn = V.VanderMailLogIn " +
                        " WHERE V.VanderMailLogIn = '" + PIdata.authen.id + "' AND VUP.VanderGrp = '" + PIdata.authen.venderCode + "' ";
                    dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                    if (dt != null)
                    {// REMOVE TOKEN FROM AuthenKeys
                        _Qry = "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.AuthenKeys WHERE VanderMailLogIn = '" +
                            PIdata.authen.id + "'";

                        if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER))
                        {
                            try
                            {
                                foreach (PI _pi in _PIPass)
                                {
                                    /*
                                    _Qry = "SELECT DISTINCT FTDocNo from [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                                        "WHERE FTDocNo = '" + _pi.FTDocNo + "'";
                                    dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                                    if (dt != null)
                                    {
                                        _Qry = "DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                                            "WHERE FTDocNo = '" + _pi.FTDocNo + "'";
                                        bool delOK = (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER));
                                    }
                                    */
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
                                        _Qry += " DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                                            "WHERE FTDocNo = '" + _pi.FTDocNo + "' AND PONo = '" + _po.PONo + "' ";

                                        _Qry += " INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                                        "([FTInsUser], [FDInsDate], [FTInsTime], [PONo], [POItemCode], " +
                                        "[Color], [Size], [FNPOQty], [FNSeq], [FNDocType], [FTDocNo], " +
                                        "[FTDocDate], [T2_Confirm_Ship_Date], [T2_Confirm_Price], " +
                                        "[T2_Confirm_Quantity], [T2_Confirm_OrderNo], [T2_Confirm_PO_Date], " +
                                        "[T2_Confirm_By], [T2_Confirm_Note], [Estimatedeldate], " +
                                        "[Actualdeldate], [RcvQty], [RcvDate], [FTStateHasFile], [InvoiceNo]," +
                                        "[FNPINetAmt], [FNPIQuantity], [FTAWBNo])";
                                        //[FTFileRef],
                                        _Qry += " VALUES ('" + PIdata.authen.id + "', @Date, @Time, '" + _po.PONo + "', '" + _po.POItemCode + "', '" +
                                            _po.Color + "', '" + _po.Size + "', " + _po.FNPOQty + ", " + seq++ + "," + _po.FNDocType + ",'" + _pi.FTDocNo + "','" +
                                            _pi.FTDocDate + "', '" + _po.T2_Confirm_Ship_Date + "', " + _po.T2_Confirm_Price + "," +
                                            _po.T2_Confirm_Quantity + ", '" + _po.T2_Confirm_OrderNo + "','" + _po.T2_Confirm_PO_Date + "','" +
                                            _po.T2_Confirm_By + "','" + _po.T2_Confirm_Note + "', '" + _po.Estimatedeldate + "','" +
                                            _po.Actualdeldate + "'," + _po.RcvQty + ", '" + _po.RcvDate + "','" + _pi.FTStateHasFile + "','" + _pi.InvoiceNo + "', " +
                                            _pi.FNPINetAmt + ", " + _pi.FNPIQuantity + ", '" + _pi.FTAWBNo + "' ) ";
                                        //_pi.FTFileRef + "', '" +

                                        _Qry += " SELECT @TotalEff=@@ROWCOUNT ";

                                        _Qry += " UPDATE [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK SET " +
                                            " [T2_Confirm_Ship_Date] = '" + _po.T2_Confirm_Ship_Date + "', [T2_Confirm_Price] = '" + _po.T2_Confirm_Price + "', " +
                                            " [T2_Confirm_Quantity] = '" + _po.T2_Confirm_Quantity + "', [T2_Confirm_OrderNo] = '" + _po.T2_Confirm_OrderNo + "', " +
                                            " [T2_Confirm_PO_Date] = '" + _po.T2_Confirm_PO_Date + "', " + " [T2_Confirm_By]= '" + _po.T2_Confirm_By + "', " +
                                            " [Estimatedeldate]= '" + _pi.FTDocDate + "', " + " [Actualdeldate]= '" + _pi.FTDocDate + "', " +
                                            " [RcvQty]= '" + _po.RcvQty + "', " + " [RcvDate]= '" + _po.RcvDate + "', " +
                                            " [FTStateHasFile] = '" + _pi.FTStateHasFile + "', " + "[InvoiceNo] = '" + _pi.InvoiceNo + "', " +
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
                                Console.WriteLine(ex);
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
                        msgError = "Please check User and Password!!!";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            if (_PIProblem.Count > 0)
            {
                jsondata = JsonConvert.SerializeObject(_PIProblem);
                //return new HttpResponseMessage { StatusCode = HttpStatusCode.NotAcceptable, Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + "0" + (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + msgError + (char)34 + "}", System.Text.Encoding.UTF8, "application/json") };
                return new HttpResponseMessage { StatusCode = HttpStatusCode.NotAcceptable, Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
            }
            else
            {
                msgError = "Total accept : " + count;
                dts.Rows.Add(new Object[] { statecheck, msgError });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
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
    }
}
