using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.PO.DS
{
    public class PO026
    {
        public DataSet Grid(object[] GDObj, string strWhereClause)
        {
            {
                GData GD = new GData(GDObj);
                             
                string strQuery = @"	Select A.ItemCode, isnull(A.ItemName, B.ItemName) as ItemName
						,isnull(A.ItemSpec, B.ItemSpec) as ItemSpec
						,A.UnitCode, ReqQty
                        ,Convert(Date, InReqDate) AS InReqDate, A.PurposeOfUse, 'N' as SaveCheck
                        From poRequisitionItem A 
						Left Outer Join ItemMaster B on A.ItemCode = B.ItemCode and B.ComCode = " + CFL.Q(GD.ComCode) +
                        @" Left Outer Join CsMaster C on A.CsCode = C.CsCode and C.ComCode = " + CFL.Q(GD.ComCode) +
                        @" Left Outer Join ManageGroup D on A.PoGroup = D.MngGroup and D.MngType = N'PO' and A.SiteCode = " + CFL.Q(GD.SiteCode) +
                        @" Left Outer Join ItemSiteMaster E on A.SiteCode = E.SiteCode AND A.ItemCode = E.ItemCode"
                        + strWhereClause;

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
