using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SMART.COMMON.DA
{
    public class Document
    {
        public Document() { }

        public Document(SqlConnection db, SqlTransaction tr)
        {

            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        public ArrayList Save(object[] GDObj, bool bSaveMode, string strCurEmp, string strLangID, string strDocuNo, string[] sarDocuData)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            // There's No data then return
            if (null == sarDocuData || 1 > sarDocuData.Length)
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);

            string strQuery = "";

            // ********Header Validation Check
            ArrayList alResult = new ArrayList();

            // Not Null Check
            // DocuNo
            if (!CFL.NotNullCheck("문서첨부번호", strDocuNo, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            /*
            // there's no data then success
            if ( sarDocuData.Length == 2 )
                return CFL.EncodeData ( strErrorCode, strDocuNo, null );
            */

            // Make Query
            // delete existing data
            strQuery = @"
            Delete from DocuAttach 
            Where NoType = " + CFL.Q(sarDocuData[0]) + @" 
              and DocuNo = " + CFL.Q(strDocuNo);


            for (int i = 0; i < Convert.ToInt32(sarDocuData[1]); i++)
            {
                // 오류나서 막음
                /*
                if (sarDocuData[5 + i * 4].Equals("True"))
                    continue;
                */

                strQuery += @"
					insert into DocuAttach ( NoType, DocuNo, DocuSerNo, DocuTitle, DocuDescr, DocuFileName )
					values ( " + CFL.Q(sarDocuData[0]) + ", " + CFL.Q(strDocuNo) + ", " + i
                        + ", " + CFL.Q(sarDocuData[2 + i * 4]) + ", " + CFL.Q(sarDocuData[3 + i * 4])
                        + ", " + CFL.Q(sarDocuData[4 + i * 4]) + " )";
            }


            // create command object with Transaction
            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "01";
                strErrorMsg = "문서첨부정보 저장에 실패하였습니다.";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Transaction Commit
            if (!m_bInTrans)
                m_tr.Commit();

            // Return DocNo when Success
            return CFL.EncodeData(strErrorCode, strDocuNo, null);
        }

        public ArrayList Delete(object[] GDObj, string strLangID, string strNoType, string strDocuNo)
        {
            // Global Data
            GData GD = new GData(GDObj);


            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            // ********Header Validation Check
            ArrayList alResult = new ArrayList();

            // Not Null Check
            // DocuNo
            if (!CFL.NotNullCheck("문서첨부번호", strDocuNo, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            // Make Query
            // delete existing data
            strQuery = @"
            Delete From DocuAttach 
            Where NoType = " + CFL.Q(strNoType) + @" 
              and DocuNo = " + CFL.Q(strDocuNo);

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);


                // 첨부파일 폴더 삭제            
                string strPath = System.Configuration.ConfigurationSettings.AppSettings["ResourceRoot"] + "\\Document\\" + GD.ClientID + "\\" + strNoType + "\\" + strDocuNo;

                DirectoryInfo DirInfo = new DirectoryInfo(strPath);

                if (DirInfo.Exists == true)
                {
                    DirInfo.Delete(true);
                }

            }
            catch (Exception e)
            {
                m_tr.Rollback();

                strErrorCode = "01";
                strErrorMsg = "문서첨부정보 삭제에 실패하였습니다.";
                return CFL.EncodeData(strErrorCode, strQuery, e);
            }

            // Transaction Commit
            if (!m_bInTrans)
                m_tr.Commit();


            // Return DocNo when Success
            return CFL.EncodeData(strErrorCode, strDocuNo, null);
        }
    }
}
