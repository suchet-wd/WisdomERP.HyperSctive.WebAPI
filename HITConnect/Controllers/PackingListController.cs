using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HITConnect.Controllers
{
    public class PackingListController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "String1", "String2" };
        }

        [HttpPost]
        [Route("api/RcPackingList/")]
        public HttpResponseMessage RcPackingList()
        {
            string _Qry = "";
            int statecheck = 0;
            string msgError = "";
            string token = "";
            string jsondata = "";
            DataTable dt = null;
            DataTable dts = new DataTable();
            dts.Columns.Add("Status", typeof(string));
            dts.Columns.Add("Token", typeof(string));
            dts.Columns.Add("Message", typeof(string));
            WSM.Conn.SQLConn Cnn = new WSM.Conn.SQLConn();

            dts.Rows.Add(new Object[] { statecheck, token, msgError });
            jsondata = JsonConvert.SerializeObject(dts);
            //string jsondata = (valid) ? JsonConvert.SerializeObject(dts) : "NOT FOUND";
            //return new HttpResponseMessage { Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
            if (statecheck == 1)
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = new StringContent(jsondata, System.Text.Encoding.UTF8, "application/json") };
                //return new HttpResponseMessage { StatusCode = HttpStatusCode.Accepted, Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + "1" + (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + "" + (char)34 + "}", System.Text.Encoding.UTF8, "application/json") };
            }
            else
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.NotAcceptable, Content = new StringContent("{" + (char)34 + "Status" + (char)34 + ": " + (char)34 + "0" + (char)34 + "," + (char)34 + "Refer" + (char)34 + ": " + (char)34 + msgError + (char)34 + "}", System.Text.Encoding.UTF8, "application/json") };
            }
        }

        /*
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }



        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
            
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        */
    }
}
