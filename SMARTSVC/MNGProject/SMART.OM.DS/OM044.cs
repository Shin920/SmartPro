using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SMART.OM.DS
{
    public class OM044
    {
        public OM044() { }

        public DataSet CsList(object[] GDObj, string strCsType, string strCsCode, string strCsName, string strCsUse)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
					Select CsCode, CsName From CsMaster Where ComCode = " + CFL.Q(GD.ComCode);
            if (strCsType != "")
                strQuery += " and CsType = " + CFL.Q(strCsType);

            if (strCsUse != "")
                strQuery += " and CsUse Like " + CFL.Q(strCsUse);

            if (strCsName != "")
                strQuery += " and CsName Like " + CFL.Q("%" + strCsName + "%");

            if (strCsCode != "")
                strQuery += " And CsCode Like " + CFL.Q(strCsCode + "%");

            strQuery += " Order By CsCode ";

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

        public DataSet sDataLoad_CsMAster(object[] GDObj, string strCsCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
				
Select A.CsCode, A.CsName
	 , A.CsType, C1.CodeName
	 , A.CsGubun, A.ArCheck, A.ApCheck, A.OutCheck, A.CsNameFull, A.ForeignCheck, A.ComCheck, A.CsUse, A.RegNo
	 , A.NationCode, N1.NationName
	 , A.RegionCode, R1.RegionName
	 , A.ParentCs, C2.CsName As ParentCsName
	 , A.CsIndustry, A.CsItems, A.CsAddress, A.CsZip
	 , A.CsChief
	 , A.CsTel, A.CsFax, A.CsUrl, A.CsEmail
	 , A.TaxCodeAr, T1.TaxName
	 , A.TaxCodeAp, T2.TaxName
	 , A.PayConditionAr, R2.PayCondName
	 , A.PayConditionAp, R3.PayCondName
	 , A.CurrCode
	 , A.CsGrade, C3.CodeName
	 , A.CreditGrade, C4.CodeName
	 , A.IncoTerm, C5.CodeName
	 , A.LeadTime
	 , A.TransCode, C6.CodeName
	 , A.SendMethod, T3.TypeName
	 , A.TranPartner, A.TranTelNo, A.TranFax, A.TranEmail
	 , A.BankCode, T4.TypeName, C7.CsName As BankName
	 , A.AccountNo, A.CsDescr, A.ExtraField1, A.ExtraField2, A.ExtraField3, A.ExtraField4
	 , A.CsClass1, A.CsClass2
     , A.TrusBill_Status, A.TrusBill_YN, a.empcode, isnull(G.EmpName, '') as EmpName
, A.CsInitial
From CsMaster A
    Left Outer Join CodeMaster C1 on C1.ComCode = " + CFL.Q(GD.ComCode) + @" and C1.CodeID = 'CsType' and C1.CodeCode = A.CsType 
    Left Outer Join NationMaster N1 on N1.ComCode = " + CFL.Q(GD.ComCode) + @" and N1.NationCode = A.NationCode
    Left Outer Join RegionMaster R1 on R1.ComCode = " + CFL.Q(GD.ComCode) + @" and R1.RegionCode = A.RegionCode
    Left Outer Join CsMaster C2 on C2.ComCode = " + CFL.Q(GD.ComCode) + @" and C2.CsCode = A.ParentCs
    Left Outer Join TaxMaster T1 on T1.ComCode = " + CFL.Q(GD.ComCode) + @" and T1.TaxCode = A.TaxCodeAr 
    Left Outer Join TaxMaster T2 on T2.ComCode = " + CFL.Q(GD.ComCode) + @" and T2.TaxCode = A.TaxCodeAp 
    Left Outer Join rpPayCondition R2 on R2.ComCode = " + CFL.Q(GD.ComCode) + @" and R2.PayCondCode = A.PayConditionAr and R2.ArApGubun = 'AR'
    Left Outer Join rpPayCondition R3 on R3.ComCode = " + CFL.Q(GD.ComCode) + @" and R3.PayCondCode = A.PayConditionAp and R3.ArApGubun = 'AP'
    Left Outer Join CodeMaster C3 on C3.ComCode = " + CFL.Q(GD.ComCode) + @" and C3.CodeID = 'CsGrade' and C3.CodeCode = A.CsGrade
    Left Outer Join CodeMaster C4 on C4.ComCode = " + CFL.Q(GD.ComCode) + @" and C4.CodeID = 'CsCreditGrade' and C4.CodeCode = A.CreditGrade 
    Left Outer Join CodeMaster C5 on C5.ComCode = " + CFL.Q(GD.ComCode) + @" and C5.CodeID = 'IncoTerm' and C5.CodeCode = A.IncoTerm 
    Left Outer Join CodeMaster C6 on C5.ComCode = " + CFL.Q(GD.ComCode) + @" and C5.CodeID = 'TransCode' and C6.CodeCode = A.TransCode
    Left Outer Join TypeMaster T3 on T3.LangID = " + CFL.Q(GD.LangID) + @" and T3.TypeID = 'SendMethod'	and T3.TypeCode = A.SendMethod 
    Left Outer Join TypeMaster T4 on T4.TypeID = 'Bank' and T4.LangID = " + CFL.Q(GD.LangID) + @" and T4.TypeCode = A.BankCode
    Left Outer Join EmpMaster G on G.ComCode = " + CFL.Q(GD.ComCode) + @" and G.SiteCode = " + CFL.Q(GD.SiteCode) + " and G.empcode = A.empcode " + @"
	Left outer join CsMaster C7 on C7.ComCode = " + CFL.Q(GD.ComCode) + @" and C7.CsCode = A.BankCode " + @"
Where A.ComCode = " + CFL.Q(GD.ComCode) + @" 
  and A.CsCode = " + CFL.Q(strCsCode);


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

        public DataSet CsFieldNameLoad(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"

			select csExtraFieldName1 , csExtraFieldName2 ,csExtraFieldName3 ,
				ItemExtraFieldName1 , ItemExtraFieldName2 ,ItemExtraFieldName3
			from CompanyMaster
			where ComCode = "
                + CFL.Q(GD.ComCode);

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

        public DataSet NameCheck(object[] GDObj, string strCsCode, string strCsName)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
					Select CsCode 
					From CsMaster 
					Where ComCode = " + CFL.Q(GD.ComCode)
                        + " and CsCode != " + CFL.Q(strCsCode)
                        + " and CsName = " + CFL.Q(strCsName);

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

        public DataSet GetAutoCode(object[] GDObj, string strCsType)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            // 국내매출처
            if (strCsType == "100")
            {
                strQuery = @"
Select Top 1 CsCode
From CsMaster
Where ComCode = " + CFL.Q(GD.ComCode) + @"
  and CsCode Like 'C%'
  and CsType = '100'
Order By Cast(SUBSTRING(CsCode,2,6) As int) Desc
";
            }
            // 해외매출처
            else if (strCsType == "150")
            {
                strQuery = @"
Select Top 1 CsCode
From CsMaster
Where ComCode = " + CFL.Q(GD.ComCode) + @"
  and CsCode Like 'O%' 
  and CsType = '150'
Order By Cast(SUBSTRING(CsCode,2,6) As int) Desc
";
            }
            // 국내매입처
            else if (strCsType == "200")
            {
                strQuery = @"
Select Top 1 CsCode
From CsMaster
Where ComCode = " + CFL.Q(GD.ComCode) + @"
  and CsCode Like 'S%' 
  and CsType = '200'
Order By Cast(SUBSTRING(CsCode,2,6) As int) Desc
";
            }
            // 해외매입처
            else if (strCsType == "250")
            {
                strQuery = @"
Select Top 1 CsCode
From CsMaster
Where ComCode = " + CFL.Q(GD.ComCode) + @"
  and CsCode Like 'T%' 
  and CsType = '250'
Order By Cast(SUBSTRING(CsCode,2,6) As int) Desc
";
            }
            // 은행
            else if (strCsType == "300")
            {
                strQuery = @"
Select Top 1 CsCode
From CsMaster
Where ComCode = " + CFL.Q(GD.ComCode) + @"
  and CsCode Like 'B%' 
  and CsType = '300'
Order By Cast(SUBSTRING(CsCode,2,6) As int) Desc
";
            }
            // 신용카드
            else if (strCsType == "700")
            {
                strQuery = @"
Select Top 1 CsCode
From CsMaster
Where ComCode = " + CFL.Q(GD.ComCode) + @"
  and CsCode Like 'D%' 
  and CsType = '700'
Order By Cast(SUBSTRING(CsCode,2,6) As int) Desc
";
            }
            // 제조업체
            else if (strCsType == "900")
            {
                strQuery = @"
Select Top 1 CsCode
From CsMaster
Where ComCode = " + CFL.Q(GD.ComCode) + @"
  and CsCode Like 'M%' 
  and CsType = '900'
Order By Cast(SUBSTRING(CsCode,2,6) As int) Desc
";
            }
            else if (strCsType == "")
            {
                strQuery = @"
				Select null ";
            }


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

        public DataSet Get_CsEmpLoad(object[] GDObj, string strCsCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
				
select	
    rtrim(isnull(EmpName, '')) as EmpName
    , rtrim(isnull(EmpTell, '')) as EmpTell
    , rtrim(isnull(EmpFaxNo, '')) as EmpFaxNo
    , rtrim(isnull(emailID, '')) as emailID
    , rtrim(isnull(EmpBego, '')) as EmpBego
    , '' as SaveCheck
From CsMaster A
join CsMaster_Emp B on A.comcode= b.ComCode  and a.CsCode = b.CsCode
Where A.ComCode = " + CFL.Q(GD.ComCode) + @" 
  and A.CsCode = " + CFL.Q(strCsCode);


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
