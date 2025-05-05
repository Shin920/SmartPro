using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using SMART;
using SMART.COMMON.DS;
using DevExpress.Web;
using System.IO;

public partial class MNG_OM_Master_CsMaster_OM044_OM044 : PageBase
{
    GObj G = new GObj();

    string strOutFlag = "";
    string strOutMsg = "";
    string strJob = "";

    string strOutFlag_Emp = "";
    string strOutMsg_Emp = "";
    string strJob_Emp = "";

    string strOutFlag_Addr = "";
    string strOutMsg_Addr = "";
    string strJob_Addr = "";
    static DataSet dsGridS1 { get; set; }

    SMART.OM.DA.OM044 daObj = new SMART.OM.DA.OM044();
    SMART.OM.DS.OM044 dsObj = new SMART.OM.DS.OM044();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        // Authority Check
        if (!G.GetPermission("OM044", GObj.SecKind.FormOpen))
            Response.Redirect(CFL.GetErrorURL("Close", CFL.CRS("01", "Security", G.LangID), CFL.CRS("02", "Security", G.LangID)));

        txtPermition_I.Text = G.GetPermission("OM044", GObj.SecKind.Insert) ? "Y" : "N";
        txtPermition_D.Text = G.GetPermission("OM044", GObj.SecKind.Delete) ? "Y" : "N";

