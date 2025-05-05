<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="CreateNoPopup.aspx.cs" Inherits="Common_Forms_CreateNoPopup" %>
<asp:Content ID="CommonContent" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            //공통팝업 상태바 표기 안함.
            v_mast_statusbar_visible = false;

            //조회
            btnSearchClick();
        }

        function btnSearchClick() {
            mastLoadingPanelShow();
            Grid.PerformCallback();
        }

        function btnChoiceClick() {
            GridRowDblClick();
        }

        function GridRowDblClick() {

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
                 case 13:
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
                    v_mast_pop_data11 = sarVal[12];
                    break;
                case 14:
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
                    v_mast_pop_data11 = sarVal[12];
                    v_mast_pop_data12 = sarVal[13];
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

    <table style="width: 100%" class="smart_table_head"></table>
    <table style="width: 100%" class="smart_table_default">
        <colgroup>
            <col style="width: 25%"/>
            <col style="width: 75%"/>
        </colgroup>
        <tr>
            <th class="smart_table_th_center">
                <dx:ASPxLabel ID="lblDate" runat="server" Text="검색구분"></dx:ASPxLabel>
            </th>
            <td class="smart_table_td_default">
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 22%;"/>
                        <col style="width: 1px;"/>
                        <col style="width: 78%;"/>
                    </colgroup>
                    <tr>
                        <td>
                            <dx:ASPxDateEdit ID="deBDate" ClientInstanceName="deBDate" runat="server" Width="110" UseMaskBehavior="true"></dx:ASPxDateEdit>
                        </td>
                        <td>&nbsp~&nbsp</td>
                        <td>
                            <dx:ASPxDateEdit ID="deEDate" ClientInstanceName="deEDate" runat="server" Width="110" UseMaskBehavior="true"></dx:ASPxDateEdit>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="smart_table_td_default">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Paddings-Padding="0" AutoPostBack="false">
                                <Image IconID="actions_search_16x16devav"></Image>
                                <ClientSideEvents Click="btnSearchClick" />
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="btnChoice" ClientInstanceName="btnChoice" runat="server" AutoPostBack="false">
                                <Image SpriteProperties-CssClass="ChoiceButton"></Image>
                                <ClientSideEvents Click="btnChoiceClick" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>                
            </td>
        </tr>
    </table>
    <table style="width: 100%" class="smart_table_foot"></table>

    <table style="height: 7px;"></table>

    <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%"
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" OnAfterPerformCallback="Grid_AfterPerformCallback"
        OnCustomColumnDisplayText="Grid_CustomColumnDisplayText" AutoGenerateColumns="false">

        <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="180" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="false" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
        <SettingsPager Mode="ShowAllRecords" />
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

    <dx:ASPxTextBox ID="txtGubun" ClientInstanceName="txtGubun" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtGubun2" ClientInstanceName="txtGubun2" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtFormID" ClientInstanceName="txtFormID" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCondition" ClientInstanceName="txtCondition" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtSiteCode" ClientInstanceName="txtSiteCode" runat="server" ClientVisible="false"></dx:ASPxTextBox>

</asp:Content>
