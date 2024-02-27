using Newtonsoft.Json;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HyperConvert.Controllers
{
    public class RfidController : ApiController
    {
        [HttpPost]
        [Route("api/GetRfidInfo/")]
        public HttpResponseMessage GetRfidInfo([FromBody] APIRfid value)
        {
            string _Qry = "";
            string jsondata = "";
            //WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            // Checking All Barcode
            foreach (APIRfidBarcode rb in value.BundleRfidBarcodeList)
            {
                if (APIRfidBarcode.isParentBundleBarcode(rb.ParentBundleBarcode) == false)
                {
                    dts.Rows.Add(new Object[] { "1", "Please check Parent Bundle Barcode (" + rb.ParentBundleBarcode + ")" });
                    jsondata = JsonConvert.SerializeObject(dts);
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                    };
                }

                foreach (string s in rb.BundleBarcode)
                {
                    if (APIRfidBarcode.isBundleBarcode(s) == false)
                    {
                        dts.Rows.Add(new Object[] { "1", "Please check Bundle Barcode (" + s + ")" });
                        jsondata = JsonConvert.SerializeObject(dts);
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                        };
                    }
                }
            }

            int i = 1;
            int j = 1;

            _Qry += " DECLARE @RecCount int = 0 \n";
            _Qry += " DECLARE @Date varchar(10) = Convert(varchar(10), Getdate(), 111) \n";
            _Qry += " DECLARE @Time varchar(10) = Convert(varchar(8), Getdate(), 114) \n\n";
            _Qry += " DECLARE @Message nvarchar(500) = '' \n\n";

            _Qry += " BEGIN TRANSACTION \n";
            _Qry += " BEGIN TRY \n";

            foreach (APIRfidBarcode b in value.BundleRfidBarcodeList)
            {
                j++;
                foreach (string s in b.BundleBarcode)
                {
                    _Qry += "INSERT INTO [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.TSMGtoWisdom_Staging \n";
                    _Qry += "(FTInsUser, FDInsDate, FTInsTime, FTBoxRfId, FTBoxBarCode, FTBundleRfId, FTBundleParentBarcode, FTBundleBarcode, FTStateProcess, FTSMTimeStamp) \n";
                    _Qry += "VALUES('', @Date, @Time, ";
                    _Qry += "'" + value.BoxRfid + "', '" + value.BoxBarcode + "', '" + b.Rfid + "', '" + b.ParentBundleBarcode + "', '" + s + "', '0', '" + value.TimeStamp + "'";
                    _Qry += "); \n";
                    _Qry += "SET @RecCount = @RecCount + @@ROWCOUNT \n\n";
                    i++;
                }
            }
            _Qry += " IF @RecCount <> " + i;
            _Qry += "   BEGIN \n";
            _Qry += "       COMMIT TRANSACTION \n";
            _Qry += "   END \n\n";

            _Qry += " ELSE \n";

            _Qry += "   BEGIN \n";
            _Qry += "       ROLLBACK TRANSACTION \n";
            _Qry += "   END \n\n";
            _Qry += " END TRY \n\n";

            _Qry += " BEGIN CATCH \n";
            _Qry += "   BEGIN \n";
            _Qry += "       ROLLBACK TRANSACTION \n";
            _Qry += "   END \n\n";

            _Qry += " END CATCH \n";

            if (new WSM.Conn.SQLConn().ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE))
            {
                dts.Rows.Add(new Object[] { "0", "Save total " + j + " Rfid (Total " + i + " Records) Successful." });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else
            {
                dts.Rows.Add(new Object[] { "1", "Cannot Save " + j + " Rfid (Total " + i + " Records)!!!" });
                jsondata = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
                };
            }
        } // End GetRfidInfo
    } // End Class
} // End namespace


//if (value.Company == "" && value.OrderNo == "" &&
//    value.OrderStartDate == "" && value.OrderEndDate == "" &&
//    value.ShipmentStartDate == "" && value.ShipmentEndDate == "")
//{
//    _state = false;
//    _msg = "Please specify the condition!!!";
//}
//else
//{
//    if (!APIOrder.checkPeriodFormat(value.OrderStartDate, value.OrderEndDate) && value.OrderStartDate != "" && value.OrderEndDate != "" && _state)
//    {
//        _state = false;
//        _msg = "Order Start date must be before end date !!!";
//    }
//    if (!APIOrder.checkPeriodFormat(value.ShipmentStartDate, value.ShipmentEndDate) && value.ShipmentStartDate != "" && value.ShipmentEndDate != "" && _state)
//    {
//        _state = false;
//        _msg = "Shipment Start date must be before end date !!!";
//    }
//    if (!APIOrder.checkCompany(value.Company) && _state)
//    {
//        _state = false;
//        _msg = "Please check Company Code !!!";
//    }
//}

