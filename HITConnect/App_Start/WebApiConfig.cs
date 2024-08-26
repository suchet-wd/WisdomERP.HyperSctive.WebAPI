using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HITConnect
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            /*
            config.Routes.MapHttpRoute(
                name: "GetToken",
                routeTemplate: "api/GetToken/{id}",
                defaults: new { controller = "user", id = RouteParameter.Optional }
                constraints: new { id = "/d+" }
            );
            */

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.Clear();
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());


            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);


            config.Formatters.JsonFormatter.S‌​erializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.S‌​erializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.Indented
            };


            // Web API configuration and services  
            // config.Filters.Add(new RequireHttpsAttribute());


            WSM.Conn.DB.ServerName = "";
            WSM.Conn.DB.CmpID = System.Configuration.ConfigurationManager.AppSettings["CMPID"];
            WSM.Conn.DB.ServerName = System.Configuration.ConfigurationManager.AppSettings["SERVERNAME"];
            WSM.Conn.DB.UserName = System.Configuration.ConfigurationManager.AppSettings["username"];
            WSM.Conn.DB.UserPassword = WSM.Conn.DB.FuncDecryptDataServer(System.Configuration.ConfigurationManager.AppSettings["password"]);
            WSM.Conn.DB.APIURL = System.Configuration.ConfigurationManager.AppSettings["APIURL"];
            WSM.Conn.DB.APIIP = System.Configuration.ConfigurationManager.AppSettings["APIIP"];

            string dbName = null;
            int i = 0;

            foreach (string StrDBName in WSM.Conn.DB.SystemDBName)
            {

                dbName = "";


                try
                {

                    dbName = System.Configuration.ConfigurationManager.AppSettings[StrDBName];

                }
                catch { }


                WSM.Conn.DB.DBName[i] = dbName;

                i = i + 1;
            };

        }

    }
}
