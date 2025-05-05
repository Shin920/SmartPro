using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMART;
using SMART.COMMON.DS;

public partial class Common_Forms_AccMaster : PageBase
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
                txtAccCode.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtAccName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");
                txtCHKYN.Text = CFL.GetStr(Request["CHKYN"]).ToUpper().Replace("NULL", "Y");
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
        CommonCodeClass Obj = new CommonCodeClass();
        if(txtCHKYN.Text =="Y")
            strWhereClause += " AND LEFT(AccType,1) != RIGHT(AccType,1) AND CoaCode = " + CFL.Q(G.CoaCode);

        if (txtAccCode.Text != "")
            strWhereClause += " And AccCode = " + CFL.Q(txtAccCode.Text);

        if (txtAccName.Text != "")
            strWhereClause += " And AccName Like " + CFL.Q("%" + txtAccName.Text.Trim() + "%");

        DataSet ds = Obj.GetAccMaster(G.D, strWhereClause, "AccCode/AccName/");
        Grid.DataSource = ds;
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMessage);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
    }
}