using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.OM.DA
{
    public class OM068
    {
        public OM068() { }

        public OM068(SqlConnection db, SqlTransaction tr)
        {
            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        SMART.COMMON.DS.MessageMaster Message = new COMMON.DS.MessageMaster();

        public ArrayList Save(object[] GDObj, bool bSaveMode, string strLangID, string strWhCode
                            , string[] sarLocationCode, string[] sarLocationName, string[] sarUseYN)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            DataTableReader dr = null;

            ArrayList alResult = new ArrayList();

            // ********Header Validation Check

            // Not Null Check
            // WhCode
            if (!CFL.NotNullCheck(Message.Msg(GDObj, GD.LangID, "L00310"), strWhCode, strLangID, ref alResult, "OFtxtWhCode"))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            if (sarLocationCode.Length == 0)
            {
                return CFL.EncodeData("01", Message.Msg(GDObj, GD.LangID, "M00090"), null);
            }



            // ************* 2.Item Validation Check *****************
            for (int i = 0; i < sarLocationCode.Length; i++)
            {


                // LocationCode
                if (!CFL.NotNullCheck((i + 1) + Message.Msg(GDObj, GD.LangID, "L00311"), sarLocationCode[i], GD.LangID, ref alResult))//, "OFGrid_ItemName" + i ) )
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }

                // LocationName
                if (!CFL.NotNullCheck((i + 1) + Message.Msg(GDObj, GD.LangID, "L00312"), sarLocationName[i], GD.LangID, ref alResult))//, "OFGrid_ItemName" + i ) )
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }



                // 다른 창고에 등록된 코드가 있는지 확인..  ------------------------------------------------------

                strQuery = @"  				
Select LocationCode
From LocationMaster
Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
  and LocationCode = " + CFL.Q(sarLocationCode[i]) + @"
  and WhCode != " + CFL.Q(strWhCode);

                try
                {
                    dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
                }
                catch (Exception e)
                {
                    dr.Close();

                    strErrorCode = "01";
                    strErrorMsg = Message.Msg(GDObj, GD.LangID, "M00268");  // 유효성 체크에 실패하였습니다.
                    return CFL.EncodeData(strErrorCode, strErrorMsg, e);
                }

                // 만약 존재하면...
                if (dr.Read())
                {
                    strErrorCode = "02";
                    strErrorMsg = (i + 1) + Message.Msg(GDObj, GD.LangID, "M00269"); // 이미 등록된 보관장소 Code 가 있습니다.

                    dr.Close();  // Return 전에...닫는다.

                    return CFL.EncodeData(strErrorCode, strErrorMsg, null);
                }

                dr.Close();

                // --------------------------------------------------------------------------------------------------


                for (int j = i + 1; j < sarLocationCode.Length; j++)
                {
                    if (sarLocationCode[i] == sarLocationCode[j])
                    {
                        if (m_bInTrans)
                            m_tr.Rollback();

                        return CFL.EncodeData("01", (j + 1) + Message.Msg(GDObj, GD.LangID, "M00270"), null);
                    }
                }
            }




            if (0 < sarLocationCode.Length)
            {
                // Item Insert
                for (int i = 0; i < sarLocationCode.Length; i++)
                {                                          
                        strQuery += @"
                    
                            Delete from LocationMaster 
                            Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
                              and WhCode= " + CFL.Q(strWhCode) + " and LocationCode = " + CFL.Q(sarLocationCode[i]);

                        strQuery += @"

					        insert into LocationMaster(SiteCode, WhCode, LocationCode, LocationName, LocationDesc, UseYN)
					            values ( " + CFL.Q(GD.SiteCode) + "," + CFL.Q(strWhCode) + "," + CFL.Q(sarLocationCode[i]) + "," + CFL.Q(sarLocationName[i])
                                           + ", " + CFL.Q("") + ", " + CFL.Q(sarUseYN[i]) + ")" + @"
                        ";
                }
            }

            try
            {
                CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                strErrorCode = "03";
                strErrorMsg = Message.Msg(GDObj, GD.LangID, "M00150");
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Transaction Commit
            if (!m_bInTrans)
                m_tr.Commit();

            // Return DocNo when Success
            return CFL.EncodeData(strErrorCode, strWhCode, null);
        }

    }
}
