using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART
{
    public class PageBase : System.Web.UI.Page
    {
        GObj G = new GObj();

        public GObj basePageLoad()
        {
            G = (GObj)Session["G"];

            Session.Timeout = 360;

            // (2021.09.02) 타 Site 로그인 & 같은 Site Mobile 로그인  -->  로그아웃 되는 현상 때문에 막음.
            /*
            if (!Page.User.Identity.IsAuthenticated)
            {
                if (Page.IsCallback)
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/glb/Forms/Logout.aspx");
                }
                else
                {
                    try
                    {
                        Response.Redirect("~/glb/Forms/Logout.aspx");
                    }
                    catch
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/glb/Forms/Logout.aspx");
                    }
                }
                
                return G;
            }
            */

            if (G == null)
            {
                if (Page.IsCallback)
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/glb/Forms/Logout.aspx");
                }
                else
                {
                    try
                    {
                        Response.Redirect("~/glb/Forms/Logout.aspx");
                    }
                    catch
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/glb/Forms/Logout.aspx");
                    }
                }
                return G;
            }
            if (string.IsNullOrEmpty(G.UserID))
            {
                if (Page.IsCallback)
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/glb/Forms/Logout.aspx");
                }
                else
                {
                    try
                    {
                        Response.Redirect("~/glb/Forms/Logout.aspx");
                    }
                    catch
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/glb/Forms/Logout.aspx");
                    }
                }
                return G;
            }

            return G;
        }


    }
}
