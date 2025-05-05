using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using SMART.COMMON.DS;
using System.Data.SqlClient;

namespace SMART.OM.DA
{
    public class OM044
    {
        public OM044() { }
        public OM044(SqlConnection db, SqlTransaction tr)
        {

            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        MessageMaster Message = new MessageMaster();

        public ArrayList Delete(object[] GDObj, string strCsCode)
        {

            string strQuery = "";

            // Global Data
            GData GD = new GData(GDObj);

            string strMsg = "M00523";
            DataSet ds = Message.MsgSet(GDObj, GD.LangID, strMsg);

            //거래처 삭제
            strQuery = @"
Delete From CsMaster 
Where ComCode = " + CFL.Q(GD.ComCode) + @" 
  and CsCode = " + CFL.Q(strCsCode);

            //거래처별 담당자정보 삭제
            strQuery += @"

DELETE FROM CsMasterPerson
WHERE ComCode = " + CFL.Q(GD.ComCode) + @" 
  AND SiteCode = " + CFL.Q(GD.SiteCode) + @" 
  AND CsCode =  " + CFL.Q(strCsCode);

            try
            {
                CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                
                return CFL.EncodeData("03", Message.MsgText(ds, "M00523"), e);  // 삭제 중 오류가 발생 하였습니다.
            }

            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData("00", strCsCode, null);
        }

        


        public ArrayList Save_csMaster(object[] GDObj, bool bSaveMode
                , string strCsCode, string strCsName, string strCsType, string strCsGubun, string strArCheck
                , string strApCheck, string strOutCheck, string strCsNameFull, string strForeignCheck, string strComCheck
                , string strCsUse, string strRegNo, string strNationCode, string strRegionCode, string strParentCs
                , string strCsIndustry, string strCsItems, string strCsAddress, string strCsZip, string strCsChief
                , string strCsTel, string strCsFax, string strCsUrl, string strCsEmail, string strTaxCodeAr
                , string strTaxCodeAp, string strPayConditionAr, string strPayConditionAp, string strCurrCode, string strCsGrade
                , string strCreditGrade, string strIncoTerm, string strLeadTime, string strTransCode, string strSendMethod
                , string strTranTelNo, string strTranFax, string strTranEmail, string strBankCode, string strAccountNo
                , string strCsDescr, string strcsExtraFieldName1, string strcsExtraFieldName2, string strcsExtraFieldName3, string strCsClass1
                , string strCsClass2, string strTrus_Direction, string strTrus_Use, string strEmpCode, string strInitial
                , string[] sarEmpName, string[] sarEmpTell, string[] sarEmpFaxNo, string[] saremailID, string[] sarEmpBego
            )
        {

            string strQuery = "";

            // Global Data
            GData GD = new GData(GDObj);

            string strMsg = "M00485;M01010;L01243;L01918;L02370;L01475;L00068;L01882;L01477;L00050;M00453;L01892;L01893;L01898;M00150";

            DataSet ds = Message.MsgSet(GDObj, GD.LangID, strMsg);

            // Validation Check
            ArrayList alResult = new ArrayList();

            DataTableReader dr = null;

            // Form ID Uniqueness
            if (bSaveMode)
            {
                strQuery = @"select * from CsMaster where ComCode = " + CFL.Q(GD.ComCode) + " and CsCode = " + CFL.Q(strCsCode);

                try
                {
                    if (m_bInTrans)
                        dr = CFL.ExecuteDataTableReaderTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db);
                    else
                        dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
                }
                catch (Exception e)
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return CFL.EncodeData("01", Message.MsgText(ds, "M00485"), e); // 거래처 정보의 유효성 체크를 할 수 없습니다.
                }

                // there's any
                if (dr.Read())
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return CFL.EncodeData("02", Message.MsgText(ds, "M01010"), null); //동일한 거래처코드가 이미 존재합니다.
                }
                dr.Close();
                //cm = null;
            }

