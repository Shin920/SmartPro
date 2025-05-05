using SMART;
using SMART.COMMON.DS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_Forms_ItemSiteMaster : PageBase
{
    GObj G = new GObj();

    string strOutMessage = "";
    string strOutFlag = "";

    string strWhereClause = "";
    string strCondition = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();
        G = ((GObj)Session["G"]);
        if (!IsPostBack)
        {
            CommonCodeClass mpc = new CommonCodeClass();
            ddlItemAcc.DataSource = mpc.GetItemAccMaster(G.D, " AND ComCode = " + CFL.Q(G.ComCode), "ItemAccCode/ItemAccName/");
            ddlItemAcc.DataBind();
            ddlItemAcc.Items.Insert(0, new DevExpress.Web.ListEditItem("", ""));

            if(Request["Action"] == "Load")
            {
                string strCondition = CFL.GetStr(Request["Condition"]).ToUpper().Replace("NULL", "");

                txtItemCode.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtItemName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");

                txtCondition.Text = CFL.GetStr(Request["Condition"]);

                if (txtCondition.Text == "")
                    txtCondition.Text = "ItemCode/ItemName/ItemSpec/UnitMM/LotCheck/";


                // 기타조건 추가  ----------------------------------------------------------------------
                txtCond1.Text = CFL.GetStr(Request["Cond1"]);
                txtCond2.Text = CFL.GetStr(Request["Cond2"]);
                txtCond3.Text = CFL.GetStr(Request["Cond3"]);


                // 키필드 설정
                Grid.KeyFieldName = txtCondition.Text.Replace('/', ';');

                Grid.DataBind();

                //20231122 대안일레컴 권승안 요청..
                FormSettingBySiteNo();
            }
        }
    }

    void FormSettingBySiteNo()
    {
        if(G.SiteNo == "DaeAn")
        {
            lblItemCode.Text = "CODE";
            lblItemName.Text = "품명";
            lblItemSpec.Text = "SPEC";
        }
    }

    void GridColSettingBySiteNo()
    {
        if (G.SiteNo == "DaeAn")
        {
            if (0 <= txtCondition.Text.Trim().IndexOf("ItemCode/"))
                Grid.Columns["ItemCode"].Caption = "CODE";
            if (0 <= txtCondition.Text.Trim().IndexOf("ItemName/"))
                Grid.Columns["ItemName"].Caption = "품명";
            if (0 <= txtCondition.Text.Trim().IndexOf("ItemSpec/"))
                Grid.Columns["ItemSpec"].Caption = "SPEC";
        }
    }

    protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        Grid.ClearSort();
        Grid.DataBind();
    }

    protected void Grid_DataBinding(object sender, EventArgs e)
    {
        string strItemAcc = CFL.GetStr(ddlItemAcc.Value);
        string strItemGroup = CFL.GetStr(txtItemGroup.Text);
        string strItemCode = CFL.GetStr(txtItemCode.Text);
        string strItemName = CFL.GetStr(txtItemName.Text);
        string strItemSpec = CFL.GetStr(txtItemSpec.Text);

        // Where 조건
        strWhereClause = CFL.GetStr(Request["W"]);

        /*
        if (CFL.GetStr(Request["Condition"]) != "" && CFL.GetStr(Request["Conditon"]) != null)
        {
            strCondition = Request["Condition"];
        }
        */

        if (strItemAcc != "")
        {
            strWhereClause += @"
AND A.ItemAccCode = " + CFL.Q(strItemAcc);
        }

        if (strItemGroup != "")
        {
            strWhereClause += @"
AND A.ItemGroup = " + CFL.Q(strItemGroup);
        }

        if (strItemCode != "")
        {
            strWhereClause += @"
AND A.ItemCode LIKE " + CFL.Q(strItemCode + "%");
        }

        if (strItemName != "")
        {
            strWhereClause += @"
AND A.ItemName LIKE " + CFL.Q("%" + strItemName + "%");
        }

        if (strItemSpec != "")
        {
            strWhereClause += @"
AND A.ItemSpec LIKE " + CFL.Q("%" + strItemSpec + "%");
        }

        if (CFL.GetStr(Request["TYPE"]) == "NPPROUT")
        {
            strWhereClause += @" 
    and ItemCode in ( select distinct ItemCode from nppSubstituteMaster where SiteCode = " + CFL.Q(G.SiteCode) + @" )  and ItemType = 'MAT'";
        }

        if (CFL.GetStr(Request["TYPE"]) == "NOTSUB")
        {
            strWhereClause += @" 
    and ItemCode not in ( select distinct ItemCode from nppSubstituteMaster where SiteCode = " + CFL.Q(G.SiteCode) + @" )  ";
        }

        CommonCodeClass Obj = new CommonCodeClass();


        //  DataSet ds = Obj.GetItemSiteMasterLoad(G.D, strWhereClause, txtCondition.Text, Grid);
        if (0 <= txtCond1.Text.IndexOf("BOMCHK/"))
        {
            strWhereClause += @" 
    and a.ItemCode in (select ItemCode from nppBomHeader) ";
        }
        // 추가 조건
        DataSet ds = Obj.GetItemSiteMasterLoad(G.D, strWhereClause, txtCondition.Text, txtCond1.Text, txtCond2.Text, txtCond3.Text, Grid);

        Grid.DataSource = ds;

        //20231122 대안일레컴 권승안 요청..
        GridColSettingBySiteNo();
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