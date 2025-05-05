using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.COMMON.DS
{
    public class Announce
    {
        public Announce() { }

        public DataSet GetAnounceListLoad_ForMainForm(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT TOP 100 ISNULL(A.Importance, 'N') AS Importance
     , A.Num, A.Board_ID, A.Title, A.Readnum, CONVERT(NVARCHAR(10), A.Writeday, 120) AS Writeday, A.Writer
FROM SiteAnnounce A
WHERE A.Board_ID IN (SELECT Board_ID FROM SiteBoardEnv WHERE SiteCode = " + CFL.Q(GD.SiteCode) + @")
ORDER BY A.Importance, A.Num ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }
            finally
            {
                strQuery = null;
            }

            return ds;

        }

        public DataSet GetAnounceListLoad(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT ISNULL(A.Importance, 'N') AS Importance
     , A.Num, A.Board_ID, A.Title, A.Readnum, CONVERT(NVARCHAR(10), A.Writeday, 120) AS Writeday, A.Writer
FROM SiteAnnounce A
WHERE A.Board_ID IN ( SELECT Board_ID 
                      FROM SiteBoardEnv 
                      WHERE SiteCode = " + CFL.Q(GD.SiteCode) + @") 
  and A.SCMUse is null 
";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }
            finally
            {
                strQuery = null;
            }

            return ds;

        }


        public DataSet GetAnounceListLoad_SCM(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT ISNULL(A.Importance, 'N') AS Importance
     , A.Num, A.Board_ID, A.Title, A.Readnum, CONVERT(NVARCHAR(10), A.Writeday, 120) AS Writeday, A.Writer
FROM SiteAnnounce A
WHERE A.Board_ID IN ( SELECT Board_ID 
                      FROM SiteBoardEnv 
                      WHERE SiteCode = " + CFL.Q(GD.SiteCode) + @") 
  and A.SCMUse = 'Y' 
";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }
            finally
            {
                strQuery = null;
            }

            return ds;

        }


        public DataSet GetAnounceMngLoad(object[] GDObj, string strBoardID, string strNum)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT A.Num, A.Title, A.Content, A.Writeday, A.Readnum
     , A.Writer, A.Writer_ID, A.Email, A.PWD, ISNULL(A.Importance, 'N') AS Importance
FROM SiteAnnounce A
WHERE Board_ID = " + CFL.Q(strBoardID) + " AND Num = " + CFL.Q(strNum) + "";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }
            finally
            {
                strQuery = null;
            }

            return ds;

        }

        public DataSet GetAnounceMngNewDataLoad(object[] GDObj, string strUserID)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT '' AS TITLE, 'N' AS IMPORTANCE, '' AS PWD, A.eMail AS EMAIL, A.UserID AS WRITER_ID, '' AS CONTENT
FROM xErpUser A
WHERE A.UserID = " + CFL.Q(strUserID) + @"";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }
            finally
            {
                strQuery = null;
            }

            return ds;

        }


        // ******************************************************************************************************

        // SCM
        public DataSet getSCMID(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT 'N' as UseYn, UserID, UserName
FROM xErpUser
WHERE UserType = 'G'
  and " + CFL.Q(GD.CurDate) + @" between ExcludeBDate and ExcludeEDate
Order By UserName";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }
            finally
            {
                strQuery = null;
            }

            return ds;

        }


        public DataSet getSCMID_Saved(object[] GDObj, string strBoardID, string strNum)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT Case When B.UserID is not null Then 'Y'
            Else 'N' 
       End As UseYn
     , A.UserID
     , A.UserName
FROM xErpUser A
    Left Outer Join SiteAnnounce_SCM B On B.Board_ID = " + strBoardID + " and B.Num = " + strNum + @" and B.UserID = A.UserID
WHERE A.UserType = 'G'
  and " + CFL.Q(GD.CurDate) + @" between A.ExcludeBDate and A.ExcludeEDate
Order By A.UserName";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }
            finally
            {
                strQuery = null;
            }

            return ds;

        }


    }
}