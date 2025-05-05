using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.UI;
using SMART;
using SMART.REPORT;

public partial class Common_PrintForm_ReportPageVariable : PageBase
{
    GObj G = new GObj();

    private string sReportID { get; set; }
    private string sParam { get; set; }
    public string m_strScriptTarget = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();
        G = ((GObj)Session["G"]);

        if (!IsPostBack)
        {
            txtStatus.Text = "A";

            sReportID = CFL.GetStr(Request["RID"]);
            sParam = CFL.GetStr(Request["Param"]);
            txtMode.Text = Request["Mode"];
        }

    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        try
        {
            txtStatus.Text = "B";

            sReportID = CFL.GetStr(Request["RID"]);
            sParam = CFL.GetStr(Request["Param"]);

            //
            //sParam = sParam + "|" + txtData1.Text.Trim()
            //    + "|" + txtData2.Text.Trim()
            //    + "|" + txtData3.Text.Trim();

            if (txtData1.Text.Trim() != "") sParam += "|" + txtData1.Text.Trim();
            if (txtData2.Text.Trim() != "") sParam += "|" + txtData2.Text.Trim();
            if (txtData3.Text.Trim() != "") sParam += "|" + txtData3.Text.Trim();
            if (txtData4.Text.Trim() != "") sParam += "|" + txtData4.Text.Trim();
            if (txtData5.Text.Trim() != "") sParam += "|" + txtData5.Text.Trim();
            if (txtData6.Text.Trim() != "") sParam += "|" + txtData6.Text.Trim();
            if (txtData7.Text.Trim() != "") sParam += "|" + txtData7.Text.Trim();
            if (txtData8.Text.Trim() != "") sParam += "|" + txtData8.Text.Trim();
            if (txtData9.Text.Trim() != "") sParam += "|" + txtData9.Text.Trim();
            if (txtData10.Text.Trim() != "") sParam += "|" + txtData10.Text.Trim();
            if (txtData11.Text.Trim() != "") sParam += "|" + txtData11.Text.Trim();
            if (txtData12.Text.Trim() != "") sParam += "|" + txtData12.Text.Trim();
            if (txtData13.Text.Trim() != "") sParam += "|" + txtData13.Text.Trim();
            if (txtData14.Text.Trim() != "") sParam += "|" + txtData14.Text.Trim();
            if (txtData15.Text.Trim() != "") sParam += "|" + txtData15.Text.Trim();

            REPORT_LINK LINK = new REPORT_LINK(G.D, sReportID, sParam);
            XtraReport RPT = LINK.Classification();

            // REPORT_LINK에서 DataSorce가 아닌 리포트로 여러 PAGE를 받아와도 1장으로 바뀌기에 수정.. 
            if (RPT.Pages.Count <= 1)
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
        catch (Exception ex)
        {
            txtErrMessage.Text = ex.Message;
        }
    }
}