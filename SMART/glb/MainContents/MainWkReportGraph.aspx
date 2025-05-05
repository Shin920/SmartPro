<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="MainWkReportGraph.aspx.cs" Inherits="glb_MainContents_MainWkReportGraph" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
   
    <script type="text/javascript">

        function init() {
            v_mast_statusbar_visible = false;
            jqUpdateSizeH();
            chart.PerformCallback();
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            var w = $(window).width();
            var h = $(window).height();

            txtWidth.SetText(w);
            txtHeight.SetText(h);
        };
        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function rbDaysChanged() {
            chart.PerformCallback();
        }

    </script>

    <div class="frame_title">
        <span>생산정보</span>
    </div>

    <table style="height: 3px;"></table>

    <div id="divGrp" style="width: 100%; background-color: white; border: 1px solid gray;" runat="server">
        <dx:WebChartControl ID="chart" ClientInstanceName="chart" runat="server" Height="180" Theme="Aqua" OnCustomCallback="chart_CustomCallback">
            <BorderOptions Visibility="False" />
        </dx:WebChartControl>
    </div>

    <dx:ASPxRadioButtonList ID="rbDays" ClientInstanceName="rbDays" runat="server" RepeatColumns="3" Border-BorderWidth="0" ItemSpacing="0" Width="100%">
        <ClientSideEvents SelectedIndexChanged="rbDaysChanged" />
        <Items>
            <dx:ListEditItem Text="3개월" Value="3" Selected="true" />
            <dx:ListEditItem Text="6개월" Value="6" />
            <dx:ListEditItem Text="1년" Value="12" />
        </Items>
    </dx:ASPxRadioButtonList>

    <dx:ASPxTextBox ID="txtWidth" ClientInstanceName="txtWidth" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtHeight" ClientInstanceName="txtHeight" runat="server" ClientVisible="false" />

</asp:Content>