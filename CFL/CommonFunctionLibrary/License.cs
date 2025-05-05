using System;
using System.Collections;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Management;

namespace SMART
{
	/// <summary>
	/// Summary description for License.
	/// </summary>
	public class License
	{
		// Dummy User Count
		private const int c_iDummyUser = 2;
		private const string c_strFileName = "xERPLicense.txt";

		private byte[] m_desKey;
		private byte[] m_desIV;
		private const int c_iBlockSize = 4096;
		private string m_strLangID;
		private string m_strClientID;

		public License(string strLangID, string strClientID)
		{
			m_strLangID = strLangID;
			m_strClientID = strClientID;

			// DES algorithm Key and Initialization Vector Setting
			string strdesKey = System.Environment.MachineName + "SlashSmartUsgKorea";
			m_desKey = System.Text.Encoding.UTF8.GetBytes(strdesKey.Substring(0,
				16));
			m_desIV = System.Text.Encoding.UTF8.GetBytes("UsgKorea");
		}

		public struct LicenseInfo
		{
			public string ClientID;
			public string LicenseNo;
			public string CustomerName;
			public string RetailerName;
			public string ContractDate;
			public string ValidThru;
			public int UserAccount;
			public string LicensedModule;
			public string LicenseType;
			public string CertMethod;
			public bool UseEDI;
			public string LastChecked;
			public string ErrorCode;
			public string ErrorMsg;

			public string IPAddress;
			public string MacAddress;
		}

		/// <summary>
		/// Get License Info Structure & Update License File
		/// </summary>
		public LicenseInfo GetLicenseInfo()
		{
			string strTemp = "";
			return GetLicenseInfo(ref strTemp);
		}

		// Get License Info Structure & Update License File ( Internal Use )
		private LicenseInfo GetLicenseInfo(ref string strCurDate)
		{
			LicenseInfo LicInfo = new LicenseInfo();

			LicInfo.ClientID = m_strClientID;
			LicInfo.ContractDate = "20030101";
			LicInfo.CustomerName = "Trial User";
			LicInfo.ErrorCode = "00";
			LicInfo.ErrorMsg = "";
			strCurDate = CFL.GetSysDateTime().Substring(0, 8);
			LicInfo.LastChecked = strCurDate;
			LicInfo.LicensedModule = "YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY";
			LicInfo.LicenseNo = "Trial";
			LicInfo.LicenseType = "C";
			LicInfo.RetailerName = "GIN C&S";

			// 사용자수
			LicInfo.UserAccount = 100;  // SmartPro

			LicInfo.CertMethod = "NONE";

			LicInfo.UseEDI = false;

			// 날짜 제한		
			LicInfo.ValidThru = "20241231";         // SmartPro (내부 개발자)

			// IP 주소 제한            
			LicInfo.IPAddress = "106.248.225.106";   // SmartPro - ERP 실서버


			// Mac Address 제한            
			LicInfo.MacAddress = "9C8E995BA2C7";    // SmartPro - Live 서버

			return LicInfo;

		}


		/// <summary>
		/// Check License when User Logins
		/// </summary>
		public ArrayList CheckLicense(string strSiteCode)
		{
			// Get License Info
			string strCurDate = "";

			LicenseInfo LicInfo = GetLicenseInfo(ref strCurDate);

            if (LicInfo.ErrorCode != null && LicInfo.ErrorCode != "00")
				return CFL.EncodeData(LicInfo.ErrorCode, LicInfo.ErrorMsg, null);

			// LicenseNo Check
			string strErrorMsg = "";

            if (null == LicInfo.LicenseNo || "" == LicInfo.LicenseNo)
			{
				// 제품 등록절차에 따라 License 를 받은 후 사용하시기 바랍니다.
				switch (m_strLangID.ToLower())
				{
					case "chn":
						strErrorMsg = "按产品登记程序，请收到 License 后使用。";
						break;
					case "jpn":
						strErrorMsg = "製品登録手続きによりライセンスを受けてからご使用ください。";
						break;
					case "eng":
						strErrorMsg = "Use Smart after you get License through Appropriate Product Registration Steps.";
						break;
					default:
						strErrorMsg = "제품 등록절차에 따라 License 를 받은 후 사용하시기 바랍니다.";
						break;
				}
				return CFL.EncodeData("02", strErrorMsg, null);
			}

			// Check Valid Date
			if (CFL.Tod(strCurDate) > CFL.Tod(LicInfo.ValidThru))
			{
				// License 기한이 만료되었습니다. 공급사에 문의하십시오.
				switch (m_strLangID.ToLower())
				{
					case "chn":
						strErrorMsg = "License 到期。请咨询供应商。";
						break;
					case "jpn":
						strErrorMsg = "ライセンス期限が満了になりました。供給会社にお問い合わせください。";
						break;
					case "eng":
						strErrorMsg = "License Period Expired. Contact Your Vendor, Please.";
						break;
					default:
						strErrorMsg = "License 기한이 만료되었습니다. 공급사에 문의하십시오.";
						break;
				}
				return CFL.EncodeData("01", strErrorMsg, null);
			}


			// IP Address 체크..           
			string strIPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].ToString();
	    //  string strIPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[4].ToString();

