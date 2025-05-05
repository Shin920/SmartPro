using System;
using System.Collections;
using System.Data;

namespace SMART
{
	/// <summary>
	/// Summary description for GObj.
	/// </summary>
	public class GObj
	{
		protected const string c_UltraUserID = "xErpAdmin";
		protected const string c_UltraUserPwd = "smart";

		public GObj()
		{
		}

		#region Global Information that will be loaded when Accessed

		// Data Component
		public object[] D
		{
			get
			{
				return m_Data.ToArray();
			}
		}
		private GData m_Data;

		/// <summary>
		/// Current Date / Time
		/// </summary>
		public string CurDate { get { return m_Data.CurDate; } }

		/// <summary>
		/// Language ID
		/// </summary>
		public string LangID { get { return m_Data.LangID; } }

		/// <summary>
		/// Client ID
		/// </summary>
		public string ClientID { get { return m_Data.ClientID; } }
		public string DomainName { get { return m_Data.DomainName; } }

		/// <summary>
		/// User IP
		/// </summary>
		public string UserIP { get { return m_Data.UserIP; } }

		/// <summary>
		/// User Information
		/// </summary>
		public string UserID { get { return m_Data.UserID; } }
		public string UserName { get { return m_Data.UserName; } }
		public string UserType { get { return m_Data.UserType; } }
		public string eMail { get { return m_Data.eMail; } }
		public string UserGroup { get { return m_Data.UserGroup; } }

		/// <summary>
		/// Emp Information
		/// </summary>
		public string EmpCode { get { return m_Data.EmpCode; } }
		public string EmpName { get { return m_Data.EmpName; } }

		/// <summary>
		/// Dept
		/// </summary>
		public string DeptCode { get { return m_Data.DeptCode; } }
		public string DeptName { get { return m_Data.DeptName; } }

		/// <summary>
		/// Cc
		/// </summary>
		public string CcCode { get { return m_Data.CcCode; } }

		/// <summary>
		/// Company
		/// </summary>
		public string ComCode { get { return m_Data.ComCode; } }
		public string ComName { get { return m_Data.ComName; } }

		/// <summary>
		/// Business Area
		/// </summary>
		public string CoaCode { get { return m_Data.CoaCode; } }
		public string CoaName { get { return m_Data.CoaName; } }

		/// <summary>
		/// Site Information
		/// </summary>
		public string SiteCode { get { return m_Data.SiteCode; } }
		public string SiteName { get { return m_Data.SiteName; } }
		public string SiteInitial { get { return m_Data.SiteInitial; } }
		public string DefaultInvStatus { get { return m_Data.DefaultInvStatus; } }
		public string poDefaultExchGubun { get { return m_Data.poDefaultExchGubun; } }
		public string sdDefaultExchGubun { get { return m_Data.sdDefaultExchGubun; } }

		/// <summary>
		/// Digit Information dependent on Site
		/// </summary>
		public short DigitNo_Qty { get { return m_Data.DigitNo_Qty; } }
		public short DigitType_Qty { get { return m_Data.DigitType_Qty; } }
		public short DigitNo_Price1 { get { return m_Data.DigitNo_Price1; } }
		public short DigitType_Price1 { get { return m_Data.DigitType_Price1; } }
		public short DigitNo_Price2 { get { return m_Data.DigitNo_Price2; } }
		public short DigitType_Price2 { get { return m_Data.DigitType_Price2; } }
		public short DigitNo_sdDiscnt { get { return m_Data.DigitNo_sdDiscnt; } }
		public short DigitType_sdDiscnt { get { return m_Data.DigitType_sdDiscnt; } }
		public short DigitNo_ppScrap { get { return m_Data.DigitNo_ppScrap; } }
		public short DigitType_ppScrap { get { return m_Data.DigitType_ppScrap; } }
		public short DigitNo_ppQtyPer { get { return m_Data.DigitNo_ppQtyPer; } }
		public short DigitType_ppQtyPer { get { return m_Data.DigitType_ppQtyPer; } }
		public short DigitNo_coUnitPrice { get { return m_Data.DigitNo_coUnitPrice; } }
		public short DigitType_coUnitPrice { get { return m_Data.DigitType_coUnitPrice; } }
		public short DigitNo_coRatio { get { return m_Data.DigitNo_coRatio; } }
		public short DigitType_coRatio { get { return m_Data.DigitType_coRatio; } }
		public short DigitNo_mmOhPrice { get { return m_Data.DigitNo_mmOhPrice; } }
		public short DigitType_mmOhPrice { get { return m_Data.DigitType_mmOhPrice; } }
		/// <summary>
		/// SmartMaster Information
		/// </summary>
		public string GroupName { get { return m_Data.GroupName; } }
		public string CurrCode { get { return m_Data.CurrCode; } }
		public string glCashAcc { get { return m_Data.glCashAcc; } }
		public string glWipAcc { get { return m_Data.glWipAcc; } }
		public string CcExpDD { get { return m_Data.CcExpDD; } }

