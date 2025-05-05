using SMART;
using SMART.COMMON.DS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_Forms_CsMaster : PageBase
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
            CommonCodeClass mpc = new CommonCodeClass();
            ddlCsType.DataSource = mpc.GetCodeMaster(G.D, " AND CodeID = 'CsType' AND ComCode = " + CFL.Q(G.ComCode) + " AND CodeUse = 'Y' ", "CodeCode/CodeName/");
            ddlCsType.DataBind();

            ddlRegion.DataSource = mpc.GetRegionMaster(G.D, "", "RegionCode/RegionName/");
            ddlRegion.DataBind();

            if (Request["Action"] == "Load")
            {
                txtCsCode.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtCsName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");
                txtWhere.Text = CFL.GetStr(Request["W"]).ToUpper().Replace("NULL", "");
                txtCondition.Text = CFL.GetStr(Request["Condition"]);

                if (txtCondition.Text == "")
                    txtCondition.Text = "CsCode/CsNameFull/CurrCode/BaseExchRatio/TaxCodeAp/";

                // 기타조건 추가  ----------------------------------------------------------------------
                txtCond1.Text = CFL.GetStr(Request["Cond1"]);


                //키필드 설정
                Grid.KeyFieldName = txtCondition.Text.Replace('/', ';');
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
        string strWhereClause = "";

        string strCsCode = CFL.GetStr(txtCsCode.Text);
        string strCsName = CFL.GetStr(txtCsName.Text);
        string strCsType = CFL.GetStr(ddlCsType.Value);
        string strRegion = CFL.GetStr(ddlRegion.Value);
        string strRegNo = CFL.GetStr(txtRegNo.Text);

        if (strCsCode != "")
        {
            strWhereClause += @"
AND A.CsCode LIKE " + CFL.Q(strCsCode + "%");
        }

        if (strCsName != "")
        {
            strWhereClause += @"
AND A.CsName LIKE " + CFL.Q("%" + strCsName + "%");
        }
        
        if (strCsType != "")
        {
            strWhereClause += @"
AND A.CsType = " + CFL.Q(strCsType);
        }
        
        if (strRegion != "")
        {
            strWhereClause += @"
AND A.RegionCode = " + CFL.Q(strRegion);
        }
        
        if (strRegNo != "")
        {
            strWhereClause += @"
AND A.RegNo = " + CFL.Q(strRegNo);
        }

        if (txtWhere.Text != "")
        {
            strWhereClause += txtWhere.Text;
        }

        CommonCodeClass Obj = new CommonCodeClass();
    //  DataSet ds = Obj.GetCsMaster(G.D, strWhereClause, "CsCode/CsName/CurrCode/BaseExchRatio/TaxCodeAp/");
    //  DataSet ds = Obj.GetCsMaster(G.D, strWhereClause, txtCondition.Text, Grid);
        
        // 추가 조건
        DataSet ds = Obj.GetCsMaster(G.D, strWhereClause, txtCondition.Text, txtCond1.Text, Grid);

        Grid.DataSource = ds;
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
                e.DisplayText = (e.VisibleIndex + 1).ToString("n0");
            }
        }
    }
}