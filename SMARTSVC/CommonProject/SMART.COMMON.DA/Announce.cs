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
    public class Announce
    {
        public Announce() { }

        public Announce(SqlConnection db, SqlTransaction tr)
        {

            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        public ArrayList Delete(object[] GDObj, string strBoard_ID, string strNum)
        {
            GData GD = new GData(GDObj);
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = string.Empty;

            strQuery = @"
DELETE SiteAnnounce
WHERE Board_ID = " + CFL.Q(strBoard_ID) + @" 
  AND Num = " + CFL.Q(strNum) + @"";

            strQuery += @"

DELETE SiteAnnounce_SCM
WHERE Board_ID = " + CFL.Q(strBoard_ID) + @" 
  AND Num = " + CFL.Q(strNum) + @"";


            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception ex)
            {
                m_tr.Rollback();

                strErrorCode = "15";
                strErrorMsg = CFL.RS("15", this, GD.LangID);//"삭제가 되지 않았습니다.";
                return CFL.EncodeData(strErrorCode, strErrorMsg, ex);
            }

            if (!m_bInTrans)
                m_tr.Commit();

            ArrayList alData = new ArrayList();

            alData.Add("00");
            alData.Add("");
            alData.Add(0);
            alData.Add(0);

            return alData;
        }

        public ArrayList Save(object[] GDObj, string strSiteCode, string strBoard_ID, string strNum, string strTitle
            , string strContent, string strWriter, string strWriter_ID
            , string strEmail, string strPWD, string strImportance)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            //신규
            if (strBoard_ID == "")
            {
                strQuery += @"
DECLARE @sBOARD_ID INT
DECLARE @sNUM INT
SELECT @sBOARD_ID = Board_ID FROM SiteBoardEnv WHERE SiteCode = " + CFL.Q(strSiteCode) + @"
SELECT @sNUM = ISNULL(MAX(Num), 0) + 1 FROM SiteAnnounce WHERE Board_ID = @sBOARD_ID

INSERT INTO SiteAnnounce
( Board_ID, Num, Title, Content, Writeday
, Readnum, Writer, Writer_ID, Email, PWD
, Importance )
VALUES
( @sBOARD_ID, @sNUM, " + CFL.Q(strTitle) + ", " + CFL.Q(strContent) + @", GETDATE()
, 0, " + CFL.Q(strWriter) + ", " + CFL.Q(strWriter_ID) + ", " + CFL.Q(strEmail) + ", " + CFL.Q(strPWD) + @"
, " + CFL.Q(strImportance) + ")";

            }
            else
            {
                strQuery = @"
UPDATE SiteAnnounce
SET Title = " + CFL.Q(strTitle) + @"
  , Content = " + CFL.Q(strContent) + @"
  , Email = " + CFL.Q(strEmail) + @"
  , Importance = " + CFL.Q(strImportance) + @"
WHERE Board_ID = " + CFL.Q(strBoard_ID) + @"
  AND Num = " + CFL.Q(strNum);

            }

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "11";
                strErrorMsg = "글이 등록되지 않았습니다.";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }

            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }


        // ******************************************************************************************************


        public ArrayList Save_SCM(object[] GDObj, string strSiteCode, string strBoard_ID, string strNum, string strTitle
                                , string strContent, string strWriter, string strWriter_ID
                                , string strEmail, string strPWD, string strImportance
                                , string[] sarUserID, string[] sarUserName)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            //신규
            if (strBoard_ID == "")
            {
                strQuery = @"
DECLARE @sBOARD_ID INT
DECLARE @sNUM INT

SELECT @sBOARD_ID = Board_ID 
FROM SiteBoardEnv 
WHERE SiteCode = " + CFL.Q(strSiteCode) + @"

SELECT @sNUM = ISNULL(MAX(Num), 0) + 1 
FROM SiteAnnounce 
WHERE Board_ID = @sBOARD_ID


INSERT INTO SiteAnnounce ( Board_ID, Num, Title, Content, Writeday
                         , Readnum, Writer, Writer_ID, Email, PWD
                         , Importance, SCMUse )
    VALUES ( @sBOARD_ID, @sNUM, " + CFL.Q(strTitle) + ", " + CFL.Q(strContent) + @", GETDATE()
           , 0, " + CFL.Q(strWriter) + ", " + CFL.Q(strWriter_ID) + ", " + CFL.Q(strEmail) + ", " + CFL.Q(strPWD) + @"
           , " + CFL.Q(strImportance) + ", 'Y' )";

                
                // SCM User 정보   -------------------------------------------------------------

                for (int i = 0; i < sarUserID.Length; i++)
                {
                    strQuery += @"

Insert Into SiteAnnounce_SCM (Board_ID, Num, UserID, UserName)
    Values (@sBOARD_ID, @sNUM, " + CFL.Q(sarUserID[i].Trim()) + ", " + CFL.Q(sarUserName[i].Trim()) + ") ";

                }

            }
            else
            {
                strQuery = @"
UPDATE SiteAnnounce
SET Title = " + CFL.Q(strTitle) + @"
  , Content = " + CFL.Q(strContent) + @"
  , Email = " + CFL.Q(strEmail) + @"
  , Importance = " + CFL.Q(strImportance) + @"
WHERE Board_ID = " + strBoard_ID + @"
  AND Num = " + strNum;


                // SCM User 정보   -------------------------------------------------------------

                strQuery += @"

Delete From SiteAnnounce_SCM
WHERE Board_ID = " + strBoard_ID + @"
  AND Num = " + strNum;

                for (int i = 0; i < sarUserID.Length; i++)
                {
                    strQuery += @"

Insert Into SiteAnnounce_SCM (Board_ID, Num, UserID, UserName)
    Values (" + strBoard_ID + ", " + strNum + ", " + CFL.Q(sarUserID[i].Trim()) + ", " + CFL.Q(sarUserName[i].Trim()) + ") ";

                }


            }

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "11";
                strErrorMsg = "글이 등록되지 않았습니다.";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }

            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }


    }
}