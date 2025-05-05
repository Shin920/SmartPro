using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.COMMON.DA
{
    public class LogMaster
    {
        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        public LogMaster() { }

        public LogMaster(SqlConnection db, SqlTransaction tr)
        {
            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }


        public ArrayList SaveLoginHistory(object[] GDObj, string strIPAddress)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";


            // Save
            strQuery = @"
INSERT INTO LoginHistory ( SiteCode, UserID, UserName, EmpCode, EmpName, IPAddress, LoginTime, FormURL )
    SELECT " + CFL.Q(GD.SiteCode) + ", " + CFL.Q(GD.UserID) + ", " + CFL.Q(GD.UserName) + ", " + CFL.Q(GD.EmpCode) + ", " + CFL.Q(GD.EmpName) + ", " + CFL.Q(strIPAddress) + @"
    , GETDATE(), 'SMART Pro ERP/로그인' ";


            try
            {
                CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                strErrorCode = "01";
                strErrorMsg = "로그인 History 정보 생성에 실패하였습니다.";

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Commit
            m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }


        // 시스템 로그아웃 시간 setting
        public ArrayList SaveLoginOut(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = "";
                       
            strQuery = @"
Update LoginHistory
Set LogOutTime = getdate()
Where SiteCode = " + CFL.Q(GD.SiteCode) + @" 
  and UserID = " + CFL.Q(GD.UserID) + @" 
  and LogOutTime is null 
  and SerNo = ( Select Max(SerNo) 
                From LoginHistory 
                Where SiteCode = " + CFL.Q(GD.SiteCode) + @" 
                  and UserID = " + CFL.Q(GD.UserID) + @" 
                  and LogOutTime is null )
";
            
            try
            {
                CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                strErrorCode = "01";
                strErrorMsg = "로그인 아웃 처리에 실패하였습니다.";

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            
            // Commit
            m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }


        // 폼접속정보 관리 - 기존
        #region
        /*
        public ArrayList LogFormConnection(object[] GDObj, string strIPNO, string strFormURL)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            strQuery = @"
INSERT INTO LoginHistory ( SiteCode, UserID, UserName, EmpCode, EmpName
                         , IPAddress, LoginTime, FormURL )
    SELECT " + CFL.Q(GD.SiteCode) + ", " + CFL.Q(GD.UserID) + ", " + CFL.Q(GD.UserName) + ", " + CFL.Q(GD.EmpCode) + ", " + CFL.Q(GD.EmpName) + @"
         , " + CFL.Q(strIPNO) + ", GETDATE(), " + CFL.Q(strFormURL);

            try
            {
                CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                strErrorCode = "01";
                strErrorMsg = "폼접속 처리에 실패하였습니다.";

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Commit
            m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }
        */
        #endregion


        //  폼접속정보 관리
        public ArrayList LogFormConnection(object[] GDObj, string strIPNO, string strFormURL)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            strQuery = @"
Declare  @LoginTime		datetime
       , @LogoutTime    datetime
       , @SerNo			int
       
Select Top 1 
       @LoginTime = LoginTime
	 , @LogoutTime = LogOutTime     
     , @SerNo = SerNo
From loginhistory
Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
  and UserID = " + CFL.Q(GD.UserID) + @"
  and EmpCode = " + CFL.Q(GD.EmpCode) + @"
  and FormURL = 'Login'
Order By SerNo Desc
     
if (@LogoutTime is null) 
	Begin 
		
        -- 마지막 로그인 날짜가 오늘보다 이전일 이면...
		if (DATEDIFF(DAY, @LoginTime, GETDATE()) > 0)
			Begin 
									
				Update loginhistory
				Set LogOutTime = DATEADD(MINUTE, 50, @LoginTime)
				Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
                  and UserID = " + CFL.Q(GD.UserID) + @"
                  and EmpCode = " + CFL.Q(GD.EmpCode) + @"
		          and SerNo = @SerNo
				  
			End
		-- 같은날이면..
		else
			Begin
			
				Update loginhistory
				Set LogOutTime = DATEADD(SECOND, 50, GETDATE())
				Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
                  and UserID = " + CFL.Q(GD.UserID) + @"
                  and EmpCode = " + CFL.Q(GD.EmpCode) + @"
		          and SerNo = @SerNo
			
			End

	End     
";

            strQuery += @"

INSERT INTO LoginHistory ( SiteCode, UserID, UserName, EmpCode, EmpName
                         , IPAddress, LoginTime, FormURL )
    SELECT " + CFL.Q(GD.SiteCode) + ", " + CFL.Q(GD.UserID) + ", " + CFL.Q(GD.UserName) + ", " + CFL.Q(GD.EmpCode) + ", " + CFL.Q(GD.EmpName) + @"
         , " + CFL.Q(strIPNO) + ", GETDATE(), " + CFL.Q(strFormURL);

            try
            {
                CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                strErrorCode = "01";
                strErrorMsg = "폼접속 처리에 실패하였습니다.";

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Commit
            m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }


        // ****************************************************************************************


        // 폼별 트랜잭션 횟수 관리
        public ArrayList LogFormUseCount(object[] GDObj, string strFormID, string strUseGubun)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = "";

            // 화면 Open
            if (strUseGubun == "Login")
            {
                strQuery = @"
if Not Exists ( Select FormID From FormLoginHistory Where FormID = " + CFL.Q(strFormID) + @" )
    Begin            

        INSERT INTO FormLoginHistory ( SiteCode, FormID, ObjName, ObjID
                                     , LoginCnt, SaveCnt, SearchCnt, UpdateCnt, DeleteCnt )
            SELECT " + CFL.Q(GD.SiteCode) + ", " + CFL.Q(strFormID) + @", A.ObjName, B.ObjID  
                 , 1, 0, 0, 0, 0
            From ModuleObjLang A
                Join ModuleObj B On B.ObjID = A.ObjID
            Where A.LangID = " + CFL.Q(GD.LangID) + @" 
              and B.FormID = " + CFL.Q(strFormID) + @"

    End
Else
    Begin

        Update FormLoginHistory
        Set LoginCnt = LoginCnt + 1
        Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
          and FormID = " + CFL.Q(strFormID) + @"

    End
";
            }


            // 조회             
            if (strUseGubun == "Search")
            {
                strQuery = @"
Update FormLoginHistory
Set SearchCnt = isnull(SearchCnt, 0) + 1
Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
  and FormID = " + CFL.Q(strFormID);
            }


            // 저장             
            if (strUseGubun == "Save")
            {
                strQuery = @"
Update FormLoginHistory
Set SaveCnt = isnull(SaveCnt, 0) + 1
Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
  and FormID = " + CFL.Q(strFormID);
            }


            // 삭제        
            if (strUseGubun == "Delete")
            {
                strQuery = @"
Update FormLoginHistory
Set DeleteCnt = isnull(DeleteCnt, 0) + 1
Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
  and FormID = " + CFL.Q(strFormID);
            }


            try
            {
            //  CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db);

                CFL.ExecuteNonQuery(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
            //  m_tr.Rollback();

                strErrorCode = "01";
                strErrorMsg = "폼접속 처리에 실패하였습니다.";

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Commit
        //  m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }

    }
}