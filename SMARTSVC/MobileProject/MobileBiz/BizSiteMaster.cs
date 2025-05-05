using SMART;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileBiz
{
    public class BizSiteMaster : DBBase
    {
        public BizSiteMaster(string strClientID)
        {
            connection.ConnectDB(strClientID);
        }

        public CFL GetSiteSelect()
        {
            DataSet ds = null;
            CFL data = null;
            string sql = "";

            sql = @"
Select	A.SiteCode
      , A.SiteName
From	SiteMaster A with (NOLOCK)
Inner Join CompanyMaster B with (NOLOCK) On A.ComCode = B.ComCode
Where	A.SiteUse = " + CFL.Q("Y") + " And B.ComUse = " + CFL.Q("Y");

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
    }
}
