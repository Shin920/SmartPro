using SMART;
using System;
using System.Collections;

public partial class glb_Main : PageBase
{

    // 이종현
    /*  
    GObj G;
    public string m_strScriptTarget = "";
    */

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();
    }


    // 이종현
    /*
    // 개별화면로그인 정보생성 21.12.30 LJH    
    protected void callBackForm_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        SMART.COMMON.DA.LogMaster Log = new SMART.COMMON.DA.LogMaster();
        string strIPAddress = Request.UserHostAddress.ToString();
        G = ((GObj)Session["G"]);
        ArrayList alData = Log.LogFormConnection(G.D, strIPAddress, txtFormID.Text);

        if (alData[0].ToString() != "00")
        {
            m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
            return;
        }
    }
    */

}