using SMART;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_SiteBoard_SCM_Announce_SCMAnnounceList : PageBase
{
    GObj G = new GObj();

    string strOutFlag = "";
    string strOutMsg = "";
    string strJob = "";

    SMART.COMMON.DA.Announce ObjA = new SMART.COMMON.DA.Announce();
    SMART.COMMON.DS.Announce ObjS = new SMART.COMMON.DS.Announce();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {
            Grid.DataBind();
            LangLoad();
        }
    }
    void LangLoad()
    {
        string[] sarCode = new string[]
        {
            "L00014", "L00001", "L00002", "L00003", "L00234", "L00235", "L04183"
            , "L00839", "L04184", "L00918", "L04185", "L01156", "L00914"

        };

        SMART.COMMON.DS.MessageMaster mm = new SMART.COMMON.DS.MessageMaster();

        DataSet ds = mm.GetFormLangDataLoad(G.D, G.LangID, sarCode);

        //초기화 후 히든필드에 담기
        hidLang.Clear();

        if (ds.Tables.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                hidLang[CFL.GetStr(ds.Tables[0].Rows[i]["MCode"])] = CFL.GetStr(ds.Tables[0].Rows[i]["Message"]);
            }

            // 가져온 다국어 정보 라벨 세팅하기.
            SettingLang();
        }
    }


    //가져온 메시지 세팅하기
    void SettingLang()
    {
        btnSearch.Text = CFL.GetStr(hidLang["L00014"]);
        btnNew.Text = CFL.GetStr(hidLang["L00914"]);
        btnOpen.Text = CFL.GetStr(hidLang["L04183"]); //상세보기

        Grid.Columns["Num"].Caption = CFL.GetStr(hidLang["L00839"]); //순번
        Grid.Columns["Title"].Caption = CFL.GetStr(hidLang["L04184"]); //제목
        Grid.Columns["Readnum"].Caption = CFL.GetStr(hidLang["L04185"]); //조회수
        Grid.Columns["Writeday"].Caption = CFL.GetStr(hidLang["L00918"]); //작성일
        Grid.Columns["Writer"].Caption = CFL.GetStr(hidLang["L01156"]); //작성자

    }
    protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        Grid.ClearSort();
        Grid.DataBind();
    }

    protected void Grid_DataBinding(object sender, EventArgs e)
    {
        DataSet ds = ObjS.GetAnounceListLoad_SCM(G.D);
        Grid.DataSource = ds;
    }

    protected void Grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMsg);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
        gv.JSProperties.Add("cp_ret_job", strJob);
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        GridExport.FileName = "공지사항";
        GridExport.WriteXlsxToResponse();
    }

    protected string GetIcon(object _val)
    {
        string ret = "";
        string val = (string)_val;

        if (val == "Y")
        {
            ret = "/SmartPro/Contents/Images/icon_important.png";
        }
        /*
        else
        {
            ret = "/SmartPro/Contents/Images/icon_important_none.jpg";
        }
        */

        return ret;
    }

}