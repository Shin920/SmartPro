using SMART;
using System;
using System.Data;
using System.Net;

namespace MobileBiz
{
    public class BizLogin : DBBase
    {
        public BizLogin(string strClientID)
        {
            connection.ConnectDB(strClientID);
        }

        //로그인 정보 세션에 담기
        public void setUserInfo(System.Web.HttpRequest req, System.Web.SessionState.HttpSessionState ses, DataTable DataLogin, string strClientID)
        {
            CFLUserInfo userInfo = new CFLUserInfo();

            userInfo.UserID = CFL.GetStr(DataLogin.Rows[0]["UserID"]);
            userInfo.ComCode = CFL.GetStr(DataLogin.Rows[0]["ComCode"]);
            userInfo.DeptCode = CFL.GetStr(DataLogin.Rows[0]["DefaultDept"]);
            userInfo.DeptName = CFL.GetStr(DataLogin.Rows[0]["DeptName"]);
            userInfo.EmpCode = CFL.GetStr(DataLogin.Rows[0]["EmpCode"]);
            userInfo.EmpName = CFL.GetStr(DataLogin.Rows[0]["UserName"]);
            userInfo.SiteCode = CFL.GetStr(DataLogin.Rows[0]["SiteCode"]);
            userInfo.SiteName = CFL.GetStr(DataLogin.Rows[0]["SiteName"]);
            userInfo.LangCD = "kor";
            userInfo.ClientID = strClientID;

            ses["MobileCore"] = userInfo;
        }

        public void setUserInfoResponseCookie(System.Web.HttpResponse res, CFLUserInfo usr)
        {
            res.Cookies["MobileCore"]["UserID"] = usr.UserID;
            res.Cookies["MobileCore"]["ComCode"] = usr.ComCode;
            res.Cookies["MobileCore"]["DefaultDept"] = usr.DeptCode;
            res.Cookies["MobileCore"]["DeptName"] = usr.DeptName;
            res.Cookies["MobileCore"]["EmpCode"] = usr.EmpCode;
            res.Cookies["MobileCore"]["UserName"] = usr.EmpName;
            res.Cookies["MobileCore"]["SiteCode"] = usr.SiteCode;
            res.Cookies["MobileCore"]["SiteName"] = usr.SiteName;
            res.Cookies["MobileCore"]["LangCD"] = usr.LangCD;
            res.Cookies["MobileCore"]["ClientID"] = usr.ClientID;
        }

        public CFL LoginInfo(string strLoginId, string strLoginPw, string strSiteCode)
        {
            DataSet ds = null;
            CFL data = null;
            string sql = "";

            sql += @"
                SELECT COUNT(*) AS CNT
                        , A.PassWd AS PassWd
                FROM xErpUser A
                INNER JOIN EmpMaster B
                ON  A.UserID = B.UserID
                WHERE A.UserID = " + CFL.Q(strLoginId) + @"
                    AND A.PassWd = " + CFL.Q(strLoginPw) + @"
                GROUP BY A.PassWd
                    ";

            //로그인 정보 가져오기
            sql += @"
                Select A.UserName, A.UserType, A.eMail, A.UserGroup
	                , B.EmpCode, B.EmpName, B.DefaultDept, B.DeptName, B.CcCode
	                , C.SiteName, C.SiteInitial, C.DefaultInvStatus, C.SiteCode
	                , C.DigitNo_Qty, C.DigitType_Qty, C.DigitNo_Price1, C.DigitType_Price1
	                , C.DigitNo_Price2, C.DigitType_Price2, C.DigitNo_sdDiscnt, C.DigitType_sdDiscnt
	                , C.DigitNo_ppScrap, C.DigitType_ppScrap, C.DigitNo_ppQtyPer, C.DigitType_ppQtyPer
	                , C.DigitNo_coUnitPrice, C.DigitType_coUnitPrice, C.DigitNo_coRatio, C.DigitType_coRatio
	                , C.DigitNo_mmOhPrice, C.DigitType_mmOhPrice
	                , C.poDefaultExchGubun, C.sdDefaultExchGubun, C.ComCode, E.ComName
	                , D.GroupNameHan
	                , E.DigitNo_glAmnt1, E.DigitType_glAmnt1, E.DigitNo_glAmnt2, E.DigitType_glAmnt2
	                , E.DigitNo_ExchRate, E.DigitType_ExchRate
	                , E.EisAmnt1_BoundAmnt, E.EisAmnt1_BoundName, E.EisAmnt2_BoundAmnt, E.EisAmnt2_BoundName
	                , E.glCashAcc, E.CurrCode, E.CultureID, E.glWipAcc, E.CcExpDD, E.CoaCode, E.ModuleUse
	                , F.CoaName
	                , Convert ( char(8), getdate(), 112 ) as CurDate
                    , A.UserID
                From xErpUser A with (NOLOCK)
	                Left Outer Join 
	                (
		                Select A.EmpCode, A.EmpName, A.DefaultDept, B.DeptName, B.DeptType, B.CcCode
                        From EmpMaster A with (NOLOCK)
			                Left Outer Join	DeptMaster B with (NOLOCK) on A.DefaultDept = B.DeptCode and A.ComCode = B.ComCode
		                   , SiteMaster C with (NOLOCK)
		                Where A.UserID = " + CFL.Q(strLoginId) + @"
		                  and A.ComCode = C.ComCode and C.SiteCode = " + CFL.Q(strSiteCode) + @"
	                ) B	on 0 = 0
	                Left Outer Join	SiteMaster C with (NOLOCK) on C.SiteCode = " + CFL.Q(strSiteCode) + @"
    
                    Left Outer Join SmartMaster D with (NOLOCK)	on 0 = 0
	
                    Left Outer Join CompanyMaster E with (NOLOCK) on C.ComCode = E.ComCode
	                Left Outer Join CoaMaster F with (NOLOCK) on E.CoaCode = F.CoaCode
                Where A.UserID = " + CFL.Q(strLoginId) + @"
                  And A.PassWd = " + CFL.Q(strLoginPw);

            try
            {
                ds = SQLDataSet(sql);
                data = EncodeData(ds, 1, "완료");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, "로그인 실패");
            }

            return data;
        }

    }
}
