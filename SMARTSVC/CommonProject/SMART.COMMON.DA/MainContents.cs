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
    public class MainContents
    {
        public MainContents() { }

        public MainContents(SqlConnection db, SqlTransaction tr)
        {

            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        public ArrayList SetSave(object[] GDObj, string UniqueID, string EmpCode, DateTime StartDate, DateTime EndDate
            , string AllDay, string Subject, string Description, string Label, string Status, string Location)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            if (UniqueID == "")
            {
                strQuery += @"

INSERT INTO ScheduleAppointments
( SiteCode, EmpCode, StartDate, EndDate, AllDay, Subject, Description, Label, Status, Location )
VALUES
( " + CFL.Q(GD.SiteCode) + @"
, " + CFL.Q(EmpCode) + @"
, " + CFL.Q(StartDate.ToString("yyyy-MM-dd HH:mm")) + @"
, " + CFL.Q(EndDate.ToString("yyyy-MM-dd HH:mm")) + @"
, " + CFL.Q(AllDay) + @"
, " + CFL.Q(Subject) + @"
, " + CFL.Q(Description) + @" 
, " + CFL.Q(Label) + @"
, " + CFL.Q(Status) + @"
, " + CFL.Q(Location) + @") ";

            }
            else
            {
                strQuery += @"
UPDATE ScheduleAppointments
SET StartDate = " + CFL.Q(StartDate.ToString("yyyy-MM-dd HH:mm")) + @"
  , EndDate = " + CFL.Q(EndDate.ToString("yyyy-MM-dd HH:mm")) + @"
  , AllDay = " + CFL.Q(AllDay) + @"
  , Subject = " + CFL.Q(Subject) + @"
  , Description = " + CFL.Q(Description) + @"
  , Label = " + CFL.Q(Label) + @"
  , Status = " + CFL.Q(Status) + @"
  , Location = " + CFL.Q(Location) + @"
WHERE UniqueID = " + CFL.Q(UniqueID);
            }

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "11";
                strErrorMsg = "스케줄 등록 시 오류가 발생 하였습니다.";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }

            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }

        public ArrayList SetDelete(object[] GDObj, string UniqueID)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            strQuery += @"
DELETE ScheduleAppointments WHERE UniqueID = " + CFL.Q(UniqueID);

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "11";
                strErrorMsg = "스케줄 등록 시 오류가 발생 하였습니다.";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally
            {
                strQuery = null;
            }

            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }
    }
}
