using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;
using System.Web.UI.WebControls;
using WSM.Conn;
using static System.Net.WebRequestMethods;

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
                            /*
                            if (_pi.FNPIQuantity != _PIQuantity)
                            {
                                statecheck = 2;
                                msgError = "Please check PO Quantity!!!";
                            }
                            */

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
                                        string FTDataKey = "";
                                        DataTable _dt = new DataTable();

                                        // Retrive FTDataKey from POToVender_ACK
                                        _Qry = "SELECT FTDataKey FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK " +
                                            " WHERE PONo = '" + _po.PONo + "' AND POItemCode = '" + _po.POItemCode +
                                            "' AND Color = '" + _po.Color + "' AND Size = '" + _po.Size + "'";
                                        _dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                                        if (_dt.Rows.Count == 1)
                                        {
                                            foreach (DataRow r in _dt.Rows)
                                            {
                                                FTDataKey = r["FTDataKey"].ToString();
                                            }
                                        }
                                        // Check POToVender_ACK have data ? Yes -> Update / No -> Add
                                        _Qry = "SELECT T2_Confirm_OrderNo FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK " +
                                            " WHERE FTDataKey = '" + FTDataKey + "'";
                                        _dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                                        if (_dt.Rows.Count > 0)
                                        {
                                            _Qry = " declare @RecCount int =0,@PINo nvarchar(30) ='" + _pi.FTDocNo +
                                                "',@PINoCheck nvarchar(30)='',@pMessage nvarchar(500)='' ";

                                            _Qry += " set @PINoCheck = ISNULL(( select top 1 FTDocNo from [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm  " +
                                                "WHERE PONo='" + _po.PONo + "' AND POItemCode='" + _po.POItemCode +
                                                "' AND Color='" + _po.Color + "' AND Size='" + _po.Size + "' AND FTDocNo='" + _pi.FTDocNo +
                                                "' AND FTDocNo<>@PINo),'') ";
                                            _Qry += " IF @PINoCheck ='' ";
                                            _Qry += " BEGIN ";

                                            _Qry += " update POToVenderConfirm set FTUpdUser='" + value.authen.id +
                                                "',T2_Confirm_By='" + value.authen.id + "', FDUpdDate = Convert(varchar(10),Getdate(),111), " +
                                                " FTUpdTime = Convert(varchar(8),Getdate(),114), FTDocNo='" + _pi.FTDocNo + "', " +
                                                " T2_Confirm_OrderNo='" + _pi.FTDocNo + "', FTDocDate='" + DB.ConvertEnDB(_pi.FTDocDate) +
                                                "' ,T2_Confirm_PO_Date='" + DB.ConvertEnDB(_po.T2_Confirm_PO_Date) + "', Estimatedeldate='" +
                                                DB.ConvertEnDB(_pi.Estimatedeldate) + "', T2_Confirm_Ship_Date='" + DB.ConvertEnDB(_pi.T2_Confirm_Ship_Date) +
                                                "', T2_Confirm_Price=" + Conversion.Val(_pi.T2_Confirm_Price) + ", T2_Confirm_Quantity = " +
                                                Conversion.Val(_pi.T2_Confirm_Quantity) + " ,  Actualdeldate='" + DB.ConvertEnDB(_pi.Actualdeldate) +
                                                "', FNPIQuantity=" + Conversion.Val(_pi.FNPIQuantity) + " , FNPINetAmt=" + Conversion.Val(_pi.FNPINetAmt) +
                                                ", InvoiceNo='" + _pi.InvoiceNo + "',FTAWBNo='" + _pi.FTAWBNo + "',T2_Confirm_Note='" + _pi.T2_Confirm_Note +
                                                "',StateRead='0',StateExport='0' WHERE PONo='" + _po.PONo + "' AND POItemCode='" + _po.POItemCode +
                                                "' AND Color='" + _po.Color + "' AND Size='" + _po.Size + "' AND FTDocNo=@PINo AND StateRead='0' ";
                                            _Qry += "SET @RecCount = @@ROWCOUNT ";
                                            _Qry += " IF @RecCount > 0 ";
                                            _Qry += " BEGIN ";
                                            _Qry += " EXEC USP_UPDATEPICFM '" + value.authen.id + "','" + FTDataKey + "','" + _po.PONo + "','" + _po.POItemCode + "','" + _po.Color + "','" + _po.Size + "' ";
                                            _Qry += " END ";
                                            _Qry += " END ";
                                            _Qry += " ELSE ";
                                            _Qry += " BEGIN ";
                                            _Qry += " SET @pMessage ='Duplicate document number !!!'   ";
                                            _Qry += " END ";
                                            _Qry += " select @RecCount AS FNTotalRec,@pMessage AS FTMessage  ";

                                        }
                                        else
                                        {

                                            _Qry = "declare @RecCount int =0, @PINo nvarchar(30) ='" + _pi.FTDocNo +
                                                "', @PINoCheck nvarchar(30)='',@pMessage nvarchar(500)='' ";
                                            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);

                                            _Qry += " set @PINoCheck = ISNULL(( select top 1 FTDocNo  from [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm  WHERE PONo='" +
                                                _po.PONo + "' AND POItemCode='" + _po.POItemCode + "' AND Color='" + _po.Color + "' AND Size='" +
                                                _po.Size + "' AND FTDocNo=@PINo),'') ";
                                            _Qry += " IF  @PINoCheck = '' ";
                                            _Qry += " BEGIN ";

                                            _Qry += " insert into POToVenderConfirm( FTInsUser, FDInsDate, FTInsTime, PONo, POItemCode, " +
                                                "Color, Size, FNPOQty, FNSeq, FNDocType, FTDocNo, FTDocDate, T2_Confirm_Ship_Date, " +
                                                "T2_Confirm_Price, T2_Confirm_Quantity, T2_Confirm_OrderNo, T2_Confirm_PO_Date, " +
                                                "T2_Confirm_By, T2_Confirm_Note, Estimatedeldate, Actualdeldate, FTStateHasFile, " +
                                                "InvoiceNo, FNPIQuantity, FNPINetAmt, FTAWBNo, StateRead, StateExport ) ";

                                            _Qry += " SELECT TOP 1  '" + value.authen.id + "', FDUpdDate = Convert(varchar(10),Getdate(),111), " +
                                                " FTUpdTime=Convert(varchar(8),Getdate(),114), '" + _po.PONo + "','" + _po.POItemCode + "','" +
                                                _po.Color + "','" + _po.Size + "',X.Quantity, ISNULL((SELECT MAX(FNSeq) AS FNSeq ";
                                            _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm ";
                                            _Qry += " WHERE PONo = '" + _po.PONo + "' AND POItemCode = '" + _po.POItemCode + "' AND Color = '" +
                                                _po.Color + "' AND Size='" + _po.Size + "'  ),0) +1,0, '" + _pi.FTDocNo + "','" +
                                                DB.ConvertEnDB(_pi.FTDocDate) + "' ,'" + DB.ConvertEnDB(_pi.T2_Confirm_Ship_Date) + "'," +
                                                Conversion.Val(+_pi.T2_Confirm_Price) + ", " + Conversion.Val(_pi.T2_Confirm_Quantity) + ",'" +
                                                _po.T2_Confirm_OrderNo + "','" + DB.ConvertEnDB(_po.T2_Confirm_PO_Date) + "','" + value.authen.id + "', '" +
                                                _pi.T2_Confirm_Note + "','" + DB.ConvertEnDB(_pi.Estimatedeldate) + "','" +
                                                DB.ConvertEnDB(_pi.Actualdeldate) + "','0','" + _pi.InvoiceNo + "', " +
                                                Conversion.Val(_pi.FNPIQuantity) + " , " + Conversion.Val(_pi.FNPINetAmt) + " , '" +
                                                _pi.FTAWBNo + "','0','0'";
                                            _Qry += " FROM  [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK AS X WITH(NOLOCK) ";
                                            _Qry += " WHERE FTDataKey='" + FTDataKey + "'  ";
                                            _Qry += " SET @RecCount = @@ROWCOUNT ";
                                            _Qry += " IF @RecCount > 0 ";
                                            _Qry += " BEGIN ";
                                            _Qry += " EXEC USP_UPDATEPICFM  '" + value.authen.id + "','" + FTDataKey + "','" + _po.PONo + "','" + _po.POItemCode + "','" + _po.Color + "','" + _po.Size + "' ";
                                            _Qry += " END ";
                                            _Qry += " END ";
                                            _Qry += " ELSE ";
                                            _Qry += " BEGIN ";
                                            _Qry += " SET @pMessage ='Duplicate document number !!!'   ";
                                            _Qry += " END ";
                                            _Qry += " select @RecCount AS FNTotalRec,@pMessage AS FTMessage  ";
                                        }

                                        dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                                        foreach (DataRow R in dt.Rows)
                                        {
                                            int rec = (int)Conversion.Val(R["FNTotalRec"].ToString());
                                            if (rec > 0)
                                            {
                                                statecheck = 1;
                                                msgError = "Total Receive = " + rec ;
                                                _PIPass.Add(_pi);
                                            }
                                            else
                                            {
                                                statecheck = 2;
                                                msgError = R["FTMessage"].ToString();
                                                _PIProblem.Add(_pi);
                                            }
                                        }

                                        // OLD Code
                                        /*
                                        _Qry = "  DECLARE @TotalRow int = 0 ";
                                        _Qry += " DECLARE @TotalEff int = 0 ";
                                        _Qry += " DECLARE @TotalUpdate int = 0 ";
                                        _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) ";
                                        _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) ";
                                        _Qry += " DECLARE @Message nvarchar(500) = '' ";

                                        _Qry += " BEGIN TRANSACTION ";
                                        _Qry += " BEGIN TRY ";
                                        */
                                        /*_Qry += " DELETE FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                                            " WHERE FTDocNo = '" + _pi.FTDocNo + "' AND PONo = '" + _po.PONo + "' ";*/
                                        /*
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
                                        */
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        //msgError = "Number PI are accepted is  " + count + " PI";
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