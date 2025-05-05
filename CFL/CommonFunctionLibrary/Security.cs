using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections;
using System.Web;
using System.Text;

namespace SMART
{
	/// <summary>
	/// Security에 대한 요약 설명입니다.
	/// </summary>
	public class Security
	{
		public Security()
		{
		}
		
		#region Login

		// Will be Executed in SMARTSVC
		public static object[] Login(string strUserID, string strPassWd, string strSiteCode, string strLangID, string strClientID
			                       , string strCertDigest, string strKey, string strUserIP, object objConditions)
		{

			// License Check
			License lic = new License(strLangID, strClientID);

			ArrayList alResult = lic.CheckLicense(strSiteCode);

            if ("00" != alResult[0].ToString())
				return alResult.ToArray();

			string strModuleUseCheck = alResult[4].ToString().ToString();
			string strLicenseType = alResult[5].ToString().ToString();
			int iUserAccount = CFL.Toi(alResult[6]);
			string strCertMethod = CFL.F(alResult[7]).ToString();
			bool bUseEDI = (CFL.F(alResult[8]).ToString() == "Y");

			
			// load G Object Data
			GData GD = new GData(strUserID, strSiteCode, strClientID, strLangID, strUserIP
				               , strModuleUseCheck, strLicenseType, iUserAccount, bUseEDI);

            string strComCode = GD.ComCode;

            // User Check
            alResult = ValidateUserWf(strLangID, strUserID, strPassWd, null, strClientID, strCertDigest, strKey, strUserIP, objConditions, strCertMethod, strComCode);

            if ("00" != alResult[0].ToString())
                return alResult.ToArray();


            return GD.ToArray();
		}


		public static object[] LoginEncryptedPassword(string strUserID, string strPassWd, string strSiteCode, string strLangID, string strClientID
			                                        , string strCertDigest, string strKey, string strUserIP, object objConditions)
		{

			// License Check
			License lic = new License(strLangID, strClientID);

			ArrayList alResult = lic.CheckLicense(strSiteCode);

			if ("00" != alResult[0].ToString())
				return alResult.ToArray();

			string strModuleUseCheck = alResult[4].ToString().ToString();
			string strLicenseType = alResult[5].ToString().ToString();
			int iUserAccount = CFL.Toi(alResult[6]);
			string strCertMethod = CFL.F(alResult[7]).ToString();
			bool bUseEDI = (CFL.F(alResult[8]).ToString() == "Y");

			
			// load G Object Data
			GData GD = new GData(strUserID, strSiteCode, strClientID, strLangID, strUserIP
				               , strModuleUseCheck, strLicenseType, iUserAccount, bUseEDI);

            string strComCode = GD.ComCode;


            // User Check
            alResult = ValidateEncryptedPasswordUserWf(strLangID, strUserID, strPassWd, null, strClientID, strCertDigest, strKey, strUserIP, objConditions, strCertMethod, strComCode);

            if ("00" != alResult[0].ToString())
                return alResult.ToArray();

            
            return GD.ToArray();
		}


		public static object[] Login(string strUserID, string strSiteCode, string strLangID, string strClientID
			                       , string strKey, string strUserIP, object objConditions)
		{
			// License Check
			License lic = new License(strLangID, strClientID);

			ArrayList alResult = lic.CheckLicense(strSiteCode);

            if ("00" != alResult[0].ToString())
				return alResult.ToArray();

			string strModuleUseCheck = alResult[4].ToString().ToString();
			string strLicenseType = alResult[5].ToString().ToString();
			int iUserAccount = CFL.Toi(alResult[6]);
			string strCertMethod = CFL.F(alResult[7]).ToString();
			bool bUseEDI = (CFL.F(alResult[8]).ToString() == "Y");
			
            
            /*
			// Check Key
			CommonFunctionLibrary.AuthenticationServer.Authentication Obj = new CommonFunctionLibrary.AuthenticationServer.Authentication ();
			if ( !Obj.CheckKey ( strKey ) )
				return new object [4] { "01", "Wrong key value!", 0, 0 };
            */
			
            // load G Object Data
			GData GD = new GData(strUserID, strSiteCode, strClientID, strLangID, strUserIP
				               , strModuleUseCheck, strLicenseType, iUserAccount, bUseEDI);

	    //  Obj.Dispose();

			return GD.ToArray();
		}


