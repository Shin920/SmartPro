<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="CommonPopup.aspx.cs" Inherits="Common_Forms_CommonPopup" %>
<asp:Content ID="CommonContent" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            //공통팝업 상태바 표기 안함.
            v_mast_statusbar_visible = false;

            jqUpdateSizeH();

            //조회
            //btnSearchClick();
        }

         function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 40;  // 상단 제외 영역 높이
            var h_min = 25;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);
            } else {
                Grid.SetHeight(h_min);
            }
        };
        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function btnSearchClick() {
            mastLoadingPanelShow();
            Grid.PerformCallback();
        }

        function btnChoiceClick() {
            GridRowDblClick(null, null);
        }

        function GridRowDblClick(s, e) {
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
            }

            mastCloseForm();
        }

        function GridEndCallback(s, e) {

            //var strMsg = s.cp_ret_message;
            //var strFlag = s.cp_ret_flag;

            //if (strFlag != "00") {
            //    mastPageFooterErr(strMsg);
            //}
            mastLoadingPanelHide();
        }

    </script>

    <table style="width: 100%;">
        <tr>
            <td>
                <dx:ASPxLabel ID="lblTitle" ClientInstanceName="lblTitle" runat="server" Text="" ClientVisible="false" ></dx:ASPxLabel>
            </td>
            <td align="right">
                <dx:ASPxButton ID="btnChoice" ClientInstanceName="btnChoice" runat="server" AutoPostBack="false">
                    <Image SpriteProperties-CssClass="ChoiceButton"></Image>
                    <ClientSideEvents Click="btnChoiceClick" />
                </dx:ASPxButton>
            </td>
        </tr>
        </table>

    <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%" 
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" OnAfterPerformCallback="Grid_AfterPerformCallback"
        OnCustomColumnDisplayText="Grid_CustomColumnDisplayText" AutoGenerateColumns="false">

        <Settings ShowFooter="false" ShowGroupFooter="Hidden" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="255" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
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
    
    <%-- 숨김필드 --%>
    <dx:ASPxTextBox ID="txtWhere" ClientInstanceName="txtWhere" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtGubun" ClientInstanceName="txtGubun" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCode" ClientInstanceName="txtCode" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtName" ClientInstanceName="txtName" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCondition" ClientInstanceName="txtCondition" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtCodeID" ClientInstanceName="txtCodeID" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxHiddenField ID="hidLang" ClientInstanceName="hidLang" runat="server"></dx:ASPxHiddenField> 
    
</asp:Content>