<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="SCMAnnounceList.aspx.cs" Inherits="glb_SiteBoard_SCM_Announce_SCMAnnounceList" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">


    <script type="text/javascript">

        function init() {
            jqUpdateSizeH();
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 110;  // 상단 제외 영역 높이
            var h_min = 70;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);
            } else {
                Grid.SetHeight(h_min);
            }
        };
        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function btnNew_Click() {
            v_mast_pop_callback = callbackMaster
            mastPopFormShow("SCM 공지사항 등록", "/SmartPro/glb/SiteBoard/SCM_Announce/SCMAnnounceMng.aspx?Action=Load", 1250, 625);
        }

        function btnSearch_Click() {
            Grid.PerformCallback();
        }

        function btnOpen_Click() {
            GridDbClick(null, null);
        }

        function GridFocusedRowChanged(s, e) {
            //포커스가 선택될때 기존 히스토리 삭제하는 코드.
            Grid.SelectAllRowsOnPage(false);
        }

        function GridDbClick(s, e) {

            var sID = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            if (sID != null) {

                var srID = sID.split("|");

                v_mast_pop_callback = callbackMaster
                mastPopFormShow("SCM 공지사항 수정", "/SmartPro/glb/SiteBoard/SCM_Announce/SCMAnnounceMng.aspx?Action=Load&ID=" + srID[0] + "&NUM=" + srID[1], 1250, 625);
            }
        }

        function callbackMaster() {
            btnSearch_Click();
        }

        function GridEndCallback(s, e) {

            Grid.SetFocusedRowIndex(-1);

            var vFlag = s.cp_ret_flag.toString();
            var vMsg = s.cp_ret_message.toString();
            var vJob = s.cp_ret_job.toString();

            if (vFlag == "N") {
                mastPageFooterErr(vMsg);
            }

            mastLoadingPanelHide();
        }

    </script>

    <table style="height: 5px;"></table>

    <table style="width :100%;">
        <tr>
            <td align="right">
                <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" AutoPostBack="false" Text="조회" Width="80">
                    <Image IconID="actions_search_16x16devav"></Image>
                    <ClientSideEvents Click="btnSearch_Click" />
                </dx:ASPxButton>
                <img src="/SmartPro/Contents/Images/dot_ver.gif" />
                <dx:ASPxButton ID="btnNew" ClientInstanceName="btnNew" runat="server" AutoPostBack="false" Text="등록" Width="80">
                    <Image IconID="actions_new_16x16office2013"></Image>
                    <ClientSideEvents Click="btnNew_Click" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnOpen" ClientInstanceName="btnOpen" runat="server" AutoPostBack="false" Text="상세보기" Width="80">
                    <Image IconID="dashboards_enablesearch_svg_gray_16x16"></Image>
                    <ClientSideEvents Click="btnOpen_Click" />
                </dx:ASPxButton>
                <img src="/SmartPro/Contents/Images/dot_ver.gif" />
                <dx:ASPxButton ID="btnExcel" runat="server" Text="Excel" Width="80" OnClick="btnExcel_Click">
                    <Image IconID="export_exporttoxls_16x16office2013"></Image>
                </dx:ASPxButton>
            </td>
        </tr>
    </table>

    <table style="height: 10px;"></table>

    <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%"
        AutoGenerateColumns="false" KeyboardSupport="true" KeyFieldName="Board_ID;Num"
        OnCustomCallback="Grid_CustomCallback" OnAfterPerformCallback="Grid_AfterPerformCallback" OnDataBinding="Grid_DataBinding">

        <Styles Header-HorizontalAlign="Center" Cell-Wrap="False" />
        <Settings VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" ColumnResizeMode="Control" AllowDragDrop="false" />
        <SettingsPager PageSize="50" AlwaysShowPager="true" />

        <ClientSideEvents FocusedRowChanged="GridFocusedRowChanged" RowDblClick="GridDbClick" EndCallback="GridEndCallback" />

        <Columns>
            <dx:GridViewDataTextColumn FieldName="Board_ID" Caption="ID" Visible="false" />
            <dx:GridViewDataColumn FieldName="Importance" Caption="중요" VisibleIndex="1" Width="35" Visible="true"
                CellStyle-HorizontalAlign="Right">
                <DataItemTemplate>
                    <img runat="server" src='<%# GetIcon(Eval("Importance")) %>' width="12" />
                </DataItemTemplate>
            </dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn FieldName="Num" Caption="순번" VisibleIndex="2" Width="50" CellStyle-HorizontalAlign="Center" />
            <dx:GridViewDataTextColumn FieldName="Title" Caption="제목" VisibleIndex="3" Width="100%" />
            <dx:GridViewDataTextColumn FieldName="Readnum" Caption="조회수" VisibleIndex="4" Width="50" />
            <dx:GridViewDataTextColumn FieldName="Writeday" Caption="작성일" VisibleIndex="5" Width="145" CellStyle-HorizontalAlign="Center" />
            <dx:GridViewDataTextColumn FieldName="Writer" Caption="작성자" VisibleIndex="6" Width="120" CellStyle-HorizontalAlign="Center" />
        </Columns>
    </dx:ASPxGridView>

    <dx:ASPxGridViewExporter ID="GridExport" runat="server" GridViewID="Grid"></dx:ASPxGridViewExporter>
    <dx:ASPxHiddenField ID="hidLang" ClientInstanceName="hidLang" runat="server"></dx:ASPxHiddenField>

</asp:Content>