using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMART;
using System.Collections;
using System.Data;
using SMART.OM.DA;

public partial class MNG_OM_Org_OM068_OM068 : PageBase
{
    GObj G = new GObj();

    string strOutFlag = "";
    string strOutMsg = "";
    string strJob = "";

    SMART.OM.DA.OM068 ObjA = new SMART.OM.DA.OM068();
    SMART.OM.DS.OM068 ObjS = new SMART.OM.DS.OM068();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!this.IsPostBack)
        {
            SMART.COMMON.DS.CommonCodeClass cm = new SMART.COMMON.DS.CommonCodeClass();

            DataSet dSite = cm.GetSiteMaster(G.D, "", "SiteCode/SiteName/");
            cbSiteCode.DataSource = dSite.Tables[0];
            cbSiteCode.DataBind();

            LangLoad();

            Grid2.DataBind();

            //메시지, 라벨 텍스트 정보가져오기.
            
        }

    }

    //다국어 메시지 가져오기.
    void LangLoad()
    {
        string[] sarCode = new string[]
        {
            "L00001"
            ,"L00002"
            ,"L00010"
            ,"L00014"
            ,"L00308"
            ,"L00309"
            ,"L00310"
            ,"L00311"
            ,"L00312"
            ,"M00150"
            ,"M00004"
            ,"M00090"
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

            //가져온 다국어 정보 라벨 세팅하기.
            SettingLang();
        }
    }

    //가져온 메시지 세팅하기
    void SettingLang()
    {
        //L00001 신규
        //L00002 저장
        //L00010 사용
        //L00014 검색
        //L00308 공장
        //L00309 창고명
        //L00310 창고코드
        //L00311 보관장소
        //L00312 보관장소명

        lblSite.Text = CFL.GetStr(hidLang["L00308"]);
        lblWhName.Text = CFL.GetStr(hidLang["L00309"]);
        btnNew.Text = CFL.GetStr(hidLang["L00001"]);
        btnSearch.Text = CFL.GetStr(hidLang["L00014"]);

        Grid.Columns["WhCode"].Caption = CFL.GetStr(hidLang["L00310"]);
        Grid.Columns["WhName"].Caption = CFL.GetStr(hidLang["L00309"]);

        lblWhCode.Text = CFL.GetStr(hidLang["L00310"]);
        lblWhName2.Text = CFL.GetStr(hidLang["L00309"]);

        btnSave.Text = CFL.GetStr(hidLang["L00002"]);

        Grid2.Columns["LocationCode"].Caption = CFL.GetStr(hidLang["L00311"]);
        Grid2.Columns["LocationName"].Caption = CFL.GetStr(hidLang["L00312"]);
        Grid2.Columns["UseYN"].Caption = CFL.GetStr(hidLang["L00010"]);

    }
    protected bool SaveMode
    {
        get
        {
            return txtSaveMode.Text == "Y";
        }
        set
        {
            // Mode Control
            btnSave.Enabled = G.GetPermission("OM068", GObj.SecKind.Insert);

            // Save Mode Information
            txtSaveMode.Text = value ? "Y" : "N";
        }
    }

    protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        if(e.Parameters == "NEW")
        {
            Grid.ClearSort();

            strOutMsg = "";
            strOutFlag = "Y";
            strJob = "NEW";

            Grid.DataBind();
        }
        else
        {
            Grid.ClearSort();

            //strOutMsg = "";
            //strOutFlag = "Y";
            //strJob = "S";

            Grid.DataBind();
        }
    }

    protected void Grid_DataBinding(object sender, EventArgs e)
    {
        if(strJob == "NEW")
        {
            strJob = "";
            return;
        }

        string strSiteCode = CFL.GetStr(cbSiteCode.Value);
        string strWhName = txtWhName.Text;
        string WhereClause = " ComCode = " + CFL.Q(G.ComCode) + " And isnull(WhUse, 'N') = 'Y' And isnull(LocationUse, 'N') = 'Y' And SiteCode = " + CFL.Q(strSiteCode);

        if (strWhName != "")
        {
            WhereClause += " And WhName Like " + CFL.Q("%" + strWhName + "%");
        }

        DataSet ds = ObjS.Grid(G.D, WhereClause);
        Grid.DataSource = ds;
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

    protected void callBackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        string job = hidField["job"].ToString();

        switch (job)
        {
            case "S":
                JobSelect();
                break;
        }
    }

    void JobSelect()
    {
        string strWhCode = CFL.GetStr(hidField["WhCode"]);
        hidField.Clear();
        hidField["job"] = "S";

        DataSet ds = ObjS.Load(G.D, strWhCode);

        if (ds.Tables[0].Rows.Count > 0)
        {
            hidField["status"] = "Y";

            //Grid02.DataBind();

            //히든 필드에 담기
            CFL.setDataSetToDevHiddenField(ds, hidField);
        }
        else
        {
            hidField["status"] = "N";
        }
    }

    protected void Grid2_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters == "UPDATE")
        {
            Grid2.UpdateEdit();
        }
        else
        {
            Grid2.ClearSort();
            Grid2.DataBind();
        }

        //Grid2.ClearSort();

        //strOutMsg = "";
        //strOutFlag = "Y";
        //strJob = "S";

        //Grid2.DataBind();
    }

    protected void Grid2_DataBinding(object sender, EventArgs e)
    {
        string strWhCode = txtWhCode.Text.Trim();
        string strWhereClause = " A.WhCode = " + CFL.Q(strWhCode);

        string strLocationName = txtLocationName.Text.Trim();

        if(strLocationName.Length > 0)
        {
            strWhereClause += " And A.LocationName Like " + CFL.Q(strLocationName + '%');
        }

        DataSet ds = ObjS.Grid2(G.D, strWhereClause);

        Grid2.DataSource = ds;
    }

    protected void Grid2_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMsg);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
        gv.JSProperties.Add("cp_ret_job", strJob);
    }

    protected void Grid2_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.Caption.ToUpper() == "NO")
        {
            if (e.VisibleIndex >= 0)
            {
                e.DisplayText = (e.VisibleIndex + 1).ToString();
            }
        }
    }

    protected void Grid2_CustomErrorText(object sender, DevExpress.Web.ASPxGridViewCustomErrorTextEventArgs e)
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

    protected void Grid2_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        e.Handled = false;

        strJob = "save";
        strOutFlag = "Y";
        strOutMsg = "";

        List<DevExpress.Web.Data.ASPxDataInsertValues> dataInsert = e.InsertValues;
        List<DevExpress.Web.Data.ASPxDataUpdateValues> dataUpdate = e.UpdateValues;
        List<DevExpress.Web.Data.ASPxDataDeleteValues> dataDelete = e.DeleteValues;
        
        string strWhCode = txtWhCode.Text.Trim();

        DataTable dt = CFL.GetDataCols(e, Grid2);
        int rCnt = dt.Rows.Count;

        string[] sarLocationCode = new string[rCnt];    // 로케이션
        string[] sarLocationName = new string[rCnt];    // 로케이션명
        string[] sarUseYN = new string[rCnt];           // 사용유무

        for (int i = 0; i < rCnt; i++)
        {
            sarLocationCode[i] = CFL.F(dt.Rows[i]["LocationCode"]).ToString();
            sarLocationName[i] = CFL.F(dt.Rows[i]["LocationName"]).ToString();
            sarUseYN[i] = CFL.F(dt.Rows[i]["UseYN"]).ToString();
        }


        if (rCnt == 0)
        {
            strOutFlag = "N";
            strOutMsg = "저장할 항목이 존재하지 않습니다.";
            return;
        }

        /*
        if ((dataInsert.Count + dataUpdate.Count + dataDelete.Count) <= 0)
        {
            strOutFlag = "N";
            strOutMsg = CFL.GetStr(hidLang["M00090"]);
            return;
        }

        string[] sarLocationCode = new string[Grid2.VisibleRowCount + dataInsert.Count + dataUpdate.Count + dataDelete.Count];
        string[] sarLocationName = new string[Grid2.VisibleRowCount + dataInsert.Count + dataUpdate.Count + dataDelete.Count];
        string[] sarUseYN = new string[Grid2.VisibleRowCount + dataInsert.Count + dataUpdate.Count + dataDelete.Count];
        string[] sarGubun = new string[Grid2.VisibleRowCount + dataInsert.Count + dataUpdate.Count + dataDelete.Count];


        for (int i = 0; i < Grid2.VisibleRowCount; i++)
        {
            sarLocationCode[i] = (string)((DevExpress.Web.ASPxGridView)sender).GetRowValues(i, "LocationCode");
            sarLocationName[i] = (string)((DevExpress.Web.ASPxGridView)sender).GetRowValues(i, "LocationName");
            if ((string)CFL.F(((DevExpress.Web.ASPxGridView)sender).GetRowValues(i, "UseYN")) != "")
                sarUseYN[i] = (string)((DevExpress.Web.ASPxGridView)sender).GetRowValues(i, "UseYN");
            else
                sarUseYN[i] = "N";
            sarGubun[i] = "Normal";
        }

        for (int i = 0; i < dataInsert.Count; i++)
        {
            sarLocationCode[Grid2.VisibleRowCount + i] = CFL.GetStr(dataInsert[i].NewValues["LocationCode"]);
            sarLocationName[Grid2.VisibleRowCount + i] = CFL.GetStr(dataInsert[i].NewValues["LocationName"]);
            sarUseYN[Grid2.VisibleRowCount + i] = CFL.GetStr(dataInsert[i].NewValues["UseYN"]);
            sarGubun[Grid2.VisibleRowCount + i] = "Insert";
        }

        for (int i = 0; i < dataUpdate.Count; i++)
        {
            sarLocationCode[Grid2.VisibleRowCount + dataInsert.Count + i] = CFL.GetStr(dataUpdate[i].NewValues["LocationCode"]);
            sarLocationName[Grid2.VisibleRowCount + dataInsert.Count + i] = CFL.GetStr(dataUpdate[i].NewValues["LocationName"]);
            sarUseYN[Grid2.VisibleRowCount + dataInsert.Count + i] = CFL.GetStr(dataUpdate[i].NewValues["UseYN"]);
            sarGubun[Grid2.VisibleRowCount + dataInsert.Count + i] = "Update";
        }

        for (int i = 0; i < dataDelete.Count; i++)
        {
            sarLocationCode[Grid2.VisibleRowCount + dataInsert.Count + dataUpdate.Count + i] = CFL.GetStr(dataDelete[i].Values["LocationCode"]);
            sarLocationName[Grid2.VisibleRowCount + dataInsert.Count + dataUpdate.Count + i] = CFL.GetStr(dataDelete[i].Values["LocationName"]);
            sarUseYN[Grid2.VisibleRowCount + dataInsert.Count + dataUpdate.Count + i] = CFL.GetStr(dataDelete[i].Values["UseYN"]);
            sarGubun[Grid2.VisibleRowCount + dataInsert.Count + dataUpdate.Count + i] = "Delete";
        }
        */

        try
        {
            ArrayList alData = ObjA.Save(G.D, SaveMode, G.LangID, strWhCode, sarLocationCode, sarLocationName, sarUseYN);

            if (alData[0].ToString() != "00")
            {
                strOutFlag = "N";
                strOutMsg = alData[1].ToString();
                return;
            }
        }
        catch
        {
            strOutFlag = "N";
            strOutMsg = CFL.GetStr(hidLang["M00150"]);
            return;
        }

        e.Handled = true;
    }


}