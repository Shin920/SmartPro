using SMART;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Master_Main : System.Web.UI.MasterPage
{
    CFLUserInfo userInfo = null;
    public bool bAdmin = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (userInfo == null)
        {
            PageBase bs = new PageBase();
            userInfo = bs.getUserInfo(Session, Request);
        }

        // logout 여부 
        if (userInfo.UserID == null)
        {
            Response.Write(CFL.GetMsg("사용자 정보가 초기화 되었습니다. \n재 로그인 하여 주십시오."));
            return;
        }
    }
}
