using SMART;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMART.COMMON.DS;

public partial class Common_Forms_CreateNoPopup : PageBase
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
            deBDate.Date = DateTime.Today.AddDays(-7);
            deEDate.Date = DateTime.Today;

            if(Request["Action"] == "Load")
            {
                txtGubun.Text = CFL.GetStr(Request["OrderGubun"]);
                txtGubun2.Text = CFL.GetStr(Request["Gubun2"]);
                txtFormID.Text = CFL.GetStr(Request["FormID"]);
                txtCondition.Text = CFL.GetStr(Request["Condition"]);
                txtSiteCode.Text = CFL.GetStr(Request["SiteCode"]).ToUpper().Replace("NULL", "");

                //키필드 설정
                Grid.KeyFieldName = txtCondition.Text.Replace('/', ';');

                // 에버소닉 팔레트번호의 경우. 날짜가 존재하지않음 해서 Enable 처리

                if(txtFormID.Text == "_NP510")
                {
                    deBDate.Enabled = false;
                    deEDate.Enabled = false;
                }
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
        string strBDate = deBDate.Date.ToString("yyyyMMdd");
        string strEDate = deEDate.Date.ToString("yyyyMMdd");

        if (txtGubun.Text == "SD")
        {
            
        }
        else if (txtGubun.Text == "PO")
        {
            
        }
        else if (txtGubun.Text == "OS")
        {
           
        }
        else if (txtGubun.Text == "NP")
        {           

        }
        else if (txtGubun.Text == "MM")
        {

            
        }
        else if (txtGubun.Text == "QM")
        {
            
            
        }
        else if (txtGubun.Text == "HR")
        {

            
        }
        else if (txtGubun.Text == "GL")
        {
            
        }
        else if (txtGubun.Text == "TR")
        {
            
        }
        else if (txtGubun.Text == "AA")
        {
            
        }
        else if (txtGubun.Text == "EX")
        {
            
        }
        else if (txtGubun.Text == "IM")
        {
            
        }
        else if(txtGubun.Text == "PP")
        {
           
        }
        else if (txtGubun.Text == "CO")
        {
           
        }
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMessage);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
    }

    protected void Grid_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.Caption.ToUpper() == "NO")
        {
            if (e.VisibleIndex >= 0)
            {
                e.DisplayText = (e.VisibleIndex + 1).ToString();
            }
        }


        if (txtGubun.Text == "MM")
        {
          
        }

        if(txtGubun.Text == "TR")
        {
            
        }

        if (txtGubun.Text == "NP")
        {
            
        }

        if(txtGubun.Text == "SD")
        {
            
        }

        if(txtGubun.Text == "IM")
        {
        
        }
        if(txtGubun.Text == "PO")
        {
        
        }
       
        if(txtGubun.Text == "OS")
        {
        
        }
    }
}