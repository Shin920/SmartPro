<%@ Page language="c#" CodeFile="WfDocument.aspx.cs" AutoEventWireup="false" Inherits="SMART.Common.Forms.WorkFlow.WfDocument.WfDocument" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD>
		<title>
			<% Response.Write ( strHTitle ); %>
		</title>
		
		<link href="/SmartPro/Common/Style/SmartProForm.css" rel="stylesheet" type="text/css" />
	</HEAD>
	
	<body onunload="ReturnParent()" MS_POSITIONING="GridLayout" style="overflow:hidden;">
	
		<script language="javascript" type="text/javascript">
		
			function ReturnParent() 
			{ 
				if ( !opener.closed )
					opener.document.body.onfocus=null;
					opener.document.body.onactivate=null;
			}				


			function SetEnable(strCtlName, bEnable)
			{
				if (bEnable == true)
				{
                    document.all[strCtlName].disabled = false;                
                }
				else
				{
                    document.all[strCtlName].disabled = true;                
                }
			}


			function returnFalse()
			{
                return false;
            }


			function pop_Route()
			{
                var popupX = (document.body.offsetWidth / 2) - (200 / 2);
                var popupY = (document.body.offsetHeight / 2) - (300 / 2);

                url = "RoutePopup2.aspx?ViewState=" + document.all['txtViewState'].value;

                targetName = "popup"

                window.open(url, targetName, "height=250,width=300, left=" + (window.event.screenX - 290) + ", top=" + (window.event.screenY + 12) + ", status=no,toolbar=no,menubar=no,location=no");
            }
        </script>
		
		<form id="WfDocument1" method="post" runat="server">
			<table class="basic2" style="z-index: 101; left: 5px; width: 450px; position: absolute; top: 5px; " cellSpacing="1" cellPadding="1" width="450" border="1">
				<tr>
					<th style="width: 100px; height: 26px; background-color: #f0f8ff;" align="center">
                        <asp:label id="lblWfRoute" runat="server" Font-Bold="true" CssClass="ta_label"></asp:label>
					</th>
					<td style="WIDTH: 375px; HEIGHT: 26px">
                        <asp:textbox id="txtRouteCode" runat="server" Width="75" Height="20px" CssClass="input_textfield" MaxLength="12"></asp:textbox>
					    <asp:textbox id="txtRouteName" runat="server" Width="135px" Height="20px" CssClass="input_textfield" MaxLength="40"></asp:textbox>

						
						<!-- 팝업 -->
						<img src="/SmartPro/Common/Image/Button/popup.gif" onclick="pop_Route();" style="cursor:pointer" alt="" />

					    <FONT color="#000080" size="2">&nbsp;<% Response.Write ( strRefCnt1 ); %></FONT>
						<asp:textbox id="txtRefCnt" runat="server" Width="16px" Height="16px" BackColor="Transparent" BorderStyle="None" ForeColor="Navy">0</asp:textbox>
                        <font color="navy">
							<% Response.Write ( strRefCnt2 ); %>
						</font>


						<div style="display:none;">
							<asp:textbox id="txtStepCnt" runat="server" Width="0px" Height="0px"></asp:textbox>
						</div>

					</td>
				</tr>
				<tr>
					<th style="WIDTH: 465px; HEIGHT: 105px" colspan="2">
						<center>
							<%Response.Write ( m_strStampRender );%>
						</center>
					</th>
				</tr>
				<tr>
					<th style="width: 100px; height: 100px; background-color: #f0f8ff;" align="center">
                        <asp:label id="lblDescr" runat="server" Font-Bold="true" CssClass="ta_label"></asp:label>
					</th>
					<td style="WIDTH: 375px; HEIGHT: 100px">
                        <asp:textbox id="txtDocuDescr" runat="server" Width="365px" Height="80px" CssClass="input_textfield" MaxLength="500" TextMode="MultiLine"></asp:textbox>
					</td>
				</tr>
			</table>
			
			<table class="basic2" style="Z-INDEX: 101; LEFT: 5px; WIDTH: 475px; POSITION: absolute; TOP: 247px">
			    <tr>
			        <td align="left"> 
						
						<%--
						<asp:LinkButton id="btn07" runat="server" Width="40px" Height="20px" Text="" OnClientClick="Event = Approve()" AutoPostBack="false">
							<asp:Image ID="btn07Img" runat="server" ImageUrl="/SmartPro/Common/Image/Button/btn02.gif" style="border-width: 0px;" AlternateText="기안" />
						</asp:LinkButton>

						<asp:button id="btn07" runat="server" Width="40px" Height="20px" Text="기안" OnClientClick="Approve()" AutoPostBack="false"></asp:button>
						<asp:button id="btn05" runat="server" Width="40px" Height="20px" Text="전결" OnClickScript="Event = ApproveAll()|" AutoPostBack="false"></asp:button>
						<asp:button id="btn04" runat="server" Width="40px" Height="20px" Text="보류" OnClickScript="Event = Hold()|" AutoPostBack="false"></asp:button>
						<asp:button id="btn06" runat="server" Width="40px" Height="20px" Text="반송" OnClickScript="Event = Back()|" AutoPostBack="false"></asp:button>
						<asp:button id="btn03" runat="server" Width="40px" Height="20px" Text="OK" OnClickScript="Event = RefOpen()|" AutoPostBack="false" Visible="false"></asp:button>
						--%>
												
						<asp:LinkButton id="btn09" runat="server" Width="40px" Height="20px" Text="" OnClientClick="Event = Approve()" AutoPostBack="false">
							<asp:Image ID="btn09Img" runat="server" ImageUrl="/SmartPro/Common/Image/Button/btn09.gif" style="border-width: 0px;" AlternateText="결재" />
						</asp:LinkButton>

						<asp:LinkButton id="btn07" runat="server" Width="40px" Height="20px" Text="" OnClientClick="Event = Approve()" AutoPostBack="false">
							<asp:Image ID="btn07Img" runat="server" ImageUrl="/SmartPro/Common/Image/Button/btn07.gif" style="border-width: 0px;" AlternateText="기안" />
						</asp:LinkButton>

						<asp:LinkButton id="btnCancel" runat="server" Width="40px" Height="20px" Text="" OnClientClick="Event = Approve()" AutoPostBack="false">
							<asp:Image ID="btnCancelImg" runat="server" ImageUrl="/SmartPro/Common/Image/Button/btnCancel.gif" style="border-width: 0px;" AlternateText="취소" />
						</asp:LinkButton>
						&nbsp;&nbsp;&nbsp;&nbsp;

						<asp:LinkButton id="btn05" runat="server" Width="40px" Height="20px" Text="" OnClientClick="Event = ApproveAll()" AutoPostBack="false">
							<asp:Image ID="btn05Img" runat="server" ImageUrl="/SmartPro/Common/Image/Button/btn05.gif" style="border-width: 0px;" AlternateText="전결" />
						</asp:LinkButton>
						&nbsp;&nbsp;&nbsp;&nbsp;
												
						<asp:LinkButton id="btn04" runat="server" Width="40px" Height="20px" Text="" OnClientClick="Event = Hold()" AutoPostBack="false">
							<asp:Image ID="btn04Img" runat="server" ImageUrl="/SmartPro/Common/Image/Button/btn04.gif" style="border-width: 0px;" AlternateText="보류" />
						</asp:LinkButton>
						&nbsp;&nbsp;&nbsp;&nbsp;
												
						<asp:LinkButton id="btn06" runat="server" Width="40px" Height="20px" Text="" OnClientClick="Event = Back()" AutoPostBack="false">
							<asp:Image ID="btn06Img" runat="server" ImageUrl="/SmartPro/Common/Image/Button/btn06.gif" style="border-width: 0px;" AlternateText="반송" />
						</asp:LinkButton>
						&nbsp;&nbsp;&nbsp;&nbsp;
												
						<asp:LinkButton id="btn03" runat="server" Width="40px" Height="20px" Text="" OnClientClick="Event = RefOpen()" AutoPostBack="false" Visible="false">
							<asp:Image ID="btn03Img" runat="server" ImageUrl="/SmartPro/Common/Image/Button/btn02.gif" style="border-width: 0px;" AlternateText="OK" />
						</asp:LinkButton>					
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

						<asp:LinkButton id="btnOK" runat="server" Width="40px" Height="20px" Text="" OnClick="btnOK_Click">
							<asp:Image ID="btnOKImg" runat="server" ImageUrl="/SmartPro/Common/Image/Button/btnOK.gif" style="border-width: 0px;" AlternateText="확인" />
						</asp:LinkButton>		
			        </td>
			        <td align="right">   
						
						<div style="display:none;">
							<asp:button id="btnLoad" runat="server" Width="0px" Height="0px" Text="Load" OnClick="btnLoad_Click"></asp:button>
							<asp:button id="btnAction" runat="server" Width="0px" Height="0px" Text="Action" OnClick="btnAction_Click"></asp:button>
						</div>
												
			        </td>
			    </tr>
			</table>
			
			<!-- Hidden -->
			<asp:textbox id="txtViewState" runat="server" Width="0px" Height="0px"></asp:textbox>
			<asp:textbox id="txtLoaded" runat="server" Width="0px" Height="0px"></asp:textbox>

			<asp:textbox id="txtAction" runat="server" Width="0px" Height="0px"></asp:textbox>
			<asp:textbox id="txtActionType" runat="server" Width="0px" Height="0px"></asp:textbox>				
			
			<asp:textbox id="txtSendMail" runat="server" Width="0px" Height="0px"></asp:textbox>			
			<asp:textbox id="txtIsWfPwd" runat="server" Width="0px" Height="0px"></asp:textbox>			
			<asp:TextBox id="hasApproved" runat="server" Width="0px" Height="0px"></asp:TextBox>
			
			</form>
		
		    <script language="Javascript" type="text/javascript" src="/SmartPro/Common/Script/CFL.js"></script>

        <script type="text/javascript">	

