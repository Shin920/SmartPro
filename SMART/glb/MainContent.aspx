<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="MainContent.aspx.cs" Inherits="glb_MainContent" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    <script type="text/javascript">

        function init() {
            v_mast_statusbar_visible = false;
            jqUpdateSizeH();
        }

        function jqUpdateSizeH() {
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 57;  // 상단 제외 영역 높이
            var h_min = 15;  //grid 최소 높이

            if (h - h_static > h_min) {
                document.getElementById("notice").style.height = ((h - h_static) / 2) + 'px';
                document.getElementById("schedule").style.height = ((h - h_static) / 2) + 'px';
                document.getElementById("calendar").style.height = (h - h_static - 517) + 'px';
                

            } else {
                document.getElementById("notice").style.height = h_min + 'px';
                document.getElementById("schedule").style.height = h_min + 'px';
                document.getElementById("calendar").style.height = 320 + 'px';
            }
        }

        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

    </script>
    
    <table id="tdTable" style="width: 100%; vertical-align: top;">
        <colgroup>
            <col style="width: 320px;" />
            <col style="width: 1px;" />
            <col style="width: 100%;" />
        </colgroup>
        <tbody>
            <tr>
                <td style="vertical-align: top;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <iframe src="MainContents/MainUserInfo.aspx" style="border: 0.5px solid #43567a; width: 320px; border-radius: 5px 5px 0 0; height: 255px; background-color: #e9edf4;" id="userinfo"></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <iframe src="MainContents/MainApproval.aspx" style="border: 0.5px solid #43567a; width: 320px; border-radius: 5px 5px 0 0; height: 255px; background-color: #e9edf4;" id="approval"></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <iframe src="MainContents/MainScheduleCalendar.aspx" style="border: 0.5px solid #43567a; width: 320px; border-radius: 5px 5px 0 0; height: 275px; background-color: #e9edf4;" id="calendar"></iframe>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp</td>
                <td style="vertical-align: top;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <iframe src="MainContents/MainNoticeBoard.aspx" style="border: 0.5px solid #43567a; width: 100%; height: 385px; border-radius: 5px 5px 0 0; background-color: #e9edf4;" id="notice"></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <iframe src="MainContents/MainUserScheduleList.aspx" style="border: 0.5px solid #43567a; width: 100%; height: 385px; border-radius: 5px 5px 0 0; background-color: #e9edf4;" id="schedule"></iframe>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>

    <%-- 
        --환율정보
        <iframe src="MainContents/MainWkReportGraph.aspx" style="resize: none; border: 0.5px solid #43567a; width: 100%; border-radius: 5px 5px 0 0; height: 255px; background-color: #e9edf4;" id="wkreport"></iframe>
        
        --개인일정리스트
        <iframe src="MainContents/MainUserScheduleList.aspx" style="resize: none; border: 0.5px solid #43567a; width: 100%; border-radius: 5px 5px 0 0; height: 255px; background-color: #e9edf4;" id="schedule"></iframe>

        --기본달력
        <iframe src="MainContents/MainScheduleCalendar.aspx" style="resize: none; border: 0.5px solid #43567a; width: 100%; border-radius: 5px 5px 0 0; height: 255px; background-color: #e9edf4;" id="calendar"></iframe>

        --결재모듈
        <iframe src="MainContents/MainApproval.aspx" style="resize: none; border: 0.5px solid #43567a; width: 100%; border-radius: 5px 5px 0 0; height: 255px; background-color: #e9edf4;" id="approval"></iframe>

        --공지사항
        <iframe src="MainContents/MainNoticeBoard.aspx" style="resize: none; border: 0.5px solid #43567a; width: 100%; border-radius: 5px 5px 0 0; height: 255px; background-color: #e9edf4;" id="notice"></iframe>
        --%>
</asp:Content>
