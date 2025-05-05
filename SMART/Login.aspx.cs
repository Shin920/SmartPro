using DevExpress.Web;
using SMART;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;

public partial class Login : System.Web.UI.Page
{
    private const string c_CookieNameUserID = "xErpUserID";
    private const string c_CookieNameSiteCode = "xErpSiteCode";
    private const string c_CookieNameLangID = "xErpLangID";
    private const string c_CookieNameClientID = "xErpClientID";
    private const int c_CookieExpiryDay = 30;
    private const int c_KeySize = 10;

    private const string c_CookieNameComCode = "xErpComCode";

    public string m_strScriptTarget = "";

    GObj G;
    GObj G2;

    protected void Page_Load(object sender, EventArgs e)
    {

        G = new GObj();
        Session.Add("G", G);

        string strLangID = "kor";

        if (!IsPostBack)
        {
            // 이미 로그인한 경우라면 로그아웃 처리
            if (G != null)
            {
                G.Logout();
                Session.Clear();
            }

            // Set Client ddl Box
            ClientCore Obj = new ClientCore();

            object[] alData = Obj.Ddl().ToArray();

            if ("00" != alData[0].ToString())
            {
                m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
                return;
            }

            for (int i = 0; i < (int)alData[3]; i++)
                cmbClient.Items.Add(new ListItem(alData[i * (int)alData[2] + 5].ToString(), alData[i * (int)alData[2] + 4].ToString()));

            // Select ClientID
            string strClientID = "Default";


            if (null != Request["ClientID"] && "" != Request["ClientID"])
                strClientID = Request["ClientID"];
            else if (null != Request.Cookies[c_CookieNameClientID])
                strClientID = Request.Cookies[c_CookieNameClientID].Value;

            // Select Item
            string strTemp = "";

            CFL.SetEncodedProperty("ID", strClientID, ref strTemp);

            ListItem li = cmbClient.Items.FindByValue(strTemp);

            if (null != li)
                li.Selected = true;

            // For Exception that Client has Deleted
            else if (cmbClient.Items.Count > 0)
            {
                strTemp = cmbClient.Items[0].Value.ToString();
                strClientID = CFL.GetEncodedProperty("ID", strTemp);
            }

            SetComCode(strClientID);

            cmbComCode_SelectedIndexChanged(null, null);
            
            //SetSiteCode(strClientID);
            strLangID = SetLanguage(strClientID);

            // ID
            if (null != Request.Cookies[c_CookieNameUserID])
                txtID.Text = Request.Cookies[c_CookieNameUserID].Value;
        }
        else
        {
            if (null != cmbLang.SelectedItem)
                strLangID = cmbLang.SelectedItem.Value.ToString();
            else
                strLangID = cmbLang.Items[0].Value.ToString();
        }
    }

    private string SetLanguage(string strClientID)
    {
        cmbLang.Items.Clear();

        SMART.COMMON.DS.LoginClass Obj = new SMART.COMMON.DS.LoginClass();

        object[] alData = Obj.LangSelect(strClientID).ToArray();

        if ("00" != alData[0].ToString())
        {
            m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
            return "kor";
        }
        else if (0 == (int)alData[3])
        {
            m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
            return "kor";
        }

        for (int i = 0; i < (int)alData[3]; i++)
            cmbLang.Items.Add(new ListItem(alData[i * (int)alData[2] + 5].ToString(), alData[i * (int)alData[2] + 4].ToString().ToLower()));

        // LangID
        ListItem li = null;
        string strLangID = "kor";

        if (null != Request["LangID"] && "" != Request["LangID"])
        {
            li = cmbLang.Items.FindByValue(Request["LangID"]);
            strLangID = Request["LangID"];
        }
        else if (null != Request.Cookies[c_CookieNameLangID])
        {
            li = cmbLang.Items.FindByValue(Request.Cookies[c_CookieNameLangID].Value);
            strLangID = Request.Cookies[c_CookieNameLangID].Value;
        }

        if (null != li)
            li.Selected = true;
        else
        {
            cmbLang.Items[0].Selected = true;
            strLangID = cmbLang.Items[0].Value.ToString();
        }

        return strLangID;
    }


    // 법인정보 (추가)    
    private void SetComCode(string strClientID)
    {
        cmbComCode.Items.Clear();

        SMART.COMMON.DS.LoginClass Obj = new SMART.COMMON.DS.LoginClass();

        object[] alData = Obj.DdlComSelect(strClientID).ToArray();

        if ("00" != alData[0].ToString())
        {
            m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
            return;
        }
        else if (0 == (int)alData[3])
        {
            m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
            return;
        }

        for (int i = 0; i < (int)alData[3]; i++)
            cmbComCode.Items.Add(new ListItem(alData[i * (int)alData[2] + 5].ToString(), alData[i * (int)alData[2] + 4].ToString()));


        // ComCode
        ListItem li = null;

        if (null != Request["ComCode"] && "" != Request["ComCode"])
        {
            li = cmbComCode.Items.FindByValue(Request["ComCode"]);
        }
        else if (null != Request.Cookies[c_CookieNameComCode])
        {
            li = cmbComCode.Items.FindByValue(Request.Cookies[c_CookieNameComCode].Value);
        }


        if (null != li)
        {
            li.Selected = true;
        }
        else
        {
            cmbComCode.Items[0].Selected = true;
        }
    }