		public static ArrayList ValidateUserWf(string strLangID, string strUserID, string strPassWd, string strWfPassWd, string strClientID
			                                 , string strCertDigest, string strKey, string strUserIP, object objConditions, string strComCode)
		{
			// Get CertMethod Information
			string strCertMethod = "NONE";

			return ValidateUserWf(strLangID, strUserID, strPassWd, strWfPassWd, strClientID, strCertDigest, strKey, strUserIP
				                , objConditions, strCertMethod, strComCode);
		}

		private static ArrayList ValidateEncryptedPasswordUserWf(string strLangID, string strUserID, string strPassWd, string strWfPassWd, string strClientID
		                                                       , string strCertDigest, string strKey, string strUserIP, object objConditions, string strCertMethod, string strComCode)
		{
			string strErrorCode = "00", strErrorMsg = "";

			// Validation Check
			// Not null Property
			if (null == strUserID || "" == strUserID)
			{
				strErrorCode = "01";
				strErrorMsg = Resource.GetString(strLangID, "01");
				return CFL.EncodeData(strErrorCode, strErrorMsg, null);
			}

			// Get Global Information
			string strQuery = @"
Select SkipPasswd
From SmartMaster";

			bool bSkipPasswd = false;

			DataTableReader dr = null;

			try
			{
				dr = CFL.ExecuteDataTableReader(strClientID, strQuery, CommandType.Text);
			}
			catch
			{
				// proceed with no error
			}

			// can be null if Exception Thrown
			if (null != dr)
			{
				if (!dr.Read())
				{
					// proceed with no error
				}
				else
				{
					if (dr["SkipPasswd"].ToString() == "Y")
						bSkipPasswd = true;
				}
			}

			dr.Close();

			// Get Certification Info
			strQuery = @"

Select PassWd, WfPassWd, UserBdate, UserEdate, ExcludeBDate, ExcludeEDate, CertDigest, Convert ( nchar(8), getdate(), 112 ) as CurDate
From xErpUser 
Where UserID = " + CFL.Q(strUserID) + @"
  and ComCode = " + CFL.Q(strComCode);


			try
			{
				dr = CFL.ExecuteDataTableReader(strClientID, strQuery, CommandType.Text);
			}
			catch (Exception e)
			{
				strErrorCode = "01";
				strErrorMsg = Resource.GetString(strLangID, "02");
				return CFL.EncodeData(strErrorCode, strErrorMsg, e);
			}

			// Check CertMethod
			if (!dr.Read())
			{
				strErrorCode = "01";
				strErrorMsg = Resource.GetString(strLangID, "03");
				return CFL.EncodeData(strErrorCode, strErrorMsg, null);
			}

			// Check Valid Period
			decimal dUserBdate = CFL.Tod(CFL.F(dr["UserBdate"]));
			decimal dUserEdate = CFL.Tod(CFL.F(dr["UserEdate"]));
			decimal dCurDate = CFL.Tod(CFL.F(dr["CurDate"]));
			if (dUserBdate > dCurDate || dUserEdate < dCurDate)
			{
				strErrorCode = "03";
				strErrorMsg = Resource.GetString(strLangID, "04") + " [" + strUserID + @"] " + Resource.GetString(strLangID, "05");
				return CFL.EncodeData(strErrorCode, strErrorMsg, null);
			}

			string strPassWdSaved = CFL.F(dr["PassWd"]).ToString();
			string strWfPassWdSaved = CFL.F(dr["WfPassWd"]).ToString();
			bool bExcludeCert = (CFL.Tod(CFL.F(dr["ExcludeBDate"])) <= dCurDate
				&& CFL.Tod(CFL.F(dr["ExcludeEDate"])) >= dCurDate);
			string strCertDigestSaved = CFL.F(dr["CertDigest"]).ToString();

			dr.Close();
			ArrayList alData = null;
			switch (strCertMethod)
			{
				case "USBK":
					goto case "CLNT";
				case "CLNT":
					if (!bExcludeCert)
					{
						alData = CheckCertificate(strLangID, strCertMethod, strKey, strCertDigest, strCertDigestSaved);

						if ("00" != alData[0].ToString())
							return alData;
					}
					// PassWord Check
					goto default;
				case "RSA":
					// Authorization Completed by RSA Solution
					break;
				default:
					// skip if Method is USBK or CLNT & SkipPasswd
					if (strCertMethod == "NONE" || !bSkipPasswd)
					{
						// Encrypt Password


						// Check Passwd
						if (strPassWd != strPassWdSaved)
						{
							strErrorCode = "02";
							strErrorMsg = Resource.GetString(strLangID, "06");
							return CFL.EncodeData(strErrorCode, strErrorMsg, null);
						}
						if (null != strWfPassWd)
						{

							if (strWfPassWd != strWfPassWdSaved)
							{
								strErrorCode = "03";
								strErrorMsg = Resource.GetString(strLangID, "07");
								return CFL.EncodeData(strErrorCode, strErrorMsg, null);
							}
						}
					}
					break;
			}

			return CFL.EncodeData(strErrorCode, strErrorMsg, null);
		}


