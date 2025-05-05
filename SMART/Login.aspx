<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html lang="ko">
<head runat="server">
    <meta charset="UTF-8">
    <title>Smart Pro - ERP Solution</title>
    <meta name="viewport" content="width=device-width"/>
    <link href="/SmartPro/Contents/Login/css/login_mes.css" rel="stylesheet" />
    <link rel="shortcut icon" href="/SmartPro/Contents/Login/images/ERP.ico" />
    <script type="text/javascript">
        /* 로그인 로그아웃 후 뒤로가기 안되게 방지 */
        window.history.forward();
        function noBack() {
            window.history.forward();
        }

    </script>
</head>
<body onload="noBack();" class="erp_bg">
    <form id="frmLogin" runat="server">

        <div class="">
            <div class="loginbox">

                <%--
                .mes_bg_00 : 공용
                .mes_bg_01 : 전기전자
                .mes_bg_02 : 자동차
                .mes_bg_03 : 식품
                .mes_bg_04 : 바이오
                --%>

                <div class="lb_left mes_bg_03">
                    <img src="/SmartPro/Contents/Login/images/title_ERP.png" alt="smart pro ERP" />
                </div>

                <div class="lb_right">

                    <asp:TextBox ID="txtID" runat="server" CssClass="input_id" placeholder="아이디"></asp:TextBox>
                    <asp:TextBox ID="txtPWD" runat="server" CssClass="input_pw" TextMode="Password" placeholder="패스워드"></asp:TextBox>
                    <a href="javascript:btnLogin.OnClick();" class="btn_login">LOGIN</a>

                    <div class="lb_option">
                        <asp:DropDownList ID="cmbComCode" runat="server" OnSelectedIndexChanged="cmbComCode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <asp:DropDownList ID="cmbSiteCode" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="cmbClient" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="cmbLang" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

        <%-- --%>
        <dx:ASPxButton ID="btnLogin" ClientInstanceName="btnLogin" runat="server" Text="로그인" OnClick="btnLogin_Click" ClientVisible="false" />
    <% 
        Response.Write ( m_strScriptTarget ); 
    %>
    </form>
</body>
</html>
