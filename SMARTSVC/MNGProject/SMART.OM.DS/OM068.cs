using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.OM.DS
{
    public class OM068
    {
        public DataSet Grid(object[] GDObj, string strWhereClause)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
select WhCode, WhName from WhMaster";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += " where " + strWhereClause;

            strQuery += " Order by WhCode ";
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

        public DataSet Load(object[] GDObj, string strWhCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
			select WhCode, WhName from WhMaster
			where ComCode = " + CFL.Q(GD.ComCode) + " and SiteCode = " + CFL.Q(GD.SiteCode) + " and WhUse = N'Y'" + " and WhCode = " + CFL.Q(strWhCode);

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

        public DataSet Grid2(object[] GDObj, string strWhereClause)
        {
            // Global Data
            GData GD = new GData(GDObj);


            string strQuery = @"
select 'N' as Chk, Rtrim(A.LocationCode) as LocationCode, RTrim(A.LocationName) as LocationName, A.UseYN , 'N' as SaveCheck
from LocationMaster A
JOIN WhMaster B on a.SiteCode = b.SiteCode AND a.WhCode = b.WhCode";

            if (strWhereClause != null && 0 != strWhereClause.Length)
            {
                strQuery += " where " + strWhereClause;
                strQuery += " AND A.SiteCode =" + CFL.Q(GD.SiteCode);
                strQuery += " AND B.ComCode =" + CFL.Q(GD.ComCode);
            }


            strQuery += " Order by A.LocationCode ";

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
