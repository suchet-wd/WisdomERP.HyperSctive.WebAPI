﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HyperActive.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("api/GetAuthorization/")]
        public HttpResponseMessage GetAuthorization([FromBody] UserToken value)
        {
            string JSONresult = "";
            DataTable dts = new DataTable();

            UserAuthen _ua = new UserAuthen();
            _ua.id = value.id;
            _ua.pwd = value.pwd;
            // Check id / pwd 
            List<string> _result = ValidateField(_ua);

            try
            {
                WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();
                if (_result.Count == 0) //&& DelAuthenKey(Cnn, value)
                {
                    if (CheckUserPermission(Cnn, _ua) == true)
                    {
                        //dts.Columns.Add("id", typeof(string));
                        dts.Columns.Add("Status", typeof(string));
                        dts.Columns.Add("Message", typeof(string));
                        dts.Columns.Add("Token", typeof(string));
                        dts.Rows.Add(new Object[] { 0, "Successful", SetToken(Cnn, _ua) });
                    }
                    else
                    {
                        dts.Columns.Add("Status", typeof(string));
                        dts.Columns.Add("Message", typeof(string));
                        dts.Rows.Add(new Object[] { 1, "Please check User authentication!!!" });
                        //SetToken(Cnn, value);
                        JSONresult = JsonConvert.SerializeObject(dts);
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                        };
                    }
                }
                else
                {
                    dts.Columns.Add("Status", typeof(string));
                    dts.Columns.Add("Message", typeof(string));
                    dts.Rows.Add(new Object[] { 1, _result[1].ToString() });
                    JSONresult = JsonConvert.SerializeObject(dts);
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotAcceptable,
                        Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dts.Columns.Add("Status", typeof(string));
                dts.Columns.Add("Message", typeof(string));
                dts.Rows.Add(new Object[] { 1, "Error !!!" });
                JSONresult = JsonConvert.SerializeObject(dts);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                    Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
                };
            }

            JSONresult = JsonConvert.SerializeObject(dts);

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(JSONresult, System.Text.Encoding.UTF8, "application/json")
            };
        }

        //public static List<string> ValidateField(UserToken value)
        public static List<string> ValidateField(UserAuthen value)
        {
            List<string> _result = new List<string>();
            if (value == null)
            {
                _result.Add("1");
                _result.Add("You don't have permission !!!");
            }
            else
            {
                if (value.id == "")
                {
                    _result.Add("1");
                    _result.Add("Please check ID !!!");
                }
                else
                {
                    if (value.pwd == "")
                    {
                        _result.Add("1");
                        _result.Add("Please check password !!!");
                    }
                    else
                    {
                        if (value.token == "")
                        {
                            _result.Add("1");
                            _result.Add("Please check Token !!!");
                        }
                    }
                }
            }
            return _result;
        }


        public static bool CheckUserPermission(WSM.Conn.SQLConn Cnn, UserAuthen value)
        {
            bool _state = false;
            if (value.id != "" && value.pwd != "")
            {
                string _Qry = "";
                if (value.token == null)
                {
                    _Qry = "SELECT TOP 1 hu.Pwd FROM [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.HyperUser AS hu WITH(NOLOCK) \n";
                    _Qry += "WHERE hu.HyperLogIn = '" + value.id + "' \n";
                }
                else
                {
                    _Qry = "SELECT TOP 1 hu.Pwd FROM [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.HyperUser AS hu WITH(NOLOCK) \n";
                    _Qry += "WHERE hu.HyperLogIn = '" + value.id + "' AND hu.TokenKey = '" + value.token + "' \n";
                    // Token Expired after 5 min
                    _Qry += "AND dateadd(minute, 0, hu.FDLastUpdate) >= dateadd(minute, -5, getdate())";
                }
                
                try
                {
                    string s = WSM.Conn.DB.FuncDecryptDataServer(Cnn.GetField(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE, ""));
                    _state = (s == value.pwd) ? true : false;
                    if (_state == false)
                    {
                        _Qry = "UPDATE [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.HyperUser SET FNLoginFailed =  ";
                        _Qry += "(SELECT ISNULL(FNLoginFailed,0) + 1 ) ";
                        _Qry += "WHERE HyperLogIn = '" + value.id + "' ";
                        Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);

                    }
                    else
                    {
                        _Qry = "UPDATE [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.HyperUser SET FNLoginFailed = 0 ";
                        _Qry += "WHERE HyperLogIn = '" + value.id + "' ";
                        Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
                    }
                    DelAuthenKey(Cnn, value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return _state;
        }

        public static bool DelAuthenKey(WSM.Conn.SQLConn Cnn, UserAuthen value)
        {
            bool _result = false;
            try
            {
                string _Qry = "UPDATE [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.HyperUser SET TokenKey = '', FDLastUpdate = Getdate() ";
                _Qry += "WHERE HyperLogIn = '" + value.id + "' ";
                _result = Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return _result;
        }

        public string SetToken(WSM.Conn.SQLConn Cnn, UserAuthen value)
        {
            string _result = "";
            // Save ID & Token to DB then send JSON file
            string _Qry = "";
            _Qry = "UPDATE [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.HyperUser SET TokenKey = NEWID() ,FDLastUpdate = Getdate() ";
            _Qry += "WHERE HyperLogIn = '" + value.id + "' ";

            if (Cnn.ExecuteOnly(_Qry, WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE))
            {
                _Qry = "SELECT TOP 1 TokenKey FROM [" + WSM.Conn.DB.DataBaseName.HITECH_HYPERACTIVE + "].dbo.HyperUser WITH (NOLOCK) ";
                _Qry += "WHERE HyperLogIn = '" + value.id + "' ";

                try
                {
                    _result = new WSM.Conn.SQLConn().GetField(_Qry, WSM.Conn.DB.DataBaseName.HITECH_PRODUCTION, "");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return _result;
        }
    }
}