			// 일단 막음
			/*
             
            // IP 주소 
            if (LicInfo.IPAddress != strIPAddress)
            {
                return CFL.EncodeData("03", "해당 IP 에서는 실행할 수 없습니다.", null);
            }
            
            */


			// Mac Address 체크                       
			string strMacAddress = GetMacAddress();


			//  if (LicInfo.MacAddress != strMacAddress)
			//  if ("3440B58CD20F" != strMacAddress && "5EF3FC50B903" != strMacAddress)     // SmartPro 운영서버 - 2개 네트워크 카드용
			/*
            if ("9C8E995BA2C7" != strMacAddress)                                        // SmartPro 내부개발자용 : 막음
            {
                return CFL.EncodeData("04", "해당 네트워크 카드에서는 실행할 수 없습니다.", null);
            }
            */

			// ----------------------------------------------------------------------------------------------------------------


			// DB Connect for User AccountCheck
			OleDbConnection db = null;
			ClientCore.SetDB(m_strClientID, ref db);

			// Check UserAccount
			if (!UserAccountCheck(LicInfo.UserAccount, db, null, ref strErrorMsg, strSiteCode))
				return CFL.EncodeData("02", strErrorMsg, null);

			// Encode InstalledModule, LicenseType, UserAccount in the ArrayList
			ArrayList alResult = new ArrayList();
			alResult.Add("00");
			alResult.Add("");
			alResult.Add(5);
			alResult.Add(1);
			alResult.Add(LicInfo.LicensedModule);
			alResult.Add(LicInfo.LicenseType);
			alResult.Add(LicInfo.UserAccount);
			alResult.Add(LicInfo.CertMethod);
			alResult.Add(LicInfo.UseEDI);

			return alResult;
		}


		// Mac Address 체크
		protected string GetMacAddress()
		{
			ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

			ManagementObjectCollection moc = mc.GetInstances();

			string MACAddress = String.Empty;

			foreach (ManagementObject mo in moc)
			{

				if (MACAddress == String.Empty)  // Only return Mac Address From First Card
				{
					if ((bool)mo["IPEnabled"] == true)
						MACAddress = mo["MacAddress"].ToString();
				}

				mo.Dispose();
			}

			return MACAddress.Replace(":", "");
		}


		// Get Specified Property from License File contents
		private static bool GetValue(string strPropertyName, string strContent, ref string strValue)
		{
			int iStart = strContent.IndexOf(strPropertyName);
			if (iStart < 0)
				return false;
			else
			{
				int iEnd = strContent.IndexOf(System.Environment.NewLine, iStart);
				if (iEnd < 0)
					iEnd = strContent.Length;

				strValue = strContent.Substring(iStart + strPropertyName.Length + 3
					, iEnd - iStart - strPropertyName.Length - 3);
			}

			return true;
		}

