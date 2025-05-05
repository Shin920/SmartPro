
namespace SMART
{
    public class PageBase : System.Web.UI.Page
    {
        public CFLUserInfo baseUser = null;

        public void basePageInit()
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                if (Page.IsCallback)
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/Account/Logout.aspx");
                }
                else
                {
                    Response.Redirect("~/Account/Logout.aspx");
                }
                return;
            }

            //사용자 정보
            if (baseUser == null)
            {
                setUser(Request, Session);
            }
            if (baseUser.UserID == null)
            {
                if (Page.IsCallback)
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/Account/Logout.aspx");
                }
                else
                {
                    Response.Redirect("~/Account/Logout.aspx");
                }
                return;
            }
        }

        public void basePageLoad()
        {

            if (!Page.User.Identity.IsAuthenticated)
            {
                if (Page.IsCallback)
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/Account/Logout.aspx");
                }
                else
                {
                    Response.Redirect("~/Account/Logout.aspx");
                }
                return;
            }

            //사용자 정보
            if (baseUser == null)
            {
                setUser(Request, Session);
            }
            if (baseUser.UserID == null)
            {
                setUser(Request, Session);
            }
        }

        private void setUser(System.Web.HttpRequest req, System.Web.SessionState.HttpSessionState ses)
        {
            baseUser = this.getUserInfo(ses, req);
        }

        protected bool isPageInit
        {
            get { return (!this.IsPostBack && !this.IsCallback); }
        }

        public CFLUserInfo getUserInfo(System.Web.SessionState.HttpSessionState ses, System.Web.HttpRequest req)
        {
            CFLUserInfo ret = null;
            if (ses["MobileCore"] != null)
            {
                ret = (CFLUserInfo)ses["MobileCore"];
            }
            else if (req.Cookies["MobileCore"] != null)
            {
                ret = new CFLUserInfo();

                ret.UserID = CFL.GetStr(req.Cookies["MobileCore"]["UserID"]);
                ret.ComCode = CFL.GetStr(req.Cookies["MobileCore"]["ComCode"]);
                ret.DeptCode = CFL.GetStr(req.Cookies["MobileCore"]["DefaultDept"]);
                ret.DeptName = CFL.GetStr(req.Cookies["MobileCore"]["DeptName"]);
                ret.EmpCode = CFL.GetStr(req.Cookies["MobileCore"]["EmpCode"]);
                ret.EmpName = CFL.GetStr(req.Cookies["MobileCore"]["UserName"]);
                ret.SiteCode = CFL.GetStr(req.Cookies["MobileCore"]["SiteCode"]);
                ret.SiteName = CFL.GetStr(req.Cookies["MobileCore"]["SiteName"]);
                ret.LangCD = "kor";
                ret.ClientID = Server.HtmlEncode(Request.Cookies[CFL.c_CookieNameClientID].Value);
                ret.UserIP = Server.HtmlEncode(Request.UserHostAddress);
                // 세션에 담음
                ses["MobileCore"] = ret;

            }
            else if (req.ServerVariables["HTTP_HOST"].ToUpper().IndexOf("LOCALHOST") >= 0)
            {
                //
                // 개발 환경
                //
                ret = new CFLUserInfo();

                ret.ComCode = "COM1";
                ret.UserID = "SPAdmin";
                ret.DeptCode = "99999";
                ret.DeptName = "99999";
                ret.EmpCode = "99999";
                ret.EmpName = "Administrator";
                ret.SiteCode = "N001";
                ret.SiteName = "";
                ret.LangCD = "kor";
                ret.ClientID = Server.HtmlEncode(Request.Cookies[CFL.c_CookieNameClientID].Value);
                ret.UserIP = Server.HtmlEncode(Request.UserHostAddress);
                // 세션에 담음
                ses["MobileCore"] = ret;
            }
            else
            {
                ret = new CFLUserInfo();
            }


            return ret;
        }

    }
}
