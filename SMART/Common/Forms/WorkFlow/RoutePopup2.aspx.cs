using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WorkFlow;
using SMART;

public partial class Common_Forms_WorkFlow_RoutePopup2 : System.Web.UI.Page
{

    GObj G = new GObj();


    SMART.OM.DS.WorkFlow obj = new SMART.OM.DS.WorkFlow();

    public string strViewState = "";

    protected void Page_Load(object sender, EventArgs e)
    {        
        G = (GObj)Session["G"];


        strViewState = Request["ViewState"];

        DataSet ds1 = obj.RouteList(G.D, strViewState);
        

        if (ds1 != null)
            Grid.DataSource = ds1.Tables[0].DefaultView;


        Grid.DataBind();
        

        //if (!IsPostBack)
        //{
           
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add("table");
        //    Grid.DataSource = ds.Tables[0];
        //    Grid.DataBind();
            
        //}

    }
}