using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

namespace SMART
{
    /// <summary>
    /// Summary description for WfSiteMaster.
    /// </summary>
    [WebService(Name = "WorkFlow", Namespace = "http://WorkFlow.UsgKorea.com")]
    public class WorkFlow : System.Web.Services.WebService
    {
        public WorkFlow()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();
        }

        public WorkFlow(SqlConnection db, SqlTransaction tr)
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();
            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// WfSiteMster
        /// </summary>

        [WebMethod]
        public ArrayList WfSMGrid(bool bStartQuery, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            if (!bStartQuery)
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);

            string strQuery = @"
select	A.FormID, IsNull ( C.ObjName, D.ObjName ) as ObjName, IsNull ( B.WfUse, N'Y' ) as WfUse, B.WfDescr
from	ModuleObj A
	Left Outer Join	WfSiteMaster B
		on	A.FormID = B.FormID and B.SiteCode = " + CFL.Q(GD.SiteCode) + @"
	Left Outer Join	ComMenu C
		on	A.ObjID = C.ObjID and C.LangID = " + CFL.Q(GD.LangID) + @" and C.ComCode = " + CFL.Q(GD.ComCode) + @"
	Left Outer Join	ModuleObjLang D
		on	A.ObjID = D.ObjID and D.LangID = " + CFL.Q(GD.LangID) + @" 
where	A.ObjCategory = " + CFL.Q("F") + @" and A.WfUse = " + CFL.Q("Y") + @" 
	and IsNull ( D.ObjUse, " + CFL.Q("N") + @" ) = " + CFL.Q("Y") + @"
	and IsNull ( C.ObjUse, " + CFL.Q("Y") + @" ) = " + CFL.Q("Y");

            // Check Developing Menus
            if (GD.LicenseType != "D")
                strQuery += @"
	and IsNull ( D.Developing, " + CFL.Q("Y") + @" ) = " + CFL.Q("N");

