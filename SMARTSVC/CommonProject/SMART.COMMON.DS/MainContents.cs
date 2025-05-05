using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.COMMON.DS
{
    public class MainContents
    {
        public DataSet GetAnounceListLoad_ForMainForm(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery = @"
SELECT TOP 100 ISNULL(A.Importance, 'N') AS Importance
     , A.Num, A.Board_ID, A.Title, A.Readnum, CONVERT(NVARCHAR(10), A.Writeday, 120) AS Writeday, A.Writer
FROM SiteAnnounce A
WHERE A.Board_ID IN (SELECT Board_ID FROM SiteBoardEnv WHERE SiteCode = " + CFL.Q(GD.SiteCode) + @")
  and A.SCMUse is null
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


        public DataSet GetAnounceListLoad_ForMainForm_SCM(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery = @"
SELECT TOP 100 ISNULL(A.Importance, 'N') AS Importance
     , A.Num, A.Board_ID, A.Title, A.Readnum, CONVERT(NVARCHAR(10), A.Writeday, 120) AS Writeday, A.Writer
FROM SiteAnnounce A
    Join SiteAnnounce_SCM B On B.Board_ID = A.Board_ID and B.Num = A.Num and B.UserID = " + CFL.Q(GD.UserID) + @"
WHERE A.Board_ID IN (SELECT Board_ID FROM SiteBoardEnv WHERE SiteCode = " + CFL.Q(GD.SiteCode) + @")
  and A.SCMUse = 'Y'
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


        public DataSet GetExchangeRate(object[] GDObj, string strBDate, string strEDate)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT CurrCode, CONVERT(DATE, ExchDate) AS ExchDate, ExchRate1
FROM ExchangeRate
WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
  AND LEFT(ExchDate, 6) BETWEEN " + CFL.Q(strBDate) + @" AND " + CFL.Q(strEDate) + @"
  AND CurrCode IN('USD', 'EUR', 'JPY')
ORDER BY ExchDate ";


            strQuery += @"
SELECT CONVERT(INT, MIN(ExchRate1)) AS [MIN],  CONVERT(INT, MAX(ExchRate1)) AS [MAX]
FROM ExchangeRate
WHERE ComCode = " + CFL.Q(GD.ComCode) + @"
  AND LEFT(ExchDate, 6) BETWEEN " + CFL.Q(strBDate) + @" AND " + CFL.Q(strEDate) + @"
  AND CurrCode IN('USD', 'EUR', 'JPY')
  AND ExchRate1 > 1 ";



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

        public DataSet GetEmpMasterInfo(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT CONVERT(NVARCHAR(10), CONVERT(DATE, A.BirthDay), 120) AS BirthDay
     , CONVERT(NVARCHAR(10), CONVERT(DATE, A.EmpBDate), 120) AS IpsaDay
	 , A.Email, A.MobileNo, B.DeptCode, B.DeptName
FROM EmpMaster A
LEFT JOIN DeptMaster B ON A.SiteCode = B.SiteCode AND A.DefaultDept = B.DeptCode
WHERE A.SiteCode = " + CFL.Q(GD.SiteCode) + @"
  AND A.EmpCode = " + CFL.Q(GD.EmpCode);

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

        public DataSet GetAppointmentsLoad(object[] GDObj, string strWhereClause)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT [UniqueID]
     , [Type]
     , [StartDate]
     , [EndDate]
     , [AllDay]
     , [Subject]
     , [Location]
     , [Description]
     , [Status]
     , [Label]
     , [ResourceID]
     , [ResourceIDs]
     , [ReminderInfo] 
     , [RecurrenceInfo] 
FROM [dbo].[ScheduleAppointments]
WHERE SiteCode = " + CFL.Q(GD.SiteCode);

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetResourcesLoad(object[] GDObj, string strWhereClause)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT [UniqueID]
	 , [ResourceID]
	 , [ResourceName]
	 , [Color]
	 , [Image]
FROM [dbo].[ScheduleResources]
WHERE SiteCode = " + CFL.Q(GD.SiteCode);

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += strWhereClause;

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetWorkReport(object[] GDObj, string strBDate, string strEDate)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT SUBSTRING(A.EndTime, 1, 4) + '-' + SUBSTRING(A.EndTime, 5, 2) AS ReportDate
     , CONVERT(NUMERIC(16, 2), SUM(ISNULL(A.GoodQty, 0))) AS GoodQty
FROM nppOperationWkReport A
JOIN ItemSiteMaster B ON A.SiteCode= B.SiteCode AND A.ItemCode = B.ItemCode
WHERE A.SiteCode = " + CFL.Q(GD.SiteCode) + @"
  AND LEFT(A.EndTime, 8) BETWEEN " + CFL.Q(strBDate) + @" AND " + CFL.Q(strEDate) + @"
GROUP BY SUBSTRING(A.EndTime, 1, 4) + '-' + SUBSTRING(A.EndTime, 5, 2) ";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }

        public DataSet GetScheduleList(object[] GDObj, string strEmpCode)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = "";

            strQuery += @"
SELECT CONVERT(DATE, StartDate) AS StartDate, CONVERT(DATE, EndDate) AS EndDate
     , [Subject], [Description]
FROM ScheduleAppointments
WHERE SiteCode = " + CFL.Q(GD.SiteCode) + @"
  AND EmpCode = " + CFL.Q(strEmpCode) + @"

ORDER BY StartDate, EndDate";

            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;

        }
    }
}
