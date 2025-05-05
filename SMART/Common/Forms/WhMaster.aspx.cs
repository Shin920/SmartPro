using SMART;
using SMART.COMMON.DS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_Forms_WhMaster : PageBase
{

    GObj G = new GObj();

    string strOutMessage = "";
    string strOutFlag = "";

    string strWhereClause = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();
        G = ((GObj)Session["G"]);
        if (!IsPostBack)
        {
            if (Request["Action"] == "Load")
            {
                txtWhCode.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtWhName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");

                strWhereClause = CFL.GetStr(Request["W"]);

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
        if (txtWhCode.Text.Trim() != "")
        {
            strWhereClause += @"
AND WhCode LIKE " + CFL.Q(txtWhCode.Text.Trim() + "%") + @" ";
        }
        if (txtWhName.Text.Trim() != "")
        {
            strWhereClause += @"
AND WhName LIKE " + CFL.Q("%" + txtWhName.Text.Trim() + "%") + @" ";
        }

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.GetWhMaster(G.D, strWhereClause, "WhCode/WhName/");
        Grid.DataSource = ds;
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMessage);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
    }
}