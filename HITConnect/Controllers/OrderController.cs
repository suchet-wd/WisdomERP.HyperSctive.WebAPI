using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace HyperConvert.Controllers
{
    public class ProductionController : ApiController
    {
        [HttpPost]
        [Route("api/GetOrderInfo/")]
        public HttpResponseMessage GetOrderInfo([FromBody] APIOrder value)
        {
            string _Qry = "";
            string JSONresult = "";
            XmlDocument docXML = new XmlDocument();
            _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.SP_Send_Data_To_Hyperconvert_API1 '" + value.OrderNo + "'";
            try
            {
                WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
                docXML = Cnn.GetDataXML(_Qry, WSM.Conn.DB.DataBaseName.HITECH_MERCHAN);
                JSONresult = JsonConvert.SerializeObject(docXML);
                //JsonConvert.SerializeXmlNode(docXML); //, Newtonsoft.Json.Formatting.Indented


                JSONresult = JSONresult.Replace("\"[]\"", "[]");
                JSONresult = JSONresult.Replace("[[],", "[");
                JSONresult = JSONresult.Replace(":\"_", ":\"");
                //JSONresult = JSONresult.Replace("}}", "}");
                JSONresult = JSONresult.Replace("{\"root\":", "");
                JSONresult = JSONresult.Replace("\"_\",", "");
                JSONresult = JSONresult.Replace("AssortQuantity\":{", "AssortQuantity\":[{");
                JSONresult = JSONresult.Replace("},\"ProductOperation", "}],\"ProductOperation");
                JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

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
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            //bool _state = true;
            //string _msg = "";
            //DataTable dt = new DataTable();
            //DataTable dtr = new DataTable();

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
            //        "ISNULL(os.FDShipDate,'') AS 'FDShipDate', ISNULL(os.FDShipDateOrginal,'') AS 'FDShipDateOrginal', \n " +
            //        "ISNULL(os.FNPackPerCarton,'') AS 'FNPackPerCarton', ISNULL(os.FNPackCartonSubType,'') AS 'FNPackCartonSubType', \n" +
            //        "ISNULL(os.FTNameTH,'') AS 'PackTypeTH', ISNULL(os.FTNameEN,'') AS 'PackTypeEN', \n" +
            //        "ISNULL(s.FTStyleCode,'') AS 'StyleCode', ISNULL(s.FTStyleNameEN,'') AS 'FTStyleNameEN', ISNULL(s.FTStyleNameTH,'') AS 'FTStyleNameTH' \n";
            //        //",ISNULL(sb.FTSubOrderNo,'') AS 'SubOrderNo', ISNULL(sb.FTColorway,'') AS 'SubColorway', ISNULL(sb.FTNikePOLineItem,'') AS 'POLineItem', \n" +
            //        //"ISNULL(sb.FTSizeBreakDown,'') AS 'SubSizeBreakDown', ISNULL(sb.FNQuantity,0) AS 'SubQty', ISNULL(sb.FNGrandQuantity,0) AS 'SubGrandQty' \n\n";

            //    _Qry += "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrder AS od WITH (NOLOCK) \n\n";

            //    _Qry += "OUTER APPLY (SELECT os.FDShipDate, os.FDShipDateOrginal, os.FNPackPerCarton, os.FNPackCartonSubType, \n" +
            //        " os.FTStateEmb, os.FTStatePrint, os.FTStateHeat, os.FTStateLaser, l.FTNameTH, l.FTNameEN \n";
            //    _Qry += " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrderSub AS os WITH (NOLOCK) " +
            //        "OUTER APPLY (SELECT l.FTNameTH, l.FTNameEN FROM [" + WSM.Conn.DB.DataBaseName.HITECH_SYSTEM + "].dbo.HSysListData AS l WITH (NOLOCK) \n" +
            //        "WHERE l.FTListName = 'FNPackCartonSubType' AND l.FNListIndex = os.FNPackCartonSubType) as l  \n" +
            //        "WHERE os.FTOrderNo = od.FTOrderNo) AS os \n\n";

            //    //_Qry += "OUTER APPLY(SELECT sb.FTSubOrderNo, sb.FTColorway, sb.FTNikePOLineItem, sb.FTSizeBreakDown, sb.FNQuantity, \n" +
            //    //    "sb.FNGrandQuantity FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrderSub_BreakDown AS sb WITH (NOLOCK) \n" +
            //    //    "WHERE sb.FTOrderNo = od.FTOrderNo) as sb \n\n";

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

            //        if (_dtProdOrder.Rows.Count > 0)
            //        {
            //            foreach (DataRow _dr in _dtProdOrder.Rows)
            //            {
            //                Order _o = new Order();
            //                _o.Job = _dr["OrderNo"].ToString();
            //                _o.CustomerName = _dr["CustName"].ToString();
            //                _o.CustomerPo = _dr["PONumber"].ToString();
            //                _o.Note = _dr["FTRemark"].ToString();
            //                List<OrderDetails> _odt = new List<OrderDetails>();
            //                List<OrderProdDetailsRatio> _odr = new List<OrderProdDetailsRatio>();

            //                foreach (DataRow _r in dt.Select("OrderNo = '" + _dr["OrderNo"].ToString() + "'"))
            //                {
            //                    OrderDetails _od = new OrderDetails();
            //                    _od.StyleNo = _r["StyleCode"].ToString();
            //                    _od.JobProductionNo = _r["FTBarcodeSendSuplNo"].ToString();
            //                    _od.SubjobNo = _r["FTBarcodeSendSuplNo"].ToString();
            //                    _od.GacDate = _r["FDShipDate"].ToString();
            //                    //_od.PoLineItem = _r["POLineItem"].ToString();
            //                    _od.Colorway = _r["Colorway"].ToString();
            //                    _od.Size = _r["SizeBreakDown"].ToString();
            //                    //_od.SubJobQuantity = int.Parse(_r["Qty"].ToString());
            //                    //_od.AssortQuantity = 
            //                    //_od.BoxQuantity = int.Parse(_r["PackPerCarton"].ToString());
            //                    _od.PackType = _r["PackTypeEN"].ToString();

            //                    _Qry = "select sb.FTOrderNo, sb.FTColorway, sb.FTSizeBreakDown, sb.FNQuantity,sb.FNGrandQuantity " +
            //                        " FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrderSub_BreakDown AS sb WITH (NOLOCK) " +
            //                        " where FTOrderNo = '" + value.OrderNo + "' ";
            //                    dtr = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_MERCHAN);

            //                    foreach (DataRow _rr in dtr.Rows)
            //                    {
            //                        OrderProdDetailsRatio _ratio = new OrderProdDetailsRatio();
            //                        _ratio.Colorway = _rr["FTColorway"].ToString();
            //                        _ratio.Size = _rr["FTSizeBreakDown"].ToString();
            //                        _ratio.Quantity = int.Parse(_rr["FNQuantity"].ToString());
            //                        _odr.Add(_ratio);
            //                    }

            //                    _od.PackRatio = _odr;


            //                    _odt.Add(_od);
            //                }
            //                _o.OrderDetails = _odt;
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
        }

        [HttpPost]
        [Route("api/GetOrderProdInfo/")]
        public HttpResponseMessage GetOrderProdInfo([FromBody] APIOrderProd value)
        {
            string _Qry = "";
            string jsondata = "";
            List<OrderProd> _listJsons = new List<OrderProd>();
            bool _state = true;
            string _msg = "";
            DataTable dt = new DataTable();

            if (APIOrderProd.isOrderProdNo(value.OrderNo))
            {
                try
                {
                    WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

                    _Qry = "SELECT DISTINCT o.FTOrderNo AS 'OrderNo', o.FTOrderProdNo AS 'OrderProdNo', cmp.FTCmpCode AS 'Company', \n" +
                        "o.FDInsDate AS 'OrderDate', d.FTColorway AS 'Colorway', \n" +
                        "d.FTSizeBreakDown AS 'SizeBreakDown',d.FNQuantity AS 'Qty', \n" +
                        "m.FNHSysMarkId, m.FTNote AS 'Note', ods.FDShipDate AS 'GacDate', ods.FTPORef AS 'PONo', \n" +
                        "sea.FTSeasonCode AS 'SeasonCode', sty.FTStyleCode AS 'StyleCode', \n" +
                        "cust.FTCustCode AS 'CustCode', cust.FTCustNameEN AS 'CustName', cust.FTCustNameTH \n" +
                        ",b.FTBarcodeBundleNo AS 'BarcodeBundleNo', b.FTBarcodeSendSuplNo AS 'BarcodeSendSuplNo' \n";

                    _Qry += " \n FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd  AS o WITH ( NOLOCK ) \n\n";

                    _Qry += "OUTER APPLY (SELECT * " +
                        "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd_MarkMain AS m WITH (NOLOCK ) " +
                        "WHERE o.FTOrderProdNo = m.FTOrderProdNo ) AS m   \n\n";


                    _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd_Detail AS d WITH (NOLOCK ) " +
                        "WHERE o.FTOrderProdNo = d.FTOrderProdNo) AS d \n\n";

                    _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODBarcode_SendSupl AS b WITH (NOLOCK ) " +
                        "WHERE b.FTOrderProdNo = o.FTOrderProdNo) AS b \n\n";

                    _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrder AS od WITH (NOLOCK ) " +
                        "WHERE od.FTOrderNo = o.FTOrderNo) AS od \n\n";

                    _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrderSub AS ods WITH (NOLOCK ) " +
                        "WHERE ods.FTOrderNo = o.FTOrderNo) AS ods \n\n";

                    _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd_SendSupl AS ss WITH (NOLOCK ) " +
                        "WHERE o.FTOrderProdNo = ss.FTOrderProdNo) AS ss \n\n";

                    _Qry += "OUTER APPLY (SELECT cmp.FTCmpCode FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCmp AS cmp WITH (NOLOCK ) " +
                        "WHERE cmp.FNHSysCmpId = m.FNHSysCmpId) AS cmp \n\n";

                    _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TMERMSeason AS sea WITH (NOLOCK ) " +
                        "WHERE sea.FNHSysSeasonId = od.FNHSysSeasonId) AS sea \n\n";

                    _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TMERMStyle AS sty WITH (NOLOCK ) " +
                        "WHERE sty.FNHSysStyleId = od.FNHSysStyleId) AS sty \n\n";

                    _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCustomer AS cust WITH (NOLOCK ) " +
                        "WHERE cust.FNHSysCustId = od.FNHSysCustId) AS cust \n\n";



                    _Qry += "WHERE o.FTOrderNo <> ''\n";

                    if (value != null)
                    {
                        _Qry += "AND  o.FTOrderNo= '" + value.OrderNo + "' \n";
                    }


                    dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION);

                    DataTable _dtProdOrder = dt.DefaultView.ToTable(true, "OrderNo", "OrderProdNo", "PONo", "SeasonCode");

                    foreach (DataRow _r in _dtProdOrder.Rows)
                    {
                        OrderProd o = new OrderProd();
                        o.OrderProdNo = _r["OrderNo"].ToString();
                        o.CustomerPO = _r["PONo"].ToString();
                        o.StyleNo = _r["SeasonCode"].ToString();
                        o.Job = _r["OrderNo"].ToString();
                        o.SubJobNo = _r["OrderProdNo"].ToString();

                        List<OrderProdDetails> _lod = new List<OrderProdDetails>();
                        foreach (DataRow _rr in dt.Rows)
                        {
                            OrderProdDetails od = new OrderProdDetails();
                            od.ParentBundleBarcode = _rr["OrderProdNo"].ToString();
                            od.BundleBarcode = _rr["BarcodeBundleNo"].ToString();
                            od.BundleNo = _rr["BarcodeSendSuplNo"].ToString();
                            od.Colorway = _rr["Colorway"].ToString();
                            od.PoLineItem = _rr["OrderProdNo"].ToString();
                            od.Size = _rr["SizeBreakDown"].ToString();
                            od.BundleQuantity = int.Parse(_rr["Qty"].ToString());
                            od.Marker = _rr["OrderProdNo"].ToString();
                            od.Route = _rr["OrderProdNo"].ToString();
                            od.Supplier = _rr["OrderProdNo"].ToString();
                            _lod.Add(od);
                        }
                        o.OrderProdDetails = _lod;
                        _listJsons.Add(o);
                    }
                    jsondata = JsonConvert.SerializeObject(_listJsons);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _state = false;
                    _msg = "Connection Error!!!";
                }
            }
            else
            {
                _state = false;
                _msg = "This Production Number not found!!!";
            }

            return (_state) ? new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
            } : new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotAcceptable,
                Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + _state +
                    (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + _msg + (char)34 + "}",
                    System.Text.Encoding.UTF8, "application/json")
            }; ;
        }
    }
}