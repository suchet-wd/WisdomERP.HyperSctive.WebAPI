using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WisdomHRApi
{

    public class RefreshUserInfoJSON
    {
        public string username;
        public string password;
        public string cmpcode;
    }

    public class JSonHeaderDocument
    {
        public string HSysCmpId { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string DocumentBy { get; set; }
        public string HSysWHId { get; set; }
        public string WHCode { get; set; }
        public string WHLocCode { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public string WHCodeTo { get; set; }
        public string WHLocCodeTo { get; set; }
        public string OrderNo { get; set; }
        public string IssueSect { get; set; }
        public string CmpId { get; set; }
        public string PurchaseNo { get; set; }
        public string TypeReturn { get; set; }
        public string CarNo { get; set; }
        public string EmpCode { get; set; }
        public string DocumentRefNo { get; set; }
        public string Seq { get; set; }

    }

    public class UserRegisterInfo
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpSurName { get; set; }
        public string EmpIdCard { get; set; }
        public string EmpPhone { get; set; }
        public string EmpBirthday { get; set; }

    }
}