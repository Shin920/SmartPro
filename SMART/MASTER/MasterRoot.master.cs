using SMART;
using SMART.COMMON.DS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MASTER_MasterRoot : System.Web.UI.MasterPage
{
    GObj G = new GObj();

    // 상단 Logo
    public string m_strComLogo = "";

    
    protected void Page_Load(object sender, EventArgs e)
    {
        PageBase bs = new PageBase();
        bs.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {
            lblUserName.Text = G.UserName;
            lblSiteName.Text = G.SiteName;
        }

        // 고객사 로고
        if (G.SiteNo == "DaeAn")  // 대안일레콤
        {
            m_strComLogo = "../Contents/Main/images/Logo_DaeAn.jpg";
        }
        else if (G.SiteNo == "Skin")  // 스킨러버스
        {
            m_strComLogo = "../Contents/Main/images/logo_SkinLovers.png";
        }
        else if (G.SiteNo == "EvoSonic")  // 에보소닉
        {
            m_strComLogo = "../Contents/Main/images/logo_Evosonic.png";
        }
        else
        {
            m_strComLogo = "../Contents/Main/images/Logo_GinCni.png";
        }

    }

    protected void callBackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        string job = hidField["job"].ToString();

        switch (job)
        {
            case "F":
                JobFindForm();
                break;
            case "Logout":
                JobLogout();
                break;
        }

    }

    void JobFindForm()
    {
        SMART.COMMON.DS.ModuleObj Obj = new SMART.COMMON.DS.ModuleObj();
        DataSet ds = Obj.FindFormData(G.D, txtFindForm.Text.Trim());

        hidField.Clear();
        hidField["job"] = "F";

        if (ds.Tables[0].Rows.Count <= 0)
        {
            hidField["status"] = "N";
            hidField["msg"] = "입력하신 화면이 존재하지 않습니다. \n올바른 화면 ID나 화면명을 입력해 주십시오.";
            return;
        }

        hidField["status"] = "Y";
        hidField["cnt"] = ds.Tables[0].Rows.Count.ToString();
        CFL.setDataSetToDevHiddenField(ds, hidField);
    }

    void JobLogout()
    {
        SMART.COMMON.DA.LogMaster Log = new SMART.COMMON.DA.LogMaster();
        string strIPAddress = Request.UserHostAddress.ToString();
        ArrayList alData = Log.SaveLoginOut(G.D);
    }


}