            // Not Null Check
            // CsCode
            if (!CFL.NotNullCheck(Message.MsgText(ds, "L01243"), strCsCode, GD.LangID, ref alResult, "OFtxtCsCode")) //거래처코드
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // EmpCode
            if (!CFL.NotNullCheck(Message.MsgText(ds, "L01918"), strCsName, GD.LangID, ref alResult, "OFtxtCsName")) //거래처약칭
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            if (!CFL.NotNullCheck(Message.MsgText(ds, "L02370"), strCsNameFull, GD.LangID, ref alResult, "OFtxtCsNameFull")) //거래처명칭
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // TaxCode
            if (!CFL.NotNullCheck(Message.MsgText(ds, "L01475"), strCsType, GD.LangID, ref alResult)) //거래처유형
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            if (!CFL.NotNullCheck(Message.MsgText(ds, "L00068"), strCsGubun, GD.LangID, ref alResult)) //거래처구분
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            if (!CFL.NotNullCheck(Message.MsgText(ds, "L01882"), strForeignCheck, GD.LangID, ref alResult)) //해외여부
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            if (!CFL.NotNullCheck(Message.MsgText(ds, "L01477"), strComCheck, GD.LangID, ref alResult)) //법인여부
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            
            if (!CFL.NotNullCheck(Message.MsgText(ds, "L00050"), strNationCode, GD.LangID, ref alResult, "OFtxtNationCode")) //국가코드
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
                        

            if (strRegNo.Length > 20)
            {
                if (m_bInTrans)
                    m_tr.Rollback();

                return CFL.EncodeData("01", Message.MsgText(ds, "M00453"), null);  // 등록번호는 20자를 넘지않아야 합니다
            }

            if (strArCheck == "Y")
            {
                if (!CFL.NotNullCheck(Message.MsgText(ds, "L01892"), strTaxCodeAr, GD.LangID, ref alResult)) //과세구분(매출)
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
            }

            if (strApCheck == "Y" || strOutCheck == "Y")
            {
                if (!CFL.NotNullCheck(Message.MsgText(ds, "L01893"), strTaxCodeAp, GD.LangID, ref alResult)) //과세구분(매입)
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
            }

