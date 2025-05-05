<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="MainUserSchedule.aspx.cs" Inherits="glb_MainContents_MainUserSchedule" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            v_mast_statusbar_visible = false;
            jqUpdateSizeH();
        }

        function jqUpdateSizeH() {
            //var w = $(window).width();
            var h = $(window).height();
            document.getElementById("divSchedule").style.height = h + 'px';
        }

        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function scheduleEndCallback(s, e) {
            if (s.cpCallbackError && s.cpCallbackError.trim() != "") {
                alert(s.cpCallbackError);
                s.cpCallbackError = "";
            }
        }

    </script>

    <div id="divSchedule" style="height: 475px; overflow: auto">
        <dxwschs:ASPxScheduler ID="schedule" ClientInstanceName="schedule" ActiveViewType="Month" runat="server" Width="100%" Theme="Glass"
            OnDataBinding="schedule_DataBinding"
            OnAppointmentInserting="schedule_AppointmentInserting" 
            OnAppointmentRowUpdating="schedule_AppointmentRowUpdating" OnAppointmentDeleting="schedule_AppointmentDeleting" >
            
            <OptionsView VerticalScrollBarMode="Auto" FirstDayOfWeek="Sunday"></OptionsView>
            <OptionsCustomization AllowAppointmentEdit="Custom" />
            <ResourceNavigator Visibility="Always">
                <SettingsPager EnableIncreaseDecrease="false" />
            </ResourceNavigator>

            <Storage EnableReminders="false">
                <Appointments AutoRetrieveId="true" ResourceSharing="True">
                    <Mappings Subject="Subject" AppointmentId="UniqueID" Type="Type" Start="StartDate" End="EndDate" AllDay="AllDay" Location="Location" Description="Description" Status="Status" Label="Label" />
                </Appointments>
            </Storage>
            
            <ClientSideEvents EndCallback="scheduleEndCallback" />

            <Views>
                <AgendaView Enabled="false"></AgendaView>
                <MonthView Enabled="false" NavigationButtonVisibility="Never" ShowMoreButtons="false" ResourcesPerPage="1"
                    AppointmentDisplayOptions-AppointmentAutoHeight="true" AppointmentDisplayOptions-AppointmentHeight="50" CellAutoHeightOptions-Mode="FitToContent" 
                    CellAutoHeightOptions-MaxHeight="70" CellAutoHeightOptions-MinHeight="70">
                    <AppointmentDisplayOptions StartTimeVisibility="Never" EndTimeVisibility="Never" StatusDisplayType="Bounds" ShowRecurrence="false" />
                </MonthView>
                <TimelineView Enabled="false" NavigationButtonVisibility="Never"></TimelineView>
                <WorkWeekView Enabled="false" NavigationButtonVisibility="Never"></WorkWeekView>
                <WeekView Enabled="false" NavigationButtonVisibility="Never"></WeekView>
                <DayView Enabled="false" NavigationButtonVisibility="Never"></DayView>
            </Views>
        </dxwschs:ASPxScheduler>
    </div>

</asp:Content>