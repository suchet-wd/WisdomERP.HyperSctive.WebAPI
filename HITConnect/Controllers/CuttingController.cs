using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace HyperActive.Controllers
{
    public class CuttingController : ApiController
    {
        [HttpPost]
        [Route("api/GetCuttingInfo/")]
        public HttpResponseMessage GetCuttingInfo([FromBody] APICutting value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Message", typeof(string));

            List<string> _result = UserController.ValidateField(value.authen);
            if (_result.Count != 0)
            {
                dts.Rows.Add(new Object[] { 1, _result[1].ToString() });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            if (UserController.CheckUserPermission(Cnn, value.authen) == true)
            {
                if (CuttingOrder.isPooNo(value.PooNo))
                {
                    //// Start API 3:  Bundle Info
                    _Qry = "EXEC [HITECH_MERCHAN].dbo.SP_Send_Data_To_Hyperconvert_API3 " + value.PooNo;
                    docXML = WSM.Conn.DB.GetDataXML(_Qry);

                    JSONresult = JsonConvert.SerializeObject(docXML);
                    JSONresult = JSONresult.Replace("\"\",", "");
                    JSONresult = JSONresult.Replace("\"[]\"", "[]");
                    JSONresult = JSONresult.Replace("[[],", "[");
                    JSONresult = JSONresult.Replace("\"_", "\"");
                    JSONresult = JSONresult.Replace("}}", "}");
                    JSONresult = JSONresult.Replace("{\"root\":", "");

                    if (JSONresult.Length > 0)
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                        };
                    }
                    else
                    {
                        Console.WriteLine("Error API No #3 DocNo = " + value.PooNo + "!!!");
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                        };

                    }
                    //Console.WriteLine("End API#3");
                }
                else
                {
                    Console.WriteLine("Error API No #3 DocNo = " + value.PooNo + "!!!");
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                    };
                }
            }
            else
            {
                dts.Rows.Add(new Object[] { 1, "Please check User authentication!!!" });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }
        }
    }
}


//WriteLogFile("----------------------------------------------------");
//WriteLogFile("API3 ### " + R[1].ToString());
//WriteLogFile("----------------------------------------------------");
//WriteLogFile(JSONresult);
//WriteLogFile("----------------------------------------------------");
//// ----------- Start Send API Process -----------
//Console.WriteLine("Start Send API No #3 DocNo = " + R[1].ToString());
//responseAPI = PostDataToApi("2", R[1].ToString(), JSONresult);

//if (responseAPI != null)
//{
//    if (responseAPI.Code == "0")
//    {
//        SaveStateSendAPI("2", R[1].ToString(), "1", responseAPI);
//        Console.WriteLine("Send API No #3 DocNo = " + R[1].ToString() + " Successful.");
//    }
//    else
//    {
//        Console.WriteLine("!!! Error API No #3 DocNo = " + R[1].ToString() + " !!! @ " + responseAPI.Msg + " [Code:" + responseAPI.Code + "]");
//        SaveStateSendAPI("2", R[1].ToString(), "2", responseAPI);
//    }
//}
//_Qry = " SELECT bs.FTBarcodeBundleNo, bs.FNHSysPartId, bs.FNSendSuplType, " +
//    " bs.FNHSysSuplId, bs.FTBarcodeBundleNo, bs.FTOrderProdNo, bs.FTSendSuplRef " +
//    " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODBarcode_SendSupl AS bs WITH ( NOLOCK ) ";
//if (value.PooNo != "")
//{
//    _Qry += " WHERE bs.FTOrderProdNo = '" + value.PooNo + "'";
//}

//    try
//    {
//        WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
//        _dataTable = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION);
//        if (_dataTable.Rows.Count > 0)
//        {

//        }

//        _state = true;

//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex);
//    }
//}
//else
//{
//    _state = false;
//    _msg = "This job number is incorrect!!!";
//}
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