		private static ArrayList ValidateUserWf(string strLangID, string strUserID, string strPassWd, string strWfPassWd, string strClientID
			                                  , string strCertDigest, string strKey, string strUserIP, object objConditions, string strCertMethod, string strComCode)
		{
			string strErrorCode = "00", strErrorMsg = "";

			// Validation Check
			// Not null Property
			if (null == strUserID || "" == strUserID)
			{
				strErrorCode = "01";
				strErrorMsg = Resource.GetString(strLangID, "01");
				return CFL.EncodeData(strErrorCode, strErrorMsg, null);
			}

			// Get Global Information
			string strQuery = @"
Select SkipPasswd	
From SmartMaster";

			bool bSkipPasswd = false;

			DataTableReader dr = null;
			try
			{
				dr = CFL.ExecuteDataTableReader(strClientID, strQuery, CommandType.Text);
			}
			catch
			{
				// proceed with no error
			}

			// can be null if Exception Thrown
			if (null != dr)
			{
				if (!dr.Read())
				{
					// proceed with no error
				}
				else
				{
					if (dr["SkipPasswd"].ToString() == "Y")
						bSkipPasswd = true;
				}
			}

			dr.Close();

			// Get Certification Info
			strQuery = @"
Select PassWd, WfPassWd, UserBdate, UserEdate, ExcludeBDate, ExcludeEDate, CertDigest, Convert ( nchar(8), getdate(), 112 ) as CurDate
From xErpUser 
Where UserID = " + CFL.Q(strUserID) + @"
  and ComCode = " + CFL.Q(strComCode);

			try
			{
				dr = CFL.ExecuteDataTableReader(strClientID, strQuery, CommandType.Text);
			}
			catch (Exception e)
			{
				strErrorCode = "01";
				strErrorMsg = Resource.GetString(strLangID, "02");
				return CFL.EncodeData(strErrorCode, strErrorMsg, e);
			}

			// Check CertMethod
			if (!dr.Read())
			{
				strErrorCode = "01";
				strErrorMsg = Resource.GetString(strLangID, "03");
				return CFL.EncodeData(strErrorCode, strErrorMsg, null);
			}

			// Check Valid Period
			decimal dUserBdate = CFL.Tod(CFL.F(dr["UserBdate"]));
			decimal dUserEdate = CFL.Tod(CFL.F(dr["UserEdate"]));
			decimal dCurDate = CFL.Tod(CFL.F(dr["CurDate"]));
			if (dUserBdate > dCurDate || dUserEdate < dCurDate)
			{
				strErrorCode = "03";
				strErrorMsg = Resource.GetString(strLangID, "04") + " [" + strUserID + @"] " + Resource.GetString(strLangID, "05");
				return CFL.EncodeData(strErrorCode, strErrorMsg, null);
			}

			string strPassWdSaved = CFL.F(dr["PassWd"]).ToString();
			string strWfPassWdSaved = CFL.F(dr["WfPassWd"]).ToString();
			bool bExcludeCert = (CFL.Tod(CFL.F(dr["ExcludeBDate"])) <= dCurDate
				&& CFL.Tod(CFL.F(dr["ExcludeEDate"])) >= dCurDate);
			string strCertDigestSaved = CFL.F(dr["CertDigest"]).ToString();

			dr.Close();
			ArrayList alData = null;
			switch (strCertMethod)
			{
				case "USBK":
					goto case "CLNT";
				case "CLNT":
					if (!bExcludeCert)
					{
						alData = CheckCertificate(strLangID, strCertMethod, strKey, strCertDigest, strCertDigestSaved);
						if ("00" != alData[0].ToString())
							return alData;
					}
					// PassWord Check
					goto default;
				case "RSA":
					// Authorization Completed by RSA Solution
					break;
				default:
					// skip if Method is USBK or CLNT & SkipPasswd
					if (strCertMethod == "NONE" || !bSkipPasswd)
					{
						// Encrypt Password
					//	string strPassWdEnc = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strPassWd, "MD5");
                        // SHA256 암호화
                        string strPassWdEnc = EncryptSHA256(strPassWd);


						// Check Passwd
						if (strPassWdEnc != strPassWdSaved)
						{
							strErrorCode = "02";
							strErrorMsg = Resource.GetString(strLangID, "06");
							return CFL.EncodeData(strErrorCode, strErrorMsg, null);
						}

						if (null != strWfPassWd)
						{
							// Encrypt Password
						//	string strWfPassWdEnc = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strWfPassWd, "MD5");
                            // SHA256 암호화
                            string strWfPassWdEnc = EncryptSHA256(strWfPassWd);


							if (strWfPassWdEnc != strWfPassWdSaved)
							{
								strErrorCode = "03";
								strErrorMsg = Resource.GetString(strLangID, "07");
								return CFL.EncodeData(strErrorCode, strErrorMsg, null);
							}
						}
					}
					break;
			}

			return CFL.EncodeData(strErrorCode, strErrorMsg, null);
		}

