<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="ReportPage_Sub.aspx.cs" Inherits="Common_PrintForm_ReportPage_Sub" %>

<asp:Content ID="ReportContent" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            //공통팝업 상태바 표기 안함.
            v_mast_statusbar_visible = false;
        }

    </script>

    <dx:ASPxWebDocumentViewer ID="docViewer" ClientInstanceName="docViewer" runat="server" Width="100%"></dx:ASPxWebDocumentViewer>

</asp:Content>