<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindForms.aspx.cs" Inherits="glb_Forms_FindForms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="/SmartPro/Contents/Script/jquery-3.6.0.js"></script>

    <script type="text/javascript">
        function init() {
            Grid.PerformCallback();
            jqUpdateSizeH();
        }
        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 20;  // 상단 제외 영역 높이
            var h_min = 0;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);
            } else {
                Grid.SetHeight(h_min);
            }
        };
        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function GridRowClick(s, e) {

            var vKeyValue = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            if (vKeyValue != null) {

                var sAry = vKeyValue.split("|");

                var sFormID = sAry[0];
                var sURL = sAry[1];
                var sTitle = sAry[2];
                var sObjID = sAry[3];
                var sParentObj = sAry[4];

                opener.parent.addTabMenu(sFormID, sTitle, sURL, sParentObj);
                self.close();
            }
        }

        function GridEndCallback(s, e) {

        }
    </script>

</head>
<body onload="init();">
    <form id="form1" runat="server">
        
        <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%" AutoGenerateColumns="false"
            OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" KeyFieldName="FormID;FormUrl;ObjName;ObjID;ParentObj"
            OnAfterPerformCallback="Grid_AfterPerformCallback" OnCustomColumnDisplayText="Grid_CustomColumnDisplayText">

            <Styles Header-HorizontalAlign="Center" Cell-Wrap="False" />
            <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" />
            <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
            <SettingsPager Mode="ShowAllRecords" />
            <SettingsLoadingPanel Mode="Disabled" />

            <ClientSideEvents EndCallback="GridEndCallback" RowClick="GridRowClick" />

            <Columns>
                <dx:GridViewDataTextColumn FieldName="FormID" Caption="화면아이디" Width="30%" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="NaviMenu" Caption="화면명" Width="70%" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ObjName" Caption="화면명" Visible="false"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="FormUrl" Visible="false"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ObjID" Visible="false"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ParentObj" Visible="false"></dx:GridViewDataTextColumn>
            </Columns>
        </dx:ASPxGridView>

        <%-- 숨김 --%>
        <dx:ASPxTextBox ID="txtFindForm" ClientInstanceName="FindForm" runat="server" ClientVisible="false"></dx:ASPxTextBox>

    </form>
</body>
</html>