		/// <summary>
		/// ModuleUseCheck ( License && ComModuleUse )
		/// </summary>
		/// <param name="Module"></param>
		/// <returns></returns>
		public bool GetModuleUse(CFL.ModuleIni Module)
		{
			if (null == m_Data.ModuleUseCheck)
				return false;
			int i = CFL.EnumModule(Module);
			if (i < m_Data.ModuleUseCheck.Length - 1)
				return "Y" == m_Data.ModuleUseCheck.Substring(i, 1);
			else
				return false;
		}

		/// <summary>
		/// Digit Information independent of Site
		/// </summary>
		public short DigitNo_glAmnt1 { get { return m_Data.DigitNo_glAmnt1; } }
		public short DigitType_glAmnt1 { get { return m_Data.DigitType_glAmnt1; } }
		public short DigitNo_glAmnt2 { get { return m_Data.DigitNo_glAmnt2; } }
		public short DigitType_glAmnt2 { get { return m_Data.DigitType_glAmnt2; } }
		public short DigitNo_ExchRate { get { return m_Data.DigitNo_ExchRate; } }
		public short DigitType_ExchRate { get { return m_Data.DigitType_ExchRate; } }

		/// <summary>
		/// CultureID
		/// </summary>
		public string CultureID { get { return m_Data.CultureID; } }

		/// <summary>
		/// EIS Information
		/// </summary>
		public decimal EisAmnt1_BoundAmnt { get { return m_Data.EisAmnt1_BoundAmnt; } }
		public string EisAmnt1_BoundName { get { return m_Data.EisAmnt1_BoundName; } }
		public decimal EisAmnt2_BoundAmnt { get { return m_Data.EisAmnt2_BoundAmnt; } }
		public string EisAmnt2_BoundName { get { return m_Data.EisAmnt2_BoundName; } }


		// *************************************************************************************************

		// 협업 - URL 정보  /  업체코드  /  재무사용 여부  /  WeightLocation  
		// 신생산 사용여부

		public string TYURL { get { return m_Data.TYURL; } }
		public string PTCode { get { return m_Data.PTCode; } }
		public string FINUse { get { return m_Data.FINUse; } }
		public string WeightLocation { get { return m_Data.WeightLocation; } }

		public string NPPUse { get { return m_Data.NPPUse; } }


        // 사업장 고유 ID
        public string SiteNo { get { return m_Data.SiteNo; } }

        // *************************************************************************************************

        #endregion


        #region About Login / Logout

        public object[] Login(string strUserID, string strPassWd, string strSiteCode, string strLangID, string strClientID
							, string strCertDigest, string strKey, string strUserIP, object objConditions)
		{

			if (null == m_Data || null != m_Data.UserID)
				Logout();

			object[] alData = Security.Login(strUserID, strPassWd, strSiteCode, strLangID, strClientID, strCertDigest, strKey, strUserIP, objConditions);

			// Include Errors that can Continue
			if ("00" == alData[0].ToString() || "C" == alData[0].ToString().Substring(0, 1))
			{
				m_Data = new GData(alData);
			}
			object[] o = Security.Login(strUserID, strPassWd, strSiteCode, strLangID, strClientID, strCertDigest, strKey, strUserIP, objConditions);
			return o;
		}

