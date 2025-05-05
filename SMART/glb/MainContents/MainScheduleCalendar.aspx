<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="MainScheduleCalendar.aspx.cs" Inherits="glb_MainContents_MainScheduleCalendar" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    
    <script type="text/javascript">

        function init() {
            v_mast_statusbar_visible = false;
        }

        function calendarCellClick(s, e) {
            alert(e.date);

        }

    </script>

    <div class="frame_title">
        <span>달력</span>
    </div>

    <dx:ASPxCalendar ID="calendar" ClientInstanceName="calendar" runat="server" Width="100%" Font-Size="Small" Theme="Office2010Blue"
        CellStyle-Font-Bold="true" Border-BorderWidth="1" Border-BorderColor="Gray" Border-BorderStyle="Outset">
        <%--<ClientSideEvents CellClick="calendarCellClick"  />--%>
    </dx:ASPxCalendar>
   

</asp:Content>