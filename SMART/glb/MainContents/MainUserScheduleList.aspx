<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="MainUserScheduleList.aspx.cs" Inherits="glb_MainContents_MainUserScheduleList" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    
    <script type="text/javascript">

        function init() {
            v_mast_statusbar_visible = false;
            jqUpdateSizeH();
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 65;  // 상단 제외 영역 높이
            var h_min = 10;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);

            } else {
                Grid.SetHeight(h_min);
            }
        };

        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size


        function btnSearch_Click() {
            Grid.PerformCallback();
        }

        function GridEndCallback() {
            Grid.SetFocusedRowIndex(-1);

            var vFlag = s.cp_ret_flag.toString();
            var vMsg = s.cp_ret_message.toString();
            var vJob = s.cp_ret_job.toString();

            if (vFlag == "N") {
                mastPageFooterErr(vMsg);
            }

            mastLoadingPanelHide();
        }

        function btnNew_Click() {

            // 팝업을 가운데 위치시키기 위해 아래와 같이 값 구하기
            var sLeft = Math.ceil((window.screen.width - 300) / 2.5);
            var sTop = Math.ceil((window.screen.width - 300) / 6);
            window.open("MainUserSchedule.aspx", "스케줄 등록", "width=850,height=570,menubar=no,status=no,toolbar=no,scrollbars=no,left=" + sLeft + ",top=" + sTop);
        }

        function GridRowDblClick() {

            var strVal = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            if (strVal == "" || strVal == null) {
                return
            }

            var sarVal = strVal.split("|");
            var strSDate = sarVal[0];
            var strEDate = sarVal[1];

            // 팝업을 가운데 위치시키기 위해 아래와 같이 값 구하기
            var sLeft = Math.ceil((window.screen.width - 300) / 2.5);
            var sTop = Math.ceil((window.screen.width - 300) / 6);
            window.open("MainUserSchedule.aspx?Action=Load&SDate=" + strSDate + "&EDate=" + strEDate, "스케줄 등록", "width=850,height=570,menubar=no,status=no,toolbar=no,scrollbars=no,left=" + sLeft + ",top=" + sTop);
        }

    </script>
    
    <div class="frame_title">
        <span>개인일정관리</span>
    </div>

    <table style="width: 100%;">
        <tr>
            <td align="right">
                <dx:ASPxButton ID="btnSearch" runat="server" AutoPostBack="false" Text="조회">
                    <Image IconID="actions_search_16x16devav"></Image>
                    <ClientSideEvents Click="btnSearch_Click" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnNew" runat="server" AutoPostBack="false" Text="신규등록">
                    <Image IconID="edit_copy_16x16office2013"></Image>
                    <ClientSideEvents Click="btnNew_Click" />
                </dx:ASPxButton>
            </td>
        </tr>
        <tr style="height: 10px;"></tr>
    </table>

    <div style="margin: -5px;">
        <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%"
            AutoGenerateColumns="false" KeyboardSupport="true" KeyFieldName="StartDate;EndDate" Theme="MetropolisBlue"
            OnCustomCallback="Grid_CustomCallback" OnAfterPerformCallback="Grid_AfterPerformCallback" OnDataBinding="Grid_DataBinding">

            <Styles Header-HorizontalAlign="Center" Header-Font-Bold="true" Header-BackColor="Thistle" Cell-Wrap="False" />
        
            <Settings VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
            <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" ColumnResizeMode="Control" AllowDragDrop="false" />
            <SettingsPager Mode="ShowAllRecords" />

            <ClientSideEvents EndCallback="GridEndCallback" RowDblClick="GridRowDblClick" />

            <Columns>
                <dx:GridViewDataTextColumn FieldName="StartDate" Caption="시작일" Width="90" CellStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="d" />
                <dx:GridViewDataTextColumn FieldName="EndDate" Caption="종료일" Width="90" CellStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="d" />
                <dx:GridViewDataTextColumn FieldName="Subject" Caption="제목" Width="40%" />
                <dx:GridViewDataTextColumn FieldName="Description" Caption="내용" Width="60%" />
            </Columns>
        </dx:ASPxGridView>
    </div>

</asp:Content>