		/*
		// Get License Info from License Server
		private LicenseInfo GetLicenseInfoFromServer (ref string strCurDate )
		{
			// Invoke WebService
			LicenseInfo LicInfo = new LicenseInfo();

			LicenseServer.EagleLicense Obj = new LicenseServer.EagleLicense();
			object[] alData = Obj.GetLicenseInfo ( m_strLangID, m_strClientID );
            Obj.Dispose();
			if ( "00" != alData[0].ToString() )
			{
				LicInfo.ErrorCode = alData[0].ToString();
				LicInfo.ErrorMsg = alData[1].ToString();
				return LicInfo;
			}
			else if ( 0 == (int)alData[3] )
			{
				LicInfo.ErrorCode = "01";
				LicInfo.ErrorMsg = alData[1].ToString();
				return LicInfo;
			}

			// SetData
			LicInfo.ClientID = m_strClientID;
			try
			{
				strCurDate = alData[12].ToString();

				LicInfo.LicenseNo = alData[4].ToString();
				LicInfo.CustomerName = alData[5].ToString();
				LicInfo.RetailerName = alData[6].ToString();
				LicInfo.ContractDate = alData[7].ToString();
				LicInfo.ValidThru = alData[8].ToString();
				LicInfo.UserAccount = CFL.Toi ( alData[9].ToString() );
				LicInfo.LicensedModule = alData[10].ToString();
				LicInfo.LicenseType = alData[11].ToString();
				LicInfo.LastChecked = alData[12].ToString();
				// Added Latly 
				LicInfo.CertMethod = alData[13].ToString();
				LicInfo.UseEDI = alData[14].ToString() == "Y";
			}
			catch
			{
				LicInfo.CertMethod = "NONE";
				LicInfo.UseEDI =  false;
			}

			return LicInfo;
		}
        */


		/// <summary>
		/// Check UserAccount when User Information Registration
		/// </summary>		
		public ArrayList UserAccountCheck(OleDbConnection db, OleDbTransaction tr)
		{
			// Get LicenseInfo
			LicenseInfo LicInfo = GetLicenseInfo();

			string strErrorMsg = "";

			if (UserAccountCheck(LicInfo.UserAccount, db, tr, ref strErrorMsg, ""))
				return CFL.EncodeData("00", "", null);
			else
				return CFL.EncodeData("01", strErrorMsg, null);
		}


		public ArrayList UserAccountCheck(object[] GDObj, SqlConnection db, SqlTransaction tr)
		{
			// Get LicenseInfo
			LicenseInfo LicInfo = GetLicenseInfo();

			string strErrorMsg = "";
			if (UserAccountCheck(GDObj, LicInfo.UserAccount, db, tr, ref strErrorMsg))
				return CFL.EncodeData("00", "", null);
			else
				return CFL.EncodeData("01", strErrorMsg, null);
		}

		private bool UserAccountCheck(object[] GDObj, int iUserAccount, SqlConnection db, SqlTransaction tr, ref string strErrorMsg)
		{
			GData GD = new GData(GDObj);

            string strQuery = @"
Select Count(*) as UserAccount	
From xErpUser	
Where ComCode = " + CFL.Q(GD.ComCode) + @"
  and convert ( nchar(8), getdate(), 112 ) between UserBdate and UserEdate";

			//OleDbCommand cm;
			//if (tr != null)
			//	cm = new OleDbCommand(strQuery, db, tr);
			//else
			//	cm = new OleDbCommand(strQuery, db);

			SqlDataReader dr;
			try
			{
				if (tr != null)
					dr = CFL.ExecuteReaderTran(GD, strQuery, CommandType.Text, ref tr, ref db);
				else
					dr = CFL.ExecuteReader(GD, strQuery, CommandType.Text);
			}
			catch (Exception e)
			{
				if (tr != null)
					tr.Rollback();

				// License 정보를 확인할 수 없습니다.
				switch (m_strLangID.ToLower())
				{
					case "chn":
						strErrorMsg = "无法确认License 信息。" + e.ToString();
						break;
					case "jpn":
						strErrorMsg = "ライセンス情報を確認出来ません。" + e.ToString();
						break;
					case "eng":
						strErrorMsg = "Failed to Check License Information." + e.ToString();
						break;
					default:
						strErrorMsg = "License 정보를 확인할 수 없습니다." + e.ToString();
						break;
				}
				return false;
			}

			dr.Read();
			int iUserAccountRead = CFL.Toi(CFL.F(dr["UserAccount"]));
			dr.Close();

			if (iUserAccountRead > iUserAccount + c_iDummyUser)
			{
				if (tr != null)
					tr.Rollback();

				// License 계약보다 많은 사용자가 등록되었습니다. 시스템 관리자에게 문의하여 주십시오.
				switch (m_strLangID.ToLower())
				{
					case "chn":
						strErrorMsg = "比License 合同多的用户被登记。请咨询系统管理员。";
						break;
					case "jpn":
						strErrorMsg = "ライセンス契約より、多い使用者が登録されました。システム管理者にお問い合わせください。";
						break;
					case "eng":
						strErrorMsg = "More Users Registered than License Agreements. Contact your System Administrator, please.";
						break;
					default:
						strErrorMsg = "License 계약보다 많은 사용자가 등록되었습니다. 시스템 관리자에게 문의하여 주십시오.";
						break;
				}
				return false;
			}

			return true;

		}