//if (_state)
//{
//    _Qry = "SELECT ISNULL(b.FTBarcodeSendSuplNo,'') AS 'FTBarcodeSendSuplNo', \n" +
//        "ISNULL(b.FNBunbleSeq,'') AS 'BunbleSeq', " +
//        "ISNULL(b.FTCmpCode,'') AS 'BCCompany', ISNULL(b.FTOrderNo,'') AS 'OrderNo', \n" +
//        "ISNULL(b.FTSendSuplNo,'') AS 'SendSuplNo', \n" +
//        "ISNULL(b.FTColorway,'') AS 'Colorway',ISNULL(b.FTSizeBreakDown,'') AS 'SizeBreakDown', \n" +
//        "ISNULL(b.FNQuantity,0) AS 'Qty', ISNULL(b.FTStateBranchAccept,'') AS 'BCAccept', \n" +
//        "ISNULL(b.FNBalQuantity,0) AS 'BalanceQty', \n" +
//        "ISNULL(b.FNHSysOperationId,'') AS 'OperationId' ,ISNULL(od.FTOrderNo,'') AS 'OrderNo', \n" +
//        "ISNULL(od.FDOrderDate, '') AS 'OrderDate', ISNULL(od.FTPORef,'') AS 'PONumber', \n " +
//        "ISNULL(od.FTRemark, '') AS 'FTRemark', \n" +
//        "ISNULL(cmp.FTCmpCode,'') AS 'Company', ISNULL(c.FTCustNameEN,'') AS 'CustName',  \n" +
//        "ISNULL(os.FDShipDate,'') AS 'FDShipDate', ISNULL(os.FDShipDateOrginal,'') AS 'FDShipDateOrginal', \n" +
//        "ISNULL(s.FTStyleCode,'') AS 'StyleCode', ISNULL(s.FTStyleNameEN,'') AS 'FTStyleNameEN', ISNULL(s.FTStyleNameTH,'') AS 'FTStyleNameTH' \n\n";

//    _Qry += "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrder AS od WITH (NOLOCK) \n\n";

//    _Qry += "OUTER APPLY (SELECT os.FDShipDate, os.FDShipDateOrginal, os.FTStateEmb, os.FTStatePrint, os.FTStateHeat, os.FTStateLaser \n";
//    _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrderSub AS os WITH (NOLOCK) " +
//        "WHERE os.FTOrderNo = od.FTOrderNo) AS os \n\n";


//    _Qry += "OUTER APPLY (SELECT c.FTCustNameEN, c.FTCustNameTH \n";
//    _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCustomer AS c WITH (NOLOCK) " +
//        "WHERE c.FNHSysCustId = od.FNHSysCustId) AS c \n\n";

//    _Qry += "OUTER APPLY(SELECT cmp.FTCmpCode, cmp.FTCmpNameEN, cmp.FTCmpNameTH \n";
//    _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCmp AS cmp WITH (NOLOCK) " +
//        "WHERE cmp.FNHSysCmpId = od.FNHSysCmpId) AS cmp \n\n";

//    _Qry += "OUTER APPLY (SELECT s.FTStyleCode, s.FTStyleNameEN,s.FTStyleNameTH  " +
//        "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TMERMStyle AS s WITH(NOLOCK) \n" +
//        "WHERE s.FNHSysStyleId = od.FNHSysStyleId) AS s \n\n";

//    _Qry += "OUTER APPLY (" +
//        " SELECT b.FTOrderNo, b.FTSendSuplNo, b.FTBarcodeSendSuplNo, b.FNSeq, \n" +
//        " b.FTOrderProdNo,b.FNHSysCmpId, b.FTColorway, b.FTSizeBreakDown, b.FNQuantity, \n" +
//        " b.FNHSysOperationId,b.FNHSysOperationIdTo,b.FTStateBranchAccept, b.FNBalQuantity, \n" +
//        " b.FNBunbleSeq, cmpb.FTCmpCode \n" +
//        " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTSendSuplToBranch_Barcode AS b WITH(NOLOCK ) \n" +
//        "  OUTER APPLY(SELECT cmp.FTCmpCode, cmp.FTCmpNameEN, cmp.FTCmpNameTH \n";
//    _Qry += "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCmp AS cmpb WITH (NOLOCK ) \n" +
//        "WHERE cmpb.FNHSysCmpId = b.FNHSysCmpId) AS cmpb \n" +
//        "WHERE b.FTOrderNo = od.FTOrderNo \n" +
//        ") AS b \n\n";

