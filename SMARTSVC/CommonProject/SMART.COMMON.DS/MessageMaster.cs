using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Text;

namespace SMART.COMMON.DS
{
    public class MessageMaster
    {
        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        public MessageMaster() { }

        public DataSet GetAllLangDataLoad(object[] GDObj, string strMCode, string strMGubun)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = @"
SELECT A.LangID, B.LangName, A.Message
FROM MessageMaster A
JOIN LangMaster B ON A.LangID = B.LangID

WHERE MCode = " + CFL.Q(strMCode) + @"
  AND MGubun = " + CFL.Q(strMGubun) + @"
ORDER BY B.SortKey ";

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

        public DataSet GetMessageCodeCheck(object[] GDObj, string strLangID, string strMCode, string strMGubun)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strQuery = @"
SELECT COUNT(*) AS CNT
FROM MessageMaster
WHERE LangID = " + CFL.Q(strLangID) + @"
  AND MCode = " + CFL.Q(strMCode) + @"
  AND MGubun = " + CFL.Q(strMGubun) + @" ";

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

        public ArrayList SetMessageDataSave(object[] GDObj, bool bSaveMode, string strLangID, string strMGubun, string strMCode, string strMessage)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = @"";


            //Insert
            if (bSaveMode)
            {
                strQuery += @"
INSERT INTO MessageMaster
(MCode, MGubun, LangID, Message)
VALUES
( " + CFL.Q(strMCode) + @"
, " + CFL.Q(strMGubun) + @"
, " + CFL.Q(strLangID) + @"
, " + CFL.Q(strMessage) + @" ) ";

            }
            //Update
            else
            {
                strQuery += @"
UPDATE MessageMaster
SET Message = " + CFL.Q(strMessage) + @"
WHERE MCode = " + CFL.Q(strMCode) + @"
  AND MGubun = " + CFL.Q(strMGubun) + @"
  AND LangID = " + CFL.Q(strLangID);

            }

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                if (!m_bInTrans)
                    m_tr.Rollback();

                strErrorCode = "03";
                strErrorMsg = CFL.RS("MSG04", this, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }

        public ArrayList SetMessageDelete(object[] GDObj, string strLangID, string strMGubun, string strMCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = @"";

            strQuery += @"
DELETE MessageMaster WHERE MCode = " + CFL.Q(strMCode) + @" AND MGubun = " + CFL.Q(strMGubun) + @" AND LangID = " + CFL.Q(strLangID);

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                if (!m_bInTrans)
                    m_tr.Rollback();

                strErrorCode = "03";
                strErrorMsg = CFL.RS("MSG04", this, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }

        public DataSet GetFormLangDataLoad(object[] GDObj, string strLangID, string[] sarMCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strMCode = "";

            for (int i = 0; i < sarMCode.Length; i++)
            {
                if (i == 0)
                {
                    strMCode += CFL.Q(sarMCode[i]);
                }
                else
                {
                    strMCode += "," + CFL.Q(sarMCode[i]);
                }
            }

            string strQuery = @"
SELECT MCode, Message
FROM MessageMaster
WHERE LangID = " + CFL.Q(strLangID) + @"
  AND MCode IN (" + strMCode + ")";

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

        public DataSet CreatedCodeNumbering(object[] GDObj, string strLangID, string strMGubun)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
DECLARE @SEQ INT

SELECT @SEQ = ISNULL(MAX(SUBSTRING(MCode, 2, 10)), 0) + 1
FROM MessageMaster A
WHERE A.LangID = " + CFL.Q(strLangID) + @"
  AND A.MGubun = " + CFL.Q(strMGubun) + @"

SELECT " + CFL.Q(strMGubun) + @" + REPLICATE('0', 5 - LEN(@SEQ)) + CAST(@SEQ AS VARCHAR) AS MCode ";

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

        public string Msg(object[] GDObj, string strLangID, string strMsgCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            ArrayList alData = new ArrayList();
            string strQuery = string.Empty;
            string strErrorCode = "00", strErrorMsg = "";

            strQuery = @"
                	Select A.Message
	                  From MessageMaster A
	                  JOIN LangMaster B ON A.LangID = B.LangID
	                 Where MCode = " + CFL.Q(strMsgCode) + @"
	                   And A.LangID = " + CFL.Q(strLangID) + @"
            ";

            try
            {
                alData = CFL.EncodeData(strErrorCode, strErrorMsg, CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text), null);
            }
            catch
            {
                return "";
            }

            if (alData[0].ToString() != "00")
            {
                return "";
            }
            else if (CFL.Toi(alData[3]) == 0)
            {
                return "";
            }
            else
            {
                return alData[4].ToString();
            }
        }

        public DataSet MsgSet(object[] GDObj, string strLangID, string strMsgCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            ArrayList alData = new ArrayList();
            string strQuery = string.Empty;
            string strErrorCode = "00", strErrorMsg = "";
            string[] sarMsgCode = strMsgCode.Split(';');
            strQuery = @"
Select 
    rtrim(a.MCode) as MCode
    , rtrim(A.Message) as Message
From MessageMaster A
JOIN LangMaster B ON A.LangID = B.LangID
Where A.LangID = " + CFL.Q(strLangID) + @" 
    and MCode in(";
            for (int x = 0; x < sarMsgCode.Length; x++)
            {
                if (x == 0)
                {
                    strQuery += CFL.Q(sarMsgCode[x].Trim());
                }
                else
                {
                    strQuery += ", " + CFL.Q(sarMsgCode[x].Trim());
                }
            }
            strQuery += @" )";
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
            DataColumn[] dtkey = new DataColumn[1];
            dtkey[0] = ds.Tables[0].Columns["MCode"];
            ds.Tables[0].PrimaryKey = dtkey;

            return ds;
        }

        public string MsgText(DataSet ds, string strCode)
        {
            DataRow dr = ds.Tables[0].Rows.Find(strCode);
            if (dr.ItemArray.Length == 0)
                return "";
            else
                return dr[1].ToString();
        }
    }
}
