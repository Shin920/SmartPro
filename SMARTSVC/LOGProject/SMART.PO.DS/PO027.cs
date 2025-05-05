using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SMART.PO.DS
{
    public class PO027
    {
        public DataSet Grid(object[] GDObj, string strWhereClause)
        {
            {
                GData GD = new GData(GDObj);

                string strQuery = @"Select A.ReqDate, A.ReqNo, C.DeptName, D.EmpName, E.CcName, F.CaseName
				From poRequisitionHeader A 
					Left Outer Join DeptMaster C on A.DeptCode = C.DeptCode	and C.ComCode = " + CFL.Q(GD.ComCode) +
                    @" Left Outer Join EmpMaster D on A.EmpCode = D.EmpCode and D.ComCode = " + CFL.Q(GD.ComCode) +
                    @" Left Outer Join CcMaster E on A.CcCode = E.CcCode and A.SiteCode = E.SiteCode  and E.ComCode = " + CFL.Q(GD.ComCode) +
                    @" Join CaseAcc F on A.CaseCode = F.CaseCode and F.CaseID = 'poRQ' and F.SysCase = '100' and F.ComCode = " + CFL.Q(GD.ComCode) +
                    strWhereClause;

                //strQuery += " and A.CcCode = " + CFL.Q(strCcCode);

                //strQuery += strWhereClause + " order by A.EmpCode, B.DeptCode, C.SiteCode";

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
}