		#endregion

		#region SecuPKI

		/// <summary>
		/// Check User Certification is Valid
		/// </summary>
		/// <param name="strKey">Key to Encode Digest</param>
		/// <param name="strCertDigest">Digest from Client</param>
		/// <param name="strCertDigestSaved">Digest from Database</param>
		/// <returns></returns>
		private static ArrayList CheckCertificate(string strLangID, string strCertMethod, string strKey, string strCertDigestEnc, string strCertDigestSaved)
		{
			string strErrorCode = "00", strErrorMsg = "";

			// Check User Certificate
			// Error Code must be "04"
			if (strCertDigestSaved != DecryptData(strCertDigestEnc, strKey))
			{
				strErrorCode = "04";
				if (strCertMethod == "USBK")
					strErrorMsg = Resource.GetString(strLangID, "08");
				else
					strErrorMsg = Resource.GetString(strLangID, "09");
			}

			return CFL.EncodeData(strErrorCode, strErrorMsg, null);
		}

		/// <summary>
		/// Generate Random Key Value to Encrypt CertDigest Value
		/// </summary>
		/// <returns></returns>
		public static string GenerateKey(int iKeyLength)
		{
			Random rnd = new Random((int)DateTime.Now.Ticks);
			// Reduce the Probability of Repeated Value
			string strKey = rnd.Next(0, 1000 ^ iKeyLength).ToString();
			return strKey.PadLeft(iKeyLength, '0').Substring(0, iKeyLength);
		}

		public static string DecryptData(string strData, string strKey)
		{
			string strResult = "NOT SET";

			try
			{
				CAPICOM.EncryptedData Encryptor = new CAPICOM.EncryptedData();
				Encryptor.Algorithm.KeyLength = CAPICOM.CAPICOM_ENCRYPTION_KEY_LENGTH.CAPICOM_ENCRYPTION_KEY_LENGTH_MAXIMUM;
				Encryptor.Algorithm.Name = CAPICOM.CAPICOM_ENCRYPTION_ALGORITHM.CAPICOM_ENCRYPTION_ALGORITHM_3DES;
				Encryptor.SetSecret(strKey + "ForTheTwoPointZero", CAPICOM.CAPICOM_SECRET_TYPE.CAPICOM_SECRET_PASSWORD);
				Encryptor.Decrypt(strData);
				strResult = Encryptor.Content;
			}
			catch
			{
			}

			return strResult;
		}

		#endregion

		#region Data Logic about SMARTUser

		/// Handling SMARTUser Information
		protected const string c_PasswdNoChange = "************";
		protected const string c_UltraUserID = "xErpAdmin";
		protected const string c_UltraUserPwd = "eagle";


