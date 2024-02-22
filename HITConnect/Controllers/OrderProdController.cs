using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HyperConvert.Controllers
{
    public class OrderProdController : ApiController
    {
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
                    _state = false;
                    _msg = "Connection Error!!!";
                }
            }
            else
            {
                _state = false;
                _msg = "This Production Number not found!!!";
            }

            return (_state) ? new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
            } : new HttpResponseMessage  {
                StatusCode = HttpStatusCode.NotAcceptable, Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + _state +
                    (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + _msg + (char)34 + "}",
                    System.Text.Encoding.UTF8, "application/json")
            }; ;
        }
    }
}