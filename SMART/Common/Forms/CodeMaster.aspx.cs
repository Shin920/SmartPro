using SMART;
using SMART.COMMON.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_Forms_CodeMaster : PageBase
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
                txtCodeID.Text = CFL.GetStr(Request["CodeID"]).ToUpper().Replace("NULL", "");
                txtCodeCode.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtCodeName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");

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
        string strWhereClause = " AND A.CodeID = " + CFL.Q(txtCodeID.Text.Trim());
        if(txtCodeCode.Text.Trim() != "")
        {
            strWhereClause += @" 
AND A.CodeCode LIKE (" + CFL.Q("%" + txtCodeCode.Text.Trim() + "%") + ")";
        }
        if (txtCodeName.Text.Trim() != "")
        {
            strWhereClause += @" 
AND A.CodeName LIKE (" + CFL.Q("%" + txtCodeName.Text.Trim() + "%") + ")";
        }

        try
        {
            CommonCodeClass obj = new CommonCodeClass();
            Grid.DataSource = obj.GetCodeMaster(G.D, strWhereClause, "CodeCode/CodeName/");
        }
        catch { }
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMessage);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
    }
}