		/// <summary>
		/// Save User Information
		/// </summary>		
		public static ArrayList UserSave(object[] GDObj, SqlConnection db, SqlTransaction tr, bool bInTrans, object FormClass
			                           , HttpServerUtility Server, bool bSaveMode, string strCurEmp, string strLangID, string strUserID, string strUserName
			                           , string strUserType, string strPassWd, string strWfPassWd, string strUserBdate, string strUserEdate, string strUserDescr
			                           , string streMail, string[] sarComCode, string[] sarEmpCode)
		{
			return UserSave(GDObj, db, tr, bInTrans, FormClass
			              , Server, bSaveMode, strCurEmp, strLangID, strUserID, strUserName
			              , strUserType, strPassWd, strWfPassWd, strUserBdate, strUserEdate, strUserDescr
			              , streMail, sarComCode, sarEmpCode
			              , "20000101", "20501231");
		}


		/// <summary>
		/// Save User Information
		/// </summary>		
		public static ArrayList UserSave(object[] GDObj, SqlConnection db, SqlTransaction tr, bool bInTrans, object FormClass
			                           , HttpServerUtility Server, bool bSaveMode, string strCurEmp, string strLangID, string strUserID, string strUserName
			                           , string strUserType, string strPassWd, string strWfPassWd, string strUserBdate, string strUserEdate, string strUserDescr
			                           , string streMail, string[] sarComCode, string[] sarEmpCode
			                           , string strExcludeBDate, string strExcludeEDate)
		{
			// Global Data
			GData GD = new GData(GDObj);

			
			string strErrorCode = "00", strErrorMsg = "";
			string strQuery = "";
			
						
			DataTableReader dr;
			
			#region

			ArrayList alResult = new ArrayList();
			
            // Check Validity

			// UserID			
			if (!CFL.NotNullCheck(Msg(GDObj, GD.LangID, "L01026"), strUserID, GD.LangID, ref alResult, "OFddlCodeGubun"))
			{
				if (bInTrans)
					tr.Rollback();
				return alResult;
			}

			// UserName			
			if (!CFL.NotNullCheck(Msg(GDObj, GD.LangID, "L01027"), strUserName, strLangID, ref alResult))
			{
				if (bInTrans)
					tr.Rollback();
				return alResult;
			}
			
            // PassWd			
			if (!CFL.NotNullCheck(Msg(GDObj, GD.LangID, "L01177"), strPassWd, strLangID, ref alResult))
			{
				if (bInTrans)
					tr.Rollback();
				return alResult;
			}

			// WfPassWd			
			if (!CFL.NotNullCheck(Msg(GDObj, GD.LangID, "L01176"), strWfPassWd, strLangID, ref alResult))
			{
				if (bInTrans)
					tr.Rollback();
				return alResult;
			}

			// Period Check			
			if (!CFL.PeriodValidate(Msg(GDObj, GD.LangID, "L01119"), strUserBdate, Msg(GDObj, GD.LangID, "L01120"), strUserEdate, strLangID, ref alResult))
			{
				if (bInTrans)
					tr.Rollback();
				return alResult;
			}

			// ************* 2.Item Validation Check *****************
			// Selected EmpCheck
			// Normal User Must have Emp
			if (strUserType == "N" && 1 > sarComCode.Length)
			{
				if (bInTrans)
					tr.Rollback();
				
				return CFL.EncodeData("01", Msg(GDObj, GD.LangID, "M00341"), null);
			}
			else if (strUserType != "N" && strUserType != "S" && 0 < sarComCode.Length)
			{
				if (bInTrans)
					tr.Rollback();
								
				return CFL.EncodeData("01", Msg(GDObj, GD.LangID, "M00340"), null);
			}

            /*
			CommonFunctionLibrary.CGW.CGW cgw = new CommonFunctionLibrary.CGW.CGW();

			//********EmpCode
			for (int i = 0; i < sarComCode.Length; i++)
			{
				// EmpCode RefCheck
				object[] alData = cgw.GridGateWay(true, "OM", "EmpMaster", "Popup", "A.ComCode = " + CFL.Q(sarComCode[i]) + " and A.EmpCode = " + CFL.Q(sarEmpCode[i]), "EmpCode/", null, GDObj);

				cgw.Dispose();

				alResult.Clear();

				foreach (object obj in alData)
					alResult.Add(obj);

				if (!CFL.RefCheck((i + 1) + CFL.RS("MSG09", FormClass, GD.LangID), strLangID, ref alResult, "OFGrid_EmpCode_" + i))
				{
					if (bInTrans)
						tr.Rollback();
					return alResult;
				}

				// 동일 법인의 사원이 중복선택 여부 : 2016.09.01 막음
				//
                //for ( int j = i + 1; j < sarComCode.Length; j++ )
				//{
				//	if ( sarComCode[j].ToLower() == sarComCode[i].ToLower() )
				//	{
				//		if ( bInTrans )
				//			tr.Rollback();
				//	
                //        return CFL.EncodeData ( "OFGrid_ComName_" + j,CFL
                ( "MSG10", FormClass, GD.LangID ), null );
				//	}
				//}
                
			}
			*/
            #endregion

            // Password Encryption
            /*
			string strPassWdEnc = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strPassWd, "MD5");
			string strWfPassWdEnc = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strWfPassWd, "MD5");
            */

            // Password Encryption -  Hash 암호화 (By SHA256)           
            string strPassWdEnc = EncryptSHA256(strPassWd);
            string strWfPassWdEnc = EncryptSHA256(strPassWd);


            // insert mode
            if (bSaveMode)
			{
				// Check UserID Uniqueness
				strQuery = @"
Select UserID 
From xErpUser 
where ComCode = " + CFL.Q(GD.ComCode) + @"
  and UserID = " + CFL.Q(strUserID);

				
				try
				{				
					dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
				}
				catch (Exception e)
				{
					strErrorCode = "01";					
					strErrorMsg = Msg(GDObj, GD.LangID, "M00342");

					return CFL.EncodeData(strErrorCode, strErrorMsg, e);
				}

				// there's any
				if (dr.Read())
				{
					strErrorCode = "02";					
					strErrorMsg = Msg(GDObj, GD.LangID, "M00343");

					return CFL.EncodeData(strErrorCode, strErrorMsg, null);
				}

				dr.Close();

				// insert
				strQuery = @"

Insert into xErpUser ( ComCode, UserID, UserName, UserType, PassWd, WfPassWd, UserBdate, UserEdate, UserDescr, eMail
	                 , ExcludeBDate, ExcludeEDate )
    values (" + CFL.Q(GD.ComCode) + ", " + CFL.Q(strUserID) + ", " + CFL.Q(strUserName) + ", " + CFL.Q(strUserType)
	   + ", " + CFL.Q(strPassWdEnc) + ", " + CFL.Q(strWfPassWdEnc) + ", " + CFL.Q(strUserBdate) + ", " + CFL.Q(strUserEdate)
	   + ", " + CFL.Q(strUserDescr) + ", " + CFL.Q(streMail)
	   + ", " + CFL.Q(strExcludeBDate) + ", " + CFL.Q(strExcludeEDate) + ")";
			}
			else
			{
				strQuery = @"

Update xErpUser 
Set UserName = " + CFL.Q(strUserName);

				if (strPassWd != c_PasswdNoChange)
					strQuery += @"
  , PassWd = " + CFL.Q(strPassWdEnc);

				if (strWfPassWd != c_PasswdNoChange)
					strQuery += @"
  , WfPassWd = " + CFL.Q(strWfPassWdEnc);

				strQuery += @"
  , UserType = " + CFL.Q(strUserType) + @"
  , UserBdate = " + CFL.Q(strUserBdate) + @"
  , UserEdate = " + CFL.Q(strUserEdate) + @"
  , UserDescr = " + CFL.Q(strUserDescr) + @"
  , eMail = " + CFL.Q(streMail) + @"
  , ExcludeBDate = " + CFL.Q(strExcludeBDate) + @"
  , ExcludeEDate = " + CFL.Q(strExcludeEDate) + @" 
Where UserID = " + CFL.Q(strUserID) + @"
  and ComCode = " + CFL.Q(GD.ComCode);
			}

			// Release Currently Selected Emp :  2016.09.1 --> 다시 사용            
			strQuery += @"

Update EmpMaster
Set UserID = null 
Where UserID = " + CFL.Q(strUserID) + @"
  and ComCode = " + CFL.Q(GD.ComCode) + @" 
  and SiteCode = " + CFL.Q(GD.SiteCode);

			// Selected Emp information
			for (int i = 0; i < sarComCode.Length; i++)
			{
				strQuery += @"

Update EmpMaster 
Set UserID = " + CFL.Q(strUserID) + @" 
Where ComCode = " + CFL.Q(sarComCode[i]) + @" 
  and SiteCode = " + CFL.Q(GD.SiteCode) + @"  
  and EmpCode = " + CFL.Q(sarEmpCode[i]);
			}



			// begin transaction ( if not called in transaction )
			if (!bInTrans)
			{				
				CFL.BeginTran(GD, ref tr, ref db);
			}
			
			try
			{				
				CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref tr, ref db);
			}
			catch (Exception e)
			{
				tr.Rollback();

                strErrorCode = "03";
				                
				strErrorMsg = Msg(GDObj, GD.LangID, "M00150");

                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
			}

			// LicenseCheck
			License lic = new License(GD.LangID, GD.ClientID);
			
			alResult = lic.UserAccountCheck(GDObj, db, tr);

            if ("00" != alResult[0].ToString())
			{
				return alResult;
			}

			if (!bInTrans)
				tr.Commit();

			return CFL.EncodeData(strErrorCode, strUserID, null);
		}