//    //_Qry += " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTSendSuplToBranch_Barcode AS b WITH (NOLOCK ) WHERE b.FTOrderNo = od.FTOrderNo) AS b \n\n";

//    _Qry += "OUTER APPLY (SELECT * \n";
//    _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TPRODMOperation AS m WITH (NOLOCK) WHERE m.FNHSysOperationId = b.FTOrderNo) AS m \n\n";

//    _Qry += "WHERE od.FTOrderNo <> '' \n";

//    if (value.OrderNo != "")
//    {
//        _Qry += " AND od.FTOrderNo = '" + value.OrderNo + "' \n\n";
//    }

//    if (value.OrderStartDate != "" && value.OrderEndDate != "")
//    {
//        _Qry += " AND od.FDOrderDate BETWEEN '" + value.OrderStartDate + "' AND '" + value.OrderEndDate + "' \n";
//    }

//    if (value.ShipmentStartDate != "" && value.ShipmentEndDate != "")
//    {
//        _Qry += " AND os.FDShipDate BETWEEN '" + value.ShipmentStartDate + "' AND '" + value.ShipmentEndDate + "' \n";
//    }

//    if (value.Company != "")
//    {
//        _Qry += " AND cmp.FTCmpCode = '" + value.Company + "' \n\n";
//    }


//    //AND os.FNSubOrderState is not null
//    _Qry += "ORDER BY od.FDOrderDate, od.FTOrderNo";

//    try
//    {
//        WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
//        dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_MERCHAN);

//        DataTable _dtProdOrder = dt.DefaultView.ToTable(true, "OrderNo", "CustName", "PONumber", "FTRemark");
//        List<Order> _listJsons = new List<Order>();
//        //DataTable _dt = new DataTable();
//        //_dt.Columns.Add("OrderNo", dt.Columns["OrderNo"].DataType);

//        //var rowGroups = dt.Rows.OfType<DataRow>().OrderBy(r => r["id"]).GroupBy(r => r["Name"]);
//        //foreach (DataRow _r in _dt.Select("FNCostType = '6'"))
//        //DataTable distinctDT = SelectDistinct(dt.Tables[0], "OrderNo");
//        if (_dtProdOrder.Rows.Count > 0)
//        {
//            foreach (DataRow _dr in _dtProdOrder.Rows)
//            {
//                Order _o = new Order();
//                _o.Job = _dr["OrderNo"].ToString();
//                _o.CustomerName = _dr["CustName"].ToString();
//                _o.CustomerPo = _dr["PONumber"].ToString();
//                _o.Note = _dr["FTRemark"].ToString();
//                List<OrderDetails> _pdt = new List<OrderDetails>();
//                foreach (DataRow _r in dt.Select("OrderNo = '" + _dr["OrderNo"].ToString() + "'"))
//                {
//                    OrderDetails _od = new OrderDetails();
//                    _od.StyleNo = _r["StyleCode"].ToString();
//                    _od.JobProductionNo = _r["FTBarcodeSendSuplNo"].ToString();
//                    _od.SubjobNo = _r["FTBarcodeSendSuplNo"].ToString();
//                    _od.GacDate = _r["FDShipDate"].ToString();
//                    //_od.PoLineItem = _r["OrderNo"].ToString();
//                    _od.Colorway = _r["Colorway"].ToString();
//                    _od.Size = _r["SizeBreakDown"].ToString();
//                    _od.SubJobQuantity = _r["Qty"].ToString();
//                    //_od.AssortQuantity = 
//                    //_od.BoxQuantity = _r["OrderNo"].ToString();
//                    //_od.PackType = _r["OrderNo"].ToString();
//                    //_od.PackRatio = _r["OrderNo"].ToString();
//                    _pdt.Add(_od);
//                }
//                _o.OrderDetails = _pdt;
//                _listJsons.Add(_o);
//            }
//        }

//        jsondata = JsonConvert.SerializeObject(_listJsons);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex);
//    }
//}
//else
//{
//    return new HttpResponseMessage
//    {
//        StatusCode = HttpStatusCode.NotAcceptable,
//        Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + _state +
//        (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + _msg + (char)34 + "}",
//        System.Text.Encoding.UTF8, "application/json")
//    };
//}