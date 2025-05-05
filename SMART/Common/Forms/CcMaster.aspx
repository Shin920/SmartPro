<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="CcMaster.aspx.cs" Inherits="Common_Forms_CcMaster" %>

<asp:Content ID="CommonContent" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            //공통팝업 상태바 표기 안함.
            v_mast_statusbar_visible = false;
        }

        function GridRowDblClick() {
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
            mastLoadingPanelHide();
        }

    </script>

    <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%"
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" OnAfterPerformCallback="Grid_AfterPerformCallback"
        KeyFieldName="CcCode;CcName" AutoGenerateColumns="false">

        <Settings ShowFooter="false" ShowGroupFooter="Hidden" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="250" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
        <SettingsPager Mode="ShowAllRecords" />
        <SettingsLoadingPanel Mode="Default" />

        <TotalSummary>
            <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="UserID"  />
        </TotalSummary>

        <ClientSideEvents EndCallback="GridEndCallback" RowDblClick="GridRowDblClick" />

        <Columns>
            <dx:GridViewDataTextColumn FieldName="CcCode" Caption="코드" Width="40%">
                <HeaderStyle HorizontalAlign="Center" /><CellStyle HorizontalAlign="Center" Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CcName" Caption="코스트센터명" Width="60%">
                <HeaderStyle HorizontalAlign="Center" /><CellStyle HorizontalAlign="Center" Wrap="False" />
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>

    <%-- 숨김필드 --%>
    <dx:ASPxTextBox ID="txtCcCode" ClientInstanceName="txtCcCode" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCcName" ClientInstanceName="txtCcName" runat="server" ClientVisible="false"></dx:ASPxTextBox>

</asp:Content>