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
    public class ProductionController : ApiController
    {
        [HttpPost]
        [Route("api/GetOrderInfo/")]
        public HttpResponseMessage GetOrderInfo([FromBody] APIOrder value)
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
                _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API1 '" + value.OrderNo + "'";
                try
                {
                    if (UserController.CheckUserPermission(Cnn, value.authen) == true)
                    {

                    }
                    docXML = Cnn.GetDataXML(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
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
        }

        [HttpPost]
        [Route("api/GetOrderDetailInfo/")]
        public HttpResponseMessage GetOrderDetailInfo([FromBody] APIOrder value)
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
                _Qry = "EXEC [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.SP_Send_Data_To_Hyperconvert_API2 '" + value.OrderNo + "'";
                try
                {
                    docXML = Cnn.GetDataXML(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
                    JSONresult = JsonConvert.SerializeObject(docXML);
                    JSONresult = JSONresult.Replace("PartDetail\":[\"[]\",{", "PartDetail\":[{");
                    JSONresult = JSONresult.Replace("PooDetail\":[\"[]\",", "PooDetail\":[");
                    JSONresult = JSONresult.Replace("\"SpreadingRatio\":[\"[]\",", "\"SpreadingRatio\":[");
                    JSONresult = JSONresult.Replace("\"\",", "");
                    JSONresult = JSONresult.Replace("{\"root\":", "");
                    JSONresult = JSONresult.Replace("\"Route\":[\"[]\",{\"Station", "\"Route\":[{\"Station");
                    JSONresult = JSONresult.Substring(0, JSONresult.Length - 1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
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
        }

        //[HttpPost]
        //[Route("api/GetOrderProdInfo/")]
        //public HttpResponseMessage GetOrderProdInfo([FromBody] APIOrderProd value)
        //{
        //    string _Qry = "";
        //    string jsondata = "";
        //    List<OrderProd> _listJsons = new List<OrderProd>();
        //    bool _state = true;
        //    string _msg = "";
        //    DataTable dt = new DataTable();

        //    if (APIOrderProd.isOrderProdNo(value.OrderNo))
        //    {
        //        try
        //        {
        //            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

        //            _Qry = "SELECT DISTINCT o.FTOrderNo AS 'OrderNo', o.FTOrderProdNo AS 'OrderProdNo', cmp.FTCmpCode AS 'Company', \n" +
        //                "o.FDInsDate AS 'OrderDate', d.FTColorway AS 'Colorway', \n" +
        //                "d.FTSizeBreakDown AS 'SizeBreakDown',d.FNQuantity AS 'Qty', \n" +
        //                "m.FNHSysMarkId, m.FTNote AS 'Note', ods.FDShipDate AS 'GacDate', ods.FTPORef AS 'PONo', \n" +
        //                "sea.FTSeasonCode AS 'SeasonCode', sty.FTStyleCode AS 'StyleCode', \n" +
        //                "cust.FTCustCode AS 'CustCode', cust.FTCustNameEN AS 'CustName', cust.FTCustNameTH \n" +
        //                ",b.FTBarcodeBundleNo AS 'BarcodeBundleNo', b.FTBarcodeSendSuplNo AS 'BarcodeSendSuplNo' \n";

        //            _Qry += " \n FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd  AS o WITH ( NOLOCK ) \n\n";

        //            _Qry += "OUTER APPLY (SELECT * " +
        //                "FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd_MarkMain AS m WITH (NOLOCK ) " +
        //                "WHERE o.FTOrderProdNo = m.FTOrderProdNo ) AS m   \n\n";


        //            _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd_Detail AS d WITH (NOLOCK ) " +
        //                "WHERE o.FTOrderProdNo = d.FTOrderProdNo) AS d \n\n";

        //            _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODBarcode_SendSupl AS b WITH (NOLOCK ) " +
        //                "WHERE b.FTOrderProdNo = o.FTOrderProdNo) AS b \n\n";

        //            _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrder AS od WITH (NOLOCK ) " +
        //                "WHERE od.FTOrderNo = o.FTOrderNo) AS od \n\n";

        //            _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MERCHAN + "].dbo.TMERTOrderSub AS ods WITH (NOLOCK ) " +
        //                "WHERE ods.FTOrderNo = o.FTOrderNo) AS ods \n\n";

        //            _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION + "].dbo.TPRODTOrderProd_SendSupl AS ss WITH (NOLOCK ) " +
        //                "WHERE o.FTOrderProdNo = ss.FTOrderProdNo) AS ss \n\n";

        //            _Qry += "OUTER APPLY (SELECT cmp.FTCmpCode FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCmp AS cmp WITH (NOLOCK ) " +
        //                "WHERE cmp.FNHSysCmpId = m.FNHSysCmpId) AS cmp \n\n";

        //            _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TMERMSeason AS sea WITH (NOLOCK ) " +
        //                "WHERE sea.FNHSysSeasonId = od.FNHSysSeasonId) AS sea \n\n";

        //            _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TMERMStyle AS sty WITH (NOLOCK ) " +
        //                "WHERE sty.FNHSysStyleId = od.FNHSysStyleId) AS sty \n\n";

        //            _Qry += "OUTER APPLY(SELECT* FROM [" + WSM.Conn.DB.DataBaseName.HITECH_MASTER + "].dbo.TCNMCustomer AS cust WITH (NOLOCK ) " +
        //                "WHERE cust.FNHSysCustId = od.FNHSysCustId) AS cust \n\n";



        //            _Qry += "WHERE o.FTOrderNo <> ''\n";

        //            if (value != null)
        //            {
        //                _Qry += "AND  o.FTOrderNo= '" + value.OrderNo + "' \n";
        //            }


        //            dt = Cnn.GetDataTable(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION);

        //            DataTable _dtProdOrder = dt.DefaultView.ToTable(true, "OrderNo", "OrderProdNo", "PONo", "SeasonCode");

        //            foreach (DataRow _r in _dtProdOrder.Rows)
        //            {
        //                OrderProd o = new OrderProd();
        //                o.OrderProdNo = _r["OrderNo"].ToString();
        //                o.CustomerPO = _r["PONo"].ToString();
        //                o.StyleNo = _r["SeasonCode"].ToString();
        //                o.Job = _r["OrderNo"].ToString();
        //                o.SubJobNo = _r["OrderProdNo"].ToString();

        //                List<OrderProdDetails> _lod = new List<OrderProdDetails>();
        //                foreach (DataRow _rr in dt.Rows)
        //                {
        //                    OrderProdDetails od = new OrderProdDetails();
        //                    od.ParentBundleBarcode = _rr["OrderProdNo"].ToString();
        //                    od.BundleBarcode = _rr["BarcodeBundleNo"].ToString();
        //                    od.BundleNo = _rr["BarcodeSendSuplNo"].ToString();
        //                    od.Colorway = _rr["Colorway"].ToString();
        //                    od.PoLineItem = _rr["OrderProdNo"].ToString();
        //                    od.Size = _rr["SizeBreakDown"].ToString();
        //                    od.BundleQuantity = int.Parse(_rr["Qty"].ToString());
        //                    od.Marker = _rr["OrderProdNo"].ToString();
        //                    od.Route = _rr["OrderProdNo"].ToString();
        //                    od.Supplier = _rr["OrderProdNo"].ToString();
        //                    _lod.Add(od);
        //                }
        //                o.OrderProdDetails = _lod;
        //                _listJsons.Add(o);
        //            }
        //            jsondata = JsonConvert.SerializeObject(_listJsons);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            _state = false;
        //            _msg = "Connection Error!!!";
        //        }
        //    }
        //    else
        //    {
        //        _state = false;
        //        _msg = "This Production Number not found!!!";
        //    }

        //    return (_state) ? new HttpResponseMessage
        //    {
        //        StatusCode = HttpStatusCode.Accepted,
        //        Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json")
        //    } : new HttpResponseMessage
        //    {
        //        StatusCode = HttpStatusCode.NotAcceptable,
        //        Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + _state +
        //            (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + _msg + (char)34 + "}",
        //            System.Text.Encoding.UTF8, "application/json")
        //    }; ;
        //}
    }
}