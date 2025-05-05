using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.COMMON.DA
{
    public class NoMaster
    {

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;
        

        public NoMaster()
        {

        }

        public NoMaster(SqlConnection db, SqlTransaction tr)
        {
            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

       

        public ArrayList NoCreate(object[] GDObj, string strSiteCode, string strNoType, string strYyyymmdd)
        {

            // Global Data
            GData GD = new GData(GDObj);


            string strErrorCode = "00", strErrorMsg = "";

          
            string strQuery = string.Format(@"

exec spCreateNo {0}, {1}, {2}", CFL.Q(strSiteCode), CFL.Q(strNoType), CFL.Q(strYyyymmdd));


            if (!m_bInTrans)
                CFL.BeginTran(GD, ref m_tr, ref m_db);


            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReaderTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG03", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            if (m_bInTrans)
                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            else
                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null, m_tr);

        }

        public ArrayList CreateLotNo(object[] GDObj, string strSiteCode, string strNoType, string strYyyymmdd, string strItemCode)
        {

            // Global Data
            GData GD = new GData(GDObj);


            string strErrorCode = "00", strErrorMsg = "";

            
            string strQuery = string.Format(@"

exec spCreateLotNo {0}, {1}, {2}, {3}", CFL.Q(strSiteCode), CFL.Q(strNoType), CFL.Q(strYyyymmdd), CFL.Q(strItemCode));


            if (!m_bInTrans)
                CFL.BeginTran(GD, ref m_tr, ref m_db);


            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReaderTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG03", this, GD.LangID);

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            if (m_bInTrans)
                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
            else
                return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null, m_tr);

        }


    }
}
