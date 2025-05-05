using SMART;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMART.COMMON.DS;
using DevExpress.Web;

public partial class Common_Forms_CommonPopup : PageBase
{
    GObj G = new GObj();

    string strOutMessage = "";
    string strOutFlag = "";
    DataSet ds = new DataSet();

    CommonCodeClass cm = new CommonCodeClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();
        G = ((GObj)Session["G"]);
        if (!IsPostBack)
        {
            if (Request["Action"] == "Load")
            {
                txtGubun.Text = CFL.GetStr(Request["Gubun"]).ToUpper().Replace("NULL", "");
                txtWhere.Text = CFL.GetStr(Request["W"]);
                txtCode.Text = CFL.GetStr(Request["Code"]).ToUpper().Replace("NULL", "");
                txtName.Text = CFL.GetStr(Request["Name"]).ToUpper().Replace("NULL", "");
                txtCodeID.Text = CFL.GetStr(Request["CodeID"]).ToUpper().Replace("NULL", "");
                txtCondition.Text = CFL.GetStr(Request["Condition"]);
                if (CFL.GetStr(Request["Title"]) != "")
                {
                    lblTitle.ClientVisible = true;
                    lblTitle.Text = CFL.GetStr(Request["Title"]);
                }
                //키필드 설정
                Grid.KeyFieldName = txtCondition.Text.Replace('/', ';');
                LangLoad();
            }
            Grid.ClearSort();
            Grid.DataBind();
        }
        
    }

    void LangLoad()
    {
        string[] sarCode = new string[]
        {
            "L00723", "L00724", "L01413", "L01405", "L01428", "L02426", "L02427", "L01364", "L01365"
            , "L02557", "L02558", "L00012", "L02731", "L02733", "L02734", "L02758", "L02744"
            , "L02748", "L02749", "L02750", "L02560", "L02745", "L02746", "L00463", "L00464"
            , "L02768", "L00023", "L01098", "L00024", "L02769", "L02770", "L00429", "L00073"
            , "L02771", "L02772", "L02773", "L01027", "L02452", "L02774", "L00366", "L00529"
            , "L00291", "L00292", "L00310", "L00309", "L00294", "L01332", "L00415", "L01376"
            , "L02775", "L02776", "L02777", "L00660", "L00233", "L00724", "L02778", "L00460"
            , "L00386", "L02779", "L02780", "L00649", "L02781", "L01267", "L00048", "L00049"
            , "L00020", "L02897", "L02892", "L02898", "L02538", "L02669", "L02899", "L02900"
            , "L03042", "L03043", "L01331", "L02522", "L03076", "L03077", "L03180", "L03108"
            , "L03197", "L03198", "L03206", "L03207", "L01109", "L02635", "L03269", "L01154"
            , "L03445", "L03451", "L03500", "L01378", "L03521", "L03579", "L03546", "L03547"
            , "L03588", "L02743", "L03670", "L03671", "L03690", "L03691", "L03716", "L03717"
            , "L00417", "L03719", "L03720", "L03677", "L03328", "L00605", "L02890", "L03800"
            , "L03801", "L03802", "L03803", "L03804", "L03805", "L03779", "L03818", "L03819"
            , "L03820", "L03821", "L03822", "L03823", "L00515", "L00408", "L01402"
            , "L03790", "L03791", "L00405", "L00403", "L03837", "L03836", "L03771", "L00385", "L00386"
            , "L00293", "L01261", "L03099", "L03100", "L02405", "L04319", "L02677", "L03713", "L03951", "L04516"
            , "L04666", "L00742", "L00050", "L00051", "L02021", "L04891", "L04223", "L04224", "L02670"
            , "L04489", "L00242", "L04322", "L04930", "L02890", "L04928", "L04931", "L02890", "L04947"
            , "L04948", "L00950", "L05034", "L00426", "L00427", "L05095", "L01829", "L01830", "L00394"
            , "L05389", "L05390", "L05392", "L05387", "L02699"
        };

        SMART.COMMON.DS.MessageMaster mm = new SMART.COMMON.DS.MessageMaster();
        DataSet ds = mm.GetFormLangDataLoad(G.D, G.LangID, sarCode);

        //초기화 후 히든필드에 담기
        hidLang.Clear();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            hidLang[CFL.GetStr(ds.Tables[0].Rows[i]["MCode"])] = CFL.GetStr(ds.Tables[0].Rows[i]["Message"]);
        }

    }


    protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        Grid.ClearSort();
        Grid.DataBind();
    }

    protected void Grid_DataBinding(object sender, EventArgs e)
    {

        switch (txtGubun.Text.Trim())
        {            
            case "CODE":
                ds = this.SettingCodeMaster();
                break;
            case "USER":
                ds = this.SettingUserMaster();
                break;
            case "DEPT":
                ds = this.SettingDeptMaster();
                break;
            case "IGM":
                ds = this.SettingItemGroupMaster();
                break;
                      
            case "REG": // Region
                ds = this.SettingRegion();
                break;
            
            case "NA": //국가
                ds = this.SettingNation();
                break;

            case "CC": // Region 복사
                ds = this.SettingCostCenter();
                break;
        }

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
                e.DisplayText = (e.VisibleIndex + 1).ToString();
            }
        }
    }

    #region 공통코드
    DataSet SettingCodeMaster()
    {
        string strCodeID = txtCodeID.Text.Trim();

        string strCapt_CodeCode = "";
        string strCapt_CodeName = "";
        string strCapt_Factor1 = "";

        switch (strCodeID)
        {
            case "OMJUYAGUBUN":
                strCapt_CodeCode = CFL.GetStr(hidLang["L02769"]); //구분코드";
                strCapt_CodeName = CFL.GetStr(hidLang["L02770"]); //근무구분";
                strCapt_Factor1 = CFL.GetStr(hidLang["L00429"]); //근무시간";
                break;
            case "MAKER":
                strCapt_CodeCode = CFL.GetStr(hidLang["L00073"]); //코드
                strCapt_CodeName = CFL.GetStr(hidLang["L01402"]); //제조사
                break;
            default:
                strCapt_CodeCode = CFL.GetStr(hidLang["L00073"]); //코드";
                strCapt_CodeName = CFL.GetStr(hidLang["L02771"]); //코드명";
                strCapt_Factor1 = CFL.GetStr(hidLang["L02772"]); //데이터";
                break;

        }
        if (Grid.Columns.Count <= 1)
        {
            //컬럼 세팅 이후 데이터 조회
            if (0 <= txtCondition.Text.IndexOf("CodeCode/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                //col.Caption = "코드";
                col.Caption = strCapt_CodeCode;
                col.FieldName = "CodeCode";
                col.Width = new Unit(40, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("CodeName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                //col.Caption = "코드명";
                col.Caption = strCapt_CodeName;
                col.FieldName = "CodeName";
                col.Width = new Unit(60, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("Factor1/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                //col.Caption = "코드명";
                col.Caption = strCapt_Factor1;
                col.FieldName = "Factor1";
                col.Width = new Unit(60, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
        }
        DataSet ds = new DataSet();
        string strWhereClause = "";

        if (txtCode.Text.Trim() != "")
        {
            strWhereClause += @" 
AND A.CodeCode LIKE (" + CFL.Q("%" + txtCode.Text.Trim() + "%") + ")";
        }
        if (txtName.Text.Trim() != "")
        {
            strWhereClause += @" 
AND A.CodeName LIKE (" + CFL.Q("%" + txtName.Text.Trim() + "%") + ")";
        }
        if (txtCodeID.Text.Trim() != "")
        {
            strWhereClause += @"
AND A.CodeID = " + CFL.Q(strCodeID);
        }
        if (txtWhere.Text.Trim() != "")
        {
            strWhereClause += txtWhere.Text.Trim();
        }

        ds = cm.GetCodeMaster(G.D, strWhereClause, txtCondition.Text);

        return ds;
    }
    #endregion;

   

    

   

    #region 유저정보
    DataSet SettingUserMaster()
    {
        if (Grid.Columns.Count <= 1)
        {
            //컬럼 세팅 이후 데이터 조회
            if (0 <= txtCondition.Text.IndexOf("UserID/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L02773"]); //"아이디";
                col.FieldName = "UserID";
                col.Width = new Unit(40, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("UserName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L01027"]); //"사용자명";
                col.FieldName = "UserName";
                col.Width = new Unit(60, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
        }
        DataSet ds = new DataSet();
        string strWhereClause = "";

        if (txtCode.Text.Trim() != "")
        {
            strWhereClause += @"
AND UserID LIKE " + CFL.Q(txtCode.Text.Trim() + "%") + @" ";
        }
        if (txtName.Text.Trim() != "")
        {
            strWhereClause += @"
AND UserName LIKE " + CFL.Q(txtName.Text + "%") + @" ";
        }
        if (txtWhere.Text.Trim() != "")
        {
            strWhereClause += txtWhere.Text.Trim();
        }

        ds = cm.GetUserMaster(G.D, strWhereClause, txtCondition.Text);

        return ds;
    }
    #endregion

    #region 부서정보
    DataSet SettingDeptMaster()
    {
        if (Grid.Columns.Count <= 1)
        {
            //컬럼 세팅 이후 데이터 조회
            if (0 <= txtCondition.Text.IndexOf("DeptCode/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L01098"]); //부서코드";
                col.FieldName = "DeptCode";
                col.Width = new Unit(40, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("DeptName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00024"]); //부서명";
                col.FieldName = "DeptName";
                col.Width = new Unit(60, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
        }
        DataSet ds = new DataSet();
        string strWhereClause = "";

        if (txtCode.Text.Trim() != "")
        {
            strWhereClause += @"
AND A.DeptCode LIKE " + CFL.Q(txtCode.Text.Trim() + "%") + @" ";
        }
        if (txtName.Text.Trim() != "")
        {
            strWhereClause += @"
AND A.DeptName LIKE " + CFL.Q("%" + txtName.Text.Trim() + "%") + @" ";
        }
        if (txtWhere.Text != "")
        {
            strWhereClause += txtWhere.Text;
        }

        ds = cm.GetDeptMaster(G.D, strWhereClause, txtCondition.Text);

        return ds;
    }
    #endregion

    #region 품목군정보
    DataSet SettingItemGroupMaster()
    {
        if (Grid.Columns.Count <= 1)
        {
            //컬럼 세팅 이후 데이터 조회
            if (0 <= txtCondition.Text.IndexOf("ItemGroup/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L02452"]); //품목군코드";
                col.FieldName = "ItemGroup";
                col.Width = new Unit(40, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("GroupName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L02774"]); //품목군명";
                col.FieldName = "GroupName";
                col.Width = new Unit(60, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
        }
        DataSet ds = new DataSet();
        string strWhereClause = "";

        if (txtCode.Text.Trim() != "")
        {
            strWhereClause += @" 
And ItemGroup Like " + CFL.Q("%" + txtCode.Text + "%");
        }
        if (txtName.Text.Trim() != "")
        {
            strWhereClause += @"
 And GroupName Like " + CFL.Q("%" + txtName.Text + "%");
        }
        if (txtWhere.Text != "")
        {
            strWhereClause += txtWhere.Text;
        }

        ds = cm.GetItemGroupMaster(G.D, strWhereClause, txtCondition.Text);

        return ds;
    }
    #endregion

    #region Region
    DataSet SettingRegion()
    {
        if (Grid.Columns.Count <= 1)
        {
            //컬럼 세팅 이후 데이터 조회
            //컬럼 세팅 이후 데이터 조회
            if (0 <= txtCondition.Text.IndexOf("RegionCode/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00048"]); //"지역코드";
                col.FieldName = "RegionCode";
                col.Width = new Unit(100, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("RegionName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00049"]); //"지역명";
                col.FieldName = "RegionName";
                col.Width = new Unit(100, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
        }
        DataSet ds = new DataSet();
        string strWhereClause = "";

        strWhereClause += @"
And ComCode = " + CFL.Q(G.ComCode);
        if (txtCode.Text.Trim() != "")
        {
            strWhereClause += @"
AND RegionCode LIKE " + CFL.Q(txtCode.Text.Trim() + "%") + @" ";
        }
        if (txtName.Text.Trim() != "")
        {
            strWhereClause += @"
AND RegionName LIKE " + CFL.Q("%" + txtName.Text.Trim() + "%") + @" ";
        }

        ds = cm.GetRegionMaster(G.D, strWhereClause, txtCondition.Text);

        return ds;
    }
    #endregion 

    #region TransRegion
    DataSet SettingTransRegion()
    {
        if (Grid.Columns.Count <= 1)
        {
            //컬럼 세팅 이후 데이터 조회
            //컬럼 세팅 이후 데이터 조회
            if (0 <= txtCondition.Text.IndexOf("RegionCode/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00048"]); //"지역코드";
                col.FieldName = "RegionCode";
                col.Width = new Unit(100, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("RegionName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00049"]); //"지역명";
                col.FieldName = "RegionName";
                col.Width = new Unit(100, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
        }
        DataSet ds = new DataSet();
        string strWhereClause = "";

        strWhereClause += @"
ComCode = " + CFL.Q(G.ComCode);

        if (txtCode.Text.Trim() != "")
        {
            strWhereClause += @"
AND RegionCode LIKE " + CFL.Q(txtCode.Text.Trim() + "%") + @" ";
        }
        if (txtName.Text.Trim() != "")
        {
            strWhereClause += @"
AND RegionName LIKE " + CFL.Q("%" + txtName.Text.Trim() + "%") + @" ";
        }

        ds = cm.GetTransRegionMaster(G.D, strWhereClause, txtCondition.Text);

        return ds;
    }
    #endregion 

    

    

    #region 국가코드
    DataSet SettingNation()
    {
        if (Grid.Columns.Count <= 1)
        {
            //컬럼 세팅 이후 데이터 조회
            //컬럼 세팅 이후 데이터 조회
            if (0 <= txtCondition.Text.IndexOf("NationCode/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00050"]); //  국가코드
                col.FieldName = "NationCode";
                col.Width = new Unit(50, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("NationName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00051"]); // 국가명
                col.FieldName = "NationName";
                col.Width = new Unit(50, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
        }
        DataSet ds = new DataSet();
        string strWhereClause = "";

        strWhereClause += @"";
        if (txtCode.Text.Trim() != "")
        {
            strWhereClause += @"
AND NationCode LIKE " + CFL.Q(txtCode.Text.Trim() + "%") + @" ";
        }
        if (txtName.Text.Trim() != "")
        {
            strWhereClause += @"
AND NationName LIKE " + CFL.Q(txtName.Text.Trim() + "%") + @" ";
        }

        if (txtWhere.Text.Trim() != "")
        {
            strWhereClause += txtWhere.Text.Trim();
        }

        ds = cm.GetNationMaster(G.D, strWhereClause, txtCondition.Text);

        return ds;
    }
    #endregion

    #region MngEmpMaster
    DataSet SettingMngEmpMaster()
    {
        if (Grid.Columns.Count <= 1)
        {
            //컬럼 세팅 이후 데이터 조회
            if (0 <= txtCondition.Text.IndexOf("MngGroup/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00426"]); // "그룹코드";
                col.FieldName = "MngGroup";
                col.Width = new Unit(60, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("MngName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00427"]); // "그룹명";
                col.FieldName = "MngName";
                col.Width = new Unit(60, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("EmpCode/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L02768"]); //사원번호";
                col.FieldName = "EmpCode";
                col.Width = new Unit(40, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("EmpName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = CFL.GetStr(hidLang["L00023"]); //사원명";
                col.FieldName = "EmpName";
                col.Width = new Unit(60, UnitType.Percentage);
                Grid.Columns.Add(col);
            }

        }
        DataSet ds = new DataSet();
        string strWhereClause = "";

        if (txtCode.Text.Trim() != "")
        {
            strWhereClause += @"
  AND A.EmpCode LIKE " + CFL.Q(txtCode.Text + "%") + @" ";
        }
        if (txtName.Text.Trim() != "")
        {
            strWhereClause += @"
  AND C.EmpName LIKE " + CFL.Q(txtName.Text + "%") + @" ";
        }
        if (txtWhere.Text.Trim() != "")
        {
            strWhereClause += @"
" + txtWhere.Text.Trim();
        }

        ds = cm.GetMngEmpMaster(G.D, strWhereClause, txtCondition.Text);

        return ds;
    }
    #endregion


    #region CostCenter
    DataSet SettingCostCenter()
    {
        if (Grid.Columns.Count <= 1)
        {
            //컬럼 세팅 이후 데이터 조회
            //컬럼 세팅 이후 데이터 조회
            if (0 <= txtCondition.Text.IndexOf("CcCode/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = "코스트센터"; //"지역코드";
                col.FieldName = "CcCode";
                col.Width = new Unit(100, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
            if (0 <= txtCondition.Text.IndexOf("CcName/"))
            {
                GridViewDataColumn col = new GridViewDataColumn();
                col.Caption = "코스트센터명";
                col.FieldName = "CcName";
                col.Width = new Unit(100, UnitType.Percentage);
                Grid.Columns.Add(col);
            }
        }
        DataSet ds = new DataSet();
        string strWhereClause = "";

        strWhereClause += @"
ComCode = " + CFL.Q(G.ComCode);
        if (txtCode.Text.Trim() != "")
        {
            strWhereClause += @"
AND CcCode LIKE " + CFL.Q(txtCode.Text.Trim() + "%") + @" ";
        }
        if (txtName.Text.Trim() != "")
        {
            strWhereClause += @"
AND CcName LIKE " + CFL.Q("%" + txtName.Text.Trim() + "%") + @" ";
        }

        ds = cm.Popup_CcMaster(G.D, strWhereClause, txtCondition.Text);

        return ds;
    }
    #endregion

}
