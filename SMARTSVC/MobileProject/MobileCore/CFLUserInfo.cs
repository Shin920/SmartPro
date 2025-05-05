using System;

namespace SMART
{
    [Serializable()]
    public class CFLUserInfo
    {
        //로그인 정보 가져오기
        public string UserID { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string ComCode { get; set; }
        public string LangCD { get; set; }
        public string ClientID { get; set; }

        public string UserIP { get; set; }
    }
}
