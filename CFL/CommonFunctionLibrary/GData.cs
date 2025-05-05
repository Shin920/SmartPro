using System;
using System.Collections;
using System.Data.OleDb;

namespace SMART
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class GData
	{
		public GData()
		{
		}

		public GData(string strUserID, string strSiteCode, string strClientID, string strLangID, string strUserIP
					, string strModuleUseCheck, string strLicenseType, int iUserAccount, bool bUseEDI)
		{
			// Set Information Given
			UserID = strUserID;
			SiteCode = strSiteCode;
			ClientID = strClientID;
			LangID = strLangID;
			UserIP = strUserIP;
			ModuleUseCheck = strModuleUseCheck;
			LicenseType = strLicenseType;
			UserAccount = iUserAccount;
			UseEDI = bUseEDI;

			// Assign Default Value
			UserName = "";
			UserType = "";
			eMail = "";
			UserGroup = "";
			EmpCode = "";
			EmpName = "";
			DeptCode = "";
			DeptName = "";
			CcCode = "";
			SiteName = "";
			SiteInitial = "";
			DefaultInvStatus = "";
			DigitNo_Qty = 0;
			DigitType_Qty = 0;
			DigitNo_Price1 = 0;
			DigitType_Price1 = 0;
			DigitNo_Price2 = 0;
			DigitType_Price2 = 0;
			DigitNo_sdDiscnt = 0;
			DigitType_sdDiscnt = 0;
			DigitNo_ppScrap = 0;
			DigitType_ppScrap = 0;
			DigitNo_ppQtyPer = 0;
			DigitType_ppQtyPer = 0;
			DigitNo_coUnitPrice = 0;
			DigitType_coUnitPrice = 0;
			DigitNo_coRatio = 0;
			DigitType_coRatio = 0;
			poDefaultExchGubun = "";
			sdDefaultExchGubun = "";
			ComCode = "";
			ComName = "";
			GroupName = "";
			DigitNo_glAmnt1 = 0;
			DigitType_glAmnt1 = 0;
			DigitNo_glAmnt2 = 0;
			DigitType_glAmnt2 = 0;
			DigitNo_ExchRate = 0;
			DigitType_ExchRate = 0;
			EisAmnt1_BoundAmnt = 0m;
			EisAmnt1_BoundName = "";
			EisAmnt2_BoundAmnt = 0m;
			EisAmnt2_BoundName = "";
			glCashAcc = "";
			CurrCode = "";
			CultureID = "";
			glWipAcc = "";
			CcExpDD = "";
			CoaCode = "";
			CoaName = "";
			CurDate = CFL.GetSysDateTime().Substring(0, 8);
			DomainName = "";


			// 협업 전용 (접속 DB 정보  /  업체코드 /  재무사용 여부  /  WeightLocation)  && 신생산 사용여부
			TYURL = "";
			PTCode = "";
			FINUse = "";
			WeightLocation = "";

			NPPUse = "";

            // 사업장 고유 ID
            SiteNo = "";


			// Db Connect
			OleDbConnection db = null;

			ArrayList alResult = ClientCore.SetDB(strClientID, ref db);

			if ("00" != alResult[0].ToString())
				return;

			// Load EmpMaster Information
			string strQuery = @"
Select A.UserName, A.UserType, A.eMail, A.UserGroup
	, B.EmpCode, B.EmpName, B.DefaultDept, B.DeptName, B.CcCode
	, C.SiteName, C.SiteInitial, C.DefaultInvStatus
	, C.DigitNo_Qty, C.DigitType_Qty, C.DigitNo_Price1, C.DigitType_Price1
	, C.DigitNo_Price2, C.DigitType_Price2, C.DigitNo_sdDiscnt, C.DigitType_sdDiscnt
	, C.DigitNo_ppScrap, C.DigitType_ppScrap, C.DigitNo_ppQtyPer, C.DigitType_ppQtyPer
	, C.DigitNo_coUnitPrice, C.DigitType_coUnitPrice, C.DigitNo_coRatio, C.DigitType_coRatio
	, C.DigitNo_mmOhPrice, C.DigitType_mmOhPrice
	, C.poDefaultExchGubun, C.sdDefaultExchGubun, C.ComCode, E.ComName
	, D.GroupNameHan
	, E.DigitNo_glAmnt1, E.DigitType_glAmnt1, E.DigitNo_glAmnt2, E.DigitType_glAmnt2
	, E.DigitNo_ExchRate, E.DigitType_ExchRate
	, E.EisAmnt1_BoundAmnt, E.EisAmnt1_BoundName, E.EisAmnt2_BoundAmnt, E.EisAmnt2_BoundName
	, E.glCashAcc, E.CurrCode, E.CultureID, E.glWipAcc, E.CcExpDD, E.CoaCode, E.ModuleUse
	, F.CoaName
	, Convert ( char(8), getdate(), 112 ) as CurDate

    -- 협업 전용 (DB 접속정보 / 업체코드 / 재무사용 여부 / WeightLocation)
    /*
    , G.DBUrl
    , D.PTCode
    , D.FINUseYN
    , C.WeightLocation
    */
    , '' As DBUrl
    , D.PTCode
    , '' As FINUseYN
    , '' As WeightLocation

    , D.NPPUse
    , C.SiteNo

From xErpUser A with (NOLOCK)
	Left Outer Join 
	(
		Select A.EmpCode, A.EmpName, A.DefaultDept, B.DeptName, B.DeptType, B.CcCode	
        From EmpMaster A with (NOLOCK)
			Left Outer Join	DeptMaster B with (NOLOCK) on A.DefaultDept = B.DeptCode and A.ComCode = B.ComCode
		   , SiteMaster C with (NOLOCK)
		Where A.UserID = " + CFL.Q(strUserID) + @"
		  and A.ComCode = C.ComCode and C.SiteCode = " + CFL.Q(strSiteCode) + @"
	) B	on 0 = 0

	Left Outer Join	SiteMaster C with (NOLOCK) on C.SiteCode = " + CFL.Q(strSiteCode) + @"    
    Left Outer Join SmartMaster D with (NOLOCK)	on 0 = 0	
    Left Outer Join CompanyMaster E with (NOLOCK) on C.ComCode = E.ComCode
	Left Outer Join CoaMaster F with (NOLOCK) on E.CoaCode = F.CoaCode
Where A.UserID = " + CFL.Q(strUserID) + @"
  and A.ComCode = ( Select ComCode
                    From SiteMaster
                    Where SiteCode = " + CFL.Q(strSiteCode) + " )"; 

			OleDbCommand cm = new OleDbCommand(strQuery, db);
			OleDbDataReader dr = null;

			try
			{
				dr = cm.ExecuteReader();
			}
			catch
			{
				// Errors that can continue
				db.Close();
				return;
			}

			string strComModuleUse = "YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY";

			if (dr.Read())
			{
				UserName = CFL.F(dr["UserName"]).ToString();
				UserType = CFL.F(dr["UserType"]).ToString();
				eMail = CFL.F(dr["eMail"]).ToString();
				UserGroup = CFL.F(dr["UserGroup"]).ToString();
				EmpCode = CFL.F(dr["EmpCode"]).ToString();
				EmpName = CFL.F(dr["EmpName"]).ToString();
				DeptCode = CFL.F(dr["DefaultDept"]).ToString();
				DeptName = CFL.F(dr["DeptName"]).ToString();
				CcCode = CFL.F(dr["CcCode"]).ToString();
				SiteName = CFL.F(dr["SiteName"]).ToString();
				SiteInitial = CFL.F(dr["SiteInitial"]).ToString();
				DefaultInvStatus = CFL.F(dr["DefaultInvStatus"]).ToString();
				DigitNo_Qty = CFL.Tos(CFL.F(dr["DigitNo_Qty"]));
				DigitType_Qty = CFL.Tos(CFL.F(dr["DigitType_Qty"]));
				DigitNo_Price1 = CFL.Tos(CFL.F(dr["DigitNo_Price1"]));
				DigitType_Price1 = CFL.Tos(CFL.F(dr["DigitType_Price1"]));
				DigitNo_Price2 = CFL.Tos(CFL.F(dr["DigitNo_Price2"]));
				DigitType_Price2 = CFL.Tos(CFL.F(dr["DigitType_Price2"]));
				DigitNo_sdDiscnt = CFL.Tos(CFL.F(dr["DigitNo_sdDiscnt"]));
				DigitType_sdDiscnt = CFL.Tos(CFL.F(dr["DigitType_sdDiscnt"]));
				DigitNo_ppScrap = CFL.Tos(CFL.F(dr["DigitNo_ppScrap"]));
				DigitType_ppScrap = CFL.Tos(CFL.F(dr["DigitType_ppScrap"]));
				DigitNo_ppQtyPer = CFL.Tos(CFL.F(dr["DigitNo_ppQtyPer"]));
				DigitType_ppQtyPer = CFL.Tos(CFL.F(dr["DigitType_ppQtyPer"]));
				DigitNo_coUnitPrice = CFL.Tos(CFL.F(dr["DigitNo_coUnitPrice"]));
				DigitType_coUnitPrice = CFL.Tos(CFL.F(dr["DigitType_coUnitPrice"]));
				DigitNo_coRatio = CFL.Tos(CFL.F(dr["DigitNo_coRatio"]));
				DigitType_coRatio = CFL.Tos(CFL.F(dr["DigitType_coRatio"]));
				DigitNo_mmOhPrice = CFL.Tos(CFL.F(dr["DigitNo_mmOhPrice"]));
				DigitType_mmOhPrice = CFL.Tos(CFL.F(dr["DigitType_mmOhPrice"]));
				poDefaultExchGubun = CFL.F(dr["poDefaultExchGubun"]).ToString();
				sdDefaultExchGubun = CFL.F(dr["sdDefaultExchGubun"]).ToString();
				ComCode = CFL.F(dr["ComCode"]).ToString();
				ComName = CFL.F(dr["ComName"]).ToString();
				GroupName = CFL.F(dr["GroupNameHan"]).ToString();
				DigitNo_glAmnt1 = CFL.Tos(CFL.F(dr["DigitNo_glAmnt1"]));
				DigitType_glAmnt1 = CFL.Tos(CFL.F(dr["DigitType_glAmnt1"]));
				DigitNo_glAmnt2 = CFL.Tos(CFL.F(dr["DigitNo_glAmnt2"]));
				DigitType_glAmnt2 = CFL.Tos(CFL.F(dr["DigitType_glAmnt2"]));
				DigitNo_ExchRate = CFL.Tos(CFL.F(dr["DigitNo_ExchRate"]));
				DigitType_ExchRate = CFL.Tos(CFL.F(dr["DigitType_ExchRate"]));
				EisAmnt1_BoundAmnt = CFL.Tod(CFL.F(dr["EisAmnt1_BoundAmnt"]));
				EisAmnt1_BoundName = CFL.F(dr["EisAmnt1_BoundName"]).ToString();
				EisAmnt2_BoundAmnt = CFL.Tod(CFL.F(dr["EisAmnt2_BoundAmnt"]));
				EisAmnt2_BoundName = CFL.F(dr["EisAmnt2_BoundName"]).ToString();
				glCashAcc = CFL.F(dr["glCashAcc"]).ToString();
				CurrCode = CFL.F(dr["CurrCode"]).ToString();
				CultureID = CFL.F(dr["CultureID"]).ToString();
				glWipAcc = CFL.F(dr["glWipAcc"]).ToString();
				CcExpDD = CFL.F(dr["CcExpDD"]).ToString();
				CoaCode = CFL.F(dr["CoaCode"]).ToString();
				CoaName = CFL.F(dr["CoaName"]).ToString();
				CurDate = CFL.F(dr["CurDate"]).ToString();
				strComModuleUse = CFL.F(dr["ModuleUse"]).ToString();

				// 협업 전용
				TYURL = CFL.F(dr["DBUrl"]).ToString();
				PTCode = CFL.F(dr["PTCode"]).ToString();
				FINUse = CFL.F(dr["FINUseYN"]).ToString();
				WeightLocation = CFL.F(dr["WeightLocation"]).ToString();

				// 신생산 사용여부
				NPPUse = CFL.F(dr["NPPUse"]).ToString();

                // 사업장 고유 ID
                SiteNo = CFL.F(dr["SiteNo"]).ToString();
            }

			dr.Close();
			db.Close();

			// Update ModuleUseString according to ComModuleUse
			for (int i = 0; i < strModuleUseCheck.Length; i++)
			{
				if (strModuleUseCheck.Substring(i, 1) == "Y" && strComModuleUse.Length > i && strComModuleUse.Substring(i, 1) == "Y")
					strModuleUseCheck = strModuleUseCheck.Substring(0, i) + "Y" + strModuleUseCheck.Substring(i + 1, strModuleUseCheck.Length - i - 1);
				else
					strModuleUseCheck = strModuleUseCheck.Substring(0, i) + "N" + strModuleUseCheck.Substring(i + 1, strModuleUseCheck.Length - i - 1);
			}
			ModuleUseCheck = strModuleUseCheck;

			// Get DomainName
			ClientCore.ClientInfo cli = ClientCore.GetClientInfo(strClientID);
			DomainName = cli.DomainName;

			SecKind = "A";
		}


		public GData(object GDObj)
		{
			object[] alData = (object[])GDObj;

			if (null != alData && "00" == alData[0].ToString() && alData.Length > 4)
			{
				UserName = alData[4].ToString();
				UserType = alData[5].ToString();
				SiteName = alData[6].ToString();
				SiteInitial = alData[7].ToString();
				DefaultInvStatus = alData[8].ToString();

				DigitNo_Qty = CFL.Tos(alData[9]);
				DigitType_Qty = CFL.Tos(alData[10]);
				DigitNo_Price1 = CFL.Tos(alData[11]);
				DigitType_Price1 = CFL.Tos(alData[12]);
				DigitNo_Price2 = CFL.Tos(alData[13]);
				DigitType_Price2 = CFL.Tos(alData[14]);
				DigitNo_sdDiscnt = CFL.Tos(alData[15]);
				DigitType_sdDiscnt = CFL.Tos(alData[16]);
				DigitNo_ppScrap = CFL.Tos(alData[17]);
				DigitType_ppScrap = CFL.Tos(alData[18]);
				DigitNo_ppQtyPer = CFL.Tos(alData[19]);
				DigitType_ppQtyPer = CFL.Tos(alData[20]);
				DigitNo_coUnitPrice = CFL.Tos(alData[21]);
				DigitType_coUnitPrice = CFL.Tos(alData[22]);
				DigitNo_coRatio = CFL.Tos(alData[23]);
				DigitType_coRatio = CFL.Tos(alData[24]);

				poDefaultExchGubun = alData[25].ToString();
				sdDefaultExchGubun = alData[26].ToString();

				CurrCode = alData[27].ToString();
				DigitNo_glAmnt1 = CFL.Tos(alData[28]);
				DigitType_glAmnt1 = CFL.Tos(alData[29]);
				DigitNo_glAmnt2 = CFL.Tos(alData[30]);
				DigitType_glAmnt2 = CFL.Tos(alData[31]);
				DigitNo_ExchRate = CFL.Tos(alData[32]);
				DigitType_ExchRate = CFL.Tos(alData[33]);
				glCashAcc = alData[34].ToString();
				ModuleUseCheck = alData[35].ToString();
				ComCode = alData[36].ToString();
				ComName = alData[37].ToString();
				CoaCode = alData[38].ToString();
				CoaName = alData[39].ToString();

				EmpName = alData[40].ToString();

				glWipAcc = alData[41].ToString();
				CcExpDD = alData[42].ToString();

				CurDate = alData[43].ToString();

				DeptCode = alData[44].ToString();
				DeptName = alData[45].ToString();
				CcCode = alData[46].ToString();

				eMail = alData[47].ToString();
				UserGroup = alData[48].ToString();
				EmpCode = alData[49].ToString();

				UserID = alData[50].ToString();
				SiteCode = alData[51].ToString();
				LangID = alData[52].ToString();
				ClientID = alData[53].ToString();

				GroupName = alData[54].ToString();
				UserIP = alData[55].ToString();

				CultureID = alData[56].ToString();
				LicenseType = alData[57].ToString();
				UserAccount = CFL.Toi(alData[58]);
				UseEDI = ((string)alData[59] == "Y");

				EisAmnt1_BoundAmnt = CFL.Tod(alData[60]);
				EisAmnt1_BoundName = alData[61].ToString();
				EisAmnt2_BoundAmnt = CFL.Tod(alData[62]);
				EisAmnt2_BoundName = alData[63].ToString();

				DomainName = alData[64].ToString();

				if (alData.Length > 65)
					DigitNo_mmOhPrice = CFL.Tos(alData[65]);
				if (alData.Length > 66)
					DigitType_mmOhPrice = CFL.Tos(alData[66]);

				SecKind = "A";


				// 협업 전용
				TYURL = alData[67].ToString();
				PTCode = alData[68].ToString();
				FINUse = alData[69].ToString();
				WeightLocation = alData[70].ToString();

				// 신생산 사용여부
				NPPUse = alData[71].ToString();

                // 사업장 고유 ID
                SiteNo = alData[72].ToString();

            }
		}


		public object[] ToArray()
		{
			// 협업 전용 (4개 추가) + 신생산여부 1개 추가  = 5
		//  const int c_ArraySize = 67; 
		//	const int c_ArraySize = 72;

            // 사업장 고유 ID 추가
            const int c_ArraySize = 73; 

			object[] alData = new object[c_ArraySize];

			alData[0] = "00";
			alData[1] = "";
			alData[2] = c_ArraySize;
			alData[3] = 1;

			alData[4] = UserName;
			alData[5] = UserType;
			alData[6] = SiteName;
			alData[7] = SiteInitial;
			alData[8] = DefaultInvStatus;

			alData[9] = DigitNo_Qty;
			alData[10] = DigitType_Qty;
			alData[11] = DigitNo_Price1;
			alData[12] = DigitType_Price1;
			alData[13] = DigitNo_Price2;
			alData[14] = DigitType_Price2;
			alData[15] = DigitNo_sdDiscnt;
			alData[16] = DigitType_sdDiscnt;
			alData[17] = DigitNo_ppScrap;
			alData[18] = DigitType_ppScrap;
			alData[19] = DigitNo_ppQtyPer;
			alData[20] = DigitType_ppQtyPer;
			alData[21] = DigitNo_coUnitPrice;
			alData[22] = DigitType_coUnitPrice;
			alData[23] = DigitNo_coRatio;
			alData[24] = DigitType_coRatio;

			alData[25] = poDefaultExchGubun;
			alData[26] = sdDefaultExchGubun;

			alData[27] = CurrCode;
			alData[28] = DigitNo_glAmnt1;
			alData[29] = DigitType_glAmnt1;
			alData[30] = DigitNo_glAmnt2;
			alData[31] = DigitType_glAmnt2;
			alData[32] = DigitNo_ExchRate;
			alData[33] = DigitType_ExchRate;
			alData[34] = glCashAcc;
			alData[35] = ModuleUseCheck;
			alData[36] = ComCode;
			alData[37] = ComName;
			alData[38] = CoaCode;
			alData[39] = CoaName;

			alData[40] = EmpName;

			alData[41] = glWipAcc;
			alData[42] = CcExpDD;

			alData[43] = CurDate;

			alData[44] = DeptCode;
			alData[45] = DeptName;
			alData[46] = CcCode;

			alData[47] = eMail;
			alData[48] = UserGroup;
			alData[49] = EmpCode;

			alData[50] = UserID;
			alData[51] = SiteCode;
			alData[52] = LangID;
			alData[53] = ClientID;

			alData[54] = GroupName;
			alData[55] = UserIP;

			alData[56] = CultureID;
			alData[57] = LicenseType;
			alData[58] = UserAccount;
			alData[59] = (UseEDI ? "Y" : "N");

			alData[60] = EisAmnt1_BoundAmnt;
			alData[61] = EisAmnt1_BoundName;
			alData[62] = EisAmnt2_BoundAmnt;
			alData[63] = EisAmnt2_BoundName;

			alData[64] = DomainName;

			alData[65] = DigitNo_mmOhPrice;
			alData[66] = DigitType_mmOhPrice;

			// 협업 전용 
			alData[67] = TYURL;
			alData[68] = PTCode;
			alData[69] = FINUse;
			alData[70] = WeightLocation;

			// 신생산 사용여부
			alData[71] = NPPUse;

            // 사업장 고유 ID
            alData[72] = SiteNo;


            return alData;
		}

		/// <summary>
		/// Current Date / Time
		/// </summary>
		public readonly string CurDate;

		/// <summary>
		/// Language ID
		/// </summary>
		public readonly string LangID;

		/// <summary>
		/// Client ID & DomainName
		/// </summary>
		public readonly string ClientID;
		public readonly string DomainName;

		/// <summary>
		/// User IP
		/// </summary>
		public readonly string UserIP;

		/// <summary>
		/// User Information
		/// </summary>
		public readonly string UserID;
		public readonly string UserName;
		public readonly string UserType;
		public readonly string eMail;
		public readonly string UserGroup;

		/// <summary>
		/// Emp Information
		/// </summary>
		public readonly string EmpCode;
		public readonly string EmpName;

		/// <summary>
		/// Dept
		/// </summary>
		public readonly string DeptCode;
		public readonly string DeptName;
		public readonly string DeptType;

		/// <summary>
		/// Cc
		/// </summary>
		public readonly string CcCode;

		/// <summary>
		/// Company
		/// </summary>
		public readonly string ComCode;
		public readonly string ComName;

		/// <summary>
		/// Business Area
		/// </summary>
		public readonly string CoaCode;
		public readonly string CoaName;

		/// <summary>
		/// Site Information
		/// </summary>
		public readonly string SiteCode;
		public readonly string SiteName;
		public readonly string SiteInitial;
		public readonly string DefaultInvStatus;
		public readonly string poDefaultExchGubun;
		public readonly string sdDefaultExchGubun;

		/// <summary>
		/// Digit Information dependent on Site
		/// </summary>
		public readonly short DigitNo_Qty;
		public readonly short DigitType_Qty;
		public readonly short DigitNo_Price1;
		public readonly short DigitType_Price1;
		public readonly short DigitNo_Price2;
		public readonly short DigitType_Price2;
		public readonly short DigitNo_sdDiscnt;
		public readonly short DigitType_sdDiscnt;
		public readonly short DigitNo_ppScrap;
		public readonly short DigitType_ppScrap;
		public readonly short DigitNo_ppQtyPer;
		public readonly short DigitType_ppQtyPer;
		public readonly short DigitNo_coUnitPrice;
		public readonly short DigitType_coUnitPrice;
		public readonly short DigitNo_coRatio;
		public readonly short DigitType_coRatio;
		public readonly short DigitNo_mmOhPrice;
		public readonly short DigitType_mmOhPrice;

		/// <summary>
		/// SmartMaster Information
		/// </summary>
		public readonly string CurrCode;
		public readonly string glCashAcc;
		public readonly string glWipAcc;
		public readonly string CcExpDD;
		// GroupName
		public readonly string GroupName;

		/// <summary>
		/// Digit Information independent of Site
		/// </summary>
		public readonly short DigitNo_glAmnt1;
		public readonly short DigitType_glAmnt1;
		public readonly short DigitNo_glAmnt2;
		public readonly short DigitType_glAmnt2;
		public readonly short DigitNo_ExchRate;
		public readonly short DigitType_ExchRate;

		/// <summary>
		/// Culture Information
		/// </summary>
		public readonly string CultureID;

		/// <summary>
		/// License Information
		/// </summary>
		public readonly string ModuleUseCheck;
		public readonly string LicenseType;
		public readonly int UserAccount;
		public readonly bool UseEDI;

		/// <summary>
		/// EIS Information
		/// </summary>
		public readonly decimal EisAmnt1_BoundAmnt;
		public readonly string EisAmnt1_BoundName;
		public readonly decimal EisAmnt2_BoundAmnt;
		public readonly string EisAmnt2_BoundName;

		public string SecKind;


		// 협업 전용
		public readonly string TYURL;
		public readonly string PTCode;
		public readonly string FINUse;
		public readonly string WeightLocation;

		// 신생산 사용여부
		public readonly string NPPUse;

        // 사업장 고유 ID
        public readonly string SiteNo;
    }

}