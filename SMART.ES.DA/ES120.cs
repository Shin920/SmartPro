using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace SMART.ES.DA
{
    public class ES120
    {
        public ES120() { }
        public ES120(SqlConnection db, SqlTransaction tr)
        {
            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        public ArrayList SaveAnaTgConfirm(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = @"";

            strQuery = @"
Delete From [AI].[AI].[dbo].[AnaTgConfirm]

Insert Into [AI].[AI].[dbo].[AnaTgConfirm]
Select RTRIM(ItemCode) As ItemCode, Sum(SalesQty) As TotalSalesQty, SUM(SalesWgt) As TotalSalesWgt, Count(*) As TotalSalesCnt
From [AI].[AI].[dbo].[AnaTarget]
Where ItemCode In ( 
Select CodeName From CodeMaster Where CodeID = 'AiTgItem')
Group By ItemCode
Order By Count(*) Desc
";
            try
            {
                CFL.ExecuteNonQuery(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "03";
                strErrorMsg = e.Message;
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }
    }
}