    private void SetSiteCode(string strClientID)
    {
        cmbSiteCode.Items.Clear();

        SMART.COMMON.DS.LoginClass Obj = new SMART.COMMON.DS.LoginClass();

        object[] alData = Obj.DdlSiteSelect(strClientID).ToArray();

        if ("00" != alData[0].ToString())
        {
            m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
            return;
        }
        else if (0 == (int)alData[3])
        {
            m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
            return;
        }

        for (int i = 0; i < (int)alData[3]; i++)
            cmbSiteCode.Items.Add(new ListItem(alData[i * (int)alData[2] + 5].ToString(), alData[i * (int)alData[2] + 4].ToString()));

        // SiteCode
        ListItem li = null;





        if (null != Request["SiteCode"] && "" != Request["SiteCode"])
        {
            li = cmbSiteCode.Items.FindByValue(Request["SiteCode"]);
        }
        else if (null != Request.Cookies[c_CookieNameSiteCode])
        {
            li = cmbSiteCode.Items.FindByValue(Request.Cookies[c_CookieNameSiteCode].Value);
        }

        if (null != li)
        {
            li.Selected = true;
        }
        else
        {
            cmbSiteCode.Items[0].Selected = true;
        }
    }

    
    protected void Connection_Setting()
    {

        SMART.COMMON.DA.LogMaster Log = new SMART.COMMON.DA.LogMaster();

        string strIPAddress = Request.UserHostAddress.ToString();

        G = ((GObj)Session["G"]);

        ArrayList alData = Log.LogFormConnection(G.D, strIPAddress, "Login");

        if (alData[0].ToString() != "00")
        {
            m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
            return;
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string strClientID = CFL.GetEncodedProperty("ID", cmbClient.SelectedItem.Value.ToString());


        // 법인정보
        string strComCode = "";

        if (null != cmbComCode.SelectedItem.ToString())
            strComCode = cmbComCode.SelectedItem.Value.ToString();

        

        // 사업장정보
        string strSiteCode = "";

        if (null != cmbSiteCode.SelectedItem.ToString())
            strSiteCode = cmbSiteCode.SelectedItem.Value.ToString();

        string strLangID = cmbLang.SelectedItem.Value.ToString();

        // Login
        string strCertKey = "";

        if (null != Session["CertKey"])
            strCertKey = Session["CertKey"].ToString();

        SMART.GObj Obj = new GObj();

        object[] alData = Obj.Login(txtID.Text, txtPWD.Text, strSiteCode, strLangID, strClientID, "", strCertKey, Request.UserHostAddress, null);

        if ("00" != alData[0].ToString())
        {
            // errors that can't continue with login
            if ("C" != alData[0].ToString().Substring(0, 1))
            {
                // Error in UserID
                if ("01" == alData[0].ToString())
                    m_strScriptTarget = CFL.GetMsg(alData[1].ToString(), "txtID");
                // Error in UserPasswd
                else if ("02" == alData[0].ToString())
                    m_strScriptTarget = CFL.GetMsg(alData[1].ToString(), "txtPWD");
                // Error in UserPasswd
                else
                    m_strScriptTarget = CFL.GetMsg(alData[1].ToString());

                return;
            }
            else
                m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
        }

        Session["G"] = Obj; //////////////로그인 정보를 세션에 저장

        // Remove CertKey
        Session.Remove("CertKey");

        if (null != Request["FormID"] && "" != Request["FormID"])
            Session.Add("FormID", Request["FormID"]);

        // #################################################
        // ####     Set Cookie for Save Information     ####
        // #################################################

        // UserID
        HttpCookie Temp = new HttpCookie(c_CookieNameUserID, txtID.Text);
        TimeSpan ts = new TimeSpan(c_CookieExpiryDay, 0, 0, 0);
        Temp.Expires = DateTime.Now.Add(ts);
        Response.Cookies.Add(Temp);

        // SiteCode
        Temp = new HttpCookie(c_CookieNameSiteCode, strSiteCode);
        Temp.Expires = DateTime.Now.Add(ts);
        Response.Cookies.Add(Temp);

        // LangID
        Temp = new HttpCookie(c_CookieNameLangID, strLangID);
        Temp.Expires = DateTime.Now.Add(ts);
        Response.Cookies.Add(Temp);

        // Client
        Temp = new HttpCookie(c_CookieNameClientID, strClientID);
        Temp.Expires = DateTime.Now.Add(ts);
        Response.Cookies.Add(Temp);

        // ComCode
        Temp = new HttpCookie(c_CookieNameComCode, strComCode);
        Temp.Expires = DateTime.Now.Add(ts);
        Response.Cookies.Add(Temp);


        FormsAuthentication.SetAuthCookie(txtID.Text, false);

        // 로그인 로그 정보 생성
        Connection_Setting();



        // Smart Factory 로그인 정보 전송용  -------------------------------------------------------------
        string strResult = "";


        // 에보소닉 & 스킨러버스 인 경우..
        if (G.SiteNo == "EvoSonic" || G.SiteNo == "Skin")
        {

            using (HttpClient client = new HttpClient())
            {
                string strSendUrl = "https://log.smart-factory.kr/apisvc/sendLogDataHTML.do";

                string strcrtfcKey = "";

                // 에보소닉
                if (G.SiteNo == "EvoSonic")
                {
                    strcrtfcKey = "$5$API$LHQyjrOLjiMGgcMzZiaIz5Pd49TSloKbwHShSHkkSo2";
                }                
                // 스킨러버스
                else if (G.SiteNo == "Skin")
                {
                    strcrtfcKey = "$5$API$Z/B8dCOnOGOiuaRCt32J7Qg.JnJJfO78UX7igBuDhk7";
                }
                

                string strlogDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string strsysUser = G.UserID;
                string strconectIp = G.UserIP;
                string strdataUsgqty = "0";

                MultipartFormDataContent formData = new MultipartFormDataContent();

                formData.Add(new StringContent(strcrtfcKey), "crtfcKey");
                formData.Add(new StringContent(strlogDt), "logDt");
                formData.Add(new StringContent("접속"), "useSe");
                formData.Add(new StringContent(strsysUser), "sysUser");
                formData.Add(new StringContent(strconectIp), "conectIp");
                formData.Add(new StringContent(strdataUsgqty), "dataUsgqty");

                client.DefaultRequestHeaders.Add("Accept", "*/*");

                var response = client.PostAsync(strSendUrl, formData).Result;

                if (!response.IsSuccessStatusCode)
                {
                    strResult = "전송에 실패하였습니다.";

                //  return;
                }
            }
        }
        
        //내부 관리프로그램 로그 생성.
        LoginService(G.SiteNo, G.UserID, G.UserName);

        // Smart Factory 로그인 정보 전송용 : 끝  -----------------------------------------------------------
        string strUrl = "/SmartPro/glb/Forms/CheckForm.aspx?ReturnUrl=" + HttpUtility.UrlEncode("/SmartPro/glb/Main.aspx");

        Response.Write("<script>window.open('" + strUrl + "','_self');</script>");
    }
    

    protected void cmbComCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbSiteCode.Items.Clear();

        SMART.COMMON.DS.LoginClass Obj = new SMART.COMMON.DS.LoginClass();

        string strComCode = CFL.GetStr(cmbComCode.SelectedItem.Value);


        // Select ClientID
        string strClientID = "Default";

        if (null != Request["ClientID"] && "" != Request["ClientID"])
            strClientID = Request["ClientID"];
        else if (null != Request.Cookies[c_CookieNameClientID])
            strClientID = Request.Cookies[c_CookieNameClientID].Value;


        object[] alData = Obj.DdlSiteSelectForComCode(strComCode, strClientID).ToArray();

        if ("00" != alData[0].ToString())
        {
            m_strScriptTarget = CFL.GetMsg(alData[1].ToString());
            return;
        }
        else if (0 == (int)alData[3])
        {
            m_strScriptTarget = CFL.GetMsg("해당 법인의 사업장이 구성되지 않았습니다.");
            return;
        }


        for (int i = 0; i < (int)alData[3]; i++)
            cmbSiteCode.Items.Add(new ListItem(alData[i * (int)alData[2] + 5].ToString(), alData[i * (int)alData[2] + 4].ToString()));

        // SiteCode
        ListItem li = null;

        if (null != Request["SiteCode"] && "" != Request["SiteCode"])
        {
            li = cmbSiteCode.Items.FindByValue(Request["SiteCode"]);
        }
        else if (null != Request.Cookies[c_CookieNameSiteCode])
        {
            li = cmbSiteCode.Items.FindByValue(Request.Cookies[c_CookieNameSiteCode].Value);
        }

        if (null != li)
        {
            li.Selected = true;
        }
        else
        {
            cmbSiteCode.Items[0].Selected = true;
        }
    }

    //GINCNI 고객사별 로그인 정보 추가하는 로직
    protected void LoginService(string strSiteNo, string strUserID, string strUserName)
    {
        //Key
        string strKey = "smartPro21GinCni";

        OperatingSystem os = Environment.OSVersion;
        string strServerIP = System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        string strUserIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

        try
        {
            LoginService.LoginService web = new LoginService.LoginService();
            web.Timeout = 1000;

            string strReturn = web.LoginInsert(strKey, strSiteNo, strServerIP, os.ToString(), "v21", strUserIP, strUserID, strUserName);
        }
        catch { }
            

    }

}