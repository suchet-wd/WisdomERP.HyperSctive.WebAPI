using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HITConnect.Models
{
    public class User
    {
        public string id { get; set; }
        public string pwd { get; set; }
        public string token { get; set; }
        public string vencode { get; set; }
    }
}