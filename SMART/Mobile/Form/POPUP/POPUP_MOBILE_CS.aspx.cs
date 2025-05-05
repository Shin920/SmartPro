using MobileBiz;
using SMART;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Form_POPUP_POPUP_MOBILE_CS : PageBase
{
    string strOutFlag = "";
    string strOutMsg = "";
    string strJob = "";



    protected void Page_Init(object sender, EventArgs e)
    {
        this.basePageInit();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //메시지, 라벨 텍스트 정보가져오기.
            LangLoad();

            BizCommon biz = new BizCommon(baseUser.ClientID);
        }
    }

    void LangLoad()
    {
        string[] sarCode = new string[]
        {
            "L00014","L01243","L01452"
        };

        BizCommon biz = new BizCommon(baseUser.ClientID);
        CFL alData = biz.GetFormLangDataLoad(baseUser, baseUser.LangCD, sarCode);


        //초기화 후 히든필드에 담기
        hidLang.Clear();
        if (alData.ds != null)
        {
            for (int i = 0; i < alData.ds.Tables[0].Rows.Count; i++)
            {
                hidLang[CFL.GetStr(alData.ds.Tables[0].Rows[i]["MCode"])] = CFL.GetStr(alData.ds.Tables[0].Rows[i]["Message"]);
            }
        }


        //가져온 다국어 정보 라벨 세팅하기.
        SettingLang();
    }

    void SettingLang()
    {
        lblCsName.Text = CFL.GetStr(hidLang["L01452"]);
        btnSearch.Text = CFL.GetStr(hidLang["L00014"]);

        Grid.Columns["CsCode"].Caption = CFL.GetStr(hidLang["L01243"]);
        Grid.Columns["CsName"].Caption = CFL.GetStr(hidLang["L01452"]);

    }

    protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {

        strJob = "";

        Grid.ClearSort();
        Grid.DataBind();
    }

    protected void Grid_DataBinding(object sender, EventArgs e)
    {
        BizMM Obj = new BizMM(baseUser.ClientID);

        string strWhereClause = "ComCode = " + CFL.Q(baseUser.ComCode) + " And CsUse = 'Y'";

        if (txtCsName.Text != "")
        {
            strWhereClause += @" And CsName Like " + CFL.Q("%" + txtCsName.Text + "%");
        }

        //CFL alData = Obj.popup_GetCsMaster(baseUser, strWhereClause);
        //Grid.DataSource = alData.ds.Tables[0];
    }

    protected void Grid_CustomErrorText(object sender, DevExpress.Web.ASPxGridViewCustomErrorTextEventArgs e)
    {
        if (!string.IsNullOrEmpty(strOutMsg))
        {
            e.ErrorText = strOutMsg;
        }
        else
        {
            e.ErrorText = "";
        }
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMsg);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
        gv.JSProperties.Add("cp_ret_job", strJob);
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

    }
}