using SMART;
using SMART.OM.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_MainContents_MainScheduleCalendar : PageBase
{
    GObj G = new GObj();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {

        }
    }


}