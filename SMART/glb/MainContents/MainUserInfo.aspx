<%@ Page Language="C#"  MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="MainUserInfo.aspx.cs" Inherits="glb_MainContents_MainUserInfo" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    
    <script type="text/javascript">

        function init() {
            v_mast_statusbar_visible = false;
        }

    </script>

    <div class="frame_title">
        <span>사용자정보</span>
    </div>

    <table style="width: 100%;">
        <colgroup>
            <col width="80px;" />
            <col width="1px;" />
            <col width="*" />
        </colgroup>
        <tbody>
            <tr>
                <td rowspan="3">
                    <dx:ASPxImage ID="imgUser" ClientInstanceName="imgUser" runat="server" CssClass="img_user"></dx:ASPxImage>
                </td>
                <td>&nbsp</td>
                <td style="vertical-align:top;">
                    <span id="lblEmpNameTitle" runat="server" class="usertitle"></span>
                    <span id="lblEmpName" runat="server" class="usercontents"></span>
                </td>
            </tr>
            <tr>
                <td>&nbsp</td>
                <td style="vertical-align:top;">
                    <span id="lblSIteNameTitle" runat="server" class="usertitle"></span>
                    <span id="lblSIteName" runat="server" class="usercontents"></span>
                </td>
            </tr>
            <tr>
                <td>&nbsp</td>
                <td style="vertical-align:top;">
                    <span id="lblDeptNameTitle" runat="server" class="usertitle"></span>
                    <span id="lblDeptName" runat="server" class="usercontents"></span>
                </td>
            </tr>
        </tbody>
    </table>

    <table style="width: 100%;" class="user_table_border">
        <colgroup>
            <col width="50%;" />
            <col width="1px;" />
            <col width="50%;" />
        </colgroup>
        <tr>
            <td style="vertical-align:top;" colspan="3">
                <span id="lblMobileNoTitle" runat="server" class="usertitle2"></span>
                <span id="lblMobileNo" runat="server" class="usercontents2"></span>
            </td>
        </tr>
        <tr>    
            <td style="vertical-align:top;" colspan="3">
                <span id="lblEmailTitle" runat="server" class="usertitle2"></span>
                <span id="lblEmail" runat="server" class="usercontents2"></span>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top;">
                <span id="lblBirtyDayTitle" runat="server" class="usertitle2"></span>
                <span id="lblBirtyDay" runat="server" class="usercontents2"></span>
            </td>
            <td>&nbsp</td>
            <td style="vertical-align:top;">
                <span id="lblIpsaDayTitle" runat="server" class="usertitle2"></span>
                <span id="lblIpsaDay" runat="server" class="usercontents2"></span>
            </td>
        </tr>
    </table>
    

</asp:Content>