<%
            Response.Write(@"
ViewStateName = """ + Request["ViewStateName"] + @""";
");
%>
		
            // Set ViewState and Data Load
            if (null != window.opener) {
                if (null != window.opener.document.all[ViewStateName]) {
                    if ("" == document.all['txtLoaded'].value) {
                        document.all['txtViewState'].value = window.opener.document.all[ViewStateName].value;

                        document.all['btnLoad'].click();
                    }
                }
            }

            // GetCurStep
            var strCurStep = GetEncodedData("CurStep", document.all['txtViewState'].value);

            if (strCurStep == "")
                strCurStep = "1";
            
            // Set Buttons
            if ("Approve" == document.all['txtAction'].value) {
                    document.getElementById('btnCancel').style.display = "none";

                if (strCurStep == "1") {

                    document.getElementById('btn05').disabled = "disabled";
                    document.getElementById('btn05').onclick = returnFalse;
                    document.getElementById('btn05').style.cursor = 'default';

                    document.getElementById('btn04').disabled = "disabled";
                    document.getElementById('btn04').onclick = returnFalse;
                    document.getElementById('btn04').style.cursor = 'default';

                    document.getElementById('btn06').disabled = "disabled";
                    document.getElementById('btn06').onclick = returnFalse;
                    document.getElementById('btn06').style.cursor = 'default';

                }
                else if (strCurStep == "2" || strCurStep == "3") {
                    document.getElementById('btn07').style.display = "none";  // 기안

                    document.getElementById('btn09').disabled = "disabled";  // 결재
                    document.getElementById('btn09').style.cursor = 'default';

                    document.getElementById('btn05').disabled = "disabled";
                    document.getElementById('btn05').style.cursor = 'default';

                    document.getElementById('btn04').disabled = "disabled";
                    document.getElementById('btn04').style.cursor = 'default';

                    document.getElementById('btn06').disabled = "disabled";
                    document.getElementById('btn06').style.cursor = 'default';

                }
            }
            else if ("Cancel" == document.all['txtAction'].value) {
                //	SetText('btn07','<% Response.Write(strCancel); %>');

                // 원본
                //	document.all['btn07'].value = "취소";

                document.getElementById('btn07').style.display = "none";

                document.getElementById('btn09').style.display = "none";

                document.getElementById('btn05').disabled = "disabled";
                document.getElementById('btn05').onclick = returnFalse;
                document.getElementById('btn05').style.cursor = 'default';

                document.getElementById('btn04').disabled = "disabled";
                document.getElementById('btn04').onclick = returnFalse;
                document.getElementById('btn04').style.cursor = 'default';

                document.getElementById('btn06').disabled = "disabled";
                document.getElementById('btn06').onclick = returnFalse;
                document.getElementById('btn06').style.cursor = 'default';
            }
            else if ("Hold" == document.all['txtAction'].value) {

                document.all['btn07'].value = "결재";
                document.all['btn04'].value = "취소";
            }
            else {
                
                document.getElementById('btnCancel').style.display = "none";

                document.getElementById('btn09').style.display = "none";  // 1

                document.getElementById('btn07').disabled = "disabled";
                document.getElementById('btn07').onclick = returnFalse;
                document.getElementById('btn07').style.cursor = 'default';   // 'hand'

                document.getElementById('btn05').disabled = "disabled";
                document.getElementById('btn05').onclick = returnFalse;
                document.getElementById('btn05').style.cursor = 'default';

                document.getElementById('btn04').disabled = "disabled";
                document.getElementById('btn04').onclick = returnFalse;
                document.getElementById('btn04').style.cursor = 'default';

                document.getElementById('btn06').disabled = "disabled";
                document.getElementById('btn06').onclick = returnFalse;
                document.getElementById('btn06').style.cursor = 'default';
            }


            function Approve() {
                document.all['txtActionType'].value = "Approve";

                if (document.all['txtIsWfPwd'].value != "Y") {
                    // 테스트 막음
                    document.all['btnAction'].click();
                }
                else {                    
                    ChildOpen();
                }
            }


            function ApproveAll() {
                document.all['txtActionType'].value = "ApproveAll";

                //	if(document.WfDocument.txtIsWfPwd.value  !=  "Y")
                if (document.all['txtIsWfPwd'].value != "Y") {
                    //	document.WfDocument.btnAction.click();
                    document.all['btnAction'].click();
                }
                else {
                    ChildOpen();
                }
            }

            function Hold() {
                document.all['txtActionType'].value = "Hold";

                //  if(document.WfDocument.txtIsWfPwd.value  !=  "Y")
                if (document.all['txtIsWfPwd'].value != "Y") {
                    //	document.WfDocument.btnAction.click();
                    document.all['btnAction'].click();
                }
                else {
                    ChildOpen();
                }
            }


            function Back() {
                document.all['txtActionType'].value = "Back";

                //  if(document.WfDocument.txtIsWfPwd.value  !=  "Y")
                if (document.all['txtIsWfPwd'].value != "Y") {
                    //	document.WfDocument.btnAction.click();
                    document.all['btnAction'].click();
                }
                else {
                    ChildOpen();
                }
            }


            function ChildOpen() {
                if ("" == document.all['txtAction'].value || "" == document.all['txtActionType'].value) {
                    alert( "<%Response.Write(strScript);%>");
                    return;
                }

                varWindow = window.open("Approve.aspx?Action=" + document.all['txtAction'].value + "&ActionType=" + document.all['txtActionType'].value, '', 'Width=280, Height=150, Left=' + (window.event.screenX + 30) + ', Top=' + (window.event.screenY - 200) + ', menubar=no, toolbar=no, location=no, directories=no, status=no, scrollbars=no, resizable=no');
                self.document.body.onfocus = FocusChild;
            }


            function RefOpen() {
                if ("" == document.all['txtAction'].value) {
                    alert( "<%Response.Write(strScript);%>");
                    return;
                }

                varWindow = window.open("DocuRef.aspx?Disabled=" + document.all['txtAction'].value, '', 'Width=355, Height=265, Left=' + (window.event.screenX - 150) + ', Top=' + (window.event.screenY - 300) + ', menubar=no, toolbar=no, location=no, directories=no, status=no, scrollbars=no, resizable=no');
                self.document.body.onfocus = FocusChild;
            }


            function FocusChild() {
                varWindow.focus();
            }


            function refAct() {
                window.opener.document.all['<%=Request["ViewStateName"]%>'].value = document.all['txtViewState'].value;
                window.opener.<%=Request["OkFunctionName"]%>();
                self.close();
            }

        </script>
		
<%
	Response.Write ( m_strScriptTarget );
	m_strScriptTarget = "";
%>
	
	    
	
	</body>
</html>