            if (!CFL.NotNullCheck(Message.MsgText(ds, "L01898"), strCurrCode, GD.LangID, ref alResult)) //결제통화
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }


            if (strLeadTime == "" || strLeadTime == null)
                strLeadTime = "0";

            if (strSendMethod == "" || strSendMethod == null)
                strSendMethod = "PRNT";


            // 트랜잭션 걸고 쿼리 날림
            /*
            if (!m_bInTrans)
            {
                m_tr = m_db.BeginTransaction();
            }
            */

            if (strParentCs == "")
                strParentCs = strCsCode;

            // insert mode
            if (bSaveMode)
            {
                // header insert 
                strQuery = @"
Insert into CsMaster 
( 
    ComCode, CsCode, CsName, CsType, CsGubun
    , ArCheck, ApCheck, OutCheck, CsNameFull, ForeignCheck
    , ComCheck, CsUse, RegNo, NationCode, RegionCode
    , ParentCs, CsIndustry, CsItems, CsAddress, CsZip
    , CsChief, CsTel, CsFax, CsUrl, CsEmail
    , TaxCodeAr, TaxCodeAp, PayConditionAr, PayConditionAp, CurrCode
    , CsGrade, CreditGrade, IncoTerm, LeadTime, TransCode
    , SendMethod, TranPartner, TranTelNo, TranFax, TranEmail
    , BankCode, AccountNo, CsDescr , ExtraField1 ,ExtraField2 
    , ExtraField3, CsClass1, CsClass2, TrusBill_Status, TrusBill_YN
    , empCode, CsInitial
)  
Values 
( 
    " + CFL.Q(GD.ComCode) + ", " + CFL.Q(strCsCode) + ", " + CFL.Q(strCsName) + ", " + CFL.Q(strCsType) + ", " + CFL.Q(strCsGubun) + @"
    , " + CFL.Q(strArCheck) + ", " + CFL.Q(strApCheck) + ", " + CFL.Q(strOutCheck) + ", " + CFL.Q(strCsNameFull) + ", " + CFL.Q(strForeignCheck) + @"
    , " + CFL.Q(strComCheck) + ", " + CFL.Q(strCsUse) + ", " + CFL.Q(strRegNo) + ", " + CFL.Q(strNationCode) + ", " + CFL.Q(strRegionCode) + @"
    , " + CFL.Q(strParentCs) + ", " + CFL.Q(strCsIndustry) + ", " + CFL.Q(strCsItems) + ", " + CFL.Q(strCsAddress) + ", " + CFL.Q(strCsZip) + @"
    , " + CFL.Q(strCsChief) + ", " + CFL.Q(strCsTel) + ", " + CFL.Q(strCsFax) + ", " + CFL.Q(strCsUrl) + ", " + CFL.Q(strCsEmail) + @"
    , '" + CFL.T(strTaxCodeAr) + "' , '" + CFL.T(strTaxCodeAp) + "', " + CFL.Q(strPayConditionAr) + ", " + CFL.Q(strPayConditionAp) + ", " + CFL.Q(strCurrCode) + @"
    , " + CFL.Q(strCsGrade) + ", " + CFL.Q(strCreditGrade) + ", " + CFL.Q(strIncoTerm) + ", " + CFL.Q(strLeadTime) + ", " + CFL.Q(strTransCode) + @"
    , " + CFL.Q(strSendMethod) + ", null "			// + CFL.Q( strTranFMES) 
                + ", " + CFL.Q(strTranTelNo) + ", " + CFL.Q(strTranFax) + ", " + CFL.Q(strTranEmail) + @"
    , " + CFL.Q(strBankCode) + ", " + CFL.Q(strAccountNo) + ", " + CFL.Q(strCsDescr) + " ," + CFL.Q(strcsExtraFieldName1) + " ," + CFL.Q(strcsExtraFieldName2) + @"
    , " + CFL.Q(strcsExtraFieldName3) + ", " + CFL.Q(strCsClass1) + ", " + CFL.Q(strCsClass2) + ", " + CFL.Q(strTrus_Direction) + ", " + CFL.Q(strTrus_Use) + @"
    , " + CFL.Q(strEmpCode) + ", " + CFL.Q(strInitial) + @"
)";
            }
            else
            {
                // Delete
                strQuery = @" 
Delete CsMaster Where ComCode = " + CFL.Q(GD.ComCode) + @" and  CsCode = " + CFL.Q(strCsCode) + @"
Delete CsMaster_Emp Where ComCode = " + CFL.Q(GD.ComCode) + @" and  CsCode = " + CFL.Q(strCsCode) + @" ";

                // insert
                strQuery += @"
Insert into CsMaster 
( 
    ComCode, CsCode, CsName, CsType, CsGubun
    , ArCheck, ApCheck, OutCheck, CsNameFull, ForeignCheck
    , ComCheck, CsUse, RegNo, NationCode, RegionCode
    , ParentCs, CsIndustry, CsItems, CsAddress, CsZip
    , CsChief, CsTel, CsFax, CsUrl, CsEmail
    , TaxCodeAr, TaxCodeAp, PayConditionAr, PayConditionAp, CurrCode
    , CsGrade, CreditGrade, IncoTerm, LeadTime, TransCode
    , SendMethod, TranPartner, TranTelNo, TranFax, TranEmail
    , BankCode, AccountNo, CsDescr , ExtraField1 ,ExtraField2 
    , ExtraField3, CsClass1, CsClass2, TrusBill_Status, TrusBill_YN     
    , EmpCode, CsInitial
)
Values 
( 
    " + CFL.Q(GD.ComCode) + ", " + CFL.Q(strCsCode) + ", " + CFL.Q(strCsName) + ", " + CFL.Q(strCsType) + ", " + CFL.Q(strCsGubun) + @"
    , " + CFL.Q(strArCheck) + ", " + CFL.Q(strApCheck) + ", " + CFL.Q(strOutCheck) + ", " + CFL.Q(strCsNameFull) + ", " + CFL.Q(strForeignCheck) + @"
    , " + CFL.Q(strComCheck) + ", " + CFL.Q(strCsUse) + ", " + CFL.Q(strRegNo) + ", " + CFL.Q(strNationCode) + ", " + CFL.Q(strRegionCode) + @"
    , " + CFL.Q(strParentCs) + ", " + CFL.Q(strCsIndustry) + ", " + CFL.Q(strCsItems) + ", " + CFL.Q(strCsAddress) + ", " + CFL.Q(strCsZip) + @"
    , " + CFL.Q(strCsChief) + ", " + CFL.Q(strCsTel) + ", " + CFL.Q(strCsFax) + ", " + CFL.Q(strCsUrl) + ", " + CFL.Q(strCsEmail) + @"
    , '" + CFL.T(strTaxCodeAr) + "', '" + CFL.T(strTaxCodeAp) + "', " + CFL.Q(strPayConditionAr) + ", " + CFL.Q(strPayConditionAp) + ", " + CFL.Q(strCurrCode) + @"
    , " + CFL.Q(strCsGrade) + ", " + CFL.Q(strCreditGrade) + ", " + CFL.Q(strIncoTerm) + ", " + CFL.Q(strLeadTime) + ", " + CFL.Q(strTransCode) + @"
    , " + CFL.Q(strSendMethod) + ", null "			// + CFL.Q( strTranFMES)
                        + ", " + CFL.Q(strTranTelNo) + ", " + CFL.Q(strTranFax) + ", " + CFL.Q(strTranEmail) + @"
    , " + CFL.Q(strBankCode) + ", " + CFL.Q(strAccountNo) + ", " + CFL.Q(strCsDescr) + "," + CFL.Q(strcsExtraFieldName1) + " ," + CFL.Q(strcsExtraFieldName2) + @"
    , " + CFL.Q(strcsExtraFieldName3) + ", " + CFL.Q(strCsClass1) + ", " + CFL.Q(strCsClass2) + ", " + CFL.Q(strTrus_Direction) + ", " + CFL.Q(strTrus_Use) + @"
    , " + CFL.Q(strEmpCode) + ", " + CFL.Q(strInitial) + @"
)";
            }

            for(int x=0; x < sarEmpName.Length; x++)
            {
                strQuery += @"
insert into CsMaster_Emp(ComCode, CsCode, EmpName, EmpTell, EmpFaxNo, emailID, EmpBego)
Values (  " + CFL.Q(GD.ComCode) + @", " + CFL.Q(strCsCode) + @", " + CFL.Q(sarEmpName[x]) + @", " + CFL.Q(sarEmpTell[x]) + @", " 
    + CFL.Q(sarEmpFaxNo[x]) + @", " + CFL.Q(saremailID[x]) + @", " + CFL.Q(sarEmpBego[x]) + @") ";
            }

            try
            {
                CFL.ExecuteTran(GD, strQuery.ToString(), CommandType.Text, ref m_tr, ref m_db);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                // 저장에 실패하였습니다.
                return CFL.EncodeData("03", Message.MsgText(ds, "M00150"), e);
            }

            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData("00", strCsCode, null);
        }


        public ArrayList Get_SaveCheck(object[] GDObj, string strCsCode, string strRegNo, string strCsName)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strMsg = "M01011;M01012;M01013";
            DataSet ds = Message.MsgSet(GDObj, GD.LangID, strMsg);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
