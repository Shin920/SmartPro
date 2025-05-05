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
    public class ES110
    {
        public ES110() { }
        public ES110(SqlConnection db, SqlTransaction tr)
        {
            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;


        public ArrayList SaveUploadData(object[] GDObj, DataTable dt)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            
            try
            {
                
                if (!m_bInTrans)
                    CFL.BeginTran(GD, ref m_tr, ref m_db);

                SqlBulkCopy bulk = new SqlBulkCopy(m_db, SqlBulkCopyOptions.Default, m_tr);
                bulk.DestinationTableName = "AnaTarget";
                bulk.WriteToServer(dt);

            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "03";
                strErrorMsg = e.Message;  
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }

        public ArrayList SaveUploadData2(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = @"";

            strQuery = @"
Insert Into [AI].[AI].[dbo].[AnaTarget] (
BizOffice, AnaDate, CsCode, ItemCode, PlanQty, PlanWgt, SalesQty, SalesWgt)
Select * From AnaTarget

Delete From AnaTarget
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
