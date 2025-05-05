using SMART;
using SMART.COMMON.DA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_MainContents_MainApproval : PageBase
{
    GObj G = new GObj();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {
            SettingImage();
        }
    }

    void SettingImage()
    {
        SMART.COMMON.DS.ModuleObj Obj = new SMART.COMMON.DS.ModuleObj();
        DataSet ds = Obj.FindFormData(G.D, "OM010");

        if (ds.Tables[0].Rows.Count <= 0)
        {
            imgApp01.ImageUrl = "/SmartPro/Contents/Main/images/approval_off.png";
            lblApp01.Text = "결재 정보가 없습니다.";
            imgApp01.Cursor = "default";
            return;
        }

        txtFormName.Text = CFL.GetStr(ds.Tables[0].Rows[0]["ObjName"]);
        txtFormID.Text = CFL.GetStr(ds.Tables[0].Rows[0]["FormID"]);
        txtURL.Text = CFL.GetStr(ds.Tables[0].Rows[0]["FormUrl"]);

        int iCount = 0;

        SMART.COMMON.DA.WorkFlow Wf = new SMART.COMMON.DA.WorkFlow();
        ArrayList alData = Wf.WfIBCount(G.D);
        if ("00" != alData[0].ToString())
        {
            return;
        }
        try
        {
            iCount = CFL.Toi(alData[4].ToString());
        }
        catch { }

        if (iCount == 0)
        {
            imgApp01.ImageUrl = "/SmartPro/Contents/Main/images/approval_off.png";
            lblApp01.Text = "결재 정보가 없습니다.";
            imgApp01.Cursor = "default";

            imgApp01.ClientSideEvents.Click = @"";
        }
        else
        {
            imgApp01.ImageUrl = "/SmartPro/Contents/Main/images/approval_on.png";
            lblApp01.Text = iCount.ToString("n0") + "건의 결재문서가 도착 했습니다.";
            imgApp01.Cursor = "pointer";

            imgApp01.ClientSideEvents.Click = @"
                function(s, e){ app01(); }";
        }
    }
}