        if (!IsPostBack)
        {
            //
            SMART.COMMON.DS.CommonCodeClass cm = new SMART.COMMON.DS.CommonCodeClass();

            DataSet dsCsType = cm.GetCodeMaster(G.D, " AND CodeID = 'CsType' AND ComCode = " + CFL.Q(G.ComCode) + " AND CodeUse = 'Y' ", "CodeCode/TypeCode/CodeName/");
            cmbCsType.DataSource = dsCsType;
            cmbCsType.DataBind();
            cmbCsType.Items.Insert(0, new ListEditItem("", ""));

            cmbCsType_S.DataSource = dsCsType;
            cmbCsType_S.DataBind();
            cmbCsType_S.Items.Insert(0, new ListEditItem("", ""));

            DataSet dsCsClass1 = cm.GetCodeMaster(G.D, " AND CodeID = 'CsClass1' AND ComCode = " + CFL.Q(G.ComCode) + " AND CodeUse = 'Y' ", "CodeCode/CodeName/");
            cmbCsClass1.DataSource = dsCsClass1;
            cmbCsClass1.DataBind();
            cmbCsClass1.Items.Insert(0, new ListEditItem("", ""));

            cmbCsClass2.DataSource = cm.GetCodeMaster(G.D, " AND CodeID = 'CsClass2' AND ComCode = " + CFL.Q(G.ComCode) + " AND CodeUse = 'Y' ", "CodeCode/CodeName/");
            cmbCsClass2.DataBind();
            cmbCsClass2.Items.Insert(0, new ListEditItem("", ""));

            cmbCsGrade.DataSource = cm.GetCodeMaster(G.D, " AND CodeID = 'CsGrade' AND ComCode = " + CFL.Q(G.ComCode) + " AND CodeUse = 'Y' ", "CodeCode/CodeName/");
            cmbCsGrade.DataBind();
            cmbCsGrade.Items.Insert(0, new ListEditItem("", ""));

            cmbCreditGrade.DataSource = cm.GetCodeMaster(G.D, " AND CodeID = 'CsCreditGrade' AND ComCode = " + CFL.Q(G.ComCode) + " AND CodeUse = 'Y' ", "CodeCode/CodeName/");
            cmbCreditGrade.DataBind();
            cmbCreditGrade.Items.Insert(0, new ListEditItem("", ""));

            cmbIncoTerm.DataSource = cm.GetCodeMaster(G.D, " AND CodeID = 'IncoTerm' AND ComCode = " + CFL.Q(G.ComCode) + " AND CodeUse = 'Y' ", "CodeCode/CodeName/");
            cmbIncoTerm.DataBind();
            cmbIncoTerm.Items.Insert(0, new ListEditItem("", ""));

            cmbTransCode.DataSource = cm.GetCodeMaster(G.D, " AND CodeID = 'TransCode' AND ComCode = " + CFL.Q(G.ComCode) + " AND CodeUse = 'Y' ", "CodeCode/CodeName/");
            cmbTransCode.DataBind();
            cmbTransCode.Items.Insert(0, new ListEditItem("", ""));

            cmbTaxCodeAr.DataSource = cm.GetTaxMaster(G.D, " AND A.ArApGubun = 'AR' AND A.ComCode = " + CFL.Q(G.ComCode), "TaxCode/TaxName/");
            cmbTaxCodeAr.DataBind();
            cmbTaxCodeAr.Items.Insert(0, new ListEditItem("", ""));

            cmbTaxCodeAp.DataSource = cm.GetTaxMaster(G.D, " AND A.ArApGubun = 'AP' AND A.ComCode = " + CFL.Q(G.ComCode), "TaxCode/TaxName/");
            cmbTaxCodeAp.DataBind();
            cmbTaxCodeAp.Items.Insert(0, new ListEditItem("", ""));

            cmbPayConditionAr.DataSource = cm.GetrpPayCondition(G.D, " AND ComCode = " + CFL.Q(G.ComCode) + " and ArApGubun = 'AR' ", "PayCondCode/PayCondName/");
            cmbPayConditionAr.DataBind();
            cmbPayConditionAr.Items.Insert(0, new ListEditItem("", ""));

            cmbPayConditionAp.DataSource = cm.GetrpPayCondition(G.D, " AND ComCode = " + CFL.Q(G.ComCode) + " and ArApGubun = 'AP' ", "PayCondCode/PayCondName/");
            cmbPayConditionAp.DataBind();
            cmbPayConditionAp.Items.Insert(0, new ListEditItem("", ""));

            cmbCurrCode.DataSource = cm.GetCurrencyMaster(G.D, " AND ComCode = " + CFL.Q(G.ComCode), "CurrCode/CurrName/");
            cmbCurrCode.DataBind();
            cmbCurrCode.Items.Insert(0, new ListEditItem("", ""));
                        
            cbSendMethod.DataSource = cm.GetTypeMaster(G.D, " And LangID = " + CFL.Q(G.LangID) + " and TypeID ='SendMethod'", "TypeCode/TypeName/");
            cbSendMethod.DataBind();
            cbSendMethod.Items.Insert(0, new ListEditItem("", ""));

            //메시지, 라벨 텍스트 정보가져오기.
            LangLoad();
        }
    }

    //다국어 메시지 가져오기.
    void LangLoad()
    {
        string[] sarCode = new string[]
        {
            "L00001", "L00002", "L00003", "L00007", "L00009", "L00010", "L00011", "L00014",
            "L00027", "L00056", "L00057", "L00059", "L00060", "L00061", "L00062", "L00066",
            "L00067", "L00068", "L00072", "L00100", "L00234", "L00235", "L00661", "L00663",
            "L01243", "L02370", "L04045", "L01739", "L02396", "L01475", "L01243", "L04038",
            "L04062", "L02402", "L03795", "L01477", "L03097", "L03107", "L01882",
            "L02829", "L02830", "L04039", "L04040", "L04061", "L01888", "L01889", "L04041",
            "L04042", "L01892", "L01893", "L04043", "L04044", "L03105", "L03106", "L01898",
            "L04046", "L04047", "L04049", "L04048", "L02990", "L04050", "L04051", "L04052",
            "L04053", "L04054", "L04055", "L04056", "L04058", "L04059", "L04057", "L01454", 
            "L04060", "L04039", "L02541", "L04394", "L00350", 

            "M00623", "M00086", "M00002", "M00005", "M00003", "M00182", "M00181", "M00150", 
            "M01012", "M01015", "M01016", "M01017", "M01018", "M01019", "M01020"
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
        lblCsType_S.Text = CFL.GetStr(hidLang["L01475"]);
        lblCsCode_S.Text = CFL.GetStr(hidLang["L01243"]);
        lblCsUse_S.Text = CFL.GetStr(hidLang["L00009"]);

        lblCsCode.Text = CFL.GetStr(hidLang["L01243"]);
        lblCsName.Text = CFL.GetStr(hidLang["L02370"]);
        lblCsNameFull.Text = CFL.GetStr(hidLang["L04038"]);
        lblCsType.Text = CFL.GetStr(hidLang["L01475"]);
        lblCsGubun.Text = CFL.GetStr(hidLang["L00068"]);
        lblCompany.Text = CFL.GetStr(hidLang["L01477"]);
        lblDomestic.Text = CFL.GetStr(hidLang["L01882"]);
        lblNationCode.Text = CFL.GetStr(hidLang["L04039"]);
        lblRegionCode.Text = CFL.GetStr(hidLang["L04040"]);
        lblAdress.Text = CFL.GetStr(hidLang["L00661"]);    
        lblCsTel.Text = CFL.GetStr(hidLang["L00027"]);
        lblCsFax.Text = CFL.GetStr(hidLang["L00059"]);
        lblCsUrl.Text = CFL.GetStr(hidLang["L00060"]);
        lblCsEmail.Text = CFL.GetStr(hidLang["L00061"]);
        lblRegNo.Text = CFL.GetStr(hidLang["L00062"]);
        lblCsIndustry.Text = CFL.GetStr(hidLang["L00663"]);
        lblCsItems.Text = CFL.GetStr(hidLang["L02396"]);
        lblParentCs.Text = CFL.GetStr(hidLang["L04041"]);
        lblCsChief.Text = CFL.GetStr(hidLang["L04042"]);    
        lblTaxCodeAr.Text = CFL.GetStr(hidLang["L01892"]);
        lblTaxCodeAp.Text = CFL.GetStr(hidLang["L01893"]);
        lblCsClass1.Text = CFL.GetStr(hidLang["L00066"]);
        lblCsClass2.Text = CFL.GetStr(hidLang["L00067"]);        
        lblCsDescr.Text = CFL.GetStr(hidLang["L00011"]);

        lblInitial.Text = CFL.GetStr(hidLang["L00007"]);

        lblPayConditionAr.Text = CFL.GetStr(hidLang["L04043"]);
        lblPayConditionAp.Text = CFL.GetStr(hidLang["L04044"]);
        lblCsGrade.Text = CFL.GetStr(hidLang["L00056"]);
        lblCreditGrade.Text = CFL.GetStr(hidLang["L00057"]);
        lblBankCode.Text = CFL.GetStr(hidLang["L03105"]);
        lblAccountNo.Text = CFL.GetStr(hidLang["L03106"]);
        lblCurrCode.Text = CFL.GetStr(hidLang["L01898"]);
        lblIncoTerm.Text = CFL.GetStr(hidLang["L04045"]);
        lblLeadTime.Text = CFL.GetStr(hidLang["L04046"]);
        lblTransCode.Text = CFL.GetStr(hidLang["L04047"]);
        lblPackingLogo.Text = CFL.GetStr(hidLang["L04048"]);
        lblFilePackingLogo.Text = CFL.GetStr(hidLang["L04049"]);
        lblSendMethod.Text = CFL.GetStr(hidLang["L02990"]);
        lblTranTelNo.Text = CFL.GetStr(hidLang["L04050"]);
        lblTranEmail.Text = CFL.GetStr(hidLang["L04051"]);
        lblTranFax.Text = CFL.GetStr(hidLang["L04052"]);
        lblExtraPlan1.Text = CFL.GetStr(hidLang["L04053"]);
        lblExtraPlan2.Text = CFL.GetStr(hidLang["L04054"]);
        lblExtraPlan3.Text = CFL.GetStr(hidLang["L04055"]);
        lblTrus_Use.Text = CFL.GetStr(hidLang["L04056"]);
        lblTrus_1_2.Text = CFL.GetStr(hidLang["L04057"]);
        lblEmpCode.Text = CFL.GetStr(hidLang["L01454"]); //영업담당자

        cmbCsUse_S.Items.FindByValue("Y").Text = CFL.GetStr(hidLang["L00234"]);
        cmbCsUse_S.Items.FindByValue("").Text = CFL.GetStr(hidLang["L00235"]);
        cmbCsUse_S.Items.FindByValue("N").Text = CFL.GetStr(hidLang["L04060"]);

        rdoCompany.Items.FindByValue("Y").Text = CFL.GetStr(hidLang["L03097"]);
        rdoCompany.Items.FindByValue("N").Text = CFL.GetStr(hidLang["L03107"]);

        rdoDomestic.Items.FindByValue("N").Text = CFL.GetStr(hidLang["L02829"]);
        rdoDomestic.Items.FindByValue("Y").Text = CFL.GetStr(hidLang["L02830"]);
      
        chkCsUse.Text = CFL.GetStr(hidLang["L00010"]);

        chkArCheck.Text = CFL.GetStr(hidLang["L00100"]);
        chkApCheck.Text = CFL.GetStr(hidLang["L02402"]);
        chkOutCheck.Text = CFL.GetStr(hidLang["L03795"]);
        
        chkTrus_Use.Text = CFL.GetStr(hidLang["L00010"]);
        chkTrus_1.Text = CFL.GetStr(hidLang["L04058"]);
        chkTrus_2.Text = CFL.GetStr(hidLang["L04059"]);

        Grid.Columns["CsCode"].Caption = CFL.GetStr(hidLang["L01243"]);
        Grid.Columns["CsName"].Caption = CFL.GetStr(hidLang["L02370"]);

        btnNew.Text = CFL.GetStr(hidLang["L00001"]);
        btnSearch.Text = CFL.GetStr(hidLang["L00014"]);
        btnNew_S.Text = CFL.GetStr(hidLang["L00001"]);
        btnSave.Text = CFL.GetStr(hidLang["L00002"]);
        btnDelete.Text = CFL.GetStr(hidLang["L00003"]);
        btnAdress.Text = CFL.GetStr(hidLang["L04061"]);
    
        tabMain.TabPages.FindByName("tabPage1").Text = CFL.GetStr(hidLang["L00072"]);
        tabMain.TabPages.FindByName("tabPage2").Text = CFL.GetStr(hidLang["L04062"]);

        lblEmp.Text= CFL.GetStr(hidLang["L04394"]); //담당자 정보
        GridS1.Columns["EmpName"].Caption = CFL.GetStr(hidLang["L02541"]); //업체담당자
        GridS1.Columns["EmpTell"].Caption = CFL.GetStr(hidLang["L00027"]); //전화번호
        GridS1.Columns["EmpFaxNo"].Caption = CFL.GetStr(hidLang["L00059"]); //팩스번호
        GridS1.Columns["emailID"].Caption = "E-Maill";
        GridS1.Columns["EmpBego"].Caption = CFL.GetStr(hidLang["L00350"]); //비고
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

    protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if(e.Parameters == "NEW")
        {
            strJob = "NEW";
            Grid.ClearSort();
            Grid.DataBind();
        }
        else
        {
            Grid.ClearSort();
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

        string strCsType = CFL.GetStr(cmbCsType_S.Value);
        string strCsCode = txtCsCode_S.Text.Trim();
        string strCsName = txtCsName_S.Text.Trim();
        string strCsUse = CFL.GetStr(cmbCsUse_S.Value);

        DataSet ds = dsObj.CsList(G.D, strCsType, strCsCode, strCsName, strCsUse);
        Grid.DataSource = ds;
    }

    protected void Grid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMsg);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
    }

    protected void Grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.Caption.ToUpper() == "NO")
        {
            if (e.VisibleIndex >= 0)
            {
                e.DisplayText = (e.VisibleIndex + 1).ToString();
            }
        }
    }

    protected void cmbRegionCode_Callback(object sender, CallbackEventArgsBase e)
    {
        string strDomesticCheck = CFL.GetStr(rdoDomestic.Value);

        if(strDomesticCheck == "N")
        {
            strDomesticCheck = "Y";
        }
        else
        {
            strDomesticCheck = "N";
        }

        if (strDomesticCheck != "")
        {
            SMART.COMMON.DS.CommonCodeClass cm = new SMART.COMMON.DS.CommonCodeClass();
            DataSet ds = cm.GetRegionMaster(G.D, " And ComCode = " + CFL.Q(G.ComCode) + @" AND DomesticCheck = " + CFL.Q(strDomesticCheck), "RegionCode/RegionName/");
            cmbRegionCode.DataSource = ds;
            cmbRegionCode.DataBind();
            cmbRegionCode.Items.Insert(0, new ListEditItem("", ""));
        }
    }

    protected void callBackPanel_Callback(object sender, CallbackEventArgsBase e)
    {
        string job = hidField["job"].ToString();

        switch (job)
        {
            case "SELECT":
                JobSelect();
                break;
            case "SAVECHECK":
                JobSaveCheck();
                break;
            case "SAVE":
                JobSave();
                break;
            case "DELETE":
                JobDelete();
                break;
        }
    }

    void JobSelect()
    {
        hidField.Clear();
        hidField["job"] = "SELECT";

        if (txtCsCode.Text.Trim() != "")
        {
            string strCsCode = txtCsCode.Text.Trim();
            DataSet ds = dsObj.sDataLoad_CsMAster(G.D, strCsCode);

            string strImg = "/SMARTPRO/UserImages/PackingLogo/" + strCsCode + "/Logo_" + strCsCode + ".bmp";
            

            if (ds.Tables[0].Rows.Count > 0)
            {
                hidField["status"] = "Y";
                if (File.Exists(Server.MapPath(strImg)))
                {
                    hidField["IMG_URL"] = strImg;
                }

                //히든 필드에 담기
                CFL.setDataSetToDevHiddenField(ds, hidField);
            }
            else
            {
                hidField["status"] = "N";
            }
        }
    }

    void JobSaveCheck()
    {
        hidField.Clear();
        hidField["job"] = "SAVECHECK";
        hidField["status"] = "Y";
        hidField["msg"] = "";

        if (txtRegNo.Text != "")
        {
            ArrayList alSaveChk = daObj.Get_SaveCheck(G.D, txtCsCode.Text, txtRegNo.Text, "");
            if ("01" == alSaveChk[0].ToString())
            {
                hidField["status"] = "N";
                hidField["msg"] = CFL.GetStr(hidLang["M01015"]); // "거래처와 사업자번호가 중복됩니다. 저장하실수 없습니다.";
                return;
            }

            alSaveChk = daObj.Get_SaveCheck(G.D, txtCsCode.Text, "", txtCsName.Text);

            // 같은 이름이 없으면 진짜 Save를 호출한다.
            if ("01" == alSaveChk[0].ToString())
            {
                hidField["status"] = "Y";
                hidField["msg"] = CFL.GetStr(hidLang["M01012"]); //"거래처와 명칭이 중복됩니다. 계속하시겠습니까?";
                return;
            }
        }
        else
        {
            ArrayList alSaveChk = daObj.Get_SaveCheck(G.D, txtCsCode.Text, "", txtCsName.Text);
            // 같은 이름이 없으면 진짜 Save를 호출한다.
            if ("01" == alSaveChk[0].ToString())
            {
                hidField["status"] = "Y";
                hidField["msg"] = CFL.GetStr(hidLang["M01012"]); //"거래처와 명칭이 중복됩니다. 계속하시겠습니까?";
                return;
            }
        }
    }

    void JobSave()
    {
        string strCsCode = txtCsCode.Text;
        string strCsName = txtCsName.Text;
        string strCsType = CFL.GetStr(cmbCsType.Value);
        string strCsGubun = CFL.GetStr(hidField["TypeCode"]);

        // string strCsGubun
        string strArCheck = chkArCheck.Checked ? "Y" : "N";
        string strApCheck = chkApCheck.Checked ? "Y" : "N";
        string strOutCheck = chkOutCheck.Checked ? "Y" : "N";
        
        string strCsNameFull = txtCsNameFull.Text;
        string strForeignCheck = CFL.GetStr(rdoDomestic.Value);
        string strComCheck = CFL.GetStr(rdoCompany.Value);
        string strCsUse = chkCsUse.Checked ? "Y" : "N";
        string strRegNo = txtRegNo.Text;
        string strNationCode = txtNationCode.Text;
        string strRegionCode = CFL.GetStr(cmbRegionCode.Value);
        string strParentCs = txtParentCs.Text;
        string strCsChief = txtCsChief.Text;
        string strCsIndustry = txtCsIndustry.Text;
        string strCsItems = txtCsItems.Text;
        string strCsAddress = txtCsAddress.Text;
        string strCsZip = txtCsZip.Text;

        // string strCsChief
        string strCsTel = txtCsTel.Text;
        string strCsFax = txtCsFax.Text;
        string strCsUrl = txtCsUrl.Text;
        string strCsEmail = txtCsEmail.Text;
        string strTaxCodeAr = CFL.GetStr(cmbTaxCodeAr.Value);
        string strTaxCodeAp = CFL.GetStr(cmbTaxCodeAp.Value);
        string strPayConditionAr = CFL.GetStr(cmbPayConditionAr.Value);
        string strPayConditionAp = CFL.GetStr(cmbPayConditionAp.Value);
        string strCurrCode = CFL.GetStr(cmbCurrCode.Value);
        string strCsGrade = CFL.GetStr(cmbCsGrade.Value);
        string strCreditGrade = CFL.GetStr(cmbCreditGrade.Value);
        string strIncoTerm = CFL.GetStr(cmbIncoTerm.Value);
        string strLeadTime = txtLeadTime.Text;
        string strTransCode = CFL.GetStr(cmbTransCode.Value);
        string strSendMethod = CFL.GetStr(cbSendMethod.Value);
        string strCsClass1 = CFL.GetStr(cmbCsClass1.Value);
        string strCsClass2 = CFL.GetStr(cmbCsClass2.Value);

        string strTranTelNo = txtTranTelNo.Text;
        string strTranFax = txtTranFax.Text;
        string strTranEmail = txtTranEmail.Text;

        string strBankCode = txtBankCode.Text;
        string strAccountNo = txtAccountNo.Text;
        string strCsDescr = txtCsDescr.Text;
        string strInitial = txtInitial.Text;

        string strEmpCOde = txtEmpCode.Text;

        hidField.Clear();
        hidField["job"] = "SAVE";
        hidField["status"] = "N";

        if (txtCsCode.Text.Trim() == "")
        {

            if (strCsType == "")
            {
                hidField["msg"] = CFL.GetStr(hidLang["M01016"]); // "거래처유형을 선택하여 주십시요.";
                return;
            }

            /*
            국내매출처 :  CodeCode = 100
            해외매출처 :  CodeCode = 150
            국내매입처 :  CodeCode = 200 
            해외매입처 :  CodeCode = 250
            은행 :        CodeCode = 300
            신용카드 :    CodeCode = 700
            제조업체 :    CodeCode = 900          
            */

            DataSet dsCode = dsObj.GetAutoCode(G.D, strCsType);

            if(dsCode.Tables.Count <= 0)
            {
                hidField["msg"] = CFL.GetStr(hidLang["M01017"]); //"코드생성에 실패하였습니다.";
                return;
            }

            string autoCode = dsCode.Tables[0].Rows[0]["CsCode"].ToString();
            int rCnt = dsCode.Tables[0].Rows.Count;

            // 국내매출처
            if (strCsType == "100")
            {
                if (rCnt == 0)
                {
                    strCsCode = "C00001";
                }
                else if (autoCode.Length > 6)
                {
                    strCsCode = "C" + (CFL.Tod(autoCode.Substring(1, 6)) + 1).ToString();
                }
                else
                {
                    if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 1)
                        strCsCode = "C0000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 2)
                        strCsCode = "C000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 3)
                        strCsCode = "C00" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 4)
                        strCsCode = "C0" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length >= 5)
                        strCsCode = "C" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                }
            }
            // 해외매출처
            else if (strCsType == "150")
            {
                if (rCnt == 0)
                {
                    strCsCode = "O00001";
                }
                else if (autoCode.Trim().Length > 6)
                {
                    strCsCode = "O" + (CFL.Tod(autoCode.Substring(1, 6)) + 1).ToString();
                }
                else
                {
                    if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 1)
                        strCsCode = "O0000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 2)
                        strCsCode = "O000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 3)
                        strCsCode = "O00" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 4)
                        strCsCode = "O0" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length >= 5)
                        strCsCode = "O" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                }
            }
            // 국내매입처
            else if (strCsType == "200")
            {
                if (rCnt == 0)
                {
                    strCsCode = "S00001";
                }
                else if (autoCode.Trim().Length > 6)
                {
                    strCsCode = "S" + (CFL.Tod(autoCode.Substring(1, 6)) + 1).ToString();
                }
                else
                {
                    if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 1)
                        strCsCode = "S0000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 2)
                        strCsCode = "S000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 3)
                        strCsCode = "S00" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 4)
                        strCsCode = "S0" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length >= 5)
                        strCsCode = "S" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                }
            }
            // 해외매입처
            else if (strCsType == "250")
            {
                if (rCnt == 0)
                {
                    strCsCode = "T00001";
                }
                else if (autoCode.Trim().Length > 6)
                {
                    strCsCode = "T" + (CFL.Tod(autoCode.Substring(1, 6)) + 1).ToString();
                }
                else
                {
                    if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 1)
                        strCsCode = "T0000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 2)
                        strCsCode = "T000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 3)
                        strCsCode = "T00" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 4)
                        strCsCode = "T0" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length >= 5)
                        strCsCode = "T" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                }
            }
            // 은행
            else if (strCsType == "300")
            {
                if (rCnt == 0)
                {
                    strCsCode = "B00001";
                }
                else if (autoCode.Trim().Length > 6)
                {
                    strCsCode = "B" + (CFL.Tod(autoCode.Substring(1, 6)) + 1).ToString();
                }
                else
                {
                    if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 1)
                        strCsCode = "B0000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 2)
                        strCsCode = "B000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 3)
                        strCsCode = "B00" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 4)
                        strCsCode = "B0" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length >= 5)
                        strCsCode = "B" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                }
            }
            // 신용카드
            else if (strCsType == "700")
            {
                if (rCnt == 0)
                {
                    strCsCode = "D00001";
                }
                else if (autoCode.Trim().Length > 6)
                {
                    strCsCode = "D" + (CFL.Tod(autoCode.Substring(1, 6)) + 1).ToString();
                }
                else
                {
                    if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 1)
                        strCsCode = "D0000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 2)
                        strCsCode = "D000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 3)
                        strCsCode = "D00" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 4)
                        strCsCode = "D0" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length >= 5)
                        strCsCode = "D" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                }
            }
            // 제조업체
            else if (strCsType == "900")
            {
                if (rCnt == 0)
                {
                    strCsCode = "M00001";
                }
                else if (autoCode.Trim().Length > 6)
                {
                    strCsCode = "M" + (CFL.Tod(autoCode.Substring(1, 6)) + 1).ToString();
                }
                else
                {
                    if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 1)
                        strCsCode = "M0000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 2)
                        strCsCode = "M000" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 3)
                        strCsCode = "M00" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length == 4)
                        strCsCode = "M0" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                    else if ((CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString().Length >= 5)
                        strCsCode = "M" + (CFL.Tod(autoCode.Substring(1, 5)) + 1).ToString();
                }
            }
            else
            {
                strCsCode = "";
            }


        } // 신규모드 if : 끝

        
        // 사업자번호 필수체크       
        if (strCsType != "150" && strCsType != "250" && strCsType != "650")  // 해외매출처, 해외매입처, 공공기관 제외
        {
            if (txtRegNo.Text.Trim() == "")
            {
                hidField["msg"] = CFL.GetStr(hidLang["M01018"]);// "사업자번호는 필수 입력항목입니다.";
                return;
            }


            // 2015.09.30 : 사업자번호 - 없이 입력 요청
            // 2021.09.13 : SmartPro 요청.. 사업자 번호 형식 체크 로직 제외
            /*
            string strNewRegNo = "";
            strNewRegNo = txtRegNo.Text.Trim().Replace("-", "");

            if (strNewRegNo.Length != 10)
            {
                hidField["msg"] = CFL.GetStr(hidLang["M00623"]);// "사업자번호는 '-' 표시가 없는 경우, 10자리여야 합니다.";
                return;
            }

            if (strNewRegNo.Length == 10)
            {
                txtRegNo.Text = strNewRegNo.Substring(0, 3) + "-" + strNewRegNo.Substring(3, 2) + "-" + strNewRegNo.Substring(5, 5);
            }
            */

            // 신규일때만 사업자번호 동일한게 있는지 체크
            if (txtCsCode.Text.Trim() == "")
            {
                ArrayList alCheck = daObj.RegNoCheck(G.D, txtRegNo.Text);

                if ("00" != alCheck[0].ToString())
                {
                    hidField["msg"] = CFL.GetMsg(alCheck[1].ToString());
                    return;
                }
            }
        }
        

        // 트러스빌  ------------------------------------------------------------------------------
        if (chkTrus_1.Checked && chkTrus_2.Checked)
        {
            hidField["msg"] = CFL.GetStr(hidLang["M01019"]);// "전자계산서 설정부분에 두가지를 모두 선택할 수 없습니다.";
            return;
        }

        string strTrus_Direction = "";

        if (chkTrus_1.Checked)
        {
            strTrus_Direction = "A";
        }
        else if (chkTrus_2.Checked)
        {
            strTrus_Direction = "B";
        }
        else
        {
            strTrus_Direction = null;
        }


        // 트러스빌 사용여부
        string strTrus_Use = "";

        if (chkTrus_Use.Checked)
        {
            strTrus_Use = "Y";
        }
        else
        {
            strTrus_Use = "N";
        }
        // 트러스빌 : 끝 ---------------------------------------------------------------------------
        GridS1.UpdateEdit();

        strOutFlag = "S1";
        int rCntS1 = dsGridS1.Tables[0].Rows.Count;

        string[] sarEmpName = new string[rCntS1];
        string[] sarEmpTell = new string[rCntS1];
        string[] sarEmpFaxNo = new string[rCntS1];
        string[] saremailID = new string[rCntS1];
        string[] sarEmpBego = new string[rCntS1];
        
        for (int i = 0; i < rCntS1; i++)
        {
            sarEmpName[i] = dsGridS1.Tables[0].Rows[i]["EmpName"].ToString().Trim() == "" ? "" : dsGridS1.Tables[0].Rows[i]["EmpName"].ToString().Trim();
            sarEmpTell[i] = dsGridS1.Tables[0].Rows[i]["EmpTell"].ToString().Trim() == "" ? "" : dsGridS1.Tables[0].Rows[i]["EmpTell"].ToString().Trim();
            sarEmpFaxNo[i] = dsGridS1.Tables[0].Rows[i]["EmpFaxNo"].ToString().Trim() == "" ? "0" : dsGridS1.Tables[0].Rows[i]["EmpFaxNo"].ToString().Trim();
            saremailID[i] = dsGridS1.Tables[0].Rows[i]["emailID"].ToString().Trim() == "" ? "0" : dsGridS1.Tables[0].Rows[i]["emailID"].ToString().Trim();
            sarEmpBego[i] = dsGridS1.Tables[0].Rows[i]["EmpBego"].ToString().Trim() == "" ? "0" : dsGridS1.Tables[0].Rows[i]["EmpBego"].ToString().Trim();
        }

        ArrayList alData = daObj.Save_csMaster( G.D, SaveMode
            , strCsCode, strCsName, strCsType, strCsGubun, strArCheck
            , strApCheck, strOutCheck, strCsNameFull, strForeignCheck, strComCheck
            , strCsUse, strRegNo, strNationCode, strRegionCode, strParentCs
            , strCsIndustry, strCsItems, strCsAddress, strCsZip, strCsChief
            , strCsTel, strCsFax, strCsUrl, strCsEmail, strTaxCodeAr
            , strTaxCodeAp, strPayConditionAr, strPayConditionAp, strCurrCode, strCsGrade
            , strCreditGrade, strIncoTerm, strLeadTime, strTransCode, strSendMethod
            , strTranTelNo, strTranFax, strTranEmail, strBankCode, strAccountNo
            , strCsDescr, txtExtraPlan1.Text, txtExtraPlan2.Text, txtExtraPlan3.Text , strCsClass1
            , strCsClass2, strTrus_Direction, strTrus_Use, strEmpCOde, strInitial
            , sarEmpName, sarEmpTell, sarEmpFaxNo, saremailID, sarEmpBego
            );

        if (alData[0].ToString() != "00")
        {
            try
            {
                hidField["status"] = "N";
                hidField["msg"] = CFL.GetStr(hidLang[CFL.GetStr(alData[0])]);
            }
            catch
            {
                hidField["status"] = "N";
            //  hidField["msg"] = CFL.GetStr(hidLang["M00086"]); // "저장 중 오류가 발생 하였습니다.";
                hidField["msg"] = alData[1].ToString();
            }
            return;
        }
        else
        {
            hidField["status"] = "Y";
            hidField["msg"] = CFL.GetStr(hidLang["M00002"]); // "저장이 완료되었습니다.";
            hidField["CsCode"] = CFL.GetStr(alData[1]);
            return;
        }
    }

    //문자체크
    protected Boolean RegNoCheck(string RegNo)
    {
        string result = RegNo.Replace("-", "");
        decimal numChk = 0;
        bool isNum = decimal.TryParse(result, out numChk);
        if (!isNum)
        {
            return true;
        }

        return false;
    }

    void JobDelete()
    {
        ArrayList alData = daObj.Delete(G.D, txtCsCode.Text.Trim());

        hidField.Clear();
        hidField["job"] = "DELETE";
        if (alData[0].ToString() != "00")
        {
            hidField["status"] = "N";
            hidField["msg"] = alData[1].ToString();
        }
        else
        {
            hidField["status"] = "Y";
            hidField["msg"] = alData[1].ToString();
        }
    }

    protected void filePackingLogo_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        //string strFileName = e.UploadedFile.FileName;
        //long lSize = e.UploadedFile.ContentLength;

        if (txtCsCode.Text.Trim() == "")
        {
            e.ErrorText = CFL.GetStr(hidLang["M01020"]); //"거래처를 선택해주십시오";
            return;
        }

        string strExtension = e.UploadedFile.FileName.Substring(e.UploadedFile.FileName.LastIndexOf(".") + 1);
        string strSaveFileName = "";
        string strFileReName = "Logo_" + txtCsCode.Text;
        string strPath = Server.MapPath("/SMARTPRO/UserImages/PackingLogo/" + txtCsCode.Text.Trim());

        // 폴더 생성
        if (!Directory.Exists(strPath))
        {
            Directory.CreateDirectory(strPath);
        }

        // 저장파일명
        strSaveFileName = strFileReName + "." + strExtension;

        // 파일복사
        e.UploadedFile.SaveAs(strPath + "/" + strSaveFileName);
    }

    //**************************** 거래처 담당자
    protected void GridS1_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters == "NEW")
        {
            strJob = "NEW";
            GridS1.ClearSort();
            GridS1.DataBind();

        }
        else if (e.Parameters == "UPDATE")
        {
            GridS1.UpdateEdit();
        }
        else
        {
            strJob = "";
            GridS1.ClearSort();
            GridS1.DataBind();
        }

    }

    protected void GridS1_DataBinding(object sender, EventArgs e)
    {

        DataSet ds = new DataSet();


        if (strJob == "NEW")
        {
            strJob = "";

            ds.Tables.Add();
            ds.Tables[0].Columns.Add("EmpName");
            ds.Tables[0].Columns.Add("EmpTell");
            ds.Tables[0].Columns.Add("EmpFaxNo");
            ds.Tables[0].Columns.Add("emailID");
            ds.Tables[0].Columns.Add("EmpBego");
            ds.Tables[0].Columns.Add("SaveCheck");

            ds.Tables[0].NewRow();
            GridS1.DataSource = ds;
        }
        else if (strJob == "UPDATE")
        {
            GridS1.UpdateEdit();
        }
        else
        {
            string strRoutingCode2 = txtCsCode.Text;
            ds = dsObj.Get_CsEmpLoad(G.D, strRoutingCode2);
            GridS1.DataSource = ds;
        }


    }

    protected void GridS1_CustomErrorText(object sender, DevExpress.Web.ASPxGridViewCustomErrorTextEventArgs e)
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

    protected void GridS1_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        DevExpress.Web.ASPxGridView gv = (DevExpress.Web.ASPxGridView)sender;
        gv.JSProperties.Add("cp_ret_message", strOutMsg);
        gv.JSProperties.Add("cp_ret_flag", strOutFlag);
        gv.JSProperties.Add("cp_ret_job", strJob);
    }

    protected void GridS1_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
    {
        //if (e.Column.Caption.ToUpper() == "NO")
        //{
        //    if (e.VisibleIndex >= 0)
        //    {
        //        e.DisplayText = (e.VisibleIndex + 1).ToString();
        //    }
        //}

    }
    protected void GridS1_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        e.Handled = false;

        strJob = "SAVE";
        strOutFlag = "S1";

        strOutMsg = "";

        DataTable dt = CFL.GetDataCols(e, GridS1);
        int rCnt = dt.Rows.Count;

        dsGridS1 = new DataSet();

        //if (rCnt > 0)
        //{
            dsGridS1.Tables.Add(dt);
        //}

        try
        {
            e.Handled = true;
        }
        catch
        {
            strOutFlag = "S1";
            strOutMsg = CFL.GetStr(hidLang["M00150"]);
            return;
        }
    }
}