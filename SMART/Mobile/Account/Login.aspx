<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title>Smart Pro Mobile</title>
    <meta name="viewport" content="width=device-width"/>
    <link href="/SmartPro/Mobile/Contents/css/login_m.css" rel="stylesheet" type="text/css" />
    <link href="/SmartPro/Mobile/Contents/images/Mobile.ico" rel="shortcut icon" />

    <script type="text/javascript">

    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div id="bg">
		    <img src="/SmartPro/Mobile/Contents/images/bg_img.jpg" alt="" />
	    </div>

        <div id="wrap">
            <div id="logo">
			    <img src="/SmartPro/Mobile/Contents/images/logo.png" alt="" />
		    </div>
            <div id="title">
			    <h1>SmartPro</h1>
		    </div>

            <div id="box">
                <dx:ASPxComboBox ID="cmbClient" runat="server" NullText="접속될 DB 정보를 선택하여 주십시오." Native="true" CssClass="select" />

                <dx:ASPxComboBox ID="cmbSiteCode" runat="server" NullText="사업장 정보를 선택하여 주십시오." Native="true" CssClass="select" />

                <dx:ASPxTextBox ID="txtID" ClientInstanceName="txtID" runat="server" NullText="ID" Native="true" CssClass="input" />

                <dx:ASPxTextBox ID="txtPW" ClientInstanceName="txtPW" runat="server" NullText="PASSWORD" Password="true" Native="true" CssClass="input" />

                <dx:ASPxButton ID="btnLogin" runat="server" Text="로그인" CssClass="login" OnClick="btnLogin_Click" Width="100%"></dx:ASPxButton>
            </div>

        </div>

    </form>
</body>
</html>
