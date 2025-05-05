using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace SMART.OM.DA
{
    public class OM034
    {
        public OM034() { }

        public OM034(SqlConnection db, SqlTransaction tr)
        {

            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        SMART.COMMON.DS.MessageMaster Message = new COMMON.DS.MessageMaster();

        public ArrayList set_SaveChk(object[] GDObj, string strEmpCode, string strMobileNo, string strEmail)
        {
            // Global Data
            GData GD = new GData(GDObj);


            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
Select EmpCode, EmpName
From EmpMaster A
Where A.ComCode= " + CFL.Q(GD.ComCode);

            if (strMobileNo != "" && strMobileNo != null)
                strQuery += " and a.MobileNo = " + CFL.Q(strMobileNo);
            if (strEmail != "" && strEmail != null)
                strQuery += " and  a.Email = " + CFL.Q(strEmail);
            if (strEmpCode != "" && strEmpCode != null)
                strQuery += " and  A.EmpCode = " + CFL.Q(strEmpCode);


            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery.ToString(), CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("MSG01", this, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        public ArrayList Save(object[] GDObj, bool bSaveMode, string strEmpCode, string strEmpName, string strSiteCode, string strDeptCode, string strEmpBDate, string strEmpEDate
                            , string strEmailID, string strTelNo, string strInoutAdmin, string strHourcost, string strJobWork)
        {
            // Global Data
            GData GD = new GData(GDObj);

            //DataTableReader dr = null;


            // 계속 사용하게 될 ArrayList
            ArrayList alResult = new ArrayList();

            if (!CFL.NotNullCheck(Message.Msg(GDObj, GD.LangID, "L00020"), strDeptCode, GD.LangID, ref alResult, ""))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            if (!CFL.NotNullCheck(Message.Msg(GDObj, GD.LangID, "L00023"), strEmpName, GD.LangID, ref alResult, ""))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            if (!CFL.NotNullCheck(Message.Msg(GDObj, GD.LangID, "L00308"), strSiteCode, GD.LangID, ref alResult, ""))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            if (strEmpEDate != "")
            {
                // Check 부분 생략 했음. 
                if (!CFL.PeriodValidate(Message.Msg(GDObj, GD.LangID, "L00028"), strEmpBDate, Message.Msg(GDObj, GD.LangID, "L01144"), strEmpEDate, GD.LangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
            }
            // Header insert mode
            string strQuery = "";

            strQuery = @" 
declare @UserID  nchar(20)
    , @EmpCode   nchar(16)  
set @EmpCode  = " + CFL.Q(strEmpCode);

            if (!bSaveMode)
            {
                strQuery += @"

select @UserID = UserID from EmpMaster where ComCode = " + CFL.Q(GD.ComCode) + " and EmpCode = " + CFL.Q(strEmpCode) + @"

delete from EmpMaster where ComCode = " + CFL.Q(GD.ComCode) + " and EmpCode = " + CFL.Q(strEmpCode);
            }

            // header insert
            strQuery += @"
if(isnull(@EmpCode ,'') ='')
begin
    select  @EmpCode = max(cast(EmpCode as int)) + 1
    from EmpMaster a 
    where EmpCode not in ('9999', '99999', 'bogoUser', 'K001', 'K002')
    select @EmpCode 
	    = case when len(rtrim(@EmpCode)) = 5 then cast(@EmpCode as nvarchar(5))
		    when len(rtrim(@EmpCode)) = 4 then '0'+ cast(@EmpCode as nvarchar(4))
		    when len(rtrim(@EmpCode)) = 3 then '00'+ cast(@EmpCode as nvarchar(3))
		    when len(rtrim(@EmpCode)) = 2 then '000'+ cast(@EmpCode as nvarchar(2))
		    when len(rtrim(@EmpCode)) = 1 then '0000'+ cast(@EmpCode as nvarchar(1)) end
end

Insert into EmpMaster 
(	
    ComCode, EmpCode, EmpName, SiteCode, DefaultDept, IdentityID, Gender, BirthDay,	SolarCheck,	JobCategory
    , JobPosition, JobDuty, JobRank, JobWork, SalClassCode, PaySystemCode, LayoffInsureCheck, UnionJoinCheck, EmpBDate, EmpEDate
    , Email, MobileNo, EmpStatus, EnterType, EmpNameEng, EmpNameChina, CurrentZipCode, CurrentAddress, LegalZipCode, LegalAddress
    , HomeTown, NationCode, TelNoPerson, TelNoOutOffice, TelNoInOffice, PatriotCheck, PatriotDescr, MarriageCheck, WeddingDay, CarOwnerCheck
    , CarGubun, CarDescr, PassportNo, PassportValidDate, PersonReligion, PersonHobby, PersonTalent, PersonStature, PersonWeight, EyeSightLeft
    , EyeSightRight, ExtraUnique1, ExtraUnique2, ExtraUnique3, BloodType, RhType, ColorBlind, EmpDescr, ArmyPart, ArmyGubun
    , ArmyBYyyymm, ArmyEYyyymm, ArmyRank, ArmyJob, NotArmyDescr, LivingType, LivingArea, LivingPrice, UserID, InoutAdmin 
    , HourCost
) 
Values 
(
    " + CFL.Q(GD.ComCode) + @", @EmpCode, " + CFL.Q(strEmpName) + ", " + CFL.Q(strSiteCode) + " , " + CFL.Q(strDeptCode) + @", null, 'M', '20501231', 'S', 0000
    , 0000, 0000, null, " + CFL.Q(strJobWork) + ", null, null, 'N', 'N', " + CFL.Q(strEmpBDate) + ", isnull(" + CFL.Q(strEmpEDate) + @", '')
    , " + CFL.Q(strEmailID) + ", " + CFL.Q(strTelNo) + @", 0000, null, null, null, null, null, null, null
    , null, null, null, null, null, 'N', null, null, null, null
    , null, null, null, null, null, null, null, null, null, null
    , null, null, null, null, null, null, null, null,null, null
    , null, null, null, null, null, null, null, null, @UserID, " + CFL.Q(strInoutAdmin) + @"
    , " + CFL.Tod(strHourcost) + @" 
)";

            try
            {
                CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                return CFL.EncodeData("03", Message.Msg(GDObj, GD.LangID, "L00150"), e);
            }

            // Transaction Commit
            if (!m_bInTrans)
                m_tr.Commit();

            // Return DocNo when Success
            return CFL.EncodeData("00", strEmpCode, null);
        }

        public ArrayList Delete(object[] GDObj, string strEmpCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
Delete From EmpMaster 
Where ComCode = " + CFL.Q(GD.ComCode) + @"
--and SiteCode = " + CFL.Q(GD.SiteCode) + @" 
  and EmpCode = " + CFL.Q(strEmpCode);

            try
            {
                CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                // 삭제에 실패하였습니다
                return CFL.EncodeData("01", Message.Msg(GDObj, GD.LangID, "L00163"), e);
                //  return CFL.EncodeData ( "01", CFL.RS ( "01", this, GD.LangID ), e );
            }

            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData("00", "", null);
        }

    }
}
