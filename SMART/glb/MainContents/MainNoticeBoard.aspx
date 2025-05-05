<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="MainNoticeBoard.aspx.cs" Inherits="glb_MainContents_MainNoticeBoard" %>

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
            var h_static = 28;  // 상단 제외 영역 높이
            var h_min = 5;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);

            } else {
                Grid.SetHeight(h_min);
            }
        };

        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function GridFocusedRowChanged() {
            //포커스가 선택될때 기존 히스토리 삭제하는 코드.
            Grid.SelectAllRowsOnPage(false);
        }

        function GridDbClick() {
            var sID = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            if (sID != null) {

                // 팝업을 가운데 위치시키기 위해 아래와 같이 값 구하기
                var sLeft = Math.ceil((window.screen.width - 300) / 2.5);
                var sTop = Math.ceil((window.screen.width - 300) / 6);

                var srID = sID.split("|");
                window.open("/SmartPro/glb/SiteBoard/Announce/AnnounceMng.aspx?Action=Load&From=Main&ID=" + srID[0] + "&NUM=" + srID[1], "공지사항", "width=800,height=540,menubar=no,status=no,toolbar=no,scrollbars=no,left=" + sLeft + ",top=" + sTop);
            }
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
    </script>

    <div class="frame_title">
        <span>공지사항</span>
    </div>

    <div style="margin: -5px;">
        <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%"
            AutoGenerateColumns="false" KeyboardSupport="true" KeyFieldName="Board_ID;Num" Theme="MetropolisBlue"
            OnCustomCallback="Grid_CustomCallback" OnAfterPerformCallback="Grid_AfterPerformCallback" OnDataBinding="Grid_DataBinding">

            <Styles Header-HorizontalAlign="Center" Header-Font-Bold="true" Header-BackColor="Thistle" Cell-Wrap="False" />
        
            <Settings VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
            <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" ColumnResizeMode="Control" AllowDragDrop="false" />
            <SettingsPager Mode="ShowAllRecords" />

            <ClientSideEvents FocusedRowChanged="GridFocusedRowChanged" RowDblClick="GridDbClick" EndCallback="GridEndCallback" />

            <Columns>
                <dx:GridViewDataColumn FieldName="Importance" Caption="중요" Width="50" Visible="true"
                    CellStyle-HorizontalAlign="Right">
                    <DataItemTemplate>
                        <img runat="server" src='<%# GetIcon(Eval("Importance")) %>' width="12" />
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataTextColumn FieldName="Writeday" Caption="작성일" Width="90" CellStyle-HorizontalAlign="Center" />
                <dx:GridViewDataTextColumn FieldName="Writer" Caption="작성자" Width="80" CellStyle-HorizontalAlign="Center" />
                <dx:GridViewDataTextColumn FieldName="Title" Caption="제목" Width="100%" />
                <dx:GridViewDataTextColumn FieldName="Board_ID" Caption="ID" Visible="false" />
                <dx:GridViewDataTextColumn FieldName="Num" Caption="순번" Visible="false" />
                <dx:GridViewDataTextColumn FieldName="Readnum" Caption="조회수" Visible="false" />
            </Columns>
        </dx:ASPxGridView>
    </div>

    


</asp:Content>