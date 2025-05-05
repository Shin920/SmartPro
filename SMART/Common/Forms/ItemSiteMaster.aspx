<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="ItemSiteMaster.aspx.cs" Inherits="Common_Forms_ItemSiteMaster" %>
<asp:Content ID="CommonContent" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            //공통팝업 상태바 표기 안함.
            v_mast_statusbar_visible = false;
        }

        function btnSearch_Click() {
            mastLoadingPanelShow();
            Grid.PerformCallback();
        }

        function pupGroupClick() {
            v_mast_pop_callback = callbackItemGroup
            mastPopItemGroupShow("");
        }

        function callbackItemGroup() {
            if (v_mast_pop_code != "") {
                txtItemGroup.SetText(v_mast_pop_code);
                txtGroupName.SetText(v_mast_pop_name);
            }
        }

        function btnChoiceClick() {
            GridRowDblClick(null, null);
        }

        function GridRowDblClick(s, e) {

            ////var strVal = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            ////if (strVal == "" || strVal == null) {
            ////    return
            ////}

            ////var sarVal = strVal.split("|");

            ////v_mast_pop_code = sarVal[0].trim();
            ////v_mast_pop_name = sarVal[1].trim();
            ////v_mast_pop_data1 = sarVal[2].trim();
            ////v_mast_pop_data2 = sarVal[3].trim();

            //Grid.GetRowValues(Grid.GetFocusedRowIndex(), "ItemCode;ItemName;ItemSpec;ItemUnit", GetGridRowValues)  
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

        function GetGridRowValues(values) {

            if (values == "" || values == null) {
                return
            }
            
            var iConditionLength = values.length;
            switch (iConditionLength) {
                case 1:
                    v_mast_pop_code = values[0];
                    break;
                case 2:
                    v_mast_pop_code = values[0];
                    v_mast_pop_name = values[1];
                    break;
                case 3:
                    v_mast_pop_code = values[0];
                    v_mast_pop_name = values[1];
                    v_mast_pop_data1 = values[2];
                    break;
                case 4:
                    v_mast_pop_code = values[0];
                    v_mast_pop_name = values[1];
                    v_mast_pop_data1 = values[2];
                    v_mast_pop_data2 = values[3];
                    break;
                case 5:
                    v_mast_pop_code = values[0];
                    v_mast_pop_name = values[1];
                    v_mast_pop_data1 = values[2];
                    v_mast_pop_data2 = values[3];
                    v_mast_pop_data3 = values[4];
                    break;
                case 6:
                    v_mast_pop_code = values[0];
                    v_mast_pop_name = values[1];
                    v_mast_pop_data1 = values[2];
                    v_mast_pop_data2 = values[3];
                    v_mast_pop_data3 = values[4];
                    v_mast_pop_data4 = values[5];
                    break;
                case 7:
                    v_mast_pop_code = values[0];
                    v_mast_pop_name = values[1];
                    v_mast_pop_data1 = values[2];
                    v_mast_pop_data2 = values[3];
                    v_mast_pop_data3 = values[4];
                    v_mast_pop_data4 = values[5];
                    v_mast_pop_data5 = values[6];
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

        function btnNew_Click() {
            ddlItemAcc.SetSelectedIndex(0);
            txtItemGroup.SetText("");
            txtGroupName.SetText("");
            txtItemCode.SetText("");
            txtItemName.SetText("");
            txtItemSpec.SetText("");
        }

    </script>



    <table style="width: 100%;" class="smart_table_head"></table>
    <table style="width: 100%;" class="smart_table_default">
        <colgroup>
            <col width="15%" />
            <col width="30%" />
            <col width="15%" />
            <col width="40%" />
        </colgroup>
        <tr>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblItemAcc" runat="server" Text="품목계정"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default">
                <dx:ASPxComboBox ID="ddlItemAcc" ClientInstanceName="ddlItemAcc" runat="server" Width="100%"
                    ValueField="ItemAccCode" TextField="ItemAccName" ValueType="System.String"></dx:ASPxComboBox>
            </td>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblItemGroup" runat="server" Text="품목군"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default">
                <table style="width: 100%;">
                    <colgroup>
                        <col width="35%" />
                        <col width="1" />
                        <col width="65%" />
                        <col width="1" />
                        <col width="25" />
                    </colgroup>
                    <tr>
                        <td>
                            <dx:ASPxTextBox ID="txtItemGroup" ClientInstanceName="txtItemGroup" runat="server" Width="100%"></dx:ASPxTextBox>
                        </td>
                        <td>&nbsp</td>
                        <td>
                            <dx:ASPxTextBox ID="txtGroupName" ClientInstanceName="txtGroupName" runat="server" Width="100%"></dx:ASPxTextBox>
                        </td>
                        <td>&nbsp</td>
                        <td>
                            <dx:ASPxImage ID="pupGroup" ClientInstanceName="pupGroup" runat="server" Cursor="pointer" ImageUrl="/SmartPro/Contents/Images/popup.gif">
                                <ClientSideEvents Click="pupGroupClick" />
                            </dx:ASPxImage>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblItemCode" runat="server" Text="품목코드"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default">
                <dx:ASPxTextBox ID="txtItemCode" ClientInstanceName="txtItemCode" runat="server" Width="100%"></dx:ASPxTextBox>
            </td>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblItemName" runat="server" Text="품목명"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default">
                <dx:ASPxTextBox ID="txtItemName" ClientInstanceName="txtItemName" runat="server" Width="100%"></dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblItemSpec" runat="server" Text="품목규격"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default" colspan="3">
                <table width="100%">
                    <colgroup>
                        <col width="60%" />
                        <col width="1" />
                        <col width="40%" />
                    </colgroup>
                    <tr>
                        <td>
                            <dx:ASPxTextBox ID="txtItemSpec" ClientInstanceName="txtItemSpec" runat="server" Width="100%"></dx:ASPxTextBox>
                        </td>
                        <td></td>
                        <td align="right">
                            <dx:ASPxButton ID="btnNew" ClientInstanceName="btnNew" runat="server" AutoPostBack="false" Text="Refresh" Width="80" UseSubmitBehavior="false">
                                <Image IconID="actions_new_16x16office2013"></Image>
                                <ClientSideEvents Click="btnNew_Click" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="검색" AutoPostBack="false">
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

    <%--<dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%" AutoGenerateColumns="false"
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" KeyFieldName="ItemCode;ItemName" 
        OnAfterPerformCallback="Grid_AfterPerformCallback" OnCustomColumnDisplayText="Grid_CustomColumnDisplayText">

        <Styles Header-HorizontalAlign="Center" Cell-Wrap="False" />
        <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="305" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
        <SettingsPager Mode="EndlessPaging" PageSize="50" />
        <SettingsLoadingPanel Mode="Disabled" />

        <TotalSummary>
            <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="ItemCode" />
        </TotalSummary>

        <ClientSideEvents EndCallback="GridEndCallback" RowDblClick="GridRowDblClick" />

        <Columns>
            <dx:GridViewDataColumn Caption="No" ReadOnly="true" Width="45" CellStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>

            <dx:GridViewDataTextColumn FieldName="ItemCode" Caption="품목코드" Width="20%" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ItemName" Caption="품목명" Width="35%" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ItemSpec" Caption="규격" Width="35%" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ItemUnit" Caption="단위" Width="10%" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>--%>
    <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%"
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" OnAfterPerformCallback="Grid_AfterPerformCallback"
        OnCustomColumnDisplayText="Grid_CustomColumnDisplayText" AutoGenerateColumns="false">

        <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="280" />
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
    <%-- Request 데이터 --%>
    <dx:ASPxTextBox ID="txtParam" ClientInstanceName="txtParam" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCondition" ClientInstanceName="txtCondition" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCond1" ClientInstanceName="txtCond1" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCond2" ClientInstanceName="txtCond2" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCond3" ClientInstanceName="txtCond3" runat="server" ClientVisible="false"></dx:ASPxTextBox>

</asp:Content>