using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;

namespace SMART.COMMON.DS
{
    public class ModuleObj
    {
        public ModuleObj() { }

        public DataSet Navigator(object[] GDObj, string[] sarModuleGubun, string strWhere)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @" INSERT INTO LogonMenu ";

            strQuery += @"
SELECT " + CFL.Q(GD.UserID) + @"
      , ISNULL(X.ObjLevel, Y.ObjLevel) AS ObjLevel
	  , ISNULL(X.ParentObj, Y.ParentObj) AS ParentObj
	  , ISNULL(X.ObjID, Y.ObjID) AS ObjID
	  , CASE WHEN ISNULL(Y.ObjCategory, 'S') = 'F' THEN N'Y' ELSE 'N' END AS LeafCheck
	  , CASE ISNULL(Y.ObjCategory, 'S') WHEN 'F' THEN 
		'javascript:addTabMenu(""' + RTRIM(Y.FormID) + '"", ""' 
								   + RTRIM(ISNULL(X.ObjName, Y.ObjName)) + '"",""' 
								   + RTRIM(Y.FormURL) + '"",""' 
								   + RTRIM(Y.FormID) + '"")' ELSE NULL END AS URL
	  , ISNULL(X.ObjName, Y.ObjName) AS ObjName, ISNULL(X.SortKey, Y.SortKey) AS SortKey, NULL AS OKMenu
FROM (
		SELECT ObjLevel, ParentObj, ObjID, ObjName, SortKey, ObjUse
		FROM ComMenu
		WHERE LangID = " + CFL.Q(GD.LangID) + @" AND ComCode = " + CFL.Q(GD.ComCode) + @"
	 )
	 X FULL OUTER JOIN
	 (
		SELECT A.ObjLevel, A.ParentObj, A.ObjID, A.ObjCategory, B.Developing
		     , CASE WHEN A.GlobalCheck = 'Y' THEN A.FormUrl ELSE B.FormUrl END AS FormUrl
		     , A.FormID, B.ObjName, A.SortKey
		FROM ModuleObj A
		JOIN ModuleObjLang B ON A.ObjID = B.ObjID
		WHERE B.ObjUse = 'Y' AND B.LangID = " + CFL.Q(GD.LangID) + @"
		  AND A.Mobile = 'N' AND A.ObjLevel <> '1'
		  AND EXISTS (SELECT FormID FROM FormSecurity_ID WHERE (FormID = A.FormID OR ModuleInitial = A.FormID) AND UserGroup = " + CFL.Q(GD.UserGroup) + @" AND SiteCode = " + CFL.Q(GD.SiteCode) + @")
		  AND EXISTS (SELECT ParentObj FROM ModuleObj WHERE ObjID = A.ParentObj) ";

            for (int i = 0; i < sarModuleGubun.Length; i++)
            {
                if (i == 0)
                {
                    strQuery += @"
          AND (A.FormID LIKE " + CFL.Q(sarModuleGubun[i] + "%");
                }
                else
                {
                    strQuery += @"
           OR A.FormID LIKE " + CFL.Q(sarModuleGubun[i] + "%");
                }
            }

            if (strWhere != "")
            {
                strQuery += strWhere;
            }

            strQuery += @")
	 ) Y ON X.ObjID = Y.ObjID
WHERE CASE WHEN Y.ObjID IS NULL THEN X.ObjID ELSE 'U' END LIKE 'U%'
  AND ISNULL(X.ObjUse, 'Y') = 'Y'
  AND ISNULL(Y.Developing, 'N') = 'N'

EXEC C_spTreeMenu " + CFL.Q(GD.UserID) + @"

DELETE FROM LogonMenu WHERE UserID = " + CFL.Q(GD.UserID) + @"
";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


        // 최상위 메뉴 안 보이게...
        public DataSet Navigator_NoRoot(object[] GDObj, string[] sarModuleGubun, string strWhere)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @" INSERT INTO LogonMenu ";

            strQuery += @"
SELECT " + CFL.Q(GD.UserID) + @"
      , ISNULL(X.ObjLevel, Y.ObjLevel) AS ObjLevel
	  , ISNULL(X.ParentObj, Y.ParentObj) AS ParentObj
	  , ISNULL(X.ObjID, Y.ObjID) AS ObjID
	  , CASE WHEN ISNULL(Y.ObjCategory, 'S') = 'F' THEN N'Y' ELSE 'N' END AS LeafCheck
	  , CASE ISNULL(Y.ObjCategory, 'S') WHEN 'F' THEN 
		'javascript:addTabMenu(""' + RTRIM(Y.FormID) + '"", ""' 
								   + RTRIM(ISNULL(X.ObjName, Y.ObjName)) + '"",""' 
								   + RTRIM(Y.FormURL) + '"",""' 
								   + RTRIM(Y.FormID) + '"")' ELSE NULL END AS URL
	  , ISNULL(X.ObjName, Y.ObjName) AS ObjName, ISNULL(X.SortKey, Y.SortKey) AS SortKey, NULL AS OKMenu
FROM (
		SELECT ObjLevel, ParentObj, ObjID, ObjName, SortKey, ObjUse
		FROM ComMenu
		WHERE LangID = " + CFL.Q(GD.LangID) + @" AND ComCode = " + CFL.Q(GD.ComCode) + @"
	 )
	 X FULL OUTER JOIN
	 (
		SELECT A.ObjLevel, A.ParentObj, A.ObjID, A.ObjCategory, B.Developing
		     , CASE WHEN A.GlobalCheck = 'Y' THEN A.FormUrl ELSE B.FormUrl END AS FormUrl
		     , A.FormID, B.ObjName, A.SortKey
		FROM ModuleObj A
		JOIN ModuleObjLang B ON A.ObjID = B.ObjID
		WHERE B.ObjUse = 'Y' AND B.LangID = " + CFL.Q(GD.LangID) + @"
		  AND A.Mobile = 'N' AND A.ObjLevel <> '1' 

          AND A.ObjLevel <> '2'  -- (최상위 Menu 안보이게)

		  AND EXISTS (SELECT FormID FROM FormSecurity_ID WHERE (FormID = A.FormID OR ModuleInitial = A.FormID) AND UserGroup = " + CFL.Q(GD.UserGroup) + @" AND SiteCode = " + CFL.Q(GD.SiteCode) + @")
		  AND EXISTS (SELECT ParentObj FROM ModuleObj WHERE ObjID = A.ParentObj) ";

            for (int i = 0; i < sarModuleGubun.Length; i++)
            {
                if (i == 0)
                {
                    strQuery += @"
          AND (A.FormID LIKE " + CFL.Q(sarModuleGubun[i] + "%");
                }
                else
                {
                    strQuery += @"
           OR A.FormID LIKE " + CFL.Q(sarModuleGubun[i] + "%");
                }
            }

            if (strWhere != "")
            {
                strQuery += strWhere;
            }

            strQuery += @")
	 ) Y ON X.ObjID = Y.ObjID
WHERE CASE WHEN Y.ObjID IS NULL THEN X.ObjID ELSE 'U' END LIKE 'U%'
  AND ISNULL(X.ObjUse, 'Y') = 'Y'
  AND ISNULL(Y.Developing, 'N') = 'N'

EXEC C_spTreeMenu " + CFL.Q(GD.UserID) + @"

DELETE FROM LogonMenu WHERE UserID = " + CFL.Q(GD.UserID) + @"
";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }


        public DataSet Navigator_User(object[] GDObj, string[] sarModuleGubun, string strWhere)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
SELECT CASE WHEN A.ParentObj IS NULL THEN 1 ELSE 2 END AS ObjLevel
     , A.ParentObj
	 , A.ObjID
	 , CASE WHEN A.ObjCategory = 'F' THEN 'Y' ELSE 'N' END AS LeafCheck
	 , CASE A.ObjCategory WHEN 'F' THEN 
	  'javascript:addTabMenu(""' + RTRIM(A.FormID) + '"", ""' 
                                 + RTRIM(ISNULL(A.ObjName, '')) + '"",""'
                                 + RTRIM(CASE WHEN B.GlobalCheck = 'Y' THEN B.FormURL ELSE C.FormUrl END) + '"",""'
                                 + RTRIM(A.FormID) + '"")' ELSE NULL END AS FormUrl
	 , A.ObjName
FROM UserMenu A
LEFT JOIN ModuleObj B ON A.FormID = B.FormID AND B.ObjCategory = 'F'
LEFT JOIN ModuleObjLang C ON B.ObjID = C.ObjID AND A.LangID = C.LangID
LEFT JOIN ComMenu D ON B.ObjID = D.ObjID AND A.LangID = D.LangID AND D.ComCode = " + CFL.Q(GD.ComCode) + @"
WHERE A.ComCode = " + CFL.Q(GD.ComCode) + @" 
  and ISNULL(D.ObjUse, A.ObjUse) = 'Y' 
  AND A.UserID = " + CFL.Q(GD.UserID) + @" 
  AND A.LangID = " + CFL.Q(GD.LangID) + @"
ORDER BY A.SortKey, A.ParentObj, A.ObjID, A.ObjName ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

        public DataSet FindFormData(object[] GDObj, string strInfo)
        {
            // Global Data
            GData GD = new GData(GDObj);

            string strQuery = @"
Select A.FormID, Case When A.GlobalCheck = N'Y' Then A.FormUrl Else C.FormUrl End AS FormUrl, IsNull(B.ObjName, C.ObjName) AS ObjName
     , ISNULL(A.ObjID, D.ObjID) AS ObjID, ISNULL(A.ParentObj, D.ParentObj) AS ParentObj
     , RTRIM(ISNULL(H.ObjName, I.ObjName)) + '>' + RTRIM(ISNULL(E.ObjName, F.ObjName)) + '>' + RTRIM(ISNULL(B.ObjName, C.ObjName)) AS NaviMenu
From ModuleObj A
	Left Outer Join	ComMenu B on A.ObjID = B.ObjID and B.LangID = " + CFL.Q(GD.LangID) + @" and B.ComCode = " + CFL.Q(GD.ComCode) + @"
	Left Outer Join	ModuleObjLang C on A.ObjID = C.ObjID and C.LangID = " + CFL.Q(GD.LangID) + @"
	Left Outer Join	ModuleObj D on D.ObjID = A.ParentObj
	Left Outer Join	ComMenu E on E.ObjID = A.ParentObj and E.LangID = " + CFL.Q(GD.LangID) + @" and E.ComCode = " + CFL.Q(GD.ComCode) + @"
	Left Outer Join	ModuleObjLang F on D.ObjID = F.ObjID and F.LangID = " + CFL.Q(GD.LangID) + @"
	Left Outer Join	ModuleObj G on G.ObjID = IsNull ( E.ParentObj, D.ParentObj )
	Left Outer Join	ComMenu H on H.ObjID = IsNull ( E.ParentObj, D.ParentObj ) and H.LangID = " + CFL.Q(GD.LangID) + @" and H.ComCode = " + CFL.Q(GD.ComCode) + @"
	Left Outer Join	ModuleObjLang I on G.ObjID = I.ObjID and I.LangID = " + CFL.Q(GD.LangID) + @"
Where IsNull(C.ObjUse, 'N') = 'Y' And A.ObjCategory = 'F' And IsNull(C.Developing, 'Y') = 'N'
  And (A.FormID Like " + CFL.Q("%" + strInfo + "%") + @" OR IsNull(B.ObjName, C.ObjName) Like " + CFL.Q("%" + strInfo + "%") + @")
  AND EXISTS (SELECT FormID FROM FormSecurity_ID WHERE (FormID = A.FormID OR ModuleInitial = A.FormID) AND UserGroup = " + CFL.Q(GD.UserGroup) + @" AND SiteCode = " + CFL.Q(GD.SiteCode) + @")
Order by A.FormID, A.SortKey ";


            DataSet ds = new DataSet();

            try
            {
                ds = CFL.GetDataSet(GD, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                return ds;
            }

            return ds;
        }

        


    }
}