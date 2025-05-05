using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMART;
using SMART.COMMON.DS;

public partial class Common_Forms_CcMaster : PageBase
{
    GObj G = new GObj();

    string strOutMessage = "";
    string strOutFlag = "";
    string strCurrCcCode = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();
        G = ((GObj)Session["G"]);
        if (!IsPostBack)
        {
            if (Request["Action"] == "Load")
            {
                strCurrCcCode = Request["CurCcCode"];

                txtCcCode.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtCcName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");

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
        string strWhereClause = "ComCode = " + CFL.Q(G.ComCode) + " And CcUse = 'Y'";

        if (strCurrCcCode != "" && strCurrCcCode != null)
            strWhereClause += " And CcCode <> " + CFL.Q(strCurrCcCode);

        if (txtCcCode.Text != "")
            strWhereClause += " And CcCode = " + CFL.Q(txtCcCode.Text);

        if (txtCcName.Text != "")
            strWhereClause += " And CcName = " + CFL.Q(txtCcName.Text);

        CommonCodeClass Obj = new CommonCodeClass();
        DataSet ds = Obj.Popup_CcMaster(G.D, strWhereClause, "CcCode/CcName/");
        Grid.DataSource = ds;
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMessage);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
    }
}