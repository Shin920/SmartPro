<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoutePopup2.aspx.cs" Inherits="Common_Forms_WorkFlow_RoutePopup2" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>결재경로</title>

    <script type="text/javascript">
    function SendCode(RouteCode, RouteName, StepCnt)
    {
    //  alert(RouteCode + " / " + RouteName + " / " + StepCnt);

        window.opener.document.all['txtRouteCode'].value = RouteCode;
        window.opener.document.all['txtRouteName'].value = RouteName;
        window.opener.document.all['txtStepCnt'].value = StepCnt;

        window.opener.document.all['btnLoad'].click();

        window.close();
    }
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            
            <asp:datagrid id="Grid" runat="server" AutoGenerateColumns="False" GridLines="Both" Width="280px" ForeColor="#333333" Font-Size="9" HeaderStyle-HorizontalAlign="Center">
                <AlternatingItemStyle Height="22px"></AlternatingItemStyle>                
                <HeaderStyle BackColor="#EDF1F6" Font-Bold="True" ForeColor="#777" Height="23px" />
                <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" Height="22px" />                
                <FooterStyle Wrap="False"></FooterStyle>
                <Columns>
                    <asp:TemplateColumn HeaderText="경로코드">
	                    <ItemTemplate>		                    
                            <a href="javascript:SendCode('<%# DataBinder.Eval(Container, "DataItem.RouteCode") %>','<%# DataBinder.Eval(Container, "DataItem.RouteName") %>','<%# DataBinder.Eval(Container, "DataItem.StepCnt") %>');">
                                <div style="float:right;cursor:pointer; width:60px; text-align:left">
                                    <%# DataBinder.Eval(Container, "DataItem.RouteCode") %>
                                </div>                                
                            </a>                            
	                    </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="경로명">
	                    <ItemTemplate>		                    
                            <a href="javascript:SendCode('<%# DataBinder.Eval(Container, "DataItem.RouteCode") %>','<%# DataBinder.Eval(Container, "DataItem.RouteName") %>','<%# DataBinder.Eval(Container, "DataItem.StepCnt") %>');">
                                <div style="float:right;cursor:pointer; width:120px; text-align:left">
                                    <%# DataBinder.Eval(Container, "DataItem.RouteName") %>
                                </div>                                
                            </a>                            
	                    </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="경로수">
	                    <ItemTemplate>		                    
                            <a href="javascript:SendCode('<%# DataBinder.Eval(Container, "DataItem.RouteCode") %>','<%# DataBinder.Eval(Container, "DataItem.RouteName") %>','<%# DataBinder.Eval(Container, "DataItem.StepCnt") %>');">
                                <div style="float:right;cursor:pointer; width:35px; text-align:center">
                                    <%# DataBinder.Eval(Container, "DataItem.StepCnt") %>
                                </div>
                            </a>                            
	                    </ItemTemplate>
                    </asp:TemplateColumn>

                </Columns>                
            </asp:datagrid>

        </div>
    </form>
</body>
</html>
