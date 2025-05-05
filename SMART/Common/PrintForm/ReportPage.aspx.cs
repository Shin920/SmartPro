using DevExpress.XtraReports.UI;
using SMART;
using SMART.REPORT;
using System;
using System.IO;
using System.Web.UI;

public partial class Common_PrintForm_ReportPage : PageBase
{
    GObj G = new GObj();

    private string sReportID { get; set; }
    private string sParam { get; set; }

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

                // REPORT_LINK에서 DataSorce가 아닌 리포트로 여러 PAGE를 받아와도 1장으로 바뀌기에 수정.. 
                if (RPT.Pages.Count < 1)
                {
                    RPT.CreateDocument();
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
                else if (Request["Mode"] == "Excel")
                {
                    //출력 구문
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Page.Response.Clear();

                        RPT.ExportToXlsx(ms);

                        byte[] bR = ms.ToArray();
                        Page.Response.AddHeader("Content-Disposition", "inline; filename=" + sReportID + ".xlsx");
                        Page.Response.ContentType = "application/xlsx";
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