		public object[] LoginEncryptedPassword(string strUserID, string strPassWd, string strSiteCode, string strLangID, string strClientID, string strCertDigest, string strKey, string strUserIP, object objConditions)
		{
			if (null == m_Data || null != m_Data.UserID)
				Logout();

			object[] alData = Security.LoginEncryptedPassword(strUserID, strPassWd, strSiteCode, strLangID, strClientID, strCertDigest, strKey, strUserIP, objConditions);
			// Include Errors that can Continue
			if ("00" == alData[0].ToString() || "C" == alData[0].ToString().Substring(0, 1))
			{
				m_Data = new GData(alData);
			}
			object[] o = Security.LoginEncryptedPassword(strUserID, strPassWd, strSiteCode, strLangID, strClientID, strCertDigest, strKey, strUserIP, objConditions);

			return o;

		}

		public object[] Login(string strUserID, string strSiteCode, string strLangID, string strClientID,
			string strKey, string strUserIP, object objConditions)
		{
			if (null == m_Data || null != m_Data.UserID)
				Logout();

			object[] alData = Security.Login(strUserID, strSiteCode, strLangID, strClientID, strKey, strUserIP, objConditions);

			// Include Errors that can Continue
			if ("00" == alData[0].ToString() || "C" == alData[0].ToString().Substring(0, 1))
			{
				m_Data = new GData(alData);
			}
			return alData;
		}

		public void GDataInit(object GDObj)
		{
			m_Data = new GData(GDObj);
		}

		/// <summary>
		/// delete User Information when Logout
		/// </summary>
		public void Logout()
		{
			m_strModuleIni = null;
			m_Data = new GData(CFL.EncodeData("01", "", null).ToArray());
		}

		#endregion

		/// <summary>
		/// Member Variables that are used by System Management
		/// </summary>
		public string FormID { get { return m_strFormID; } }
		protected string m_strFormID = "";
		public string ModuleIni { get { return m_strModuleIni; } }
		protected string m_strModuleIni = "";
		public string FormGrade { get { return m_strFormGrade; } }
		protected string m_strFormGrade = "";
		public string StyleSheet { get { return m_strStyleSheet; } }
		protected string m_strStyleSheet = "";
		public string GridSelColor { get { return m_strGridSelColor; } }
		protected string m_strGridSelColor = "";
		public bool WfUse { get { return m_bWfUse; } }
		protected bool m_bWfUse = false;

		/// <summary>
		/// Get the Permission for the Security Object
		/// </summary>
		/// <param name="strFormID"></param>
		/// <param name="SecureKind"></param>
		/// <returns></returns>
		public enum SecKind { FormOpen, Print, Insert, Update, Delete, A_Ratings };