		public static ArrayList SetUserData(object[] GDObj, OleDbConnection db, OleDbTransaction tr, bool bInTrans, object FormClass
		                              	  , HttpServerUtility Server, string strLangID, string strUserID, string strAbsentCheck, string strWfPassWd, string strPassWd, string streMail
                                          , bool bUseWfPwd)
		{
			// Global Data
			GData GD = new GData(GDObj);

			// Db Connect
			if (!bInTrans)
			{
				ArrayList alConnect = ClientCore.SetDB(GD, ref db);

				if ("00" != alConnect[0].ToString())
					return alConnect;
			}

			string strErrorCode = "00", strErrorMsg = "";

			OleDbCommand cm = null;

			ArrayList alResult = new ArrayList();
			
            // Check Validity
			
            // UserID			
			if (!CFL.NotNullCheck(Msg(GDObj, GD.LangID, "L01026"), strUserID, GD.LangID, ref alResult, "OFddlCodeGubun"))
			{
				if (bInTrans)
					tr.Rollback();
				return alResult;
			}

			// UserName			
			if (!CFL.NotNullCheck(Msg(GDObj, GD.LangID, "L01178"), strAbsentCheck, GD.LangID, ref alResult, "OFddlCodeGubun"))
			{
				if (bInTrans)
					tr.Rollback();
				return alResult;
			}

			// WfPassWd			
			if (!CFL.NotNullCheck(Msg(GDObj, GD.LangID, "L01176"), strWfPassWd, GD.LangID, ref alResult, "OFddlCodeGubun"))
			{
				if (bInTrans)
					tr.Rollback();
				return alResult;
			}

			// WfPassWd			
			if (!CFL.NotNullCheck(Msg(GDObj, GD.LangID, "L01020"), strPassWd, GD.LangID, ref alResult, "OFddlCodeGubun"))
			{
				if (bInTrans)
					tr.Rollback();
				return alResult;
			}

			string strQuery = @"
Update xErpUser 
Set AbsentCheck = " + CFL.Q(strAbsentCheck)	+ @"
  , eMail =  " + CFL.Q(streMail) + @"
  , UseWfPwd = " + CFL.Q(bUseWfPwd ? "Y" : "N");

			if (strWfPassWd != c_PasswdNoChange)
			{
				// Password Encryption
			//	string strWfPassWdEnc = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strWfPassWd, "MD5");
                // SHA256 암호화                           
                string strWfPassWdEnc = EncryptSHA256(strWfPassWd);

				strQuery += @"
  , WfPassWd = " + CFL.Q(strWfPassWdEnc);
			}

			if (strPassWd != c_PasswdNoChange)
			{
				// Password Encryption
			//	string strPassWdEnc = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strPassWd, "MD5");
                // SHA256 암호화           
                string strPassWdEnc = EncryptSHA256(strPassWd);

				strQuery += @"
  , PassWd = " + CFL.Q(strPassWdEnc);
			}

			strQuery += @"
Where UserID = " + CFL.Q(strUserID) + @"
  and ComCode = " + CFL.Q(GD.ComCode);



			// begin transaction ( if not called in transaction )
			if (!bInTrans)
				tr = db.BeginTransaction();

			// create command object with Transaction
			cm = new OleDbCommand(strQuery, db, tr);

			try
			{
				cm.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				tr.Rollback();

                strErrorCode = "01";				
				strErrorMsg = Msg(GDObj, GD.LangID, "M00344");

				return CFL.EncodeData(strErrorCode, strQuery, e);
			}

			if (!bInTrans)
				tr.Commit();

			return CFL.EncodeData(strErrorCode, strUserID, null);
		}


