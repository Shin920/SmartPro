using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART.COMMON.DS
{
    public class LoginClass
    {
        public LoginClass() { }

        public ArrayList LangSelect(string strClientID)
        {
            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = @"
Select LangID, LangName	from LangMaster with (NOLOCK) where LangUse = " + CFL.Q("Y") + @"	order by	SortKey";

            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(strClientID, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = "Server Connection Error";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        public ArrayList DdlSiteSelect(string strClientID)
        {
            string strErrorCode = "00", strErrorMsg = "";
            StringBuilder sblQuery = new StringBuilder();

            sblQuery.Append(@"
select	A.SiteCode, A.SiteName
from	SiteMaster A with (NOLOCK)
	inner join CompanyMaster B with (NOLOCK)
	on A.ComCode = B.ComCode
where	A.SiteUse = ").Append(CFL.Q("Y")).Append(" and B.ComUse = ").Append(CFL.Q("Y"));


            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(strClientID, sblQuery.ToString(), CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = "Server Connection Error";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally { sblQuery = null; }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        // 법인정보 가져오기
        public ArrayList DdlComSelect(string strClientID)
        {
            string strErrorCode = "00", strErrorMsg = "";

            StringBuilder sblQuery = new StringBuilder();

            sblQuery.Append(@"
Select ComCode, ComName
From CompanyMaster with (NOLOCK)	
Where ComUse = ").Append(CFL.Q("Y"));


            DataTableReader dr;
            try
            {
                dr = CFL.ExecuteDataTableReader(strClientID, sblQuery.ToString(), CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = "Server Connection Error";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }
            finally { sblQuery = null; }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

        // 법인에 속한 사업장 가져오기
        public ArrayList DdlSiteSelectForComCode(string strComCode, string strClientID)
        {

            string strErrorCode = "00", strErrorMsg = "";

            string strQuery = "";

            strQuery = @"
Select A.SiteCode, A.SiteName
From SiteMaster A with (NOLOCK)
	Join CompanyMaster B with (NOLOCK) on A.ComCode = B.ComCode
Where B.ComCode = " + CFL.Q(strComCode) + @"
  and A.SiteUse = 'Y' 
";

            DataTableReader dr;

            try
            {
                dr = CFL.ExecuteDataTableReader(strClientID, strQuery, CommandType.Text);
            }
            catch (Exception e)
            {
                strErrorCode = "01";
                strErrorMsg = "Server Connection Error";
                return CFL.EncodeData(strErrorCode, strErrorMsg, e);
            }

            return CFL.EncodeData(strErrorCode, strErrorMsg, dr, null);
        }

    }
}