            strQuery += @"
order by	A.SortKey, A.FormID";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M01", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        [WebMethod]
        public ArrayList WfSMSave(object[] GDObj, string strLangID, string[] sarFormID, string[] sarWfUse, string[] sarWfDescr)
        {
            // Global Data
            GData GD = new GData(GDObj);


            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            ArrayList alResult = new ArrayList();

            // ************* Item Validation Check *****************
            // the number of Item >= 1
            for (int i = 0; i < sarFormID.Length; i++)
            {
                // Not Null Check
                // WfUse Check
                if (!CFL.NotNullCheck((i + 1) + CFL.RS("P01", this, Server, GD.LangID), sarWfUse[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
            }

            strQuery = "delete from	WfSiteMaster where SiteCode = " + CFL.Q(GD.SiteCode);

            // Item Insert
            for (int i = 0; i < sarFormID.Length; i++)
            {
                strQuery += @"
					insert into WfSiteMaster ( SiteCode, FormID, WfUse, WfDescr )
					values ( " + CFL.Q(GD.SiteCode) + ", " + CFL.Q(sarFormID[i])
                        + ", " + CFL.Q(sarWfUse[i]) + ", " + CFL.Q(sarWfDescr[i]) + " )";
            }

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "03";
                strErrorMsg = CFL.RS("M02", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Transaction Commit
            if (!m_bInTrans)
                m_tr.Commit();

            // Return DocNo when Success
            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }

        [WebMethod]
        public ArrayList WfSMDdl(object[] GDObj, string strWfUse)
        {
            // Global Data
            GData GD = new GData(GDObj);


            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select	A.FormID, IsNull ( C.ObjName, D.ObjName ) as ObjName
from	ModuleObj A 
	Left Outer Join	WfSiteMaster B
		on	A.FormID = B.FormID and B.SiteCode = " + CFL.Q(GD.SiteCode) + @"
	Left Outer Join	ComMenu C
		on	A.ObjID = C.ObjID and C.LangID = " + CFL.Q(GD.LangID) + @" and C.ComCode = " + CFL.Q(GD.ComCode) + @"
	Left Outer Join	ModuleObjLang D
		on	A.ObjID = D.ObjID and D.LangID = " + CFL.Q(GD.LangID) + @" 
where	A.ObjCategory = " + CFL.Q("F") + @" and A.WfUse = " + CFL.Q("Y") + "and B.WfUse = " + CFL.Q("Y");

            if (null != strWfUse && "" == strWfUse)
                strQuery += " and IsNull ( B.WfUse, " + CFL.Q("Y") + @" ) = " + CFL.Q("Y");

            strQuery += @"
	and IsNull ( D.ObjUse, " + CFL.Q("N") + @" ) = " + CFL.Q("Y") + @"
	and IsNull ( C.ObjUse, " + CFL.Q("Y") + @" ) = " + CFL.Q("Y");

            // Check Developing Menus
            if (GD.LicenseType != "D")
                strQuery += @"
	and IsNull ( D.Developing, " + CFL.Q("Y") + @" ) = " + CFL.Q("N");

            strQuery += " order by A.SortKey, A.FormID";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M01", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeDdlData(strErrorCode, strErrorMsg, dr, null);
        }


        [WebMethod]
        public ArrayList WfSMGetWfUse(object[] GDObj, string strFormID)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select	IsNull ( B.WfUse, " + CFL.Q("Y") + @" )
from	ModuleObj A
	Left Outer Join	WfSiteMaster B
		on	A.FormID = B.FormID and B.SiteCode = " + CFL.Q(GD.SiteCode) + @"
where A.FormID = " + CFL.Q(strFormID) + @" and A.ObjCategory = " + CFL.Q("F") + @" and A.WfUse = " + CFL.Q("Y");

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M01", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        /// <summary>
        /// WfRoute
        /// </summary>

        [WebMethod]
        public ArrayList WfRGrid(bool bStartQuery, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);


            string strErrorCode = "00", strErrorMsg = "";

            if (!bStartQuery)
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);

            string strQuery = @"
				select	RouteCode, RouteName, StepCnt
				from	WfRoute
				";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += " where " + strWhereClause;

            strQuery += " order by RouteCode";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M01", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        [WebMethod]
        public ArrayList WfRSave(object[] GDObj, bool bSaveMode, string strComCode, string strCurEmp, string strLangID, string strSiteCode
            , string strRouteCode, string strRouteName, string strFirstStepTitle, string strUserGroup, string strRouteDescr, string strRouteUse
            , string[] sarStepTitle, string[] sarUserID, string[] sarAbsentEmp)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            DataTableReader dr = null;
            ArrayList alResult = new ArrayList();

            // ********Header Validation Check

            // Not Null Check
            // RouteCode
            if (!CFL.NotNullCheck(CFL.RS("POP01", this, Server, GD.LangID), strRouteCode, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // RouteName
            if (!CFL.NotNullCheck(CFL.RS("POP02", this, Server, GD.LangID), strRouteName, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // FirstStepTitle
            if (!CFL.NotNullCheck(CFL.RS("POP03", this, Server, GD.LangID), strFirstStepTitle, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // RouteUse
            if (!CFL.NotNullCheck(CFL.RS("POP04", this, Server, GD.LangID), strRouteUse, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            // ************* 2.Item Validation Check *****************
            // the number of Item >= 1
            if (1 > sarStepTitle.Length)
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return CFL.EncodeData("01", CFL.RS("M03", this, Server, GD.LangID), null);
            }

            CGW cgw = new CGW();
            //********ItemCode
            for (int i = 0; i < sarStepTitle.Length; i++)
            {
                // EmpCode RefCheck
                //			alResult = cgw.GridGateWay ( true, "OM", "EmpMaster", "Popup", "A.SiteCode = " + CFL.Q ( strSiteCode ) 
                //				+ " and A.EmpCode = " + CFL.Q ( sarEmpCode[i] ), "EmpCode/", null, GDObj );
                //			if ( !CFL.RefCheck ( CFL.RS ( "P02", this, Server, GD.LangID ), strLangID, ref alResult ) )
                //			{
                //				if ( m_bInTrans )
                //					m_tr.Rollback();
                //				return alResult;
                //			}

                // Sinbad Starts Inserting - 2003/12/02
                alResult = cgw.GridGateWay(true, "OM", "xErpUser", "Popup", "UserID = " + CFL.Q(sarUserID[i]), "UserID/", null, GDObj);
                if (!CFL.RefCheck(CFL.RS("M22", this, Server, GD.LangID), strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return CFL.EncodeData("01", CFL.RS("M21", this, Server, GD.LangID), null);
                }
                // Sinbad Ends Inserting - 2003/12/02

                if (sarStepTitle[i].Length > 10)
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return CFL.EncodeData("01", CFL.RS("POP06", this, Server, GD.LangID), null);
                }

                for (int j = i + 1; j < sarUserID.Length; j++)
                {
                    if (sarUserID[j].ToLower() == sarUserID[i].ToLower())
                    {
                        if (m_bInTrans)
                            m_tr.Rollback();
                        return CFL.EncodeData("01", CFL.RS("POP07", this, Server, GD.LangID), null);
                    }
                }
            }

            if (strUserGroup != "")
            {
                alResult = cgw.GridGateWay(true, "OM", "UserGroup", "Popup", "UserGroup = " + CFL.Q(strUserGroup), "", null, GDObj);

                if (!CFL.RefCheck(CFL.RS("M19", this, Server, GD.LangID), GD.LangID, ref alResult, "OFtxtUserGroup"))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    //		return alResult;

                    // modified by Sinbad 2003.12.03
                    return CFL.EncodeData("01", CFL.RS("M20", this, Server, GD.LangID), null);
                }
            }


            // insert mode
            if (bSaveMode)
            {
                // Uniqueness Check
                strQuery = @"select	RouteCode from WfRoute	where	RouteCode = " + CFL.Q(strRouteCode)
                    + " and SiteCode = " + CFL.Q(strSiteCode);

                try
                {
                    dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
                }
                catch (Exception e)
                {
                    strErrorCode = "03";
                    strErrorMsg = CFL.RS("M04", this, Server, GD.LangID);
                    return CFL.EncodeData(strErrorCode, strErrorMsg, e);
                }

                // there's any
                if (dr.Read())
                {
                    strErrorCode = "04";
                    strErrorMsg = CFL.RS("M05", this, Server, GD.LangID);
                    return CFL.EncodeData(strErrorCode, strErrorMsg, null);
                }
                dr.Close();
            }
            else
            {
                // in update mode delete existing data
                strQuery = @"delete from WfRoute where SiteCode = " + CFL.Q(strSiteCode)
                    + @" and RouteCode = " + CFL.Q(strRouteCode) + @"
					delete from WfRouteItem where SiteCode = " + CFL.Q(strSiteCode)
                    + @" and RouteCode = " + CFL.Q(strRouteCode);
            }
            // 20021212 inux 중복되는 stepTitle포함한 스텝수 구하기..
            string temp = "";
            string tempOld = "";
            int iStepCount = 0;
            for (int i = 0; i < sarStepTitle.Length; i++)
            {
                temp = sarStepTitle[i];
                if (temp != tempOld)
                {
                    ++iStepCount;
                }
                tempOld = sarStepTitle[i];
            }
            // header insert
            strQuery += @"
				insert into WfRoute ( SiteCode, RouteCode, RouteName, FirstStepTitle, RouteDescr, StepCnt, RouteUse, UserGroup )
				values ( " + CFL.Q(strSiteCode) + ", " + CFL.Q(strRouteCode) + ", " + CFL.Q(strRouteName)
                    + ", " + CFL.Q(strFirstStepTitle) + ", " + CFL.Q(strRouteDescr) + ", " + (iStepCount + 1)
                    + ", " + CFL.Q(strRouteUse) + ", " + CFL.Q(strUserGroup) + " )";

            // Item Insert
            /* 20021203 inux -- sarStepTitle의 이름이 중복될경우 routeStep+0.1을 함. */
            float tempRouteStep = 1.0f;
            string tempStepTitle = "";
            for (int i = 0; i < sarStepTitle.Length; i++)
            {
                if (sarStepTitle[i] == tempStepTitle)
                {
                    tempRouteStep = tempRouteStep + 0.1f;
                }
                else
                {
                    tempRouteStep = (int)tempRouteStep + 1.0f;
                }
                strQuery += @"
					insert into WfRouteItem ( SiteCode, RouteCode, RouteStep, StepTitle, UserId, AbsentEmp )
					values ( " + CFL.Q(strSiteCode) + ", " + CFL.Q(strRouteCode) + ", " + (tempRouteStep)
                    + ", " + CFL.Q(sarStepTitle[i]) + ", " + CFL.Q(sarUserID[i]) + ", " + CFL.Q(sarAbsentEmp[i]) + " )";

                tempStepTitle = sarStepTitle[i];
            }


            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "05";
                strErrorMsg = CFL.RS("M02", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            /*
                        // Access History Save
                        OM.FormAccessInfo FormAccessInfo = new OM.FormAccessInfo ( m_db, m_tr );
                        alResult = FormAccessInfo.AccessHistory("S",strComCode,"OM008", strCurEmp
                            ,"WfRoute", "SiteCode", CFL.Q(strSiteCode)
                            , "RouteCode", CFL.Q(strRouteCode)
                            , "", "", "", "");
                        if ( "00" != alResult[0].ToString() )
                            return alResult;
            */
            // Transaction Commit
            if (!m_bInTrans)
                m_tr.Commit();

            // Return DocNo when Success
            return CFL.EncodeData(strErrorCode, strRouteCode, null);
        }

        [WebMethod]
        public ArrayList WfRLoad(object[] GDObj, string strRouteCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
				select RouteName, FirstStepTitle, RouteDescr, RouteUse, 
					    ( select count(*) from WfRouteItem where SiteCode =" + CFL.Q(GD.SiteCode) + " and RouteCode = " + CFL.Q(strRouteCode) + @")+1
						as StepCnt, 
					   UserGroup, StepCnt as StepNum
				from WfRoute
				where SiteCode = " + CFL.Q(GD.SiteCode) + " and RouteCode = " + CFL.Q(strRouteCode);

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M06", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        [WebMethod]
        public ArrayList WfRItemGrid(bool bStartQuery, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);



            string strErrorCode = "00", strErrorMsg = "";

            if (!bStartQuery)
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);

            string strQuery = @"
select	StepTitle, A.UserId, B.UserName, AbsentEmp
from	WfRouteItem A
	Left outer Join	xErpUser B		on	A.UserId = B.UserId 
";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += " where " + strWhereClause;

            strQuery += " order by RouteStep";


            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M06", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        // 복수결제시 결제경로 스탬프를 위한 query
        [WebMethod]
        public ArrayList WfRItemGrid2(bool bStartQuery, string formID, string originNo, string originSerNo, string routeCode, object[] GDObj, string strMoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);

            if (originSerNo == null || originSerNo == "")
                originSerNo = "0";

            string strErrorCode = "00", strErrorMsg = "";

            if (!bStartQuery)
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);

            string strQuery = @"
if ( Select	count(*) 
	 From WfDocument
	 Where SiteCode = " + CFL.Q(GD.SiteCode) + @"
	   and FormID = " + CFL.Q(formID) + @"
	   and OriginNo = " + CFL.Q(originNo) + @"
	   and OriginSerNo = " + CFL.Toi(originSerNo) + @"
       and MoreKey = " + CFL.Q(strMoreKey) + @" ) = 0
    begin	
	    select	A.StepTitle, A.UserID, B.UserName,
			    null as WfStatus,
			    null as WfDateTime, 
			    A.AbsentEmp, (select UserName from xErpUser Where UserID = A.AbsentEmp) as AbsentEmpName, RouteStep
	    from	WfRouteItem A, xErpUser B
	    where	A.UserID = B.UserID
		    and SiteCode = " + CFL.Q(GD.SiteCode) + @"
		    and RouteCode = " + CFL.Q(routeCode) + @"
	    order By RouteStep	
    end
else
    begin
	    Select	A.StepTitle, C.UserID, D.UserName, C.WfStatus, C.WfDateTime, A.AbsentEmp, E.UserName as AbsentEmpName, A.RouteStep
	    From	WfRouteItem A 
            Inner Join WfDocument B on A.SiteCode = B.SiteCode and A.RouteCode = B.RouteCode
		    left outer join WfDocuRoute C on B.SiteCode = C.SiteCode and B.FormID = C.FormID and B.OriginNo = C.OriginNo and B.OriginSerNo = C.OriginSerNo 
                                         and B.MoreKey = C.MoreKey
                                         and A.RouteStep = C.RouteStep
		    left outer join xErpUser D on C.UserID = D.UserID
		    left outer join xErpUser E on A.AbsentEmp = E.UserID
	    Where	B.SiteCode = " + CFL.Q(GD.SiteCode) + @"  
            and B.FormID = " + CFL.Q(formID) + @" 
            and B.OriginNo = " + CFL.Q(originNo) + @" 
            and B.OriginSerNo = " + CFL.Toi(originSerNo) + @"
            and B.MoreKey = " + CFL.Q(strMoreKey) + @" 
	    Order by A.RouteStep
    end
";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M06", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }




        [WebMethod]
        public ArrayList WfRDelete(object[] GDObj, string strRouteCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"delete from WfRoute where SiteCode = " + CFL.Q(GD.SiteCode)
                + @" and RouteCode = " + CFL.Q(strRouteCode) + @"
					delete from WfRouteItem where SiteCode = " + CFL.Q(GD.SiteCode)
                + @" and RouteCode = " + CFL.Q(strRouteCode);

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M07", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }


        [WebMethod]
        public ArrayList WfRPopup(bool bStartQuery, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = "select ";
            // Generate Column Information
            ArrayList alColumns = new ArrayList();

            string strTemp = "";

            // First Column
            if (0 <= strCondition.IndexOf("RouteCode/"))
            {
                alColumns.Add("RouteCode");
                alColumns.Add(CFL.RS("POP01", this, Server, GD.LangID));
                alColumns.Add(80);
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                strQuery += " RouteCode ";
            }
            // Second Column
            if (0 <= strCondition.IndexOf("RouteName/"))
            {
                alColumns.Add("RouteName");
                alColumns.Add(CFL.RS("POP02", this, Server, GD.LangID));
                alColumns.Add(120);
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                if (7 < strQuery.Length)
                    strQuery += ",";

                strQuery += " RouteName ";
            }

            // Third Column
            if (0 <= strCondition.IndexOf("StepCnt/"))
            {
                alColumns.Add("StepCnt");
                alColumns.Add(CFL.RS("POP05", this, Server, GD.LangID));
                alColumns.Add(30);
                strTemp = "";
                CFL.SetEncodedProperty("Hidden", "True", ref strTemp);
                alColumns.Add(strTemp);
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                if (7 < strQuery.Length)
                    strQuery += ",";

                strQuery += " StepCnt ";
            }

            if (!bStartQuery)
                return CFL.EncodeData(strErrorCode, strErrorMsg, alColumns, null);

            strQuery += @"
from	WfRoute
where	RouteUse = " + CFL.Q("Y");

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += " and " + strWhereClause;
            // Restrict to Values starts with User Key-In
            strTemp = CFL.GetEncodedProperty("RouteCode", strCondition);
            if (strTemp != "")
                strQuery += " and RouteCode like " + CFL.Q(strTemp + "%");
            strTemp = CFL.GetEncodedProperty("RouteName", strCondition);
            if (strTemp != "")
                strQuery += " and RouteName like " + CFL.Q(strTemp + "%");
            strTemp = CFL.GetEncodedProperty("txtRouteCode", strCondition);
            if (strTemp != "")
                strQuery += " and RouteCode like " + CFL.Q(strTemp + "%");
            strTemp = CFL.GetEncodedProperty("txtRouteName", strCondition);
            if (strTemp != "")
                strQuery += " and RouteName like " + CFL.Q(strTemp + "%");
            strQuery += " order by RouteCode";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M06", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, alColumns, null);
        }

        /* inux 20021126 condition의 userid가 route item에 포함된 route는 제외 */
        [WebMethod]
        public ArrayList WfRPopup2(bool bStartQuery, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = "select ";
            // Generate Column Information
            ArrayList alColumns = new ArrayList();

            string strTemp = "";

            // First Column
            if (0 <= strCondition.IndexOf("RouteCode/"))
            {
                alColumns.Add("RouteCode");
                alColumns.Add(CFL.RS("POP01", this, Server, GD.LangID));
                alColumns.Add(80);
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                strQuery += " RouteCode ";
            }
            // Second Column
            if (0 <= strCondition.IndexOf("RouteName/"))
            {
                alColumns.Add("RouteName");
                alColumns.Add(CFL.RS("POP02", this, Server, GD.LangID));
                alColumns.Add(120);
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                if (7 < strQuery.Length)
                    strQuery += ",";

                strQuery += " RouteName ";
            }

            // Third Column
            if (0 <= strCondition.IndexOf("StepCnt/"))
            {
                alColumns.Add("StepCnt");
                alColumns.Add(CFL.RS("POP05", this, Server, GD.LangID));
                alColumns.Add(30);
                strTemp = "";
                CFL.SetEncodedProperty("Hidden", "True", ref strTemp);
                alColumns.Add(strTemp);
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                alColumns.Add("");
                if (7 < strQuery.Length)
                    strQuery += ",";

                strQuery += " StepCnt ";
            }

            if (!bStartQuery)
                return CFL.EncodeData(strErrorCode, strErrorMsg, alColumns, null);

            strQuery += @"
from	WfRoute
where	RouteUse = " + CFL.Q("Y");

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += " and " + strWhereClause;
            // Restrict to Values starts with User Key-In
            strTemp = CFL.GetEncodedProperty("RouteCode", strCondition);
            if (strTemp != "")
                strQuery += " and RouteCode like " + CFL.Q(strTemp + "%");
            strTemp = CFL.GetEncodedProperty("RouteName", strCondition);
            if (strTemp != "")
                strQuery += " and RouteName like " + CFL.Q(strTemp + "%");
            strTemp = CFL.GetEncodedProperty("txtRouteCode", strCondition);
            if (strTemp != "")
                strQuery += " and RouteCode like " + CFL.Q(strTemp + "%");
            strTemp = CFL.GetEncodedProperty("txtRouteName", strCondition);
            if (strTemp != "")
                strQuery += " and RouteName like " + CFL.Q(strTemp + "%");
            /* inux 20021126 추가 UserID가 결제경로에 있는 결제경로제외 */
            strTemp = CFL.GetEncodedProperty("UserID", strCondition);
            if (strTemp != "")
            {
                strQuery += @" and not exists  
								(select userid from WfRouteItem
								where WfRoute.SiteCode = WfRouteItem.SiteCode
								and     WfRoute.RouteCode = WfRouteItem.RouteCode
								and     WfRouteItem.USerID = " + CFL.Q(strTemp) + ")";
            }
            strQuery += " order by RouteCode";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M06", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, alColumns, null);
        }

        [WebMethod]
        public ArrayList WfRGetMail(object[] GDObj, string strRouteCode, string strRouteStep)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select	C.eMail
from	WfRouteItem A
	inner join xErpUser B
	on  A.UserID = B.UserID
	inner join xErpUser C 
	on B.UserID = C.UserID
where A.SiteCode = " + CFL.Q(GD.SiteCode) + " and A.RouteCode = " + CFL.Q(strRouteCode) + @" 
	and A.RouteStep = " + CFL.Toi(strRouteStep);

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M06", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }


        /// <summary>
        /// WfPersonalSetting
        /// </summary>

        [WebMethod]
        public ArrayList WfPSLoad(object[] GDObj, string strUserID)
        {
            SMART.COMMON.DA.WorkFlow Obj = new SMART.COMMON.DA.WorkFlow();
            return Obj.GetPersonalData(GDObj, strUserID);
        }


        [WebMethod]
        public ArrayList WfPSGrid(bool bStartQuery, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            if (!bStartQuery)
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);

            // Get Conditions
            string strSiteCode = CFL.GetEncodedProperty("SiteCode", strCondition);
            string strUserID = CFL.GetEncodedProperty("UserID", strCondition);

            string strQuery = @"
select	A.FormID, IsNull ( E.ObjName, F.ObjName ) as ObjName, C.RouteCode, D.RouteName
from	ModuleObj A
	Left outer Join	WfSiteMaster B	on	A.FormID = B.FormID and B.SiteCode = " + CFL.Q(strSiteCode) + @"
	Left outer Join	DefaultRoute C	on	A.FormID = C.FormID and C.SiteCode = " + CFL.Q(strSiteCode) + @" 
		and C.UserID = " + CFL.Q(GD.UserID) + @"
	Left outer Join	WfRoute D	on	C.RouteCode = D.RouteCode and D.SiteCode = " + CFL.Q(strSiteCode) + @"
	Left Outer Join	ComMenu E
		on	A.ObjID = E.ObjID and E.LangID = " + CFL.Q(GD.LangID) + @" and E.ComCode = " + CFL.Q(GD.ComCode) + @"
	Left Outer Join	ModuleObjLang F
		on	A.ObjID = F.ObjID and F.LangID = " + CFL.Q(GD.LangID) + @" 
where	A.ObjCategory = " + CFL.Q("F") + @" and A.WfUse = " + CFL.Q("Y") + @"
	and IsNull ( B.WfUse, " + CFL.Q("Y") + @" ) = " + CFL.Q("Y") + @"
	and IsNull ( F.ObjUse, " + CFL.Q("N") + @" )  = " + CFL.Q("Y") + @"
	and IsNull ( E.ObjUse, " + CFL.Q("Y") + @" ) = " + CFL.Q("Y");

            // Check Developing Menus
            if (GD.LicenseType != "D")
                strQuery += @"
	and IsNull ( F.Developing, " + CFL.Q("Y") + @" ) = " + CFL.Q("N");

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += " and " + strWhereClause;

            strQuery += " order by A.SortKey, A.FormID";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M08", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        [WebMethod]
        public ArrayList WfPSGrid1(bool bStartQuery, string strWhereClause, string strCondition, object[] Conditions, object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            if (!bStartQuery)
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);

            // Get Conditions
            string strSiteCode = CFL.GetEncodedProperty("SiteCode", strCondition);
            string strUserID = CFL.GetEncodedProperty("UserID", strCondition);
            string strAbsentCheck = CFL.GetEncodedProperty("AbsentCheck", strCondition);

            string strEmpFieldName = "A.UserID";
            string strAbsentEmpFieldName = "A.AbsentEmp";
            if ("Y" == strAbsentCheck)
            {
                strEmpFieldName = "A.AbsentEmp ";
                strAbsentEmpFieldName = "A.UserID";
            }

            string strQuery = @"
select	A.RouteCode, B.RouteName, A.StepTitle, " + strAbsentEmpFieldName + @", C.UserName, A.RouteStep
from	WfRouteItem A
	Left outer Join	WfRoute B	on	A.RouteCode = B.RouteCode and A.SiteCode = B.SiteCode
	Left outer Join xErpUser C	on	" + strAbsentEmpFieldName + @" = C.UserID 
where	A.SiteCode = " + CFL.Q(strSiteCode) + " and " + strEmpFieldName + " = " + CFL.Q(strUserID) + @"
	and IsNull ( B.RouteUse, " + CFL.Q("N") + @" ) = " + CFL.Q("Y") + @"
";

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += " and " + strWhereClause;

            strQuery += " order by B.RouteCode";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M08", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        /*
		[WebMethod]
		public ArrayList WfPSSave(object[] GDObj, string strUserID, string strLangID, string strSiteCode
			, string strAbsentCheck, string strWfPassWd, string strPassWd, string streMail, string[] sarFormID, string[] sarDefaultRoute
			, string[] sarRouteCode, string[] sarRouteStep, string[] sarAbsentEmp , bool bUseWfPwd )
		{
			// Global Data
			GData GD = new GData(GDObj);
            

			string strErrorCode = "00", strErrorMsg = "";
			string strQuery = "";

			ArrayList alResult = new ArrayList();

			// ********Header Validation Check

			// Not Null Check
			// UserID
			if ( !CFL.NotNullCheck ( CFL.RS ( "P03", this, Server, GD.LangID ), strUserID, strLangID, ref alResult ) )
			{
				if ( m_bInTrans )
					m_tr.Rollback();
				return alResult;
			}
			
			// ************* 2.Item Validation Check *****************
			// the number of Item >= 1
			CGW cgw = new CGW ();

			// Default Route
			for ( int i = 0; i < sarFormID.Length; i++ )
			{
				if ( "" != sarDefaultRoute[i] )
				{
					// DefaultRoute RefCheck
					alResult = cgw.GridGateWay ( true, "OM", "WorkFlow", "WfRPopup", "SiteCode = " + CFL.Q ( strSiteCode ) + " and RouteCode = " + CFL.Q ( sarDefaultRoute[i] ), "RouteCode/" , null, GDObj);
					if ( !CFL.RefCheck ( CFL.RS ( "P04", this, Server, GD.LangID ), strLangID, ref alResult ) )
					{
						if ( m_bInTrans )
							m_tr.Rollback();
						return alResult;
					}
				}
			}

			// create command object with Transaction
			if ( !m_bInTrans )
				m_tr = CFL.GetDBConnection(GD).BeginTransaction();

			OM.xErpUser Obj = new OM.xErpUser ( m_db, m_tr );
			alResult = Obj.GetPersonalData ( GDObj, strUserID );
			if ( "00" != alResult[0].ToString() )
				return alResult;
			string strPrevAbsentCheck = alResult[4].ToString();

			// Default RouteSave
			strQuery = @"
delete from DefaultRoute where SiteCode = " + CFL.Q ( strSiteCode )
				+ " and UserID = " + CFL.Q ( strUserID );
			for ( int i = 0; i < sarFormID.Length; i++ )
			{
				if ( "" != sarDefaultRoute[i] )
					strQuery += @"
insert into DefaultRoute ( SiteCode, UserID, FormID, RouteCode )
values ( " + CFL.Q ( strSiteCode ) + ", " + CFL.Q ( strUserID )
						+ ", " + CFL.Q ( sarFormID[i] ) + ", " + CFL.Q ( sarDefaultRoute[i] ) + " )";
			}

			// Swap Absent Emp
			if ( "Y" == strPrevAbsentCheck && "N" == strAbsentCheck )
				strQuery += @"
update	WfRouteItem
set	UserID = AbsentEmp
where	AbsentEmp = " + CFL.Q ( strUserID );
			else if ( "N" == strPrevAbsentCheck && "Y" == strAbsentCheck )
				strQuery += @"
update	WfRouteItem
set	AbsentEmp = UserID
where	UserID = " + CFL.Q ( strUserID );

			string strAbsentEmpFieldName = "AbsentEmp";
			if ( "Y" == strAbsentCheck )
			{
				strAbsentEmpFieldName = "UserID";
			}

			// AbsentEmp Setting Save
			string tempUserID = "";
			for ( int i = 0; i < sarRouteCode.Length; i++ )
			{
				if(sarAbsentEmp[i]=="" && "Y" == strAbsentCheck) 
				{
					tempUserID = strUserID;
				}
				else
				{
					tempUserID = sarAbsentEmp[i];
				}

				strQuery += @"
update	WfRouteItem
set	" + strAbsentEmpFieldName + " = " + CFL.Q ( tempUserID ) + @"
where	SiteCode = " + CFL.Q ( strSiteCode ) + " and RouteCode = " + CFL.Q ( sarRouteCode[i] )
					+ " and RouteStep = " + CFL.Q ( sarRouteStep[i] );
			}

			// insert mode
			// get new number
			alResult = Obj.SetPersonalData ( GDObj, strLangID, strUserID, strAbsentCheck, strWfPassWd, strPassWd, streMail, bUseWfPwd );
			if ( "00" != alResult[0].ToString() )
				return alResult;

			OleDbCommand cm = new OleDbCommand(strQuery, m_db, m_tr );
			try 
			{
				cm.ExecuteNonQuery ();
			}
			catch ( Exception e )
			{
				m_tr.Rollback();
				strErrorCode = "03";
				strErrorMsg = CFL.RS ( "M02", this, Server, GD.LangID );
				return CFL.EncodeData ( strErrorCode, strErrorMsg, e );
			}

			// Transaction Commit
			if ( !m_bInTrans )
				m_tr.Commit();

			// Return DocNo when Success
			return CFL.EncodeData ( strErrorCode, strUserID, null );
		}
         * 
         * */

        [WebMethod]
        public ArrayList GetDefaultRoute(object[] GDObj, string strSiteCode, string strFormID, string strUserID)
        {
            // Global Data
            GData GD = new GData(GDObj);



            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select RouteCode
from DefaultRoute
where SiteCode = " + CFL.Q(strSiteCode) + " and FormID = " + CFL.Q(strFormID) + @"
	and UserID = " + CFL.Q(strUserID);

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M08", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }


        /// <summary>
        /// WfDocument
        /// </summary>

        [WebMethod]
        public string GetDomainName(string strClientID)
        {
            ClientCore Cli = new ClientCore();
            return Cli.GetDomainName(strClientID);
        }

        [WebMethod]
        public ArrayList WfDSave2(object[] GDObj, string strLangID, string strSiteCode, string strFormID, string strOriginNo
                                , string strOriginSerNo, string strRouteCode, string strDocuDescr, string strCurStep, string strDocuStatus
                                , string[] sarUserID, string[] sarWfStatus, string[] sarWfDateTime
                                , string[] sarRefUserID, string[] sarReadCheck, string[] sarReadDateTime, string[] sarRouteStep, string WfAmount
                                , string strMoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            // when there's No WfInformation
            if (null == sarUserID || 1 > sarUserID.Length)
                return WfDDelete(GDObj, strSiteCode, strFormID, strOriginNo, strOriginSerNo, strMoreKey);

            string strQuery = "";

            ArrayList alResult = new ArrayList();

            // ********Header Validation Check

            // Not Null Check
            // LangID
            if (!CFL.NotNullCheck(CFL.RS("P06", this, Server, GD.LangID), strLangID, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // SiteCode
            if (!CFL.NotNullCheck(CFL.RS("P07", this, Server, GD.LangID), strSiteCode, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // FormID
            if (!CFL.NotNullCheck(CFL.RS("P08", this, Server, GD.LangID), strFormID, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // OriginNo
            if (!CFL.NotNullCheck(CFL.RS("P09", this, Server, GD.LangID), strOriginNo, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // RouteCode
            if (!CFL.NotNullCheck(CFL.RS("P10", this, Server, GD.LangID), strRouteCode, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // DocuStatus
            if (!CFL.NotNullCheck(CFL.RS("P11", this, Server, GD.LangID), strDocuStatus, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            // ************* 2.Item Validation Check *****************

            //******** WfDocuRoute
            for (int i = 0; i < sarUserID.Length; i++)
            {
                // Not Null Check
                // EmpCode Check
                if (!CFL.NotNullCheck(CFL.RS("P12", this, Server, GD.LangID), sarUserID[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
                // WfStatus Check
                if (!CFL.NotNullCheck(CFL.RS("P11", this, Server, GD.LangID), sarWfStatus[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
                // WfDateTime Check
                if (!CFL.NotNullCheck(CFL.RS("P13", this, Server, GD.LangID), sarWfDateTime[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
            }

            //******** WfDocuRef
            for (int i = 0; i < sarRefUserID.Length; i++)
            {
                // Not Null Check
                // RefEmpCode Check
                if (!CFL.NotNullCheck(CFL.RS("P14", this, Server, GD.LangID), sarRefUserID[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
                // ReadCheck Check
                if (!CFL.NotNullCheck(CFL.RS("P15", this, Server, GD.LangID), sarReadCheck[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
            }
            // Convert 결제액

            int iWfAmount = CFL.Toi(WfAmount);

            // Convert OriginSerNo
            int iOriginSerNo = CFL.Toi(strOriginSerNo);
            // Determine CurStep
            int iCurStep = CFL.Toi(strCurStep);

            // in update mode delete existing data
            strQuery = @"

Delete From WfDocument 
Where SiteCode = " + CFL.Q(strSiteCode) + @" 
  and FormID = " + CFL.Q(strFormID) + @"
  and OriginNo = " + CFL.Q(strOriginNo) + @" 
  and OriginSerNo = " + iOriginSerNo + @"
  and MoreKey = " + CFL.Q(strMoreKey) + @" 


Delete From WfDocuRoute 
Where SiteCode = " + CFL.Q(strSiteCode) + @" 
  and FormID = " + CFL.Q(strFormID) + @"
  and OriginNo = " + CFL.Q(strOriginNo) + @" 
  and OriginSerNo = " + iOriginSerNo + @"
  and MoreKey = " + CFL.Q(strMoreKey) + @"


Delete From WfDocuRef 
Where SiteCode = " + CFL.Q(strSiteCode) + @" 
  and FormID = " + CFL.Q(strFormID) + @"
  and OriginNo = " + CFL.Q(strOriginNo) + @" 
  and OriginSerNo = " + iOriginSerNo + @"
  and MoreKey = " + CFL.Q(strMoreKey);

            // header insert
            strQuery += @"

Insert into WfDocument ( SiteCode, FormID, OriginNo, OriginSerNo, RouteCode, DocuDescr, CurStep, DocuStatus, WfAmount, MoreKey )
values ( " + CFL.Q(strSiteCode) + ", " + CFL.Q(strFormID) + ", " + CFL.Q(strOriginNo) + @"
	, " + iOriginSerNo + ", " + CFL.Q(strRouteCode) + ", " + CFL.Q(strDocuDescr) + @"
	, " + iCurStep + ", " + CFL.Q(strDocuStatus) + ", " + iWfAmount + ", " + CFL.Q(strMoreKey) + " )";

            // WfDocuRoute Insert
            for (int i = 0; i < sarUserID.Length; i++)
            {
                strQuery += @"

Insert into WfDocuRoute ( SiteCode, FormID, OriginNo, OriginSerNo, RouteStep,UserID, WfStatus, WfDateTime, MoreKey )
values ( " + CFL.Q(strSiteCode) + ", " + CFL.Q(strFormID) + ", " + CFL.Q(strOriginNo) + @"
	, " + iOriginSerNo + ", " + sarRouteStep[i] + ", " + CFL.Q(sarUserID[i]) + ", " + CFL.Q(sarWfStatus[i]) + @"
	, " + CFL.Q(sarWfDateTime[i]) + ", " + CFL.Q(strMoreKey) + " )";
            }

            // WfDocuRef Insert
            for (int i = 0; i < sarRefUserID.Length; i++)
            {
                strQuery += @"
insert into WfDocuRef ( SiteCode, FormID, OriginNo, OriginSerNo, UserID, ReadCheck, ReadDateTime, MoreKey )
values ( " + CFL.Q(strSiteCode) + ", " + CFL.Q(strFormID) + ", " + CFL.Q(strOriginNo) + @"
	, " + iOriginSerNo + ", " + CFL.Q(sarRefUserID[i]) + ", " + CFL.Q(sarReadCheck[i]) + @"
	, " + CFL.Q(sarReadDateTime[i]) + ", " + CFL.Q(strMoreKey) + " )";
            }


            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();

                strErrorCode = "03";
                strErrorMsg = CFL.RS("M09", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Transaction Commit
            if (!m_bInTrans)
                m_tr.Commit();

            // Return DocNo when Success
            return CFL.EncodeData(strErrorCode, strOriginNo, null);
        }


        // 결재 Control 에서 이부분은 사용안하는 것 같음.
        [WebMethod]
        public ArrayList WfDSave(object[] GDObj, string strLangID, string strSiteCode, string strFormID, string strOriginNo
            , string strOriginSerNo, string strRouteCode, string strDocuDescr, string strCurStep, string strDocuStatus
            , string[] sarUserID, string[] sarWfStatus, string[] sarWfDateTime
            , string[] sarRefUserID, string[] sarReadCheck, string[] sarReadDateTime
            , string strMoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            // when there's No WfInformation
            if (null == sarUserID || 1 > sarUserID.Length)
                return WfDDelete(GDObj, strSiteCode, strFormID, strOriginNo, strOriginSerNo, strMoreKey);

            string strQuery = "";

            ArrayList alResult = new ArrayList();

            // ********Header Validation Check

            // Not Null Check
            // LangID
            if (!CFL.NotNullCheck(CFL.RS("P06", this, Server, GD.LangID), strLangID, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // SiteCode
            if (!CFL.NotNullCheck(CFL.RS("P07", this, Server, GD.LangID), strSiteCode, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // FormID
            if (!CFL.NotNullCheck(CFL.RS("P08", this, Server, GD.LangID), strFormID, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // OriginNo
            if (!CFL.NotNullCheck(CFL.RS("P09", this, Server, GD.LangID), strOriginNo, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // RouteCode
            if (!CFL.NotNullCheck(CFL.RS("P10", this, Server, GD.LangID), strRouteCode, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }
            // DocuStatus
            if (!CFL.NotNullCheck(CFL.RS("P11", this, Server, GD.LangID), strDocuStatus, strLangID, ref alResult))
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                return alResult;
            }

            // ************* 2.Item Validation Check *****************

            //******** WfDocuRoute
            for (int i = 0; i < sarUserID.Length; i++)
            {
                // Not Null Check
                // EmpCode Check
                if (!CFL.NotNullCheck(CFL.RS("P12", this, Server, GD.LangID), sarUserID[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
                // WfStatus Check
                if (!CFL.NotNullCheck(CFL.RS("P11", this, Server, GD.LangID), sarWfStatus[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
                // WfDateTime Check
                if (!CFL.NotNullCheck(CFL.RS("P13", this, Server, GD.LangID), sarWfDateTime[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
            }

            //******** WfDocuRef
            for (int i = 0; i < sarRefUserID.Length; i++)
            {
                // Not Null Check
                // RefEmpCode Check
                if (!CFL.NotNullCheck(CFL.RS("P14", this, Server, GD.LangID), sarRefUserID[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
                // ReadCheck Check
                if (!CFL.NotNullCheck(CFL.RS("P15", this, Server, GD.LangID), sarReadCheck[i], strLangID, ref alResult))
                {
                    if (m_bInTrans)
                        m_tr.Rollback();
                    return alResult;
                }
            }

            // Convert OriginSerNo
            int iOriginSerNo = CFL.Toi(strOriginSerNo);
            // Determine CurStep
            int iCurStep = CFL.Toi(strCurStep);

            // in update mode delete existing data
            strQuery = @"
delete from WfDocument where SiteCode = " + CFL.Q(strSiteCode) + @" and FormID = " + CFL.Q(strFormID) + @"
	and OriginNo = " + CFL.Q(strOriginNo) + @" and OriginSerNo = " + iOriginSerNo + @"

delete from WfDocuRoute where SiteCode = " + CFL.Q(strSiteCode) + @" and FormID = " + CFL.Q(strFormID) + @"
	and OriginNo = " + CFL.Q(strOriginNo) + @" and OriginSerNo = " + iOriginSerNo + @"

delete from WfDocuRef where SiteCode = " + CFL.Q(strSiteCode) + @" and FormID = " + CFL.Q(strFormID) + @"
	and OriginNo = " + CFL.Q(strOriginNo) + @" and OriginSerNo = " + iOriginSerNo;

            // header insert
            strQuery += @"

insert into WfDocument ( SiteCode, FormID, OriginNo, OriginSerNo, RouteCode, DocuDescr, CurStep, DocuStatus )
values ( " + CFL.Q(strSiteCode) + ", " + CFL.Q(strFormID) + ", " + CFL.Q(strOriginNo) + @"
	, " + iOriginSerNo + ", " + CFL.Q(strRouteCode) + ", " + CFL.Q(strDocuDescr) + @"
	, " + iCurStep + ", " + CFL.Q(strDocuStatus) + " )";

            // WfDocuRoute Insert
            for (int i = 0; i < sarUserID.Length; i++)
            {
                strQuery += @"

insert into WfDocuRoute ( SiteCode, FormID, OriginNo, OriginSerNo, RouteStep,UserID, WfStatus, WfDateTime )
values ( " + CFL.Q(strSiteCode) + ", " + CFL.Q(strFormID) + ", " + CFL.Q(strOriginNo) + @"
	, " + iOriginSerNo + ", " + (i + 1) + ", " + CFL.Q(sarUserID[i]) + ", " + CFL.Q(sarWfStatus[i]) + @"
	, " + CFL.Q(sarWfDateTime[i]) + " )";
            }

            // WfDocuRef Insert
            for (int i = 0; i < sarRefUserID.Length; i++)
            {
                strQuery += @"
insert into WfDocuRef ( SiteCode, FormID, OriginNo, OriginSerNo, UserID, ReadCheck, ReadDateTime )
values ( " + CFL.Q(strSiteCode) + ", " + CFL.Q(strFormID) + ", " + CFL.Q(strOriginNo) + @"
	, " + iOriginSerNo + ", " + CFL.Q(sarRefUserID[i]) + ", " + CFL.Q(sarReadCheck[i]) + @"
	, " + CFL.Q(sarReadDateTime[i]) + " )";
            }


            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "03";
                strErrorMsg = CFL.RS("M09", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Transaction Commit
            if (!m_bInTrans)
                m_tr.Commit();

            // Return DocNo when Success
            return CFL.EncodeData(strErrorCode, strOriginNo, null);
        }


        [WebMethod]
        public ArrayList WfDLoad1(object[] GDObj, string strSiteCode, string strFormID, string strOriginNo, string strOriginSerNo, string strMoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            int iOriginSerNo = CFL.Toi(strOriginSerNo);

            string strQuery = @"
Select RouteCode, DocuDescr, CurStep, DocuStatus
From WfDocument
Where SiteCode = " + CFL.Q(strSiteCode) + " and FormID = " + CFL.Q(strFormID) + @"
  and OriginNo = " + CFL.Q(strOriginNo) + @" 
  and OriginSerNo = " + iOriginSerNo + @"
  and MoreKey = " + CFL.Q(strMoreKey);


            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M10", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        [WebMethod]
        public ArrayList WfDLoad2(object[] GDObj, string strSiteCode, string strFormID, string strOriginNo, string strOriginSerNo, string strMoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);


            string strErrorCode = "00", strErrorMsg = "";
            int iOriginSerNo = CFL.Toi(strOriginSerNo);

            string strQuery = @"
Select A.UserID, B.UserName, WfStatus, WfDateTime, A.RouteStep
From WfDocuRoute A, xErpUser B
Where A.SiteCode = " + CFL.Q(strSiteCode) + @" 
  and FormID = " + CFL.Q(strFormID) + @"
  and OriginNo = " + CFL.Q(strOriginNo) + @" 
  and OriginSerNo = " + iOriginSerNo + @"
  and MoreKey = " + CFL.Q(strMoreKey) + @" 
  and A.UserID = B.UserID 
  and A.SiteCode = " + CFL.Q(GD.SiteCode) + @"
Order by RouteStep";

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M10", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        [WebMethod]
        public ArrayList WfDLoad3(object[] GDObj, string strSiteCode, string strFormID, string strOriginNo, string strOriginSerNo, string strMoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            int iOriginSerNo = CFL.Toi(strOriginSerNo);

            string strQuery = @"
Select A.UserID , B.UserName, ReadCheck, ReadDateTime
From WfDocuRef A
	Left Outer Join	xErpUser B on A.UserID = B.UserID and A.SiteCode = " + CFL.Q(strSiteCode) + @"
Where A.SiteCode = " + CFL.Q(strSiteCode) + @" 
  and FormID = " + CFL.Q(strFormID) + @"
  and OriginNo = " + CFL.Q(strOriginNo) + @" 
  and OriginSerNo = " + iOriginSerNo + @"
  and MoreKey = " + CFL.Q(strMoreKey);

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M10", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        [WebMethod]
        public ArrayList WfDDelete(object[] GDObj, string strSiteCode, string strFormID, string strOriginNo, string strOriginSerNo, string strMoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            int iOriginSerNo = CFL.Toi(strOriginSerNo);

            string strQuery = @"

Delete From WfDocument 
Where SiteCode = " + CFL.Q(strSiteCode) + @" 
  and FormID = " + CFL.Q(strFormID) + @"
  and OriginNo = " + CFL.Q(strOriginNo) + @" 
  and OriginSerNo = " + iOriginSerNo + @"
  and MoreKey = " + CFL.Q(strMoreKey) + @" 


Delete From WfDocuRoute 
Where SiteCode = " + CFL.Q(strSiteCode) + @" 
  and FormID = " + CFL.Q(strFormID) + @"
  and OriginNo = " + CFL.Q(strOriginNo) + @" 
  and OriginSerNo = " + iOriginSerNo + @"
  and MoreKey = " + CFL.Q(strMoreKey) + @" 


Delete From WfDocuRef 
Where SiteCode = " + CFL.Q(strSiteCode) + @" 
  and FormID = " + CFL.Q(strFormID) + @"
  and OriginNo = " + CFL.Q(strOriginNo) + @" 
  and OriginSerNo = " + iOriginSerNo + @"
  and MoreKey = " + CFL.Q(strMoreKey);


            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M11", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            if (!m_bInTrans)
                m_tr.Commit();

            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }


        [WebMethod]
        public ArrayList WfDOneQue(object[] GDObj, string strLangID, string strSiteCode, string strCurTime, string strUserID
            , string[] sarFormID, string[] sarOriginNo, string[] sarOriginSerNo, string[] sarCurStep, string[] sarStepCnt
            , string strAction)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            // Check WorkFlow ItemCount
            if (sarFormID.Length < 1)
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M12", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);
            }

            // Approve Each Item
            for (int i = 0; i < sarFormID.Length; i++)
            {
                strQuery = @"
	declare @curStep float,
			@routeCode varchar(12)

	SET @routeCode = (select RouteCode
					from    WfDocument
					where SiteCode	 = " + CFL.Q(strSiteCode) + @"
					and     FormID	 = " + CFL.Q(sarFormID[i]) + @"
					and     OriginNo = " + CFL.Q(sarOriginNo[i]) + @" )

	SET @curStep = (select  RouteStep
			from    WfRouteItem
			where	SiteCode =  " + CFL.Q(strSiteCode) + @"
			and     UserID    = " + CFL.Q(strUserID) + @"
			and     RouteCode = @routeCode )

	delete from	WfDocuRoute	where	SiteCode = " + CFL.Q(strSiteCode) + " and FormID = " + CFL.Q(sarFormID[i]) + @"
		and OriginNo = " + CFL.Q(sarOriginNo[i]) + " and OriginSerNo = " + CFL.Toi(sarOriginSerNo[i]) + @"
		and RouteStep = @curStep 

	insert into WfDocuRoute ( SiteCode, FormID, OriginNo, OriginSerNo, RouteStep, UserID, WfStatus, WfDateTime )
	values ( " + CFL.Q(strSiteCode) + ", " + CFL.Q(sarFormID[i]) + ", " + CFL.Q(sarOriginNo[i]) + @"
		, " + CFL.Toi(sarOriginSerNo[i]) + ", @curStep , " + CFL.Q(strUserID) + ", " + CFL.Q("A") + @"
		, " + CFL.Q(strCurTime) + @" )

	--합의를 포함한 RouteStep
	declare @totCurStep int, --단계에 총 합의결재수

			@totAprStep int --현단계 합의중 실제 결재된수

	SET @totCurStep =(select count(RouteStep)
			from    WfRouteItem
			where	SiteCode = " + CFL.Q(strSiteCode) + @"
			and     FLOOR(RouteStep)   =  FLOOR(@curStep)
			and     RouteCode = @routeCode )

	SET @totAprStep = (select count(*)
			from WfDocuRoute
			where SiteCode		= " + CFL.Q(strSiteCode) + @"
			and     FormID		= " + CFL.Q(sarFormID[i]) + @"
			and     OriginNo	= " + CFL.Q(sarOriginNo[i]) + @"
			and     FLOOR(RouteStep)   =  FLOOR(@curStep) )	 

	if ( @totAprStep  >= @totCurStep )
	BEGIN
		set @curStep = @curStep +1
	END

	--결재 완료인지 아닌지 여부
	DECLARE @status nchar(1)

	IF (select count(*)+1 --전체 결재수 
	from    WfRouteItem
	where	RouteCode = @routeCode
	and     SiteCode =  " + CFL.Q(strSiteCode) + @")
	<= (select  count(*) --현재 결재수

	from    WfDocuRoute
	where	SiteCode =  " + CFL.Q(strSiteCode) + @"
	and     FormID   =  " + CFL.Q(sarFormID[i]) + @"
	and     OriginNo     =  " + CFL.Q(sarOriginNo[i]) + @")
	BEGIN
	SET @status =  'Z' -- 결재 완료
	END
	ELSE
	BEGIN
	SET @status = 'Y' --결재중

	END


	update WfDocument	set	CurStep = FLOOR(@curStep) , DocuStatus = @status 
	where	SiteCode = " + CFL.Q(strSiteCode) + " and FormID = " + CFL.Q(sarFormID[i]) + @"
		and OriginNo = " + CFL.Q(sarOriginNo[i]) + " and OriginSerNo = " + CFL.Toi(sarOriginSerNo[i])

    + " select @status ";

                // WorkFlow Status
                string strStatus = "";
                bool bFailed = false;


                DataTableReader dr;
                try
                {
                    dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
                    if (dr.Read())
                        strStatus = dr.GetString(0);
                    dr.Close();
                }
                catch
                {
                    bFailed = true;
                    m_tr.Rollback();
                    strErrorMsg += sarOriginNo[i] + CFL.RS("M13", this, Server, GD.LangID) + "\\n";
                }

                //  If WorkFlow Process Completed
                if (!bFailed && strStatus == "Z")
                {
                    CommGW.gwWorkFlow gwWf = new CommGW.gwWorkFlow();
                    ArrayList alResult = gwWf.WfGateWay(GDObj, sarFormID[i], sarOriginNo[i], sarOriginSerNo[i]);
                    if (alResult[0].ToString() != "00")
                    {
                        bFailed = true;
                        m_tr.Rollback();
                        strErrorMsg += sarOriginNo[i] + CFL.RS("M13", this, Server, GD.LangID) + "\\n";
                    }
                }

                // Transaction Commit
                if (!bFailed && !m_bInTrans)
                    m_tr.Commit();
            }

            ArrayList alReturn = new ArrayList();
            alReturn.Add("00");
            alReturn.Add(strErrorMsg);
            alReturn.Add(0);
            alReturn.Add(0);
            return alReturn;
        }

        [WebMethod]
        public ArrayList WfDReadCheck(object[] GDObj, string strLangID, string strSiteCode, string strCurTime, string strUserID
            , string[] sarFormID, string[] sarOriginNo, string[] sarOriginSerNo, string[] sarReadCheck)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";
            string strQuery = "";

            // Check ItemCount
            if (sarFormID.Length < 1)
            {
                if (m_bInTrans)
                    m_tr.Rollback();
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M14", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);
            }

            // Make UpdateQuery
            for (int i = 0; i < sarFormID.Length; i++)
            {
                if ("N" == sarReadCheck[i])
                    strQuery += @"
update WfDocuRef set ReadCheck = 'Y', ReadDateTime = " + CFL.Q(strCurTime) + @"
where	SiteCode = " + CFL.Q(strSiteCode) + " and FormID = " + CFL.Q(sarFormID[i]) + @"
	and OriginNo = " + CFL.Q(sarOriginNo[i]) + " and OriginSerNo = " + CFL.Toi(sarOriginSerNo[i]) + @"
	and UserID = " + CFL.Q(strUserID);

            }

            if (strQuery.Length < 1)
            {
                if (m_bInTrans)
                    m_tr.Rollback();

                strErrorCode = "01";
                strErrorMsg = CFL.RS("M14", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, null);
            }

            try
            {
                CFL.ExecuteTran(GD, strQuery, CommandType.Text, ref m_tr, ref m_db, !m_bInTrans);
            }
            catch (Exception e)
            {
                m_tr.Rollback();
                strErrorCode = "02";
                strErrorMsg = CFL.RS("M15", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            // Transaction Commit
            if (!m_bInTrans)
                m_tr.Commit();

            // Return DocNo when Success
            return CFL.EncodeData(strErrorCode, strErrorMsg, null);
        }

        /// <summary>
        /// WfInBox
        /// </summary>

        [WebMethod]
        public ArrayList WfIBQuery(object[] GDObj, string strLangID, string strSiteCode, string strUserID, string strFormID)
        {
            // Global Data
            GData GD = new GData(GDObj);
            strUserID = GD.UserID;


            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select	Null as chkDummy, A.FormID, IsNull ( M.ObjName, N.ObjName ) as ObjName, A.OriginNo, E.TypeName
	, case when A.DocuStatus = 'B' then F.TypeName else B.StepTitle end as StepName
	, case when A.DocuStatus = 'B' then K.UserName else H.UserName end as UserName
	, case when A.DocuStatus = 'B' then J.WfDateTime else C.WfDateTime end, A.OriginSerNo
	, case when D.GlobalCheck = " + CFL.Q("Y") + @" then D.FormUrl else N.FormUrl end + '?Action=Origin&OriginNo=' + rtrim ( A.OriginNo ) + '&OriginSerNo=' + cast ( A.OriginSerNo as varchar(7) ) as Url
	, 'width=' + cast ( D.FormWidth as varchar(4) ) + ',height=' + cast ( D.FormHeight as varchar(4) ) + ',' as FormSize
	, A.CurStep, L.StepCnt,  A.WfAmount, " + CFL.Q(GD.CurrCode) + @"
from	WfDocument A 
	Left Outer Join WfRouteItem B 
		on A.SiteCode = B.SiteCode and A.RouteCode = B.RouteCode and FLOOR(A.CurStep) = FLOOR(B.RouteStep)
	Left Outer Join	WfDocuRoute C
		on	A.SiteCode = C.SiteCode and A.FormID = C.FormID and A.OriginNo = C.OriginNo and A.OriginSerNo = C.OriginSerNo
			and A.CurStep - 1 = C.RouteStep 
	Left Outer Join	ModuleObj D
		on	A.FormID = D.FormID 
	Left Outer Join TypeMaster E
		on	E.LangID = " + CFL.Q(strLangID) + @" and A.DocuStatus = E.TypeCode and E.TypeID = 'wfDocuStatus' 
	Left Outer Join TypeMaster F
		on	F.LangID = " + CFL.Q(strLangID) + @" and case when A.DocuStatus = 'B' then 'V' else A.DocuStatus end = F.TypeCode 
			and F.TypeID = 'wfDocuStatus' 
	Left Outer Join	WfDocuRoute G
		on	A.SiteCode = G.SiteCode and A.FormID = G.FormID and A.OriginNo = G.OriginNo and A.OriginSerNo = G.OriginSerNo 
			and G.RouteStep = 1 
	Left Outer Join	xErpUser H
		on	G.UserID = H.UserID
	Left Outer Join 
	(
	select	SiteCode, FormID, OriginNo, OriginSerNo, Max ( RouteStep ) as BackedStep
	from	WfDocuRoute
	where	WfStatus = 'B'
	group by	SiteCode, FormID, OriginNo, OriginSerNo
	) I	on	A.SiteCode = I.SiteCode and A.FormID = I.FormID and A.OriginNo = I.OriginNo and A.OriginSerNo = I.OriginSerNo
	Left Outer Join	WfDocuRoute J
		on	A.SiteCode = J.SiteCode and A.FormID = J.FormID and A.OriginNo = J.OriginNo 
			and A.OriginSerNo = J.OriginSerNo and I.BackedStep = J.RouteStep
	Left Outer Join	xErpUser K
		on	J.UserID = K.UserID
	Left Outer Join	WfRoute L
		on	A.SiteCode = L.SiteCode and A.RouteCode = L.RouteCode
	Left Outer Join	ComMenu M
		on	D.ObjID = M.ObjID and M.LangID = " + CFL.Q(GD.LangID) + @" and M.ComCode = " + CFL.Q(GD.ComCode) + @"
	Left Outer Join	ModuleObjLang N
		on	D.ObjID = N.ObjID and N.LangID = " + CFL.Q(GD.LangID) + @" 
where	A.DocuStatus in ( 'X', 'Y', 'B', 'H' )
	and case when A.DocuStatus = 'B' then G.UserID else B.UserID end = " + CFL.Q(strUserID);

            if (null != strFormID && "" != strFormID)
                strQuery += " and A.FormID = " + CFL.Q(strFormID);

            strQuery += @"
order by	case when A.DocuStatus = 'B' then J.WfDateTime else C.WfDateTime end, A.FormID, A.OriginNo, A.OriginSerNo
";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M16", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        [WebMethod]
        public ArrayList WfIBCount(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
Select Count(*)
From WfDocument A, WfRouteItem B
Where A.RouteCode = B.RouteCode 
  and A.SiteCode = B.SiteCode 
  and FLOOR(A.CurStep) = FLOOR(B.RouteStep)	
  and B.UserID = " + CFL.Q(GD.UserID) + @"
  and DocuStatus IN ('X', 'Y', 'H') 
  and A.FormID not in ('IM021') ";
            // X:기안 ->Y:결재중 ->Z:결재완료 & B:반송 & H : 보류;

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M16", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }



        /// <summary>
        /// WfMyBox
        /// </summary>

        [WebMethod]
        public ArrayList WfMBQuery(object[] GDObj, string strLangID, string strSiteCode, string strUserID
            , string strBDate, string strEDate, string strFormID, bool bIncludeA, bool bIncludeC)
        {
            // Global Data
            GData GD = new GData(GDObj);

            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select	A.FormID, IsNull ( J.ObjName, K.ObjName ) as ObjName, A.OriginNo, C.TypeName, case when A.DocuStatus = 'B' then E.TypeName else D.StepTitle end as StepTitle
	, I.UserName, case when A.RouteStep = 1 then G.FirstStepTitle else F.StepTitle end, A.WfDateTime, A.OriginSerNo
	, case when B.GlobalCheck = " + CFL.Q("Y") + @" then B.FormUrl else K.FormUrl end  + '?Action=Origin&OriginNo=' + rtrim ( A.OriginNo ) + '&OriginSerNo=' + cast ( A.OriginSerNo as varchar(7) ) as Url
	, 'width=' + cast ( B.FormWidth as varchar(4) ) + ',height=' + cast ( B.FormHeight as varchar(4) ) + ',' as FormSize,   A.WfAmount
from	(
	select	A.SiteCode, A.FormID, A.OriginNo, A.OriginSerNo, B.CurStep, B.DocuStatus, A.WfDateTime, A.RouteStep, B.RouteCode, B.WfAmount
	from	WfDocuRoute A, WfDocument B
	where	A.UserID = " + CFL.Q(strUserID);

            if (null != strFormID && "" != strFormID)
                strQuery += @"
		and A.FormID = " + CFL.Q(strFormID);

            strQuery += @" and left ( A.WfDateTime, 8 ) between " + CFL.Q(strBDate) + @" and " + CFL.Q(strEDate) + @"
		and A.SiteCode = B.SiteCode and A.FormID = B.FormID and A.OriginNo = B.OriginNo and A.OriginSerNo = B.OriginSerNo
	) A
	Left Outer Join	ModuleObj B
		on	A.FormID = B.FormID
	Left Outer Join	TypeMaster C
		on	A.DocuStatus = C.TypeCode and C.LangID = " + CFL.Q(strLangID) + @" and C.TypeID = 'wfDocuStatus'
	Left Outer Join	WfRouteItem D
		on	A.CurStep = D.RouteStep and A.RouteCode = D.RouteCode and A.SiteCode = D.SiteCode
	Left Outer Join	TypeMaster E
		on	E.TypeCode = 'V'  and E.LangID = " + CFL.Q(strLangID) + @" and E.TypeID = 'wfDocuStatus'
	Left Outer Join	WfRouteItem F
		on	A.RouteStep = F.RouteStep and A.RouteCode = F.RouteCode and A.SiteCode = F.SiteCode
	Left Outer Join	WfRoute G
		on	A.RouteCode = G.RouteCode and A.SiteCode = G.SiteCode
	Left Outer Join	WfDocuRoute H
		on	A.FormID = H.FormID and A.SiteCode = H.SiteCode and A.OriginNo = H.OriginNo and A.OriginSerNo = H.OriginSerNo and H.RouteStep = 1
	Left Outer Join	xErpUser I
		on	H.UserID = I.UserID
	Left Outer Join	ComMenu J
		on	B.ObjID = J.ObjID and J.LangID = " + CFL.Q(GD.LangID) + @" and J.ComCode = " + CFL.Q(GD.ComCode) + @"
	Left Outer Join	ModuleObjLang K
		on	B.ObjID = K.ObjID and K.LangID = " + CFL.Q(GD.LangID) + @" 
";

            string strWhereClause = "";
            if (!bIncludeA)
            {
                strWhereClause += "where A.RouteStep = 1";
            }
            if (!bIncludeC)
            {
                strWhereClause += ((strWhereClause.Length < 1) ? "where	" : " and ") + @"A.DocuStatus <> " + CFL.Q("Z");
            }

            strQuery += strWhereClause + @"
order by	A.WfDateTime, A.FormID, A.OriginNo, A.OriginSerNo
";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M17", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }


        /// <summary>
        /// WfReadBox
        /// </summary>

        [WebMethod]
        public ArrayList WfRBQuery(object[] GDObj, string strLangID, string strSiteCode, string strUserID
            , string strBDate, string strEDate, string strFormID, bool bIncludeA)
        {
            // Global Data
            GData GD = new GData(GDObj);

            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select	Null as chkDummy, A.FormID, IsNull ( H.ObjName, I.ObjName ) as ObjName, A.OriginNo
	, D.TypeName, case when A.DocuStatus = 'B' then E.TypeName else B.StepTitle end as StepTitle
	, G.TypeName, F.UserName, A.WfDateTime, A.OriginSerNo, A.ReadCheck
	, case when C.GlobalCheck = " + CFL.Q("Y") + @" then C.FormUrl else I.FormUrl end + '?Action=Origin&OriginNo=' + rtrim ( A.OriginNo ) + '&OriginSerNo=' + cast ( A.OriginSerNo as varchar(7) ) as Url
	, 'width=' + cast ( C.FormWidth as varchar(4) ) + ',height=' + cast ( C.FormHeight as varchar(4) ) + ',' as FormSize
from	(
	select	A.SiteCode, A.FormID, A.OriginNo, A.OriginSerNo, A.RouteCode, A.DocuStatus, A.CurStep, B.ReadCheck, C.UserID, C.WfDateTime
	from	WfDocument A, WfDocuRef B, WfDocuRoute C
	where	A.SiteCode = B.SiteCode and A.FormID = B.FormID and A.OriginNo = B.OriginNo and A.OriginSerNo = B.OriginSerNo";

            if (null != strFormID && "" != strFormID)
                strQuery += @"
		and A.FormID = " + CFL.Q(strFormID);

            strQuery += @"
		and B.UserID = " + CFL.Q(strUserID);

            if (!bIncludeA)
                strQuery += @"
		and B.ReadCheck = " + CFL.Q("N");

            strQuery += @"
		and A.SiteCode = C.SiteCode and A.FormID = C.FormID and A.OriginNo = C.OriginNo and A.OriginSerNo = C.OriginSerNo
		and C.RouteStep = 1
		and left ( C.WfDateTime, 8 ) between " + CFL.Q(strBDate) + @" and " + CFL.Q(strEDate) + @"
	) A
	Left Outer Join	WfRouteItem B
		on	A.SiteCode = B.SiteCode and A.RouteCode = B.RouteCode
			and A.CurStep = B.RouteStep
	Left Outer Join	ModuleObj C
		on	A.FormID = C.FormID
	Left Outer Join	TypeMaster D
		on	D.LangID = " + CFL.Q(strLangID) + @" and A.DocuStatus = D.TypeCode and D.TypeID = 'wfDocuStatus'
	Left Outer Join	TypeMaster E
		on	E.LangID = " + CFL.Q(strLangID) + @" and E.TypeCode = 'V' and E.TypeID = 'wfDocuStatus'
	Left Outer Join	xErpUser F
		on	A.UserID = F.UserID
	Left Outer Join	TypeMaster G
		on	G.LangID = " + CFL.Q(strLangID) + @" and A.ReadCheck = G.TypeCode and G.TypeID = 'wfReadCheck'
	Left Outer Join	ComMenu H
		on	C.ObjID = H.ObjID and H.LangID = " + CFL.Q(GD.LangID) + @" and H.ComCode = " + CFL.Q(GD.ComCode) + @"
	Left Outer Join	ModuleObjLang I
		on	C.ObjID = I.ObjID and I.LangID = " + CFL.Q(GD.LangID) + @" 
order by	A.WfDateTime, A.FormID, A.OriginNo, A.OriginSerNo
";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M18", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        [WebMethod]
        public ArrayList WfRBCount(object[] GDObj)
        {
            // Global Data
            GData GD = new GData(GDObj);

            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select	count(*)
from	WfDocuRef
where	UserID = " + CFL.Q(GD.UserID) + @" and ReadCheck = " + CFL.Q("N");

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M16", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }


        /// <summary>
        ///  현재 결제중인 문서의 수(해당 결제경로를 사용하고 있는)
        /// </summary>
        [WebMethod]
        public ArrayList WfDoCount(object[] GDObj, string siteCode, string RouteCode)
        {
            // Global Data
            GData GD = new GData(GDObj);


            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select	count(*)
from	WfDocument
where	SiteCode = " + CFL.Q(siteCode) + @" and RouteCode = " + CFL.Q(RouteCode) + @"
AND     DocuStatus IN (N'X',N'Y',N'H')";
            //X:기안 ->Y:결재중 ->Z:결재완료 & B:반송 & H : 보류;


            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M16", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        /// <summary>
        ///  현재 결제중인 문서의 수(해당 결제경로를 사용하고 있는)
        /// </summary>
        [WebMethod]
        public ArrayList WfRouteStep(object[] GDObj, string userID, string RouteCode)
        {
            // Global Data
            GData GD = new GData(GDObj);

            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
				select  RouteStep,UserId, AbsentEmp
				from 	WfRouteItem
				where 	SiteCode = " + CFL.Q(GD.SiteCode) + @"
				and     RouteCode = " + CFL.Q(RouteCode) + @"
				and     " + CFL.Q(userID) + " IN (userid, absentEmp)";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M16", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        /// <summary>
        /// CanBeApprove<- 합의시 다음단계로 넘어갈수 있는지 여부, hasApprove<- 합의가 되었는지 여부
        /// </summary>
        [WebMethod]
        public ArrayList CanBeApprove(object[] GDObj, string RouteCode, int RouteStep, string FormID, string OriginNo, string OriginSerNo, string MoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);

            if (OriginSerNo == null || OriginSerNo == "")
            {
                OriginSerNo = "0";
            }


            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
					SELECT    'CanBeApproved' = 
							CASE 
								WHEN 
								(select  	COUNT(*)
								from 	WfRouteItem
								where 	SiteCode = " + CFL.Q(GD.SiteCode) + @"
								and     RouteCode =  " + CFL.Q(RouteCode) + @"
								and     FLOOR(RouteStep) = " + RouteStep + @"
								)
								<=	
								(
								SELECT  count(*)+1
								From	WfDocument A, WfDocuRoute B
								where   A.FormID        = B.FormID
								and     A.SiteCode      = B.SiteCode
								and     A.OriginNo      = B.OriginNo
								and     A.OriginSerNo	= B.OriginSerNo
								and		A.OriginSerNo	= " + CFL.Q(OriginSerNo) + @"
								and     A.FormID		=  " + CFL.Q(FormID) + @"
								and     A.OriginNo		=  " + CFL.Q(OriginNo) + @"
								and 	A.SiteCode		= " + CFL.Q(GD.SiteCode) + @"
								and     A.RouteCode		=  " + CFL.Q(RouteCode) + @"
								and     FLOOR(B.RouteStep ) = " + RouteStep + @"
								and     B.WfStatus IN ('A', 'D') 
                                -- 추가 (2013.08.26)
                                and     A.MoreKey = B.MoreKey
								and		A.MoreKey	= " + CFL.Q(MoreKey) + @"
                                
								)
								Then 'TRUE'
							ELSE 'FALSE'
						END
						,'hasAgreed'=
						CASE 
							WHEN 
							(Select CurStep
							 From WfDocument
							 Where FormID =  " + CFL.Q(FormID) + @"
							   and OriginSerNo = " + CFL.Q(OriginSerNo) + @"
						 	   and OriginNo =  " + CFL.Q(OriginNo) + @"
							   and SiteCode =" + CFL.Q(GD.SiteCode) + @"
                               and MoreKey = " + CFL.Q(MoreKey) + @"  -- 추가 (2013.08.26)
							)
							>	
							(Select max(RouteStep)
							 From WfDocuRoute
							 Where FormID =   " + CFL.Q(FormID) + @"
							   and OriginSerNo =" + CFL.Q(OriginSerNo) + @"
							   and OriginNo = " + CFL.Q(OriginNo) + @"
							   and SiteCode =" + CFL.Q(GD.SiteCode) + @"
                               and MoreKey = " + CFL.Q(MoreKey) + @"  -- 추가 (2013.08.26)
							)
							Then 'TRUE'
						ELSE 'FALSE'
						END	
						,'hasHold'=
						CASE 
							WHEN 
							(Select count(*)
							 From WfDocuRoute
							 Where FormID =  " + CFL.Q(FormID) + @"
							   and OriginSerNo =" + CFL.Q(OriginSerNo) + @"
							   and OriginNo = " + CFL.Q(OriginNo) + @"
							   and SiteCode =" + CFL.Q(GD.SiteCode) + @"
							   and WfStatus = N'C'
							   and FLOOR(RouteStep) = " + RouteStep + @"
                               and MoreKey = " + CFL.Q(MoreKey) + @"  -- 추가 (2013.08.26)
							)
							> 0	
							Then 'TRUE'
						ELSE 'FALSE'
						END	

			";

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M16", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        /// <summary>
        ///  현재 결제중인지 여부를 가져옴
        /// </summary>
        [WebMethod]
        public ArrayList hasApproved(object[] GDObj, string FormID, string OriginNo, string OriginSerNo, string UserID, string MoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);

            if (OriginSerNo == null || OriginSerNo == "")
            {
                OriginSerNo = "0";
            }


            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
					Select	count(*)
					From WfDocuRoute
					Where SiteCode =" + CFL.Q(GD.SiteCode) + @"
					  and FormID    = " + CFL.Q(FormID) + @"
					  and OriginNo =  " + CFL.Q(OriginNo) + @"
					  and OriginSerNo = " + OriginSerNo + @"
					  and UserID  = " + CFL.Q(UserID) + @"
                      and MoreKey = " + CFL.Q(MoreKey);

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M16", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        /// <summary>
        ///  전결가능한 결제리스트 조회
        /// </summary>
        [WebMethod]
        public ArrayList getApproveAll(object[] GDObj, string RouteCode, int RouteStep, string FormID, string OriginNo, string OriginSerNo, string UserID, string MoreKey)
        {
            // Global Data
            GData GD = new GData(GDObj);
            if (OriginSerNo == null || OriginSerNo == "")
            {
                OriginSerNo = "0";
            }


            // Load Default Route Information
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
					SELECT UserID, RouteStep 
					FROM WfRouteItem A
					WHERE SiteCode = " + CFL.Q(GD.SiteCode) + @"
					  AND RouteCode =" + CFL.Q(RouteCode) + @"
					  AND FLOOR(RouteStep) >= " + RouteStep + @"	
					  AND UserId NOT IN ( SELECT UserID
		       							  FROM 	WfDocuRoute
										  WHERE SiteCode	= " + CFL.Q(GD.SiteCode) + @"
											AND	FormID		= " + CFL.Q(FormID) + @"
											AND	OriginNo	= " + CFL.Q(OriginNo) + @"
											AND	OriginSerNo = " + OriginSerNo + @" 
                                            AND MoreKey     = " + MoreKey + @" )
					Order by RouteStep";

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = CFL.RS("M16", this, Server, GD.LangID);
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }


    }

}