		public static ArrayList SetCertDigest(string strClientID, string strLangID
			                                , OleDbConnection db, OleDbTransaction tr, bool bInTrans, object FormClass
			                                , HttpServerUtility Server, string strUserID, string strStatus, string strCertDigest)
		{
			// Db Connect
			if (!bInTrans)
			{
				ArrayList alConnect = ClientCore.SetDB(strClientID, ref db);
				if ("00" != alConnect[0].ToString())
					return alConnect;
			}

			string strErrorCode = "00", strErrorMsg = "";

			OleDbCommand cm = null;

			if (strStatus == "R" || strStatus == "D" || strStatus == "A")
				strCertDigest = strStatus;

			string strQuery = "update xErpUser set CertDigest = " + CFL.Q(strCertDigest);
			strQuery += " where UserID = " + CFL.Q(strUserID);

			// begin transaction ( if not called in transaction )
			if (!bInTrans)
				tr = db.BeginTransaction();

			// create command object with Transaction
			cm = new OleDbCommand(strQuery, db, tr);
			try
			{
				cm.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				tr.Rollback();
				strErrorCode = "01";
				strErrorMsg = "수정에 실패하였습니다.";

				return CFL.EncodeData(strErrorCode, strQuery, e);
			}
			if (!bInTrans)
				tr.Commit();

			return CFL.EncodeData(strErrorCode, strUserID, null);
		}

