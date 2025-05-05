<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="ReportPageVariable.aspx.cs" Inherits="Common_PrintForm_ReportPageVariable" %>

<asp:Content ID="ReportContent" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            //공통팝업 상태바 표기 안함.
            v_mast_statusbar_visible = false;

            //데이터넣기
            txtData1.SetText(parent.v_mast_pop_data1);
            txtData2.SetText(parent.v_mast_pop_data2);
            txtData3.SetText(parent.v_mast_pop_data3);
            txtData4.SetText(parent.v_mast_pop_data4);
            txtData5.SetText(parent.v_mast_pop_data5);
            txtData6.SetText(parent.v_mast_pop_data6);
            txtData7.SetText(parent.v_mast_pop_data7);
            txtData8.SetText(parent.v_mast_pop_data8);
            txtData9.SetText(parent.v_mast_pop_data9);
            txtData10.SetText(parent.v_mast_pop_data10);
            txtData11.SetText(parent.v_mast_pop_data11);
            txtData12.SetText(parent.v_mast_pop_data12);
            txtData13.SetText(parent.v_mast_pop_data13);
            txtData14.SetText(parent.v_mast_pop_data14);
            txtData15.SetText(parent.v_mast_pop_data15);

            if (txtStatus.GetText() == "A") {
                btnRun.DoClick();
            }
            if (txtMode.GetText() == "Excel") {
                //window.setTimeout(function () {
                //    //팝업창 닫기
                //    mastCloseForm();
                //}, 100);
            }

            //에러데이터가 있는 경우는 표기후 닫는다. C# 로드시 생성된 데이터로 확인.
            if (txtErrMessage.GetText() != "") {
                mastPageFooterErr(txtErrMessage.GetText());
                mastCloseForm();
            }
        }
        function aa() {
            alert('a');
        }
    </script>

    <dx:ASPxWebDocumentViewer ID="docViewer" ClientInstanceName="docViewer" runat="server" Width="100%"></dx:ASPxWebDocumentViewer>
    
    <dx:ASPxButton ID="btnRun" ClientInstanceName="btnRun" runat="server" ClientVisible="false" OnClick="btnRun_Click" />

    <dx:ASPxTextBox ID="txtData1" ClientInstanceName="txtData1" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData2" ClientInstanceName="txtData2" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData3" ClientInstanceName="txtData3" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData4" ClientInstanceName="txtData4" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData5" ClientInstanceName="txtData5" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData6" ClientInstanceName="txtData6" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData7" ClientInstanceName="txtData7" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData8" ClientInstanceName="txtData8" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData9" ClientInstanceName="txtData9" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData10" ClientInstanceName="txtData10" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData11" ClientInstanceName="txtData11" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData12" ClientInstanceName="txtData12" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData13" ClientInstanceName="txtData13" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData14" ClientInstanceName="txtData14" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtData15" ClientInstanceName="txtData15" runat="server" ClientVisible="false" />
    <dx:ASPxTextBox ID="txtMode" ClientInstanceName="txtMode" runat="server" ClientVisible="false" />

    <dx:ASPxTextBox ID="txtStatus" ClientInstanceName="txtStatus" runat="server" ClientVisible="false" />

    <dx:ASPxTextBox ID="txtErrMessage" ClientInstanceName="txtErrMessage" runat="server" ClientVisible="false" />
<%
	Response.Write ( m_strScriptTarget );
	m_strScriptTarget = "";
%>
</asp:Content>