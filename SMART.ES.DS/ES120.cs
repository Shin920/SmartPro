using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.ES.DS
{
    public class ES120
    {
        public ES120() { }
        public DataSet Grid(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "";

            strQuery = @"
Select IsNull(RTRIM(ItemCode), '') As ItemCode, TotalSalesQty, TotalSalesWgt, TotalSalesCnt 
From [AI].[AI].[dbo].[AnaTgConfirm]
Order By TotalSalesCnt
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

        public DataSet Grid2(object[] GDObj, string strItemCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = "";

            strQuery = @"
Select IsNull(RTRIM(AnaDate), '') As AnaDate, SalesQty, SalesWgt 
From [AI].[AI].[dbo].[AnaTarget]
Where ItemCode = " + CFL.Q(strItemCode) + @"
Order By AnaDate";

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
