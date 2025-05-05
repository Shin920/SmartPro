using DevExpress.XtraReports.UI;
using SMART;
using SMART.REPORT;
using System;
using System.IO;
using System.Web.UI;
using System.Collections;
using System.Web;
//using SMART.REPORT.OM;
//using SMART.REPORT.OS;
//using SMART.REPORT.MM;
//using SMART.REPORT.PO;

public partial class Common_PrintForm_ReportPage_Sub : PageBase
{
    GObj G = new GObj();

    private string sReportID { get; set; }
    private string sParam { get; set; }

    string strEmpID1 = "";
    string strEmpName1 = "";
    string strEmpID2 = "";
    string strEmpName2 = "";
    string strEmpID3 = "";
    string strEmpName3 = "";

    string strSeverAddress = HttpContext.Current.Request.Url.Host;


    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();
        G = ((GObj)Session["G"]);

        if (!IsPostBack)
        {
            if (Request["Action"] == "Load")
            {

                sReportID = CFL.GetStr(Request["RID"]);
                sParam = CFL.GetStr(Request["Param"]);
                REPORT_LINK LINK = new REPORT_LINK(G.D, sReportID, sParam);
                XtraReport RPT = LINK.Classification();
                RPT.CreateDocument();

                switch (sReportID)
                {
                    
                }

                if (Request["Mode"] == "Print")
                {
                    //출력 구문
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Page.Response.Clear();

                        RPT.ExportToPdf(ms);

                        byte[] bR = ms.ToArray();
                        Page.Response.AddHeader("Content-Disposition", "inline; filename=" + sReportID + ".pdf");
                        Page.Response.ContentType = "application/pdf";
                        Page.Response.OutputStream.Write(bR, 0, bR.Length);
                        Page.Response.End();
                    }
                }
                else
                {
                    docViewer.OpenReport(RPT);
                }
            }
        }
    }
}