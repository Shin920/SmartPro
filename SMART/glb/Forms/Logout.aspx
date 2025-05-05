<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logout.aspx.cs" Inherits="glb_Forms_Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, user-scalable=no, maximum-scale=1.0, minimum-scale=1.0" />
    <title></title>
    <script type="text/javascript">
        /* 로그인 로그아웃 후 뒤로가기 안되게 방지 */
        window.history.forward();
        function noBack() {
            window.history.forward();
        }
    </script>
</head>
<body onload="noBack();">
    <form id="form1" runat="server"></form>
</body>
</html>

<script type="text/javascript">
    top.document.location = "/SmartPro/Login.aspx";
</script>
