using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SMART.REPORT.OM;
//using SMART.REPORT.OS;
//using SMART.REPORT.EX;
//using SMART.REPORT.SD;
//using SMART.REPORT.QM;
//using SMART.REPORT.NP;
//using SMART.REPORT.PO;
//using SMART.REPORT.MM;
//using SMART.REPORT.HR;
//using SMART.REPORT.GL;
//using SMART.REPORT.QM;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using DevExpress.Utils.Filtering.Internal;
//using SMART.REPORT.MM;
//using SMART.REPORT.SD;

namespace SMART.REPORT
{
    //폼과 출력물을 연결해 주는 클래스
    public class REPORT_LINK
    {
        private string aRID { get; set; }
        private string sParam { get; set; }
        object[] GD;

        string strEmpID1 = "";
        string strEmpName1 = "";
        string strEmpID2 = "";
        string strEmpName2 = "";
        string strEmpID3 = "";
        string strEmpName3 = "";

        string strSeverAddress = HttpContext.Current.Request.Url.Host;

        public REPORT_LINK(object[] GDObj, string stID, string strParam)
        {
            GD = GDObj;
            aRID = stID;
            sParam = strParam;
        }

        public XtraReport Classification()
        {

            CultureInfo provider = CultureInfo.InvariantCulture;

            string strReturnPeriod = "";
            string strReturnPeriodFromTo = "";
            string strWriteDownDate = "";

            string strSum1 = ""; string strSum2 = ""; string strSum3 = "";
            string strCs1 = ""; string strCs2 = ""; string strCs3 = "";
            string strJu1 = ""; string strJu2 = ""; string strJu3 = "";
            string strSelectedSiteCode = ""; string selectedYear = ""; string strQuarter = ""; string strReportType = ""; string strQuarterSerNo = "";
            string strBdate = ""; string strEdate = "";

            string strRegName = ""; string strRegNo = ""; string strCsIndustry = ""; string strCsItems = ""; string strReporter = ""; string strTaxOffice = "";
            string strW = "";
            
            int iPageNo = 0;

            XtraReport rpt = new XtraReport();
            switch (aRID)
            {

            }

            return rpt;
        }

        private string FormatDate(string sourceDate, string formatPattern)
        {
            System.IFormatProvider format = new System.Globalization.CultureInfo("ko-KR", true);
            System.DateTime myDateTime = System.DateTime.ParseExact(sourceDate, "yyyyMMdd", format, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

            return myDateTime.ToString(formatPattern);
        }
    }
}



