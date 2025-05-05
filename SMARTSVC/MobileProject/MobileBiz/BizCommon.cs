using SMART;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBiz
{
    public class BizCommon : DBBase
    {
        public BizCommon(string strClientID)
        {
            connection.ConnectDB(strClientID);
        }

        public CFL GetFormLangDataLoad(CFLUserInfo G, string strLangID, string[] sarMCode)
        {
            DataSet ds = null;
            CFL data = null;
            string strMCode = "";

            for (int i = 0; i < sarMCode.Length; i++)
            {
                if (i == 0)
                {
                    strMCode += CFL.Q(sarMCode[i]);
                }
                else
                {
                    strMCode += "," + CFL.Q(sarMCode[i]);
                }
            }

            string strQuery = @"
SELECT MCode, Message
FROM MessageMaster
WHERE LangID = " + CFL.Q(strLangID) + @"
  AND MCode IN (" + strMCode + ")";


            try
            {
                ds = SQLDataSet(strQuery);
                data = EncodeData(ds, 1, "조회가 완료 되었습니다.");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, "조회 중 오류가 발생 하였습니다." + ex.ToString());
            }

            return data;

        }

        public CFL GetWhMaster(CFLUserInfo G, string strWhere)
        {
            DataSet ds = null;
            CFL data = null;
            string sql = "";

            sql = @"SELECT WhCode, WhName FROM WhMaster WHERE ComCode = " + CFL.Q(G.ComCode) + @" AND WhUse = 'Y'";

            if (strWhere != "")
                sql += strWhere;

            sql += @"
            ORDER BY WhCode ";

            try
            {
                ds = SQLDataSet(sql);
                data = EncodeData(ds, 1, "조회가 완료 되었습니다.");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, "조회 중 오류가 발생 하였습니다." + ex.ToString());
            }

            return data;
        }

        public CFL GetEquipMaster(CFLUserInfo G, string strWhere)
        {
            DataSet ds = null;
            CFL data = null;
            string sql = "";

            sql = @"SELECT EquipCode, b.WcName as EquipName 
                    FROM pmEquipMaster a
                    Left Outer Join nppWcMaster b on a.SiteCode = b.SiteCode and a.WcCode = b.WcCode
                    WHERE ComCode = " + CFL.Q(G.ComCode);

            if (strWhere != "")
                sql += strWhere;

            sql += @"
            ORDER BY EquipCode ";

            try
            {
                ds = SQLDataSet(sql);
                data = EncodeData(ds, 1, "조회가 완료 되었습니다.");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, "조회 중 오류가 발생 하였습니다." + ex.ToString());
            }

            return data;
        }

        public CFL GetCateg(CFLUserInfo G, string strWhere)
        {
            DataSet ds = null;
            CFL data = null;
            string sql = "";

            sql = @"SELECT Item_Categ_D_Code AS ItemGroup, Item_Categ_D_Name AS GroupName FROM ItemSiteMaster WHERE SiteCode = " + CFL.Q(G.SiteCode) + @" Group By Item_Categ_D_Code, Item_Categ_D_Name";

            if (strWhere != "")
                sql += strWhere;

            try
            {
                ds = SQLDataSet(sql);
                data = EncodeData(ds, 1, "조회가 완료 되었습니다.");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, "조회 중 오류가 발생 하였습니다." + ex.ToString());
            }

            return data;
        }

        public CFL NoCreate(CFLUserInfo G, string strSiteCode, string strNoType, string strYyyymmdd)
        {


            DataSet ds = null;
            CFL data = null;
            string strQuery = string.Format(@"

exec spCreateNo {0}, {1}, {2}", CFL.Q(strSiteCode), CFL.Q(strNoType), CFL.Q(strYyyymmdd));

            DataTableReader dr;

            try
            {
                ds = SQLDataSet(strQuery);
                data = EncodeData(ds, 1, "완료");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, ex.Message);
            }

            return data;

        }

        public CFL Worker_Load(CFLUserInfo G, string strItemSpec)
        {
            DataSet ds = null;
            CFL data = null;
            string sql = "";

            sql += @"
select b.ItemCode, b.ItemName, b.ItemSpec
from nppWorkOrder a
join ItemSiteMaster b on a.SiteCode = b.SiteCode and a.ItemCode = b.ItemCode
where a.SiteCode = " + CFL.Q(G.SiteCode) + @"
and isnull(ItemSpec, LEFT(ItemName, CHARINDEX('(',ItemName)-1)) = " + CFL.Q(strItemSpec);

            try
            {
                ds = SQLDataSet(sql);
                data = EncodeData(ds, 1, "조회가 완료 되었습니다.");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, "조회 중 오류가 발생 하였습니다." + ex.ToString());
            }

            return data;
        }

        public CFL GetCaseAcc(CFLUserInfo G, string strCodeID, string strWhere)
        {
            DataSet ds = null;
            CFL data = null;
            string strQuery = "";

            strQuery += @"
SELECT SysCase, CaseName,CaseCode
FROM CaseAcc A 
WHERE A.ComCode=" + CFL.Q(G.ComCode) + @" 
    and a.CaseUse ='Y'
    and a.CaseID = " + CFL.Q(strCodeID);
            if (strWhere != "")
            {
                strQuery += " " + strWhere;
            }

            strWhere += @"
ORDER BY A.SortKey ";

            DataTableReader dr;

            try
            {
                ds = SQLDataSet(strQuery);
                data = EncodeData(ds, 1, "완료");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, ex.Message);
            }

            return data;
        }

        public CFL GetCodeMaster(CFLUserInfo G, string strCodeID, string strWhere)
        {
            DataSet ds = null;
            CFL data = null;
            string strQuery = "";

            strQuery += @"
SELECT A.CodeCode, A.CodeName
FROM CodeMaster A 
WHERE A.ComCode=" + CFL.Q(G.ComCode) + " and CodeID = " + CFL.Q(strCodeID);
            if (strWhere != "")
            {
                strQuery += " " + strWhere;
            }

            strWhere += @"
ORDER BY A.SortKey ";

            DataTableReader dr;

            try
            {
                ds = SQLDataSet(strQuery);
                data = EncodeData(ds, 1, "완료");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, ex.Message);
            }

            return data;
        }

        public CFL getDDLPalletCode(CFLUserInfo G)
        {
            DataSet ds = null;
            CFL data = null;
            string strQuery = "";

            strQuery = @"
SELECT 
    A.PalletCode
	, A.PalletName + ' (' + B.CsName + ')' as PalletName 
FROM PalletMaster A  
left outer join CsMaster B ON B.ComCode=" + CFL.Q(G.ComCode) + @" AND B.CsCode=A.CsCode
WHERE A.SiteCode=" + CFL.Q(G.SiteCode) + @" and PalletUse=N'Y' --AND convert(varchar(8), GETDATE(), 112) BETWEEN A.BDate AND A.EDate ";

            DataTableReader dr;

            try
            {
                ds = SQLDataSet(strQuery);
                data = EncodeData(ds, 1, "완료");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, ex.Message);
            }

            return data;
        }

        public CFL Cs_Load(CFLUserInfo G, string strDate)
        {
            DataSet ds = null;
            CFL data = null;
            string sql = "";

            sql += @"
  Select A.CsCode
        ,B.CsName
    From (Select B.CsCode
		    From sdIrItem A
			Join sdIrHeader B ON A.SiteCode = B.SiteCode And A.IrNo = B.IrNo
		   Where A.IrExpectDate = " + CFL.Q(strDate) + @"
		Group By B.CsCode
		 )A
	Join CsMaster B ON A.CsCode = B.CsCode And B.ComCode = " + CFL.Q(G.ComCode) + @" And isnull(B.CsUse, 'N') = 'Y'
            ";

            try
            {
                ds = SQLDataSet(sql);
                data = EncodeData(ds, 1, "조회가 완료 되었습니다.");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, "조회 중 오류가 발생 하였습니다." + ex.ToString());
            }

            return data;
        }

        public CFL GetSiteMaster(CFLUserInfo G, string strWhere)
        {
            DataSet ds = null;
            CFL data = null;
            string strQuery = "";

            strQuery += @"
SELECT A.SiteCode, A.SiteName
FROM SiteMaster A 
";
            if (strWhere != "")
            {
                strQuery += @" 
Where " + strWhere;
            }

            strWhere += @"
ORDER BY A.ComCode, A.SiteCode";

            DataTableReader dr;

            try
            {
                ds = SQLDataSet(strQuery);
                data = EncodeData(ds, 1, "완료");
            }
            catch (Exception ex)
            {
                data = EncodeData(ds, -1, ex.Message);
            }

            return data;
        }
    }
}
