using MobileBiz;
using SMART;
using System;
using System.Web;
using System.Web.Security;

public partial class Account_Login : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ReadINI INI = new ReadINI();
            string[] sarSection = INI.GetSectionNames();

            for (int i = 0; i < sarSection.Length; i++)
            {
                string strName = CFL.GetStr(INI.GetEntryValue(sarSection[i], "Name"));
                cmbClient.Items.Add(strName, sarSection[i]);
            }

            if (cmbClient.Items.Count <= 0)
            {
                Response.Write(CFL.GetMsg("DB접속 정보가 세팅이 되어있지 않습니다."));
                return;
            }


            if (Request.Cookies[CFL.c_CookieNameClientID] != null)
            {
                for (int i = 0; i < cmbClient.Items.Count; i++)
                {
                    if (CFL.GetStr(cmbClient.Items[i].Value) == CFL.GetStr(Request.Cookies[CFL.c_CookieNameClientID].Value))
                    {
                        cmbClient.SelectedIndex = i;
                    }
                }
            }
            
            if(cmbClient.Value == null)
            {
                cmbClient.SelectedIndex = 0;
            }

            //사업장코드 가져오기
            SetSiteCode(CFL.GetStr(cmbClient.Value));


            //로그인 아이디 기존내용이 있으면 가져오기.
            if (Request.Cookies[CFL.c_CookieNameUserID] != null)
            {
                txtID.Text = Request.Cookies[CFL.c_CookieNameUserID].Value;
            }
        }

        //포커스
        if (txtID.Text == "")
        {
            txtID.Focus();
        }
        else if (txtPW.Text == "")
        {
            txtPW.Focus();
        }
    }

    private void SetSiteCode(string strClientID)
    {
        cmbSiteCode.Items.Clear();

        BizSiteMaster Obj = new BizSiteMaster(strClientID);
        CFL retData = Obj.GetSiteSelect();


        // 제한시간 경과로 추가 (2024.01.18)
        Obj.connection.DisConnectDB();


        if (retData.errCode < 0)
        {
            Response.Write(CFL.GetMsg(retData.msg));
            return;
        }

        for (int i = 0; i < retData.ds.Tables[0].Rows.Count; i++)
        {
            cmbSiteCode.Items.Add(CFL.GetStr(retData.ds.Tables[0].Rows[i]["SiteName"]), CFL.GetStr(retData.ds.Tables[0].Rows[i]["SiteCode"]));
        }

        if (Request.Cookies[CFL.c_CookieNameSiteCode] != null)
        {
            cmbSiteCode.Value = Request.Cookies[CFL.c_CookieNameSiteCode].Value;
        }
        else
        {
            cmbSiteCode.SelectedIndex = 0;
        }
    }


    protected void btnLogin_Click(object sender, EventArgs e)
    {

        //변수세팅
        string strLoginId = txtID.Text.Trim();
        string strLoginPw = txtPW.Text.Trim();
        string strSiteCode = CFL.GetStr(cmbSiteCode.Value);
        string strClient = CFL.GetStr(cmbClient.Value);

        // 비밀번호 암호화 처리
    //  string strPassWdEnc = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strLoginPw, "MD5");

        // SHA256 암호화
        string strPassWdEnc = CFL.EncryptSHA256(strLoginPw);


        if (string.IsNullOrEmpty(strSiteCode))
        {
            Response.Write(CFL.GetMsg("사업장 정보를 선택하여 주십시오."));
            return;
        }

        //DB 처리

        BizLogin biz = new BizLogin(strClient);
        CFL loginData = biz.LoginInfo(strLoginId, strPassWdEnc, strSiteCode);

        if (loginData.errCode > 0)
        {
            if (loginData.ds.Tables[0].Rows.Count <= 0)
            {
                Response.Write(CFL.GetMsg("로그인 정보를 확인 할 수 없습니다."));
                return;
            }
            else if (CFL.Toi(loginData.ds.Tables[0].Rows[0]["CNT"]) > 0)
            {
                biz.setUserInfo(Request, Session, loginData.ds.Tables[1], strClient);

                // UserID
                HttpCookie Temp = new HttpCookie(CFL.c_CookieNameUserID, txtID.Text);
                TimeSpan ts = new TimeSpan(CFL.c_CookieExpiryDay, 0, 0, 0);
                Temp.Expires = DateTime.Now.Add(ts);
                Response.Cookies.Add(Temp);

                // SiteCode
                Temp = new HttpCookie(CFL.c_CookieNameSiteCode, strSiteCode);
                Temp.Expires = DateTime.Now.Add(ts);
                Response.Cookies.Add(Temp);

                // LangID
                Temp = new HttpCookie(CFL.c_CookieNameLangID, "kor");
                Temp.Expires = DateTime.Now.Add(ts);
                Response.Cookies.Add(Temp);

                // Client
                Temp = new HttpCookie(CFL.c_CookieNameClientID, strClient);
                Temp.Expires = DateTime.Now.Add(ts);
                Response.Cookies.Add(Temp);

                FormsAuthentication.SetAuthCookie(txtID.Text, false);

                //Response.Redirect("~/Default.aspx?logintime=" + DateTime.Now.ToString("yyyyMMddHHmmss") + "&guid=" + Guid.NewGuid().ToString());
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                Response.Write(CFL.GetMsg("로그인 정보를 확인 할 수 없습니다."));
                return;
            }
        }
        else
        {
            Response.Write(CFL.GetMsg("로그인 정보 조회 중 오류가 발생 하였습니다."));
            return;
        }
    }
}