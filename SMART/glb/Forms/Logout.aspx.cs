using SMART;
using System;
using System.Collections;
using System.Web.Security;

public partial class glb_Forms_Logout : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 로그아웃 히스토리 생성
        Connection_Setting();

        Session.Abandon();
        Response.Cookies.Clear();
    }

    protected void Connection_Setting()
    {
        GObj G = (GObj)Session["G"];

        if (G == null)
            return;

        SMART.COMMON.DA.LogMaster Log = new SMART.COMMON.DA.LogMaster();

        string strIPAddress = Request.UserHostAddress.ToString();

    //  ArrayList alData = Log.LogFormConnection(G.D, strIPAddress, "LogOut");
        ArrayList alData = Log.SaveLoginOut(G.D);
    }
}