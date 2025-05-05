using SMART;
using SMART.COMMON.DS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public partial class glb_Forms_FindForms : PageBase
{

    GObj G = new GObj();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {
            if (Request["Action"] == "Load")
            {
                txtFindForm.Text = Request["Info"];
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
        ModuleObj Obj = new ModuleObj();
        DataSet ds = Obj.FindFormData(G.D, txtFindForm.Text.Trim());
        Grid.DataSource = ds;
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {

    }

    protected void Grid_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
    {

    }

}