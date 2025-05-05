using System;
using System.Data;

namespace SMART.OM.DS
{
    public class OM034
    {
        public DataSet Grid(object[] GDObj, string strWhereClause)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"select 
				A.EmpCode, A.EmpName, B.DeptCode, B.DeptName, A.SiteCode, C.SiteName, A.HourCost
				from EmpMaster A, DeptMaster B, SiteMaster C  "
                + strWhereClause
                + " order by A.EmpCode, B.DeptCode, C.SiteCode ";

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

        public DataSet Load_OM034(object[] GDObj, string strEmpCode, string strSiteCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
Select A.EmpCode, A.EmpName, A.SiteCode, B.DeptCode, B.DeptName
     , A.EmpBdate, A.EmpEdate, A.Email, A.MobileNo, A.InoutAdmin, A.HourCost
     , A.JobWork
From EmpMaster A, DeptMaster B , SiteMaster C
Where A.ComCode= " + CFL.Q(GD.ComCode) + @"
  and B.ComCode = " + CFL.Q(GD.ComCode) + @"
  and A.ComCode = B.ComCode
  and A.DefaultDept = B.DeptCode  
  and A.SiteCode = C.SiteCode
  and A.EmpCode = " + CFL.Q(strEmpCode);

            if (strSiteCode != "")
                strQuery += " and C.SiteCode = " + CFL.Q(strSiteCode);
            else
                strQuery += " and  C.SiteCode = " + CFL.Q(GD.SiteCode);

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
