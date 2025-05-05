<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="DeptMaster_Tree.aspx.cs" Inherits="Common_Forms_DeptMaster_Tree" %>

<asp:Content ID="CommonContent" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            //공통팝업 상태바 표기 안함.
            v_mast_statusbar_visible = false;

            jqUpdateSizeH();
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 45;  // 상단 제외 영역 높이
            var h_min = 10;  //grid 최소 높이

            if (h - h_static > h_min) {
                callbackTree.SetHeight(h - h_static);

            } else {
                callbackTree.SetHeight(h_min);
            }
        };
        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function treeNodeClick(s, e) {

            v_mast_pop_code = e.node.name;
            v_mast_pop_name = e.node.text;

            mastCloseForm();

        }

        function callbackTreeEndCallback(s, e) {

            //var strMsg = s.cp_ret_message;
            //var strFlag = s.cp_ret_flag;

            //if (strFlag != "00") {
            //    alert(strMsg);
            //}
            mastLoadingPanelHide();
        }

    </script>

    <div style="height: 25px; width: 100%; background-color: royalblue; text-align: center; padding-top: 7px;">
        <dx:ASPxLabel ID="lblMenuTitle" ClientInstanceName="lblMenuTitle" runat="server" Height="25" Text="" ForeColor="White"></dx:ASPxLabel>
    </div>

    <%-- 트리뷰 바인딩을 위해 콜백판넬 이용 --%>
    <dx:ASPxCallbackPanel ID="callbackTree" ClientInstanceName="callbackTree" runat="server" OnCallback="callbackTree_Callback" BackColor="Honeydew" ScrollBars="Auto">
        <Border BorderWidth="1" BorderStyle="Solid" />
        <ClientSideEvents EndCallback="callbackTreeEndCallback" />
        <PanelCollection>
            <dx:PanelContent ID="panelTree" runat="server" >

                <dx:ASPxTreeView ID="tree" ClientInstanceName="tree" runat="server" Width="100%">
                    <ClientSideEvents NodeClick="treeNodeClick" />
                </dx:ASPxTreeView>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <%-- 숨김필드 --%>
    <dx:ASPxTextBox ID="txtDeptCode" ClientInstanceName="txtDeptCode" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtDeptName" ClientInstanceName="txtDeptName" runat="server" ClientVisible="false"></dx:ASPxTextBox>

    <dx:ASPxTextBox ID="txtParentDeptCode" ClientInstanceName="txtParentDeptCode" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtParentDeptName" ClientInstanceName="txtParentDeptName" runat="server" ClientVisible="false"></dx:ASPxTextBox>

</asp:Content>