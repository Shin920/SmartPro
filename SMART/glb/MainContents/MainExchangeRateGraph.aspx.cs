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

public partial class glb_MainContents_MainExchangeRateGraph : PageBase
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

        int iMonth = CFL.Toi(rbMonth.Value);

        string strBDate = DateTime.Today.AddYears(-1).AddMonths(iMonth * -1).ToString("yyyyMM");
        string strEDate = DateTime.Today.AddYears(-1).ToString("yyyyMM");

        chart.Width = CFL.Toi(txtWidth.Text.Trim()) - 10;

        //
        DataSet ds = Obj.GetExchangeRate(G.D, strBDate, strEDate);

        if (ds.Tables[0].Rows.Count > 0)
        {
            GraphSetting(ds);
        }
    }

    void GraphSetting(DataSet ds)
    {
        chart.Series.Clear();
        
        Series sUSD = new Series("USD", ViewType.Spline);
        Series sEUR = new Series("EUR", ViewType.Spline);
        Series sJPY = new Series("JPY", ViewType.Spline);
        
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (CFL.GetStr(ds.Tables[0].Rows[i]["CurrCode"]) == "USD")
            {
                sUSD.Points.AddPoint(CFL.GetStr(ds.Tables[0].Rows[i]["ExchDate"]), CFL.Todb(ds.Tables[0].Rows[i]["ExchRate1"]));
            }

            if (CFL.GetStr(ds.Tables[0].Rows[i]["CurrCode"]) == "EUR")
            {
                sEUR.Points.AddPoint(CFL.GetStr(ds.Tables[0].Rows[i]["ExchDate"]), CFL.Todb(ds.Tables[0].Rows[i]["ExchRate1"]));
            }

            if (CFL.GetStr(ds.Tables[0].Rows[i]["CurrCode"]) == "JPY")
            {
                sJPY.Points.AddPoint(CFL.GetStr(ds.Tables[0].Rows[i]["ExchDate"]), CFL.Todb(ds.Tables[0].Rows[i]["ExchRate1"]));
            }
        }

        chart.Series.Add(sUSD);
        chart.Series.Add(sEUR);
        chart.Series.Add(sJPY);

        ((PointSeriesView)chart.Series[0].View).PointMarkerOptions.Size = 1;
        ((PointSeriesView)chart.Series[1].View).PointMarkerOptions.Size = 1;
        ((PointSeriesView)chart.Series[2].View).PointMarkerOptions.Size = 1;

        chart.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
        chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
        chart.Legend.MarkerSize = new System.Drawing.Size(7, 7);
        chart.Legend.Direction = LegendDirection.LeftToRight;
        chart.Legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
        chart.Legend.Padding.All = 5;

        if (ds.Tables[1].Rows.Count > 0)
        {
            XYDiagram diagram = (XYDiagram)chart.Diagram;
            diagram.AxisY.WholeRange.Auto = false;
            diagram.AxisY.WholeRange.SetMinMaxValues(CFL.Toi(ds.Tables[1].Rows[0]["MIN"]) - 250, CFL.Toi(ds.Tables[1].Rows[0]["MAX"]) + 250);
        }
    }

}