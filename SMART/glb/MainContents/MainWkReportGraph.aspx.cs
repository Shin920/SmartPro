using DevExpress.XtraCharts;
using SMART;
using SMART.OM.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class glb_MainContents_MainWkReportGraph : PageBase
{
    GObj G = new GObj();
    protected void Page_Load(object sender, EventArgs e)
    {
        this.basePageLoad();

        G = (GObj)Session["G"];

        if (!IsPostBack)
        {

        }
    }

    protected void chart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
    {
        SMART.COMMON.DS.MainContents Obj = new SMART.COMMON.DS.MainContents();

        int iDays = CFL.Toi(rbDays.Value);

        string strBDate = DateTime.Today.AddMonths(iDays * -1).ToString("yyyyMMdd");
        string strEDate = DateTime.Today.ToString("yyyyMMdd");

        chart.Width = CFL.Toi(txtWidth.Text.Trim()) - 10;

        DataSet ds = Obj.GetWorkReport(G.D, strBDate, strEDate);

        if (ds.Tables[0].Rows.Count > 0)
        {
            GraphSetting(ds);
        }
        else
        {
            chart.Series.Clear();
        }
    }

    void GraphSetting(DataSet ds)
    {
        chart.Series.Clear();

        Series sSeries = new Series("생산실적", ViewType.Line);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sSeries.Points.AddPoint(CFL.GetStr(ds.Tables[0].Rows[i]["ReportDate"]), CFL.Todb(ds.Tables[0].Rows[i]["GoodQty"]));
        }

        chart.Series.Add(sSeries);

        ((PointSeriesView)chart.Series[0].View).PointMarkerOptions.Size = 1;

        chart.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
        chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
        chart.Legend.MarkerSize = new System.Drawing.Size(7, 7);
        chart.Legend.Direction = LegendDirection.LeftToRight;
        chart.Legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
        chart.Legend.Padding.All = 5;
    }

}