		public bool GetPermission(string strFormID, SecKind SecureKind)
		{
			// Check Invalid Form ID
			if (null == strFormID && 5 != strFormID.Length)
				return false;

			string strModuleIni = strFormID.Substring(0, 2).ToUpper();

			if (m_strModuleIni == null || strModuleIni != m_strModuleIni)
			{

				// Load Form Security String
				object[] alData = CFL.ArrayListToObject(Security.LoadFormSecurity(D, strModuleIni));

				// ignore errors
				if (alData.Length > 4)
					m_strFormGrade = alData[4].ToString();
				else
					m_strFormGrade = "";

				// Load StyleSheet for this Module for this User
				//m_strStyleSheet = Obj.LoadStyleSheet(D, strModuleIni, ref m_strGridSelColor, ClientID);

				// Set Current Module Information
				m_strModuleIni = strModuleIni;
			}

			if (m_strFormID == null || strFormID != m_strFormID)
			{
				// Load WfUse
				ArrayList alData = Security.GetWfUse(D, strFormID);
				if ("00" != alData[0].ToString() || 5 > alData.Count)
				{
					m_bWfUse = false;
				}
				else
				{
					m_bWfUse = alData[4].ToString() == "Y";
				}

				m_strFormID = strFormID;
			}

			// Exception for Ultra SuperUser
			if (c_UltraUserID.ToLower() == UserID.ToLower())
				return true;

			// return Permission
			int iFormIdx = CFL.Toi(strFormID.Substring(2, 3));

			string strGrade = "";

			if (iFormIdx < m_strFormGrade.Length)
				strGrade = m_strFormGrade.Substring(iFormIdx, 1);

			FormSecKind = strGrade;

			switch (SecureKind)
			{
				case SecKind.FormOpen:
					if ("X" != strGrade)
						return true;
					break;
				case SecKind.Print:
					if ("X" != strGrade && "E" != strGrade)
						return true;
					break;
				case SecKind.Insert:
					if ("A" == strGrade || "B" == strGrade || "C" == strGrade)
						return true;
					break;
				case SecKind.Update:
					if ("A" == strGrade || "B" == strGrade)
						return true;
					break;
				case SecKind.Delete:
					if ("A" == strGrade || "B" == strGrade)
						return true;
					break;
				case SecKind.A_Ratings:
					if ("A" == strGrade)
						return true;
					break;
				default:
					return false;
			}

			return false;
		}

        public bool IsDead
		{
			get
			{
				return m_Data == null;
			}
		}

		public string FormSecKind
		{
			get
			{
				return m_Data.SecKind;
			}
			set
			{
				m_Data.SecKind = value;
			}
		}

		public decimal ExchRate(string strCurrCode, string strDate)
		{
			if (CurrCode == strCurrCode)
				return 1m;
			else
			{
				/*
				CGWProject.CGW.CGW  Obj = new CGWProject.CGW.CGW();
				
				decimal d = Obj.ExchRate(D, poDefaultExchGubun, strCurrCode, strDate);
				
				Obj.Dispose();
				
				return d;
				*/

				// (2024.08.22) 환율가져오는 이전 로직이 SmartPro v21 에서는 안먹어서, 변경함
				ArrayList alData = ExchRate(D, strCurrCode, strDate);

				if ("00" != alData[0].ToString() || 5 > alData.Count)
					return 0.0m;
							
				return CFL.Tod(alData[4].ToString());				

			}
		}

		public decimal ExchRate(string strCurrCode, string strDate, CFL.ModuleIni ExchangeGubun)
		{
			decimal d;
			CGWProject.CGW.CGW cgw = new CGWProject.CGW.CGW();

			if (CurrCode == strCurrCode)
				d = 1m;
			else
			{
				switch (ExchangeGubun)
				{
					case CFL.ModuleIni.PO:
						d = cgw.ExchRate(D, poDefaultExchGubun, strCurrCode, strDate);
						break;
					case CFL.ModuleIni.SD:
						d = cgw.ExchRate(D, sdDefaultExchGubun, strCurrCode, strDate);
						break;
					default:
						d = cgw.ExchRate(D, poDefaultExchGubun, strCurrCode, strDate);
						break;
				}
			}
			
			return d;
		}


		// (2024.08.22) 환율가져오는 이전 로직이 SmartPro v21 에서는 안먹어서, 변경함
		public ArrayList ExchRate(object[] GDObj, string strCurrCode, string strDate)
		{
			// Global Data
			GData GD = new GData(GDObj);

			string strErrorCode = "00", strErrorMsg = "";
			
			string strQuery = @"
Select ExchRate1, ExchRate2, ExchRate3 
From ExchangeRate 
Where ExchDate = " + CFL.Q(strDate) + @"
  and CurrCode = " + CFL.Q(strCurrCode) + @"
  and ComCode = " + CFL.Q(GD.ComCode);

			DataTableReader dr;

			try
			{
				dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
			}
			catch (Exception e)
			{
				strErrorCode = "01";
				strErrorMsg = "환율조회에 오류가 발생하였습니다.";

				return CFL.EncodeData(strErrorCode, strErrorMsg, e);
			}

			return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
		}


	}
}