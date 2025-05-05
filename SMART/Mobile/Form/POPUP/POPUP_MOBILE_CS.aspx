<%@ Page Language="C#" MasterPageFile="~/Master/Page.master" AutoEventWireup="true" CodeFile="POPUP_MOBILE_CS.aspx.cs" Inherits="Form_POPUP_POPUP_MOBILE_CS" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">
        function PageInit() {
            jqUpdateSizeH();
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();

            var h = $(window).height();
            var h_static = 100;  // 상단 제외 영역 높이
            var h_min = 20;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);
            } else {
                Grid.SetHeight(h_min);
            }
        };

        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function GridFocusedRowChanged(s, e) {

            var strVal = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            if (strVal == "" || strVal == null) {
                return
            }

            var sarVal = strVal.split("|");

            v_mast_pop_code = sarVal[0].trim();
            v_mast_pop_name = sarVal[1].trim();

            mastCloseForm();
        }

        function GridEndCallback(s, e) {

            //var strMsg = s.cp_ret_message;
            //var strFlag = s.cp_ret_flag;

            //if (strFlag != "00") {
            //    alert(strMsg);
            //}
            //mastLoadingPanelHide();
        }

        function GridCallbackError() {

        }

        function btnSearch_Click() {
            Grid.PerformCallback();
        }

    </script>

    <table class="mobile_table">
        <colgroup>
            <col style="width: 20%;" />
            <col style="width: 50%;" />
            <col style="width: 30%;" />
        </colgroup>
        <tr>
            <th class="mobile_th">
                <dx:ASPxLabel ID="lblCsName" ClientInstanceName="lblCsName" runat="server" Text="현장명" Font-Bold="true"></dx:ASPxLabel>
            </th>
            <td class="mobile_td">
                <dx:ASPxTextBox ID="txtCsName" ClientInstanceName="txtCsName" runat="server" Width="100%"></dx:ASPxTextBox>
            </td>
            <td class="mobile_td">
                <dx:ASPxButton ID="btnSearch" runat="server" CssClass="btn" Native="true" Text="검색" AutoPostBack="false" UseSubmitBehavior="false">
                    <ClientSideEvents Click="btnSearch_Click" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>

    <table style="height: 7px;"></table>

    <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%" KeyFieldName="CsCode;CsName"
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" OnCustomErrorText="Grid_CustomErrorText"
        OnAfterPerformCallback="Grid_AfterPerformCallback" OnCustomColumnDisplayText="Grid_CustomColumnDisplayText">

        <Styles Header-HorizontalAlign="Center" Cell-Wrap="False" />
        <Settings ShowFooter="false" ShowGroupFooter="Hidden" ShowStatusBar="Hidden" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
        <SettingsPager Mode="EndlessPaging" PageSize="100" />
        <SettingsLoadingPanel Mode="Disabled" />

        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="InoutQty" SummaryType="Sum" DisplayFormat="N0" />
        </TotalSummary>

        <ClientSideEvents EndCallback="GridEndCallback" CallbackError="GridCallbackError" FocusedRowChanged="GridFocusedRowChanged" />

        <Columns>
            
            <dx:GridViewDataTextColumn FieldName="CsCode"  Caption="코드" Width="33%" CellStyle-HorizontalAlign="Center">
                <HeaderStyle BackColor="#f0f3ea" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CsName"  Caption="거래처명" Width="67%" CellStyle-HorizontalAlign="Center">
                <HeaderStyle BackColor="#f0f3ea" />
            </dx:GridViewDataTextColumn>

        </Columns>


    </dx:ASPxGridView>

    <%--hidden--%>
    <dx:ASPxHiddenField ID="hidLang" ClientInstanceName="hidLang" runat="server"></dx:ASPxHiddenField>
    
</asp:Content>