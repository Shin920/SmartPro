using SMART;
using SMART.OM.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_MainContents_MainUserScheduleList : PageBase
{
    GObj G = new GObj();

    string strOutFlag = "";
    string strOutMsg = "";
    string strJob = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {
            Grid.DataBind();
        }
    }

    protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        Grid.ClearSort();
        Grid.DataBind();
    }
    protected void Grid_DataBinding(object sender, EventArgs e)
    {
        SMART.COMMON.DS.MainContents ObjS = new SMART.COMMON.DS.MainContents();
        DataSet ds = ObjS.GetScheduleList(G.D, G.EmpCode);
        Grid.DataSource = ds;
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMsg);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
        gv.JSProperties.Add("cp_ret_job", strJob);
    }

   
}