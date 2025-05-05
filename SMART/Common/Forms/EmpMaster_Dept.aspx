<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="EmpMaster_Dept.aspx.cs" Inherits="Common_Forms_EmpMaster_Dept" %>

<asp:Content ID="CommonContent" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            //공통팝업 상태바 표기 안함.
            v_mast_statusbar_visible = false;
        }

        function GridRowDblClick(s, e) {

            var strVal = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            if (strVal == "" || strVal == null) {
                return
            }

            var sarVal = strVal.split("|");
            
            v_mast_pop_code = sarVal[0].trim();
            v_mast_pop_name = sarVal[1].trim();
            v_mast_pop_data1 = sarVal[2].trim();
            v_mast_pop_data2 = sarVal[3].trim();

            mastCloseForm();
        }

        function GridEndCallback(s, e) {

            var strMsg = s.cp_ret_message;
            var strFlag = s.cp_ret_flag;

            if (strFlag != "00") {
                alert(strMsg);
            }
        }

    </script>


    <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%"
        OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" OnAfterPerformCallback="Grid_AfterPerformCallback"
        KeyFieldName="EmpCode;EmpName;DefaultDept;DeptName" AutoGenerateColumns="false">

        <Settings ShowFooter="false" ShowGroupFooter="Hidden" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="285" />
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
        <SettingsPager Mode="ShowAllRecords" />
        <SettingsLoadingPanel Mode="Default" />

        <TotalSummary>
            <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="EmpCode"  />
        </TotalSummary>

        <ClientSideEvents EndCallback="GridEndCallback" RowDblClick="GridRowDblClick" />

        <Columns>
            <dx:GridViewDataTextColumn FieldName="EmpCode" Caption="사원코드" Width="20%">
                <HeaderStyle HorizontalAlign="Center" /><CellStyle HorizontalAlign="Center" Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EmpName" Caption="사원명" Width="25%">
                <HeaderStyle HorizontalAlign="Center" /><CellStyle HorizontalAlign="Center" Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DefaultDept" Caption="부서코드" Width="20%">
                <HeaderStyle HorizontalAlign="Center" /><CellStyle HorizontalAlign="Center" Wrap="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DeptName" Caption="부서명" Width="35%">
                <HeaderStyle HorizontalAlign="Center" /><CellStyle HorizontalAlign="Center" Wrap="False" />
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>
        
    <%-- 숨김필드 --%>
    <dx:ASPxTextBox ID="txtID" ClientInstanceName="txtID" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtEmpName" ClientInstanceName="txtEmpName" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtEmpCode" ClientInstanceName="txtEmpCode" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtWhereClause" ClientInstanceName="txtWhereClause" runat="server" ClientVisible="false"></dx:ASPxTextBox>

</asp:Content>