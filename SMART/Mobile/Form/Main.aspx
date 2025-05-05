<%@ Page Language="C#" MasterPageFile="~/Master/Page.master" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Form_Main" %>
<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">
        function PageInit() {

        }
    </script>

    <!--con-->
	<div id="con">
	    <ul class="clearfix">
			<li><a href="javascript:mastAddTabMenu('PO001', '구매입고', '/SmartPro/Mobile/Form/PO/PO001/PO001.aspx')">1. 구매입고</a></li>
			<li><a href="javascript:mastAddTabMenu('MM015', '제품출고', '/SmartPro/Mobile/Form/MM/MM015/MM015.aspx')">2. 제품출고</a></li>
			<li><a href="javascript:mastAddTabMenu('MM017', '실사결과', '/SmartPro/Mobile/Form/MM/MM017/MM017.aspx')">3. 재고실사</a></li>
<%--		    <li><a href="javascript:mastAddTabMenu('SM001', '일마감조회', '/SmartPro/Mobile/Form/SM/SM001/SM001.aspx')">1. 일마감조회</a></li>
		    <li><a href="javascript:mastAddTabMenu('SM002', '월마감조회', '/SmartPro/Mobile/Form/SM/SM002/SM002.aspx')">2. 월마감조회</a></li>
			<li><a href="javascript:mastAddTabMenu('SM003', '년마감조회', '/SmartPro/Mobile/Form/SM/SM003/SM003.aspx')">3. 년마감조회</a></li>
		    <li><a href="javascript:mastAddTabMenu('SM010', 'AS 리포트',  '/SmartPro/Mobile/Form/SM/SM010/SM010.aspx')">4. AS 리포트</a></li>		 --%>   
	    </ul>
    </div>

</asp:Content>