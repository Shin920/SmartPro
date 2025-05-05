<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="SCMAnnounceMng.aspx.cs" Inherits="glb_SiteBoard_SCM_Announce_SCMAnnounceMng" validateRequest="false" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript" src="/SmartPro/Contents/smarteditor2/js/service/HuskyEZCreator.js" charset="utf-8"></script>
    <script type="text/javascript">

        function init() {
            //공통팝업 상태바 표기 안함.
            v_mast_statusbar_visible = false;
            DataLoad(); 

            jqUpdateSizeH();
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 35;  // 상단 제외 영역 높이
            var h_min = 70;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);

            } else {
                Grid.SetHeight(h_min);
            }
        };


        function DataLoad() {

            mastLoadingPanelShow();

            hidField.Clear();
            hidField.Set("job", "SELECT");
            callBackPanel.PerformCallback();
        }


        function btnSave_Click()
        {
            // 저장 후, 바인딩 없이 단순 선택했을 경우..
        //  Grid.GetSelectedFieldValues("UserID;UserName", btnSave2_Click);


            var rCnt = Grid.GetVisibleRowsOnPage();
            var iCnt = 0;

            for (var i = 0; i < rCnt; i++)
            {
                if (Grid.batchEditApi.GetCellValue(i, "UseYn") == "Y")
                {
                    iCnt++;
                }
            }

            if (iCnt == 0)
            {
                alert('SCM ID를 하나 이상을 선택하여 주십시요.');
                return;
            }


            // --------------------------------------------------------------


            // 기존 그리드 없이 저장할 때, 이 부분만 있었음.
            mastLoadingPanelShow();

            oEditors.getById["ir1"].exec("UPDATE_CONTENTS_FIELD", []);
            memoContent.SetText(document.all["ir1"].value);


            // --------------------------------------------------------------


            if (Grid.batchEditApi.HasChanges())
            {
                Grid.UpdateEdit();
            }
            else
            {
                Grid.PerformCallback("UPDATE");
            }            
        }


        /*         
        // 저장 후, 바인딩 없이 단순 선택했을 경우.. 사용함
        function btnSave2_Click(values)
        {

            var rowData = [];

            if (values.length == 0)
            {
                alert('SCM ID를 하나 이상을 선택하여 주십시요.');
                return;
            }

            for (i = 0; i < values.length; i++)
            {
                rowData[i] = values[i];
            }

            txtRow.Clear();

            txtRow.SetText(rowData);

            // -----------------------

            mastLoadingPanelShow();

            oEditors.getById["ir1"].exec("UPDATE_CONTENTS_FIELD", []);
            memoContent.SetText(document.all["ir1"].value);

            hidField.Clear();
            hidField.Set("job", "SAVE");
            callBackPanel.PerformCallback();
        }
        */

        function btnDelete_Click() {

            if (txtBoard_ID.GetText() == "") {
                return;
            }

            var sInput = prompt("등록시 입력한 비밀번호를 입력하여 주십시오.", "");
            if (sInput == null || sInput == "") {
                return;
            }

            if (sInput != txtOrigin_Pw.GetText()) {
                mastPageFooterErr("입력하신 비밀 번호 정보가 다릅니다.");
                return;
            }

            mastLoadingPanelShow();

            hidField.Clear();
            hidField.Set("job", "DELETE");
            callBackPanel.PerformCallback();
        }

        function btnClose_Click() {
            mastCloseForm();
        }

        function panelEndCallback(s, e) {

            var job = hidField.Get("job");
            var ret = hidField.Get("status");
            var msg = hidField.Get("msg");

            if (ret == "N") {
                mastPageFooterErr(msg);
            } else {
                if (job == "SELECT") {

                    txtTitle.SetText(hidField.Get("TITLE"));
                    rbImportance.SetValue(hidField.Get("IMPORTANCE"));
                    txtPw.SetText(hidField.Get("PWD"));
                    txtOrigin_Pw.SetText(hidField.Get("PWD"));
                    txtEmail.SetText(hidField.Get("EMAIL"));
                    txtWriter_ID.SetText(hidField.Get("WRITER_ID"));
                    memoContent.SetText(hidField.Get("CONTENT"));

                    //작성자 본인이면 수정가능
                    if (txtWriter_ID.GetText().toUpperCase() != txtUserID.GetText().toUpperCase()) {
                        btnSave.SetEnabled(false);
                        btnDelete.SetEnabled(false);
                    } else {
                        btnSave.SetEnabled(true);
                        btnDelete.SetEnabled(true);
                    }

                    //텍스트 표기
                    setTimeout("ContentDisplayTimeout()", 300);
                } else if (job == "SAVE") {
                    alert("저장 되었습니다.");
                    btnClose_Click(null, null);

                } else if (job == "DELETE") {
                    alert("삭제 되었습니다.");
                    btnClose_Click(null, null);

                }
                mastPageFooter(msg);
            }

            mastLoadingPanelHide();
        }

        function ContentDisplayTimeout() {
            var sHTML = memoContent.GetText();
            oEditors.getById["ir1"].exec("PASTE_HTML", [sHTML]);
        }


        // **************************************************************

        function GridEndCallback(s, e) {
            Grid.SetFocusedRowIndex(-1);

            var vFlag = s.cp_ret_flag.toString();
            var vMsg = s.cp_ret_message.toString();
            var vJob = s.cp_ret_job.toString();

            mastLoadingPanelHide();

            if (vFlag == "N") {
                mastPageFooterErr(vMsg);
            } else {
                if (vJob == "SAVE") {
                    alert("저장 되었습니다.");
                    btnClose_Click(null, null);
                }
            }
        }

        function GridCallbackError(s, e) {
            if (e.message != "") {
                mastPageFooterErr(e.message);
                mastLoadingPanelHide();
            }
        }


        var fieldName;
        function GridStartEditing(s, e) {
            fieldName = e.focusedColumn.fieldName;
        }


        function GridEndEditing(s, e) {


        }


        function btnSearch_Click()
        {            
            Grid.PerformCallback();
        }


        function chkGridCheckedChanged(s, e)
        {
            var chked = s.GetValue();
            var rCnt = 0;

            rCnt = Grid.GetVisibleRowsOnPage();

            if (rCnt > 0)
            {
                if (chked == true)
                {
                    for (var i = 0 - rCnt; i < 0; i++)
                    {
                        Grid.batchEditApi.SetCellValue(i, "UseYn", "Y");
                    }

                    for (var i = 0; i < rCnt; i++)
                    {
                        Grid.batchEditApi.SetCellValue(i, "UseYn", "Y");
                    }
                }
                else
                {
                    for (var i = 0 - rCnt; i < 0; i++)
                    {
                        Grid.batchEditApi.SetCellValue(i, "UseYn", "N");
                    }

                    for (var i = 0; i < rCnt; i++)
                    {
                        Grid.batchEditApi.SetCellValue(i, "UseYn", "N");
                    }
                }
            }
        }

    </script>

    <table style="width: 100%;">
        <tr>
            <td>
                <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="검색" Width="80" AutoPostBack="false">
                    <Image IconID="actions_search_16x16devav"></Image>
                    <ClientSideEvents Click="btnSearch_Click" />
                </dx:ASPxButton>
            </td>
            <td align="right">
                <dx:ASPxButton ID="btnSave" ClientInstanceName="btnSave" runat="server" AutoPostBack="false" Text="저장" Width="80">
                    <Image IconID="actions_save_16x16devav"></Image>
                    <ClientSideEvents Click="btnSave_Click" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnDelete" ClientInstanceName="btnDelete" runat="server" AutoPostBack="false" Text="삭제" Width="80">
                    <Image IconID="edit_delete_16x16"></Image>
                    <ClientSideEvents Click="btnDelete_Click" />
                </dx:ASPxButton>
                <img src="/SmartPro/Contents/Images/dot_ver.gif" />
                <dx:ASPxButton ID="btnClose" ClientInstanceName="btnClose" runat="server" AutoPostBack="false" Text="닫기" Width="80">
                    <Image IconID="richedit_closeheaderandfooter_16x16"></Image>
                    <ClientSideEvents Click="btnClose_Click" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>

    <table style="height: 5px;"></table>


    <table style="width: 100%;">
         <colgroup>
            <col style="width: 31%;" />
            <col style="width: 1%;"/>
            <col style="width: 69%;" />
        </colgroup>
        <tr>
            <td style="vertical-align: top;">

                <!-- SCM 선택 그리드 -->
                <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%" AutoGenerateColumns="false"
                    OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" 
                    KeyFieldName="UserID;UserName" 
                    OnAfterPerformCallback="Grid_AfterPerformCallback" OnCustomColumnDisplayText="Grid_CustomColumnDisplayText"
                    OnBatchUpdate="Grid_BatchUpdate">
    
                    <Styles Header-HorizontalAlign="Center" Cell-Wrap="False"/>
                    <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" ShowStatusBar="Hidden" />
                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="false" AllowSelectSingleRowOnly="false" ColumnResizeMode="Control" AllowDragDrop="false"  />
                    <SettingsPager Mode="ShowPager" PageSize="100" />

                    <SettingsEditing Mode="Batch">
                        <BatchEditSettings EditMode="Cell" StartEditAction="Click" ShowConfirmOnLosingChanges="false" 
                            AllowValidationOnEndEdit="false" KeepChangesOnCallbacks="False" AllowEndEditOnValidationError="false" />
                    </SettingsEditing>

                    <%--<SettingsLoadingPanel Mode="Disabled" />--%>        
        
                    <ClientSideEvents EndCallback="GridEndCallback" />
    
                    <TotalSummary>
                        <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="MsMachineCode"  />
                    </TotalSummary>

                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="No" Caption="No" Width="30" CellStyle-HorizontalAlign="Center" />
                        
                        <dx:GridViewDataCheckColumn FieldName="UseYn" Width="30px" HeaderStyle-Paddings-Padding="0" FixedStyle="Left" PropertiesCheckEdit-AllowGrayed="false" CellStyle-ForeColor="Black">
                            <HeaderTemplate>
                                <dx:ASPxCheckBox ID="GridChk" ClientInstanceName="GridChk" runat="server">
                                    <ClientSideEvents ValueChanged="chkGridCheckedChanged" />
                                </dx:ASPxCheckBox>
                            </HeaderTemplate>
                            <PropertiesCheckEdit ValueChecked="Y" ValueUnchecked="N" ValueType="System.String" Style-HorizontalAlign="Center" />
                        </dx:GridViewDataCheckColumn>
                        
                        <%--
                        <dx:GridViewCheckBoxClu ShowSelectCheckbox="True" Caption=" " VisibleIndex="1" Width="35" HeaderStyle-Paddings-Padding="0"
	                        HeaderStyle-HorizontalAlign="Center">
	                        <HeaderTemplate>
		                        <dx:ASPxCheckBox ID="SelectAllCheckBox" ClientInstanceName="SelectAllCheckBox" runat="server"
			                        ClientSideEvents-CheckedChanged="function(s, e) { Grid.SelectAllRowsOnPage(s.GetChecked()); }" />
	                        </HeaderTemplate>
                        </dx:GridViewCheckBoxClu>           
                        --%>

                        <dx:GridViewDataTextColumn FieldName="UserID" Caption="사용자 ID" Width="35%" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="UserName" Caption="사용자명" Width="65%"></dx:GridViewDataTextColumn>                        
                    </Columns>
                </dx:ASPxGridView>
                

            </td>
            <td>&nbsp</td>
            <td style="vertical-align: top;">

                <table style="width: 100%;" class="smart_table_head"></table>

                <table style="width: 100%;" class="smart_table_default">
                    <colgroup>
                        <col width="100px;" />
                        <col width="*" />
                        <col width="100px;" />
                        <col width="200px;" />
                    </colgroup>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblTitle" runat="server" Text="제 목"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtTitle" ClientInstanceName="txtTitle" runat="server" Width="100%"></dx:ASPxTextBox>
                        </td>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblImportance" runat="server" Text="중요도"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxRadioButtonList ID="rbImportance" ClientInstanceName="rbImportance" runat="server" ValueType="System.String" 
                                Paddings-Padding="0" Border-BorderWidth="0" RepeatColumns="2" Width="100%">
                                <Items>
                                    <dx:ListEditItem Text="기본" Value="N" Selected="true" />
                                    <dx:ListEditItem Text="중요" Value="Y" />
                                </Items>
                            </dx:ASPxRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblPw" runat="server" Text="비번"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <table style="width: 100%;">
                                <colgroup>
                                    <col width="100px;" />
                                    <col width="1" />
                                    <col width="*" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dx:ASPxTextBox ID="txtPw" ClientInstanceName="txtPw" runat="server" Width="100%" Password="true" MaxLength="10"></dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp</td>
                                    <td>
                                        <dx:ASPxLabel ID="lblPwInfo" runat="server" ForeColor="Brown" Text="* 비밀번호는 최소 4자, 최대 10자까지 입니다."></dx:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblEmail" runat="server" Text="이메일"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtEmail" ClientInstanceName="txtEmail" runat="server" Width="100%"></dx:ASPxTextBox>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;" class="smart_table_foot"></table>

                <table style="height: 5px;"></table>

                <textarea name="ir1" id="ir1" rows="10" cols="100" style="width:100%; height:400px; display:none;"></textarea>
                <script type="text/javascript">
                    var oEditors = [];
                    nhn.husky.EZCreator.createInIFrame({
                        oAppRef: oEditors,
                        elPlaceHolder: "ir1",
                        sSkinURI: "/SmartPro/Contents/smarteditor2/SmartEditor2Skin.html",
                        htParams: {
                            bUseToolbar: true,				// 툴바 사용 여부 (true:사용/ false:사용하지 않음)
                            bUseVerticalResizer: true,		// 입력창 크기 조절바 사용 여부 (true:사용/ false:사용하지 않음)
                            bUseModeChanger: true,			// 모드 탭(Editor | HTML | TEXT) 사용 여부 (true:사용/ false:사용하지 않음)
                            //aAdditionalFontList : aAdditionalFontSet,		// 추가 글꼴 목록
                            fOnBeforeUnload: function () {
                                //alert("완료!");
                            }
                        }, //boolean
                        fOnAppLoad: function () {
                            //예제 코드
                            //oEditors.getById["ir1"].exec("PASTE_HTML", ["로딩이 완료된 후에 본문에 삽입되는 text입니다."]);
                        },
                        fCreator: "createSEditor2"
                    });

                    function pasteHTML() {
                        var sHTML = "<span style='color:#FF0000;'>이미지도 같은 방식으로 삽입합니다.<\/span>";
                        oEditors.getById["ir1"].exec("PASTE_HTML", [sHTML]);
                    }

                    function showHTML() {
                        var sHTML = oEditors.getById["ir1"].getIR();
                        alert(sHTML);
                    }

                    function submitContents(elClickedObj) {
                        oEditors.getById["ir1"].exec("UPDATE_CONTENTS_FIELD", []);	// 에디터의 내용이 textarea에 적용됩니다.

                        try {
                            elClickedObj.form.submit();
                        } catch (e) { }
                    }

                    function setDefaultFont() {
                        var sDefaultFont = '궁서';
                        var nFontSize = 24;
                        oEditors.getById["ir1"].setDefaultFont(sDefaultFont, nFontSize);
                    }

                </script>

                <dx:ASPxMemo ID="memoContent" ClientInstanceName="memoContent" runat="server" 
                    ClientVisible="false" Height="300px" Width="100%" Border-BorderWidth="0"></dx:ASPxMemo>                   

                <dx:ASPxTextBox ID="txtBoard_ID" ClientInstanceName="txtBoard_ID" Text="0" runat="server" ClientVisible="false"></dx:ASPxTextBox>
                <dx:ASPxTextBox ID="txtNum" ClientInstanceName="txtNum" Text="0" runat="server" ClientVisible="false"></dx:ASPxTextBox>

                <dx:ASPxTextBox ID="txtWriter_ID" ClientInstanceName="txtWriter_ID" Text="0" runat="server" ClientVisible="false"></dx:ASPxTextBox>
                <dx:ASPxTextBox ID="txtUserID" ClientInstanceName="txtUserID" Text="0" runat="server" ClientVisible="false"></dx:ASPxTextBox>
                <dx:ASPxTextBox ID="txtOrigin_Pw" ClientInstanceName="txtOrigin_Pw" Text="0" runat="server" ClientVisible="false"></dx:ASPxTextBox>

                <dx:ASPxCallbackPanel ID="callBackPanel" ClientInstanceName="callBackPanel" OnCallback="callBackPanel_Callback" runat="server">
                    <ClientSideEvents EndCallback="function(s, e) { panelEndCallback(s, e) } " />
                    <PanelCollection>
                        <dx:PanelContent>

                            <dx:ASPxHiddenField ID="hidField" ClientInstanceName="hidField" runat="server"></dx:ASPxHiddenField>

                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>

            </td>
        </tr>
    </table>
    
    <!-- Hidden -->
    <dx:ASPxTextBox ID="txtRow" ClientInstanceName="txtRow" runat="server" ClientVisible="false"></dx:ASPxTextBox>

</asp:Content>