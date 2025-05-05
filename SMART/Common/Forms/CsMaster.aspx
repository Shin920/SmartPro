<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="CsMaster.aspx.cs" Inherits="Common_Forms_CsMaster" %>
<asp:Content ID="CommonContent" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        //공통팝업 상태바 표기 안함.
        v_mast_statusbar_visible = false;

        function init() {
            jqUpdateSizeH();
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 105;  // 상단 제외 영역 높이
            var h_min = 70;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);
            } else {
                Grid.SetHeight(h_min);
            }
        };
        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function btnSearch_Click() {
            mastLoadingPanelShow();
            Grid.PerformCallback();
        }

        function btnChoiceClick() {
            GridRowDblClick(null, null);
        }

        function GridRowDblClick(s, e) {

            //var strVal = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            //if (strVal == "" || strVal == null) {
            //    return
            //}

            //var sarVal = strVal.split("|");

            //v_mast_pop_code = sarVal[0].trim();
            //v_mast_pop_name = sarVal[1].trim();
            //v_mast_pop_data1 = sarVal[2].trim();
            //v_mast_pop_data2 = sarVal[3].trim();
            //v_mast_pop_data3 = sarVal[4].trim();
            var strVal = Grid.GetRowKey(Grid.GetFocusedRowIndex());

            if (strVal == "" || strVal == null) {
                return
            }

            var iConditionLength = txtCondition.GetText().split("/").length;
            var sarVal = strVal.split("|");
            switch (iConditionLength) {
                case 1:
                    v_mast_pop_code = sarVal[0];
                    break;
                case 2:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    break;
                case 3:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    break;
                case 4:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    v_mast_pop_data2 = sarVal[3];
                    break;
                case 5:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    v_mast_pop_data2 = sarVal[3];
                    v_mast_pop_data3 = sarVal[4];
                    break;
                case 6:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    v_mast_pop_data2 = sarVal[3];
                    v_mast_pop_data3 = sarVal[4];
                    v_mast_pop_data4 = sarVal[5];
                    break;

                case 7:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    v_mast_pop_data2 = sarVal[3];
                    v_mast_pop_data3 = sarVal[4];
                    v_mast_pop_data4 = sarVal[5];
                    v_mast_pop_data5 = sarVal[6];
                    break;

                case 8:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    v_mast_pop_data2 = sarVal[3];
                    v_mast_pop_data3 = sarVal[4];
                    v_mast_pop_data4 = sarVal[5];
                    v_mast_pop_data5 = sarVal[6];
                    v_mast_pop_data6 = sarVal[7];
                    break;
                case 9:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    v_mast_pop_data2 = sarVal[3];
                    v_mast_pop_data3 = sarVal[4];
                    v_mast_pop_data4 = sarVal[5];
                    v_mast_pop_data5 = sarVal[6];
                    v_mast_pop_data6 = sarVal[7];
                    v_mast_pop_data7 = sarVal[8];
                    break;
                case 10:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    v_mast_pop_data2 = sarVal[3];
                    v_mast_pop_data3 = sarVal[4];
                    v_mast_pop_data4 = sarVal[5];
                    v_mast_pop_data5 = sarVal[6];
                    v_mast_pop_data6 = sarVal[7];
                    v_mast_pop_data7 = sarVal[8];
                    v_mast_pop_data8 = sarVal[9];
                    break;
                case 11:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    v_mast_pop_data2 = sarVal[3];
                    v_mast_pop_data3 = sarVal[4];
                    v_mast_pop_data4 = sarVal[5];
                    v_mast_pop_data5 = sarVal[6];
                    v_mast_pop_data6 = sarVal[7];
                    v_mast_pop_data7 = sarVal[8];
                    v_mast_pop_data8 = sarVal[9];
                    v_mast_pop_data9 = sarVal[10];
                    break;
                case 12:
                    v_mast_pop_code = sarVal[0];
                    v_mast_pop_name = sarVal[1];
                    v_mast_pop_data1 = sarVal[2];
                    v_mast_pop_data2 = sarVal[3];
                    v_mast_pop_data3 = sarVal[4];
                    v_mast_pop_data4 = sarVal[5];
                    v_mast_pop_data5 = sarVal[6];
                    v_mast_pop_data6 = sarVal[7];
                    v_mast_pop_data7 = sarVal[8];
                    v_mast_pop_data8 = sarVal[9];
                    v_mast_pop_data9 = sarVal[10];
                    v_mast_pop_data10 = sarVal[11];
                    break;
            }

            mastCloseForm();
        }

        function GridEndCallback(s, e) {

            //var strMsg = s.cp_ret_message;
            //var strFlag = s.cp_ret_flag;

            //if (strFlag != "00") {
            //    alert(strMsg);
            //}
            mastLoadingPanelHide();
        }

    </script>

    <table style="width: 100%;" class="smart_table_head"></table>
    <table style="width: 100%;">
        <colgroup>
            <col style="width: 15%;" />
            <col style="width: 35%;" />
            <col style="width: 15%;" />
            <col style="width: 35%;" />
        </colgroup>
        <tr>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblCsCode" runat="server" Text="거래처코드"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default">
                <dx:ASPxTextBox ID="txtCsCode" ClientInstanceName="txtCsCode" runat="server" Width="100%"></dx:ASPxTextBox>
            </td>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblCsName" runat="server" Text="거래처명"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default">
                <dx:ASPxTextBox ID="txtCsName" ClientInstanceName="txtCsName" runat="server" Width="100%"></dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblCsType" runat="server" Text="거래처유형"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default">
                <dx:ASPxComboBox ID="ddlCsType" ClientInstanceName="ddlCsType" runat="server" Width="100%"
                    ValueField="CodeCode" TextField="CodeName" ValueType="System.String"></dx:ASPxComboBox>
            </td>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblRegion" runat="server" Text="지역"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default">
                <dx:ASPxComboBox ID="ddlRegion" ClientInstanceName="ddlRegion" runat="server" Width="100%"
                    ValueField="RegionCode" TextField="RegionName" ValueType="System.String"></dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblRegNo" runat="server" Text="사업자번호"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default" colspan="3">
                <table width="100%">
                    <colgroup>
                        <col width="67%" />
                        <col width="1" />
                        <col width="33%" />
                    </colgroup>
                    <tr>
                        <td>
                            <dx:ASPxTextBox ID="txtRegNo" ClientInstanceName="txtRegNo" runat="server" Width="100%"></dx:ASPxTextBox>
                        </td>
                        <td>&nbsp</td>
                        <td align="right">

                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="조회" AutoPostBack="false">
                                <Image IconID="actions_search_16x16devav"></Image>
                                <ClientSideEvents Click="btnSearch_Click" />
                            </dx:ASPxButton>

                            <dx:ASPxButton ID="btnChoice" ClientInstanceName="btnChoice" runat="server" Text="선택" AutoPostBack="false">
                                <Image SpriteProperties-CssClass="ChoiceButton"></Image>
                                <ClientSideEvents Click="btnChoiceClick" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 100%;" class="smart_table_foot"></table>

    <table style="height: 7px;"></table>

    <%--<dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%"
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" OnAfterPerformCallback="Grid_AfterPerformCallback"
        KeyFieldName="CsCode;CsName;CurrCode;BaseExchRatio;TaxCodeAp" AutoGenerateColumns="false" OnCustomColumnDisplayText="Grid_CustomColumnDisplayText">

        <Settings ShowFooter="true" ShowGroupFooter="Hidden" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="265" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
        <SettingsPager Mode="EndlessPaging" />
        <SettingsLoadingPanel Mode="Default" />

        <TotalSummary>
            <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="CsCode"  />
        </TotalSummary>

        <ClientSideEvents EndCallback="GridEndCallback" RowDblClick="GridRowDblClick" />

        <Columns>
            <dx:GridViewDataColumn Caption="No" ReadOnly="true" Width="45" CellStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>

            <dx:GridViewDataTextColumn FieldName="CsCode" Caption="거래처코드" Width="40%">
                <HeaderStyle HorizontalAlign="Center" /><CellStyle HorizontalAlign="Center" Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CsName" Caption="거래처명" Width="60%">
                <HeaderStyle HorizontalAlign="Center" /><CellStyle HorizontalAlign="Center" Wrap="False" />
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>--%>

    <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%"
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" OnAfterPerformCallback="Grid_AfterPerformCallback"
        OnCustomColumnDisplayText="Grid_CustomColumnDisplayText" AutoGenerateColumns="false">

        <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
        <SettingsPager Mode="ShowPager"  PageSize="100"/>
        <SettingsLoadingPanel Mode="Disabled" />

        <Styles>
            <Header HorizontalAlign="Center"></Header>
            <Cell HorizontalAlign="Center" Wrap="False"></Cell>
        </Styles>

        <ClientSideEvents EndCallback="GridEndCallback" RowDblClick="GridRowDblClick" />

        <Columns>
            <dx:GridViewDataColumn Caption="No" ReadOnly="true" Width="40"></dx:GridViewDataColumn>
        </Columns>
    </dx:ASPxGridView>
    
    <dx:ASPxTextBox ID="txtWhere" ClientInstanceName="txtWhere" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCondition" ClientInstanceName="txtCondition" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCond1" ClientInstanceName="txtCond1" runat="server" ClientVisible="false"></dx:ASPxTextBox>

</asp:Content>