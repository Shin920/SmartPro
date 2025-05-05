<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckForm.aspx.cs" Inherits="glb_Forms_CheckForm" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

		 <script type="text/javascript">
            /* 로그인 로그아웃 후 뒤로가기 안되게 방지 */
			window.history.forward();
			function noBack() {
				window.history.forward();
			}
         </script>
	</head>
	<body onload="noBack();">
		<form id="CheckForm" runat="server"></form>
		<%=m_strScriptTarget%>
	</body>
</html>
