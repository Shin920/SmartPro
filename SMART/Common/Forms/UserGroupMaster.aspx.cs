using SMART;
using SMART.COMMON.DS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_Forms_UserGroupMaster : PageBase
{
    GObj G = new GObj();

    string strOutMessage = "";
    string strOutFlag = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();
        G = ((GObj)Session["G"]);
        if (!IsPostBack)
        {
            if (Request["Action"] == "Load")
            {
                txtUserGroup.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtGroupName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");

                //Data조회
                Grid.DataBind();
            }
        }
    }
    protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        Grid.ClearSort();
        Grid.DataBind();
    }

    protected void Grid_DataBinding(object sender, EventArgs e)
    {
        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetUserGroupMaster(G.D, " WHERE ComCode = " + CFL.Q(G.ComCode) + " and CONVERT(CHAR(8), GETDATE(), 112) BETWEEN GroupBdate AND GroupEdate ", "UserGroup/GroupName/");
        Grid.DataSource = ds;
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMessage);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
    }
}