		public static ArrayList LoadFormSecurity(object[] GDObj, string strModuleInitial)
		{
			// Global Data
			GData GD = new GData(GDObj);

			string strErrorCode = "00", strErrorMsg = "";

			// Get the FormGrade
			StringBuilder strQuery = new StringBuilder();

            strQuery.Append(@"

Select FormGrade	
From FormSecurity
Where SiteCode = ").Append(CFL.Q(GD.SiteCode)).Append(@" 
  and UserGroup = ").Append(CFL.Q(GD.UserGroup)).Append(@" 
  and ModuleInitial = ").Append(CFL.Q(strModuleInitial)).Append(@" 
  and ").Append(CFL.MakeModuleIniFilter(GD.ModuleUseCheck, "ModuleInitial"));


			DataTableReader dr;

			try
			{
				dr = CFL.ExecuteDataTableReader(GD, strQuery.ToString(), CommandType.Text);
			}
			catch (Exception e)
			{
				strErrorCode = "01";				
				strErrorMsg = Msg(GDObj, GD.LangID, "M00345");

				return CFL.EncodeData(strErrorCode, strErrorMsg, e);
			}

			return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
		}


		public static ArrayList GetWfUse(object[] GDObj, string strFormID)
		{
			// Global Data
			GData GD = new GData(GDObj);

			string strErrorCode = "00", strErrorMsg = "";

			string strQuery = @"
Select IsNull ( B.WfUse, " + CFL.Q("Y") + @" )
From ModuleObj A
	Left Outer Join	WfSiteMaster B on A.FormID = B.FormID and B.SiteCode = " + CFL.Q(GD.SiteCode) + @"
Where A.FormID = " + CFL.Q(strFormID) + @" 
  and A.ObjCategory = " + CFL.Q("F") + @" 
  and A.WfUse = " + CFL.Q("Y");

			DataTableReader dr;
			try
			{
				dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
			}
			catch (Exception e)
			{
				strErrorCode = "01";				
				strErrorMsg = Msg(GDObj, GD.LangID, "M00345");

				return CFL.EncodeData(strErrorCode, strErrorMsg, e);
			}

			return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
		}

        #endregion

        private static string Msg(object[] GDObj, string strLangID, string strMsgCode)
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


        // SHA256 - 암호화 모듈
        public static string EncryptSHA256(string text)
        {
            var sha = new System.Security.Cryptography.SHA256Managed();

            byte[] data = sha.ComputeHash(Encoding.ASCII.GetBytes(text));

            var sb = new StringBuilder();

            foreach (byte b in data)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

    }
}