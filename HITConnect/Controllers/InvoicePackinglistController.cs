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
        /*private string columnList = "SELECT ISNULL(FTDataKey, '') AS FTDataKey, ISNULL(invno, '') AS invno, ISNULL(invdate, '') AS invdate, " +
            "ISNULL(pono, '') AS pono, ISNULL(itemno, '') AS itemno, ISNULL(colorcode, '') AS colorcode, ISNULL(size, '') AS size, " +
            "ISNULL(colorname, '') AS colorname, ISNULL(rollno, '') AS rollno, ISNULL(width, '') AS width, ISNULL(actualwidth, '') AS actualwidth, " +
            "ISNULL(actuallength, 0) AS actuallength, ISNULL(actualweight, 0) AS actualweight, ISNULL(potno, '') AS potno, " +
            "ISNULL(barcode, '') AS barcode, ISNULL(clothno, '') AS clothno, ISNULL(stock, '') AS stock, ISNULL(packno, '') AS packno, " +
            "ISNULL(packtype, '') AS packtype, ISNULL(ordertype, '') AS ordertype, ISNULL(ORDERNO, '') AS ORDERNO, ISNULL(ArticleNo, '') AS ArticleNo, " +
            "ISNULL(Composition, '') AS Composition, ISNULL(StdWt, 0) AS StdWt, ISNULL(NetWtKG, 0) AS NetWtKG, ISNULL(GrossWtKG, 0) AS GrossWtKG, " +
            "ISNULL(Madein, '') AS Madein, ISNULL(Shipto, '') AS Shipto, ISNULL(FTDateCreate, '') AS FTDateCreate, ISNULL(FTStateClose, '') AS FTStateClose, " +
            "ISNULL(FTStateCloseDate, '') AS FTStateCloseDate, ISNULL(FTStateCloseBy, '') AS FTStateCloseBy, ISNULL(FTVanderCode, '') AS FTVanderCode " +
            "FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[POPackRoll] ";*/

        /*private string columnList2 = "SELECT [FTDataKey],[invno],[invdate],[pono],[itemno],[colorcode],[size],[colorname],[rollno],[width]," +
            "[actualwidth],[actuallength],[actualweight],[potno],[barcode],[clothno],[stock],[packno],[packtype],[ordertype],[ORDERNO]," +
            "[ArticleNo],[Composition],[StdWt],[NetWtKG],[GrossWtKG],[Madein],[Shipto],[FTDateCreate],[FTStateClose],[FTStateCloseDate]," +
            "[FTStateCloseBy],[FTVanderCode] FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].[dbo].[POPackRoll] ";*/

        [HttpPost]
        [Route("api/InvoicePackinglist/")]
        public HttpResponseMessage InvoicePackinglist(InvoicePackinglist value)
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

            try
            {
                if (statecheck != 2)
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
                                if (!DateTime.TryParseExact(row.invdate, "yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate))
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
                                    _Qry += " SELECT @TotalRow =COUNT(*) FROM [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPackRoll " +
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
                                        _Qry = " DECLARE @TotalEff int = 0 ";
                                        _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) ";
                                        _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) ";
                                        _Qry += " DECLARE @Message nvarchar(500) = '' ";

                                        _Qry += " BEGIN TRANSACTION ";
                                        _Qry += " BEGIN TRY ";
                                        int count = 0;
                                        foreach (POPackroll pr in row.poPackRoll)
                                        {
                                            count = 0;
                                            foreach (Roll r in pr.roll)
                                            {
                                                _Qry += " INSERT INTO [" + WSM.Conn.DB.DataBaseName.DB_VENDER + "].dbo.POPackRoll ( " +
                                                    " [invno], [invdate], [pono], [itemno], [colorcode], [size], [colorname], [rollno], " +
                                                    " [width], [actualwidth], [actuallength], [actualweight], [lotno], [barcode], " +
                                                    " [clothno], [stock], [packno], [packtype], [ordertype], [ORDERNO], [ArticleNo], " +
                                                    " [Composition], [StdWt], [NetWtKG], [GrossWtKG], [Madein], [Shipto], [FTDataKey], " +
                                                    " [FTInsUser], [FDInsDate], [FTInsTime] ) VALUES (";
                                                _Qry += "'" + row.invno + "','" + row.invdate + "','" + pr.pono + "','" + pr.itemno + "','" +
                                                    pr.colorcode + "','" + pr.size + "','" + pr.colorname + "','" + r.rollno + "','" +
                                                    r.width + "','" + r.actualweight + "','" + r.actuallength + "','" + r.actualweight +
                                                    "','" + r.actualweight + "','" + r.lotno + "','" + r.barcode + "','" + r.clothno + "','" +
                                                    r.stock + "','" + r.packno + "','" + r.ordertype + "','" + r.ORDERNO + "','" +
                                                    r.ArticleNo + "','" + r.Composition + "','" + r.StdWt + "','" + r.NetWtKG + "','" +
                                                    r.GrossWtKG + "','" + r.Madein + "','" + r.Shipto + "','" + row.invno + pr.pono +
                                                    count++ + "','" + value.authen.id + "', @Date, @Time) ";
                                                _Qry += " SELECT @TotalEff = @@ROWCOUNT + @TotalEff ";
                                            }
                                        }
                                        _Qry += " IF (@TotalEff = " + count + ") ";
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
                                        }
                                        else
                                        {
                                            statecheck = 2;
                                            msgError = "Cannot save this PO";
                                            _PRProblem.Add(row);
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