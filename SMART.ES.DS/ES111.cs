using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SMART.ES.DS
{
    public class ES111
    {
        public ES111() { }

        public DataSet Grid(object[] GDObj, string strBDate, string strEDate, string strBizOffice, string strCsCode, string strItemCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "";

            strQuery = @"
Select IDX
     , IsNull(RTRIM(BizOffice), '') As BizOffice
     , IsNull(RTRIM(AnaDate), '') As AnaDate
     , IsNull(RTRIM(CsCode), '') As CsCode
     , IsNull(RTRIM(ItemCode ), '') As ItemCode
     , PlanQty
     , PlanWgt
     , SalesQty
     , SalesWgt
From [AI].[AI].[dbo].[AnaTarget]
Where 1 = 1
";
            if(strBDate != "")
            {
                if(strEDate != "")
                {
                    strQuery += @"
  And AnaDate Between " + CFL.Q(strBDate) + " And " + CFL.Q(strEDate);
                }
                else
                {
                    strQuery += @"
  And AnaDate >=" + CFL.Q(strBDate);
                }
            }
            else
            {
                if(strEDate != "")
                {
                    strQuery += @"
  And AnaDate <= " + CFL.Q(strEDate);
                }
            }

            if(strBizOffice != "")
            {
                strQuery += @"
  And BizOffice = " + CFL.Q(strBizOffice);
            }

            if(strCsCode != "")
            {
                strQuery += @"
  And CsCode = " + CFL.Q(strCsCode);
            }

            if(strItemCode != "")
            {
                strQuery += @"
  And ItemCode = " + CFL.Q(strItemCode);
            }

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

        public DataSet GetBizOffice(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "";

            strQuery = @"
Select Distinct IsNull(RTRIM(BizOffice), '') As BizOffice
From [AI].[AI].[dbo].[AnaTarget]
Order By BizOffice";
            
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
