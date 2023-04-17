using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HITConnect
{
    public class Payment
    {
        public string id { get; set; }
        public string pwd { get; set; }
        public string token { get; set; }
        public string status { get; set; }
        public string venderCode { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string PI { get; set; }
        public string PO { get; set; }
    }
}