		// User AccountCheck Core
		// iUserAccount : Number in License File
		private bool UserAccountCheck(int iUserAccount, OleDbConnection db, OleDbTransaction tr, ref string strErrorMsg, string strSiteCode)
		{
			string strQuery = @"
Select Count(*) as UserAccount	
From xErpUser	
Where Convert( nchar(8), getdate(), 112 ) between UserBdate and UserEdate
  and ComCode = ( Select ComCode
                  From SiteMaster
                  Where SiteCode = " + CFL.Q(strSiteCode) + " )";

			OleDbCommand cm;

			if (tr != null)
				cm = new OleDbCommand(strQuery, db, tr);
			else
				cm = new OleDbCommand(strQuery, db);

			OleDbDataReader dr;
			try
			{
				dr = cm.ExecuteReader();
			}
			catch (Exception e)
			{
				if (tr != null)
					tr.Rollback();

				// License 정보를 확인할 수 없습니다.
				switch (m_strLangID.ToLower())
				{
					case "chn":
						strErrorMsg = "无法确认License 信息。" + e.ToString();
						break;
					case "jpn":
						strErrorMsg = "ライセンス情報を確認出来ません。" + e.ToString();
						break;
					case "eng":
						strErrorMsg = "Failed to Check License Information." + e.ToString();
						break;
					default:
						strErrorMsg = "License 정보를 확인할 수 없습니다." + e.ToString();
						break;
				}
				return false;
			}

			dr.Read();
			int iUserAccountRead = CFL.Toi(CFL.F(dr["UserAccount"]));
			dr.Close();

			if (iUserAccountRead > iUserAccount + c_iDummyUser)
			{
				if (tr != null)
					tr.Rollback();

				// License 계약보다 많은 사용자가 등록되었습니다. 시스템 관리자에게 문의하여 주십시오.
				switch (m_strLangID.ToLower())
				{
					case "chn":
						strErrorMsg = "比License 合同多的用户被登记。请咨询系统管理员。";
						break;
					case "jpn":
						strErrorMsg = "ライセンス契約より、多い使用者が登録されました。システム管理者にお問い合わせください。";
						break;
					case "eng":
						strErrorMsg = "More Users Registered than License Agreements. Contact your System Administrator, please.";
						break;
					default:
						strErrorMsg = "License 계약보다 많은 사용자가 등록되었습니다. 시스템 관리자에게 문의하여 주십시오.";
						break;
				}
				return false;
			}

			return true;
		}

		public static ArrayList Clear(string strLangID)
		{
			if (File.Exists(CFL.GetIniPath() + "\\" + c_strFileName))
			{
				try
				{
					File.Delete(CFL.GetIniPath() + "\\" + c_strFileName);
				}
				catch (Exception ex)
				{
					string strErrorMsg;
					// License 정보를 초기화하는데 실패하였습니다. 시스템 관리자에게 문의하여 주십시오.
					switch (strLangID.ToLower())
					{
						case "chn":
							strErrorMsg = "License 信息初始化失败。请咨询系统管理员。";
							break;
						case "jpn":
							strErrorMsg = "ライセンス情報の初期化に失敗しました。システム管理者にお問い合わせください。";
							break;
						case "eng":
							strErrorMsg = "Failed to Initializing License Information. Contact your System Administrator, please.";
							break;
						default:
							strErrorMsg = "License 정보를 초기화하는데 실패하였습니다. 시스템 관리자에게 문의하여 주십시오.";
							break;
					}
					return CFL.EncodeData("01", strErrorMsg, ex);
				}
			}

			return CFL.EncodeData("00", "", null);
		}

	}
}
