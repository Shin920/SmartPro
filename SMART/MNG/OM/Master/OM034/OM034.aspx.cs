using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMART;
using System.Collections;
using System.Data;
using DevExpress.Web;

public partial class MNG_OM_Master_OM034_OM034 : PageBase
{
    GObj G = new GObj();

    string strOutFlag = "";
    string strOutMsg = "";
    string strJob = "";

    SMART.OM.DA.OM034 ObjA = new SMART.OM.DA.OM034();
    SMART.OM.DS.OM034 ObjS = new SMART.OM.DS.OM034();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = ((GObj)Session["G"]);

        if (!IsPostBack)
        {
            txtDigitNo_glAmnt1.Text = G.DigitNo_glAmnt1.ToString();
            txtDigitType_glAmnt1.Text = G.DigitType_glAmnt1.ToString();

            SMART.COMMON.DS.CommonCodeClass cm = new SMART.COMMON.DS.CommonCodeClass();

            string strWhereClause = " And SiteCode=" + CFL.Q(G.SiteCode);
            string strCondition = "SiteCode/SiteName/";

            DataSet dSite = cm.GetSiteMaster(G.D, strWhereClause, strCondition);
            ddlSiteCode.DataSource = dSite.Tables[0];
            ddlSiteName.DataSource = dSite.Tables[0];
            ddlSiteCode.DataBind();
            ddlSiteName.DataBind();

            strWhereClause = " And CodeUse='Y' and CodeID='hrJobWork' and ComCode=" + CFL.Q(G.ComCode);
            strCondition = "CodeCode/CodeName/";

            DataSet ds = cm.GetCodeMaster(G.D, strWhereClause, strCondition);
            cbJob.DataSource = ds;
            cbJob.DataBind();
            cbJob.Items.Insert(0, new ListEditItem("", ""));

            SaveMode = true;

            //메시지, 라벨 텍스트 정보가져오기.
            LangLoad();
        }

