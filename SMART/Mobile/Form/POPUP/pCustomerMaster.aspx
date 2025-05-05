<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Page.master" AutoEventWireup="true" CodeFile="pCustomerMaster.aspx.cs" Inherits="Form_POPUP_pCustomerMaster" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" Runat="Server">

    <script type="text/javascript">
        function PageInit() {
            AdjustSize();

            Grid.PerformCallback();
        }

        function AdjustSize() {
            var height = Math.max(0, document.documentElement.clientHeight);
            Grid.SetHeight(height - 30);
        }

        function GridFocusedRowChanged() {
            var sGridRowKey = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            if (sGridRowKey != null) {

                var sarGridData = sGridRowKey.split("|");

                v_mast_pop_status = "Y";
                v_mast_pop_code = sarGridData[0];
                v_mast_pop_name = sarGridData[1];

                //팝업창 닫기
                mastCloseForm();

            }
        }

    </script>

    <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%" 
        Styles-Cell-Wrap="False" Styles-Header-HorizontalAlign="Center" KeyFieldName="CsCode;CsName"
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding">

        <Settings VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" ColumnResizeMode="Control" AllowDragDrop="false" />
        <SettingsPager Mode="ShowAllRecords" />

        <%-- 선택해야 하는 팝업의 Padding 값을 넣어 선택하기 쉽게 만든다. --%>
        <Styles>
            <Cell HorizontalAlign="Center">
                <Paddings PaddingTop="5" PaddingBottom="5" />
            </Cell>
        </Styles>

        <ClientSideEvents FocusedRowChanged="GridFocusedRowChanged" />

        <Columns>
            <dx:GridViewDataTextColumn FieldName="CsCode" Caption="거래처" Width="30%" />
            <dx:GridViewDataTextColumn FieldName="CsName" Caption="거래처명" Width="70%" />

        </Columns>
    </dx:ASPxGridView>

    <dx:ASPxTextBox ID="txtDate" runat="server" ClientVisible="false" />

</asp:Content>