Select Rtrim(CsCode)
From CsMaster 
Where ComCode = " + CFL.Q(GD.ComCode);

            if (strCsCode != "" && strCsCode != null)
            {
                strQuery += @" and CsCode != " + CFL.Q(strCsCode);
            }

            if (strRegNo != "" && strRegNo != null)
            {
                strQuery += @" and RegNo = " + CFL.Q(strRegNo);
            }

            if (strCsName != "" && strCsName != null)
            {
                strQuery += @" and CsName = " + CFL.Q(strCsName);
            }

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery.ToString(), CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "02";
                strErrorMsg = Message.MsgText(ds, "M01011"); //거래처 명칭을 체크할 수 없습니다.
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            if (dr.Read())
            {
                strErrorCode = "01";
                if (strCsName != "")
                    strErrorMsg = dr[0].ToString() + Message.MsgText(ds, "M01012"); //거래처와 명칭이 중복됩니다. 계속하시겠습니까?
                if (strRegNo != "")
                    strErrorMsg = dr[0].ToString() + Message.MsgText(ds, "M01013"); //사업자 번호가 중복됩니다.
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }


        public ArrayList RegNoCheck(object[] GDObj, string strRegNo)
        {
            // Global Data
            GData GD = new GData(GDObj);
            string strMsg = "M01014;M01013";
            DataSet ds = Message.MsgSet(GDObj, GD.LangID, strMsg);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
Select RegNo, CsName 
From CsMaster 
Where ComCode = " + CFL.Q(GD.ComCode) + " and RegNo= " + CFL.Q(strRegNo);

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery.ToString(), CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "02";
                strErrorMsg = Message.MsgText(ds, "M01014"); // 거래처 명칭을 체크할 수 없습니다.
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            if (dr.Read())
            {
                strErrorCode = "01";
                strErrorMsg = dr[1].ToString() + " : " + dr[0].ToString() + " " + Message.MsgText(ds, "M01013");  // 사업자 번호가 중복됩니다.
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }


    }
}