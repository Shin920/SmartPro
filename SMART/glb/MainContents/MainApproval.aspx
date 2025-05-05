<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="MainApproval.aspx.cs" Inherits="glb_MainContents_MainApproval" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            v_mast_statusbar_visible = false;
        }

        function app01() {

            var fID = txtFormID.GetText();
            var fUrl = txtURL.GetText();
            var fName = txtFormName.GetText();

            mastParentAddTabMenu(fID, fName, fUrl, fID);
        }

        function app02() {
            alert('');
        }

    </script>

    <div class="frame_title">
        <span>Approval</span>
    </div>

    <table style="width:100%; " class="approval_table">
        <colgroup>
            <col width="100px;">
            <col width="*">
        </colgroup>
        <tbody>
            <tr>
                <td class="approval_img">
                    <dx:ASPxImage ID="imgApp01" ClientInstanceName="imgApp01" runat="server" />
                </td>
                <td>
                    <dx:ASPxLabel ID="lblApp01" ClientInstanceName="lblApp01" runat="server"></dx:ASPxLabel>
                </td>
            </tr>
            <%--
            <tr>
                <td class="approval_img">
                    <dx:ASPxImage ID="imgApp02" ClientInstanceName="imgApp02" runat="server" />
                </td>
                <td>
                    <dx:ASPxLabel ID="lblApp02" ClientInstanceName="lblApp02" runat="server"></dx:ASPxLabel>
                </td>
            </tr>
            --%>
        </tbody>
    </table>

    <dx:ASPxTextBox ID="txtFormName" ClientInstanceName="txtFormName" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtFormID" ClientInstanceName="txtFormID" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtURL" ClientInstanceName="txtURL" runat="server" ClientVisible="false" />

</asp:Content>