        string now = CFL.DateConvert(G.CurDate, CFL.DCType.DAddSlash).Replace("/", "-");
        calEmpBDate.Text = now;
    }

    //다국어 메시지 가져오기.
    void LangLoad()
    {
        string[] sarCode = new string[]
        {
            "L00001",
            "L00002",
            "L00003",
            "L00014",
            "L00019",
            "L00020",
            "L00021",
            "L00022",
            "L00023",
            "L00024",
            "L00025",
            "L00026",
            "L00028",
            "L00027",
            "L00029",
            "L00030",
            "L00308",
            "M00300",
            "M00299",
            "M00297",
            "M00295",
            "M00294",
            "M00162",
            "M00002",
            "M00228",
            "M00004",
            "M00005",
            "L01098"
        };

        SMART.COMMON.DS.MessageMaster mm = new SMART.COMMON.DS.MessageMaster();
        DataSet ds = mm.GetFormLangDataLoad(G.D, G.LangID, sarCode);

        //초기화 후 히든필드에 담기
        hidLang.Clear();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            hidLang[CFL.GetStr(ds.Tables[0].Rows[i]["MCode"])] = CFL.GetStr(ds.Tables[0].Rows[i]["Message"]);
        }

        //가져온 다국어 정보 라벨 세팅하기.
        SettingLang();
    }

    //가져온 메시지 세팅하기
    void SettingLang()
    {
        //L00001  신규
        //L00002  저장
        //L00003  삭제
        //L00014  검색
        //L00019  소속사업장
        //L00020  부서
        //L00021  사원
        //L00022  사원코드
        //L00023  사원명
        //L00024  부서명
        //L00025  시급
        //L00026  메일ID
        //L00027  전화번호
        //L00028 입사일
        //L00029  출고권한
        //L00030 원가계산(Hour)

        lblSite.Text = CFL.GetStr(hidLang["L00308"]);
        btnSearch.Text = CFL.GetStr(hidLang["L00014"]);
    //  lblDept.Text = CFL.GetStr(hidLang["L00020"]);
    //  lblEmp.Text = CFL.GetStr(hidLang["L00021"]);
        lblEmpCode.Text = CFL.GetStr(hidLang["L00022"]);
        lblEmpName.Text = CFL.GetStr(hidLang["L00023"]);
        lblSiteName.Text = CFL.GetStr(hidLang["L00308"]);
        lblDept2.Text = CFL.GetStr(hidLang["L00020"]);
        lblMailID.Text = CFL.GetStr(hidLang["L00026"]);
        lblTelNo.Text = CFL.GetStr(hidLang["L00027"]);
        lblEmpBdate.Text = CFL.GetStr(hidLang["L00028"]);
        lblInoutAdmin.Text = CFL.GetStr(hidLang["L00029"]);
        lblHourCost.Text = CFL.GetStr(hidLang["L00030"]);
        
        Grid01.Columns["EmpCode"].Caption = CFL.GetStr(hidLang["L00022"]);
        Grid01.Columns["EmpName"].Caption = CFL.GetStr(hidLang["L00023"]);
        Grid01.Columns["DeptCode"].Caption = CFL.GetStr(hidLang["L01098"]);
        Grid01.Columns["DeptName"].Caption = CFL.GetStr(hidLang["L00024"]);
        Grid01.Columns["SiteName"].Caption = CFL.GetStr(hidLang["L00308"]);
        Grid01.Columns["HourCost"].Caption = CFL.GetStr(hidLang["L00025"]);

        btnNew.Text = CFL.GetStr(hidLang["L00001"]);
        btnSave.Text = CFL.GetStr(hidLang["L00002"]);
        btnDelete.Text = CFL.GetStr(hidLang["L00003"]);

    }


    protected bool SaveMode
    {
        get
        {
            return txtSaveMode.Text == "Y";
        }
        set
        {
            // Save Mode Information
            txtSaveMode.Text = value ? "Y" : "N";
        }
    }

    protected void Grid01_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        Grid01.ClearSort();
        Grid01.DataBind();
    }

    protected void Grid01_DataBinding(object sender, EventArgs e)
    {
        string strWhereClause = @" 
Where A.ComCode = " + CFL.Q(G.ComCode) + @" 
  and B.ComCode = " + CFL.Q(G.ComCode) + @" 
  and C.ComCode = " + CFL.Q(G.ComCode) + @" 
  and A.SiteCode = " + CFL.Q(G.SiteCode) + @" 
  and B.SiteCode = " + CFL.Q(G.SiteCode) + @" 
  and C.SiteCode = " + CFL.Q(G.SiteCode) + @" 

  and A.DefaultDept =  B.DeptCode  
  and C.SiteCode = A.SiteCode ";

        if(ddlSiteCode.Text != "")
        {
            strWhereClause += @" 
  And A.SiteCode = " + CFL.Q(CFL.GetStr(ddlSiteCode.Value));
        }

        //if (txtDeptCode.Text != "")
        //{
        //    strWhereClause += " And B.DeptCode = " + CFL.Q(txtDeptCode.Text);
        //}
        //if(txtDeptName.Text != "")
        //{
        //    strWhereClause += " And B.DeptName Like " + CFL.Q("%" + txtDeptName.Text + "%");
        //}
        //if(txtEmpCode.Text != "")
        //{
        //    strWhereClause += " And A.EmpCode = " + CFL.Q(txtEmpCode.Text);
        //}
        //if(txtEmpName.Text != "")
        //{
        //    strWhereClause += " And A.EmpName Like " + CFL.Q("%" + txtEmpName.Text + "%");
        //}

        DataSet ds = ObjS.Grid(G.D, strWhereClause);
        Grid01.DataSource = ds;
    }

    protected void Grid01_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMsg);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
        gv.JSProperties.Add("cp_ret_job", strJob);
    }

    protected void Grid01_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
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
            case "SELECT":
                JobSelect();
                break;
            case "SAVE":
                JobSave();
                break;
            case "DELETE":
                JobDelete();
                break;
            default:
                break;
        }
    }

    void JobSelect()
    {
        DataSet ds = new DataSet();

        string strSiteCode = CFL.GetStr(hidField["SiteCode"]);
        string strEmpCode = CFL.GetStr(hidField["EmpCode"]);

        ds = ObjS.Load_OM034(G.D, strEmpCode, strSiteCode);

        hidField.Clear();
        hidField["job"] = "SELECT";

        if (ds.Tables[0].Rows.Count > 0)
        {
            hidField["status"] = "Y";
            hidField["msg"] = "";

            //값넣기
            CFL.setDataSetToDevHiddenField(ds, hidField);
        }
        else
        {
            hidField["status"] = "N";
            hidField["msg"] = CFL.GetStr(hidLang["M00162"]);
        }
    }

    private bool IsNumeric(string value)
    {
        foreach (char _char in value)
        {
            if (!Char.IsNumber(_char))
                return false;
        }
        return true;
    }

    void JobSave()
    {
        string strEmpBDate = CFL.GetStr(hidField["EmpBDate"]);
        hidField.Clear();
        hidField["job"] = "SAVE";

        string strTelNo = txtTelNo.Text;
        string strEmail = txtMailID.Text;
        string strJobWork = CFL.GetStr(cbJob.Value);

        if(txtEmpCode2.Text == "")
        {
            hidField["status"] = "N";
            hidField["msg"] = CFL.GetStr(hidLang["M00294"]);
            return;
        }
        
        
        if(SaveMode)
        {
            ArrayList alData = ObjA.set_SaveChk(G.D, txtEmpCode2.Text.Trim(), "", "");

            if ((int)alData[3] > 0)
            {
                hidField["status"] = "N";
                hidField["msg"] = CFL.GetStr(hidLang["M00295"]) + "( " + alData[4] + " / " + alData[5] + " )";
                return;
            }


            if (strTelNo != "")
            {
                ArrayList alData2 = ObjA.set_SaveChk(G.D, "", strTelNo, "");

                if ((int)alData2[3] > 0)
                {
                    hidField["status"] = "N";
                    hidField["msg"] = CFL.GetStr(hidLang["M00297"]) + "( " + alData2[4] + " / " + alData2[5] + " )";
                    return;
                }
            }

            if (strEmail != "")
            {
                ArrayList alData2 = ObjA.set_SaveChk(G.D, "", "", strEmail);

                if ((int)alData2[3] > 0)
                {
                    hidField["status"] = "N";
                    hidField["msg"] = CFL.GetStr(hidLang["M00299"]) + "( " + alData2[4] + " / " + alData2[5] + " )";
                    return;
                }
            }
        }
        
        string strSiteCode = CFL.GetStr(ddlSiteName.Value);
        
        // 전화번호 필수 체크
        /*
        if(strTelNo == "")
        {
            hidField["status"] = "N";
            hidField["msg"] = CFL.GetStr(hidLang["M00300"]);
            return;
        }
        */

        //if(IsNumeric(strTelNo.Substring(0, 3)) == false)
        //{
        //    hidField["status"] = "N";
        //    hidField["msg"] = "전화번호 앞 번호 3자리를 숫자로입력하시지 않았습니다.";
        //    return;
        //}

        string strInoutAdmin = chkInoutAdmin.Checked ? "Y" : "N";
        
        ArrayList alSave = ObjA.Save(G.D, SaveMode, txtEmpCode2.Text, txtEmpName2.Text, strSiteCode, txtDeptCode2.Text, strEmpBDate, "", strEmail
                                   , txtTelNo.Text, strInoutAdmin, txtHourCost.Text, strJobWork);


        if (alSave[0].ToString() != "00")
        {
            hidField["status"] = "N";
            hidField["msg"] = alSave[1].ToString();
        }
        else
        {
            hidField["status"] = "Y";
            hidField["msg"] = SaveMode ? CFL.GetStr(hidLang["M00002"]) : CFL.GetStr(hidLang["M00228"]);
        }
    }

    void JobDelete()
    {
        hidField.Clear();
        hidField["job"] = "DELETE";

        string strEmpCode = txtEmpCode2.Text.Trim();
        
        ArrayList alSave = ObjA.Delete(G.D, strEmpCode);
        if (alSave[0].ToString() != "00")
        {
            hidField["status"] = "N";
            hidField["msg"] = alSave[1].ToString();
        }
        else
        {
            hidField["status"] = "Y";
            hidField["msg"] = "삭제 되었습니다.";
        }
    }

    protected void ucPhoto_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {

    }
}