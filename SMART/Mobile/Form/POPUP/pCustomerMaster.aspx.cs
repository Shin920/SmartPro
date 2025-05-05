using MobileBiz;
using SMART;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Form_POPUP_pCustomerMaster : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        this.basePageInit();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        if (!this.IsPostBack)
        {
            if (Request["Action"] == "Load")
            {
                txtDate.Text = CFL.GetStr(Request["Date"]);
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
        BizCommon Obj = new BizCommon(baseUser.ClientID);

        CFL alData = Obj.Cs_Load(baseUser, txtDate.Text);
        if (alData.errCode < 0)
        {
            return;
        }

        Grid.DataSource = alData.ds;
    }
}