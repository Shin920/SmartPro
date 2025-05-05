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
    public class ModuleObj
    {
        public ModuleObj() { }

        public ModuleObj(SqlConnection db, SqlTransaction tr)
        {

            m_db = db;
            m_tr = tr;
            m_bInTrans = true;
        }

        SqlConnection m_db;
        SqlTransaction m_tr;
        bool m_bInTrans = false;

        public ArrayList CMPopup(object[] GDObj, string strComCode, string strModuleIni, string strWhereClause)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
select	IsNull ( X.ObjLevel, Y.ObjLevel ) as ObjLevel, IsNull ( X.ParentObj, Y.ParentObj ) as ParentObj
	, IsNull ( X.ObjID, Y.ObjID ) as ObjID
	, case when IsNull ( Y.ObjCategory, " + CFL.Q("S") + " ) = " + CFL.Q("F") + @" then " + CFL.Q("Y") + @" else " + CFL.Q("N") + @" end as LeafCheck
	, IsNull ( X.ObjID, Y.ObjID ) as ChildObj, IsNull ( X.ObjName, Y.ObjName ) as ObjName
from	(
	select	ObjLevel, ParentObj, ObjID, ObjName, SortKey, ObjUse
	from	ComMenu 
	where	LangID = " + CFL.Q(GD.LangID) + @" and ComCode = " + CFL.Q(strComCode) + @"
		and ObjCategory = " + CFL.Q("S") + @"
	) X	Full Outer Join
	(
	select	A.ObjLevel, A.ParentObj, A.ObjID, A.ObjCategory, B.Developing
		, A.FormID, B.ObjName, A.SortKey
	from	ModuleObj A
		, ModuleObjLang B
	where	A.ObjID = B.ObjID and B.ObjUse = " + CFL.Q("Y") + @" and B.LangID = " + CFL.Q(GD.LangID) + @" 
		and A.ObjCategory in ( " + CFL.Q("M") + @", " + CFL.Q("S") + @", " + CFL.Q("G") + @" )
		and A.Mobile = " + CFL.Q("N") + @" and B.ObjUse = " + CFL.Q("Y") + @"
	) Y	on	X.ObjID = Y.ObjID
where	IsNull ( X.ObjUse, " + CFL.Q("Y") + @" ) = " + CFL.Q("Y");

            // Check Developing Menus
            if (GD.LicenseType != "D")
                strQuery += @"
	and IsNull ( Y.Developing, " + CFL.Q("N") + @" ) = " + CFL.Q("N");

            if (strWhereClause != null && 0 != strWhereClause.Length)
                strQuery += " and " + strWhereClause;

            strQuery += @"
order by IsNull ( X.ObjLevel, Y.ObjLevel ), IsNull ( X.SortKey, Y.SortKey ), IsNull ( X.ParentObj, Y.ParentObj )
	, IsNull ( X.ObjID, Y.ObjID ), IsNull ( X.ObjName, Y.ObjName )";

            DataTableReader dr;
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
                strErrorCode = "M01";
                strErrorMsg = CFL.RS("M01", this, GD.LangID); ;
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }



    }
}
