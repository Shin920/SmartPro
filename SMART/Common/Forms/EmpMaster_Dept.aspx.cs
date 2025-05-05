using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMART;

public partial class Common_Forms_EmpMaster_Dept : PageBase
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
                txtID.Text = Request["UserID"];
                txtEmpCode.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtEmpName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");
                txtWhereClause.Text = CFL.GetStr(Request["W"]).ToUpper().Replace("NULL", "");
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
        SMART.COMMON.DS.CommonCodeClass dsObj = new SMART.COMMON.DS.CommonCodeClass();

        string strToday = DateTime.Today.ToString().Replace("-", "").Substring(0, 8);
        string strWhereClause = txtWhereClause.Text;
        string strEmpCode = txtEmpCode.Text;
        string strEmpName = txtEmpName.Text;
        
        if(strEmpCode != "")
        {
            strWhereClause += " and A.EmpCode like " + CFL.Q(strEmpCode + "%");
        }

        if(strEmpName != "")
        {
            strWhereClause += " and A.EmpName like " + CFL.Q(strEmpName + "%");
        }

        try
        {
            Grid.DataSource = dsObj.GetEmpDeptPopup(G.D, strToday, strWhereClause);
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