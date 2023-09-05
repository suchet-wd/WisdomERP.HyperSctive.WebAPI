using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
                            if (_pi.T2_Confirm_Ship_Date2 != "")
                            {
                                if (!DateTime.TryParseExact(_pi.T2_Confirm_Ship_Date2, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                                {
                                    statecheck = 2;
                                    msgError = "Please check Date format 'yyyy/MM/dd' !!! [T2_Confirm_Ship_Date2]";
                                    break;
                                }
                            }
                            if (_pi.Actualdeldate2 != "")
                            {
                                if (!DateTime.TryParseExact(_pi.Actualdeldate2, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
                                {
                                    statecheck = 2;
                                    msgError = "Please check Date format 'yyyy/MM/dd' !!! [Actualdeldate2]";
                                    break;
                                }
                            }
                            double _PIQuantity = 0;
                            foreach (PO _po in _pi.po)
                            {
                                _PIQuantity = +_po.FNPOQty;
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

                        //int count = 0;
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

                                        _Qry = " declare @RecCount int =0,@PINo nvarchar(30) ='" + _pi.FTDocNo +
                                                "', @PINoCheck nvarchar(30)='', @pMessage nvarchar(500)='' ";
                                        if (_dt.Rows[0]["T2_Confirm_OrderNo"].ToString() != "")
                                        {
                                            _Qry += " SET @PINoCheck = ISNULL(( select top 1 FTDocNo from [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm  " +
                                                "WHERE PONo='" + _po.PONo + "' AND POItemCode='" + _po.POItemCode +
                                                "' AND Color='" + _po.Color + "' AND Size='" + _po.Size + "' AND FTDocNo='" + _pi.FTDocNo +
                                                "' ),'') ";

                                            _Qry += " IF @PINoCheck <> '' ";
                                            _Qry += " BEGIN ";

                                            _Qry += " UPDATE [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                                                " SET FTUpdUser='" + value.authen.id + "',T2_Confirm_By='" + value.authen.id + "', " +
                                                " FDUpdDate = Convert(varchar(10),Getdate(),111), FTUpdTime = Convert(varchar(8),Getdate(),114), " +
                                                " FTDocNo='" + _pi.FTDocNo + "', T2_Confirm_OrderNo='" + _pi.FTDocNo + "', " +
                                                " FTDocDate='" + DB.ConvertEnDB(_pi.FTDocDate) + "', " +
                                                " T2_Confirm_PO_Date='" + DB.ConvertEnDB(_po.T2_Confirm_PO_Date) + "', " +
                                                " Estimatedeldate='" + DB.ConvertEnDB(_pi.Estimatedeldate) + "', " +
                                                " T2_Confirm_Ship_Date='" + DB.ConvertEnDB(_pi.T2_Confirm_Ship_Date) + "', " +
                                                " T2_Confirm_Price=" + Conversion.Val(_po.T2_Confirm_Price) + ", " +
                                                " T2_Confirm_Quantity = " + Conversion.Val(_po.T2_Confirm_Quantity) + " , " +
                                                " Actualdeldate='" + DB.ConvertEnDB(_pi.Actualdeldate) + "', " +
                                                " FNPIQuantity=" + Conversion.Val(_pi.FNPIQuantity) + " , " +
                                                " FNPINetAmt=" + Conversion.Val(_pi.FNPINetAmt) + ", " +
                                                " InvoiceNo='" + _pi.InvoiceNo + "',FTAWBNo='" + _pi.FTAWBNo + "', " +
                                                " T2_Confirm_Note='" + _pi.T2_Confirm_Note + "', StateRead='0', StateExport='0', " +
                                                " T2_Confirm_ShipQuantity='" + _pi.T2_Confirm_ShipQuantity + "', " +
                                                " T2_Confirm_Ship_Date2='" + _pi.T2_Confirm_Ship_Date2 + "' , " +
                                                " T2_Confirm_ShipQuantity2='" + _pi.T2_Confirm_ShipQuantity2 + "', " +
                                                " Actualdeldate2='" + _pi.Actualdeldate2 + "' , " +
                                                " T2_Confirm_MOQQuantity='" + _pi.T2_Confirm_MOQQuantity + "', " +
                                                " Doc_Surcharge_Amount='" + _pi.Doc_Surcharge_Amount + "', " +
                                                // SendMailPriceTime = '1',
                                                " StateSendMailPrice = CASE WHEN POPrice <> " + Conversion.Val(+_po.T2_Confirm_Price) + " THEN '1' ELSE '0' END , " +
                                                " StateSendMailPriceComplete = '0' ," +
                                                " StateSendMailMOQ = CASE WHEN " + _pi.T2_Confirm_MOQQuantity + " > 0  THEN '1' ELSE '0' END , " +
                                                " StateSendMailMOQComplete = '0'" +
                                                
                                                " WHERE PONo='" + _po.PONo + "' AND POItemCode='" + _po.POItemCode +
                                                "' AND Color='" + _po.Color + "' AND Size='" + _po.Size + "' AND FTDocNo=@PINo  ";

                                            _Qry += "SET @RecCount = @@ROWCOUNT ";
                                            _Qry += " IF @RecCount > 0 ";
                                            _Qry += " BEGIN ";
                                            _Qry += " EXEC [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.USP_UPDATEPICFM '" +
                                                value.authen.id + "','" + FTDataKey + "','" + _po.PONo + "','" + _po.POItemCode + "','" +
                                                _po.Color + "','" + _po.Size + "' ";
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
                                            _Qry += " set @PINoCheck = ISNULL(( select top 1 FTDocNo  " +
                                                " from [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm  " +
                                                " WHERE PONo='" + _po.PONo + "' AND POItemCode='" + _po.POItemCode + "' " +
                                                " AND Color='" + _po.Color + "' AND Size='" + _po.Size + "' AND FTDocNo=@PINo),'') ";
                                            _Qry += " IF  @PINoCheck = '' ";
                                            _Qry += " BEGIN ";

                                            //[FTUpdUser],[FDUpdDate],[FTUpdTime],[T2_Confirm_OrderNo],[RcvQty],[RcvDate],
                                            //[FTFileRef],[ReadDate],[ReadTime],[StateExportDate],[T2_Confirm_Amount],
                                            _Qry += " INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm " +
                                                "( FTInsUser, FDInsDate, FTInsTime, PONo, POItemCode, Color, Size, FNPOQty, FNSeq, FNDocType, FTDocNo, FTDocDate, " +
                                                "T2_Confirm_Ship_Date, T2_Confirm_Price, T2_Confirm_Quantity, T2_Confirm_PO_Date, " +
                                                "T2_Confirm_By, T2_Confirm_Note, Estimatedeldate, Actualdeldate, FTStateHasFile, " +
                                                "InvoiceNo, FNPIQuantity, FNPINetAmt, FTAWBNo, StateRead, StateExport, T2_Confirm_ShipQuantity, " +
                                                "T2_Confirm_Ship_Date2, T2_Confirm_ShipQuantity2, Actualdeldate2, T2_Confirm_MOQQuantity, " +
                                                "POPrice, Doc_Surcharge_Amount, StateSendMailPrice, StateSendMailPriceComplete, StateSendMailMOQ, " +
                                                "StateSendMailMOQComplete) ";
                                            _Qry += " SELECT TOP 1  '" + value.authen.id + "', FDUpdDate = Convert(varchar(10),Getdate(),111), " +
                                                " FTUpdTime=Convert(varchar(8),Getdate(),114), '" + _po.PONo + "','" + _po.POItemCode + "','" +
                                                _po.Color + "','" + _po.Size + "',X.Quantity, ISNULL((SELECT MAX(FNSeq) AS FNSeq ";
                                            _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVenderConfirm ";
                                            _Qry += " WHERE PONo = '" + _po.PONo + "' AND POItemCode = '" + _po.POItemCode + "' AND " +
                                                "Color = '" + _po.Color + "' AND Size='" + _po.Size + "'  ),0) +1,1, '" + _pi.FTDocNo + "','" +
                                                DB.ConvertEnDB(_pi.FTDocDate) + "' ,'" + DB.ConvertEnDB(_pi.T2_Confirm_Ship_Date) + "'," +
                                                Conversion.Val(+_po.T2_Confirm_Price) + ", " + Conversion.Val(_po.T2_Confirm_Quantity) + ",'" +
                                                DB.ConvertEnDB(_po.T2_Confirm_PO_Date) + "','" + value.authen.id + "', '" +
                                                _pi.T2_Confirm_Note + "','" + DB.ConvertEnDB(_pi.Estimatedeldate) + "','" +
                                                DB.ConvertEnDB(_pi.Actualdeldate) + "','0','" + _pi.InvoiceNo + "', " +
                                                Conversion.Val(_pi.FNPIQuantity) + " , " + Conversion.Val(_pi.FNPINetAmt) + " , '" +
                                                _pi.FTAWBNo + "','0','0'," + _pi.T2_Confirm_ShipQuantity + " , '" +
                                                DB.ConvertEnDB(_pi.T2_Confirm_Ship_Date2) + "' , " + _pi.T2_Confirm_ShipQuantity2 + " , '" +
                                                DB.ConvertEnDB(_pi.Actualdeldate2) + "' , " + _pi.T2_Confirm_MOQQuantity + " , X.Price, " +
                                                _pi.Doc_Surcharge_Amount +
                                                ", CASE WHEN X.Price <> " + Conversion.Val(+_po.T2_Confirm_Price) + " THEN '1' ELSE '0' END " +
                                                ", CASE WHEN " + _pi.T2_Confirm_MOQQuantity + " > 0  THEN '1' ELSE '0' END , '0','0'";
                                            //_Qry += (_pi.T2_Confirm_MOQQuantity > 0) ? "'1'" : "'0'";

                                            _Qry += " FROM  [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POToVender_ACK AS X WITH(NOLOCK) ";
                                            _Qry += " WHERE FTDataKey='" + FTDataKey + "'  ";
                                            _Qry += " SET @RecCount = @@ROWCOUNT ";
                                            _Qry += " IF @RecCount > 0 ";
                                            _Qry += " BEGIN ";
                                            _Qry += " EXEC [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.USP_UPDATEPICFM  '" + value.authen.id + "','" + 
                                                FTDataKey + "','" + _po.PONo + "','" + _po.POItemCode + "','" + _po.Color + "','" + _po.Size + "' ";
                                            // Send Mail Confirm Price
                                            _Qry += " EXEC [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.USP_MAILNOTIFY_CONFRITM_PRICE '" +
                                                value.authen.id + "'";
                                            _Qry += " END ";
                                            _Qry += " END ";
                                            _Qry += " ELSE ";
                                            _Qry += " BEGIN ";
                                            _Qry += " SET @pMessage ='Duplicate document number !!!'   ";
                                            _Qry += " END ";

                                            _Qry += " select @RecCount AS FNTotalRec, @pMessage AS FTMessage  ";
                                        }

                                        dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.DB_VENDER);
                                        foreach (DataRow R in dt.Rows)
                                        {
                                            int rec = (int)Conversion.Val(R["FNTotalRec"].ToString());
                                            if (rec > 0)
                                            {
                                                statecheck = 1;
                                                msgError = "Total Receive = " + rec;
                                                _PIPass.Add(_pi);
                                                if (_pi.FTFileRef != null)
                                                {
                                                    foreach (PO _potemp in _pi.po)
                                                    {
                                                        _Qry = createSqlUploadPDF(value.authen.id, _pi.FTFileRef, _potemp.PONo);
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
                                                }
                                            }
                                            else
                                            {
                                                statecheck = 2;
                                                msgError = R["FTMessage"].ToString();
                                                _PIProblem.Add(_pi);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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
                msgError = "Please check PO number!!!";
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
                        _Qry = createSqlUploadPDF(value.authen.id, value.pdfFile, value.PINo);
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


        private string createSqlUploadPDF(string id, string pdfFile, string poNo)
        {
            string _Qry = "  DECLARE @TotalEff int = 0 ";
            _Qry += " DECLARE @TotalEff_ACK int = 0 ";
            _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) ";
            _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) ";
            _Qry += " DECLARE @Message nvarchar(500) = '' ";

            _Qry += " BEGIN TRANSACTION ";
            _Qry += " BEGIN TRY ";
            _Qry += " UPDATE " + WSM.Conn.DB.DataBaseName.DB_VENDER + ".dbo.POToVenderConfirm " +
                " SET FTUpdUser = '" + id + "', FDUpdDate = @Date, FTUpdTime = @Time, " +
            " FTFileRef = CAST(N'' AS xml).value('xs:base64Binary(\"" + pdfFile + "\")', 'varbinary(max)'), FTStateHasFile = '1' WHERE PONo = '" + poNo + "' ";
            _Qry += " SELECT @TotalEff=@@ROWCOUNT ";

            _Qry += " UPDATE " + WSM.Conn.DB.DataBaseName.DB_VENDER + ".dbo.POToVender_ACK " +
                " SET FTStateHasFile = '1' WHERE PONo = '" + poNo + "' ";
            _Qry += " SELECT @TotalEff_ACK=@@ROWCOUNT ";

            _Qry += " IF (@TotalEff > 0 AND @TotalEff_ACK > 0) ";
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
            return _Qry;
        }
    }
}