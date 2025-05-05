<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="OM068.aspx.cs" Inherits="MNG_OM_Org_OM068_OM068" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            jqUpdateSizeH();
            cbSiteCode.SetSelectedIndex(0);
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 130;  // 상단 제외 영역 높이
            var h_min = 30;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);
                Grid2.SetHeight(h - 163);
            } else {
                Grid.SetHeight(h_min);
                Grid2.SetHeight(h_min);
            }
        };
        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        function GridRowClick() {

            var val = Grid.GetRowKey(Grid.GetFocusedRowIndex());

            if (val != null) {
                mastLoadingPanelShow();

                var values = val.split("|");

                hidField.Clear();
                hidField.Set("job", "S");
                hidField.Set("WhCode", values[0].trim());
                callBackPanel.PerformCallback();
            }
        }

        function GridEndCallback() {
            Grid.SetFocusedRowIndex(-1);
            mastLoadingPanelHide();
        }

        function Grid2EndCallback(s, e) {
            for (i = 0; i < Grid2.pageRowCount; i++) {
                Grid2.batchEditApi.SetCellValue(i, "SaveCheck", "N", null, true);
            }
            Grid2.SetFocusedRowIndex(-1);
            mastLoadingPanelHide();
        }

        function Grid2CallbackError(s, e) {
            if (e.message != "") {
                mastPageFooterErr(e.message);
                mastLoadingPanelHide();
            }
        }

        var fieldName;
        function Grid2StartEditing(s, e) {
            fieldName = e.focusedColumn.fieldName;
        }

        function btnSearch_Click() {
            mastLoadingPanelShow();
            Grid.PerformCallback();
        }

        function btnSearch2_Click() {
            mastLoadingPanelShow();
            Grid2.PerformCallback();
        }

        function btnNew_Click() {

            txtSaveMode.SetText("Y");

            txtWhName.Clear();
            cbSiteCode.Clear();

            Grid.PerformCallback("NEW");
        }

        function btnSave_Click() {

            //업데이트 여부 확인 하여 실행
            if (Grid2.batchEditApi.HasChanges())
                Grid2.UpdateEdit();
            else {
                Grid2.PerformCallback("UPDATE");
            }

            //var irows = Grid2.batchEditHelper.GetEditState().insertedRowValues; 
            //var vrows = Grid2.pageRowCount;
            //var count = Object.keys(irows).length + vrows;

            //var msg = hidLang.Get("M00004");

            //if (!confirm(msg)) {
            //    return;
            //}

            //if (count < 1) {
            //    alert(hidLang.Get("M00090"));
            //    return;
            //}

            //mastLoadingPanelShow();
            ////for (i = 0; i < Grid2.pageRowCount; i++) {
            ////    Grid2.batchEditApi.SetCellValue(i, "SaveCheck", "Y", null, true);
            ////}
            //Grid2.UpdateEdit();
        }

        function btnPrint_Click() {
            var vSiteCode = cbSiteCode.GetValue();
            var vWhName = txtWhName.GetText();
            var vParam = vSiteCode + "|" + vWhName;

            mastPopFormShow("보관장소관리", "/SmartPro/Common/PrintForm/ReportPage.aspx?Action=Load&RID=OM068&Mode=Print&Param=" + vParam, 800, 550)
        }

        function panelEndCallback(s, e) {
            var job = hidField.Get("job");
            var ret = hidField.Get("status");
            var msg = hidField.Get("msg");

            if (ret == "N") {
                mastPageFooterErr(msg);
            }
            else if (ret == "Y") {

                if (job == "S") {
                    txtSaveMode.SetText("N");
                    txtWhCode.SetText(hidField.Get("WHCODE"));
                    txtWhName2.SetText(hidField.Get("WHNAME"));
                    
                    Grid2.PerformCallback();

                    mastLoadingPanelHide();

                } else if (job == "SAVE") {
                    mastPageFooter(msg);
                }
            }

            mastLoadingPanelHide();
        }

        function btnLabelPrt_Click() {

            var vLocationCode = "";
            var vParam = "";

            for (i = 0; i < Grid2.pageRowCount; i++) {

                if (Grid2.batchEditApi.GetCellValue(i, "Chk") == "Y") {
                    vLocationCode = Grid2.batchEditApi.GetCellValue(i, "LocationCode");

                    vParam += vLocationCode + "|"
                }

            }

            if (vParam == "") {
                alert("출력할 데이터를 선택해 주세요.")
                return;
            }

            mastPopFormShow("", "/SmartPro/Common/PrintForm/ReportPage_Sub.aspx?Action=Load&RID=OM068-1&Mode=Print&Param=" + vParam, 800, 550)
        }

        function btnQrPrt_Click() {

            var vLocationCode = "";
            var vParam = "";

            for (i = 0; i < Grid2.pageRowCount; i++) {

                if (Grid2.batchEditApi.GetCellValue(i, "Chk") == "Y") {
                    vLocationCode = Grid2.batchEditApi.GetCellValue(i, "LocationCode");

                    vParam += vLocationCode + "|"
                }

            }

            if (vParam == "") {
                alert("출력할 데이터를 선택해 주세요.")
                return;
            }

            mastPopFormShow("", "/SmartPro/Common/PrintForm/ReportPage_Sub.aspx?Action=Load&RID=OM068-2&Mode=Print&Param=" + vParam, 800, 550)
        }

        function chkGridCheckedChanged(s, e) {
            for (i = 0; i < Grid2.pageRowCount; i++) {
                if (s.GetValue()) {
                    Grid2.batchEditApi.SetCellValue(i, "Chk", "Y", null, true);
                } else {
                    Grid2.batchEditApi.SetCellValue(i, "Chk", "N", null, true);
                }
            }
        }

        function chkGridCheckedChanged2(s, e) {
            for (i = 0; i < Grid2.pageRowCount; i++) {
                if (s.GetValue()) {
                    Grid2.batchEditApi.SetCellValue(i, "UseYN", "Y", null, true);
                } else {
                    Grid2.batchEditApi.SetCellValue(i, "UseYN", "N", null, true);
                }
            }
        }

    </script>


    <table style="width: 100%;">
        <colgroup>
            <col width="45%" />
            <col width="1%" />
            <col width="55%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top;">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <table style="width: 100%;" class="smart_table_head"></table>
                            <table style="width: 100%;" class="smart_table_default">
                                <colgroup>
                                    <col width="15%" />
                                    <col width="35%" />
                                    <col width="15%" />
                                    <col width="35%" />
                                </colgroup>
                                <tr>
                                    <th class="smart_table_th_center">
                                        <dx:ASPxLabel ID="lblSite" ClientInstanceNam="lblSite" runat="server" Text="공장" Width="100%"></dx:ASPxLabel>
                                    </th>
                                    <td class="smart_table_td_default">
                                        <dx:ASPxComboBox ID="cbSiteCode" ClientInstanceName="cbSiteCode" runat="server" Width="100%" TextField="SiteName" ValueField="SiteCode">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <th class="smart_table_th_center">
                                        <dx:ASPxLabel ID="lblWhName" ClientInstanceName="lblWhName" runat="server" Text="창고명" Width="100%"></dx:ASPxLabel>
                                    </th>
                                    <td class="smart_table_td_default">
                                        <dx:ASPxTextBox ID="txtWhName" ClientInstanceName="txtWhName" runat="server" Width="100%"></dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

                <table style="height: 5px;"></table>

                <table style="width: 100%;">
                    <tr>
                        <td align="right" style="margin-right: 5px;">
                            <img src="/SmartPro/Contents/Images/dot_ver.gif" style="visibility: hidden;" />
                            <dx:ASPxButton ID="btnNew" ClientInstanceName="btnNew" runat="server" AutoPostBack="false" Text="신규" Width="80">
                                <Image IconID="actions_new_16x16office2013"></Image>
                                <ClientSideEvents Click="btnNew_Click" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Paddings-Padding="0" Text="검색" Width="80" AutoPostBack="false">
                                <Image IconID="actions_search_16x16devav"></Image>
                                <ClientSideEvents Click="btnSearch_Click" />
                            </dx:ASPxButton>
                            <img src="/SmartPro/Contents/Images/dot_ver.gif" />
                            <dx:ASPxImage ID="btnPrint" ClientInstanceName="btnPrint" runat="server" Cursor="pointer" ImageUrl="/SmartPro/Contents/Images/print_button.gif">
                                <ClientSideEvents Click="function(s, e) { btnPrint_Click() }" />
                            </dx:ASPxImage>
                        </td>
                    </tr>
                </table>

                <table style="height: 5px;"></table>

                <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%" AutoGenerateColumns="false"
                    OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" 
                    KeyFieldName="WhCode;WhName" 
                    OnAfterPerformCallback="Grid_AfterPerformCallback" OnCustomColumnDisplayText="Grid_CustomColumnDisplayText">

                    <Styles Header-HorizontalAlign="Center" Cell-Wrap="False" />
                    <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" />
                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
                    <SettingsPager Mode="ShowAllRecords" />
                    <%--<SettingsLoadingPanel Mode="Disabled" />--%>

                    <TotalSummary>
                        <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="WhCode" />
                    </TotalSummary>

                    <ClientSideEvents EndCallback="GridEndCallback" RowDblClick="GridRowClick" />

                    <Columns>
                        <dx:GridViewDataColumn Caption="No" ReadOnly="true" Width="30" CellStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>

                        <dx:GridViewDataTextColumn FieldName="WhCode" Caption="창고코드" Width="30%" CellStyle-HorizontalAlign="Left" />
                        <dx:GridViewDataTextColumn FieldName="WhName" Caption="창고명" Width="70%" CellStyle-HorizontalAlign="Left" />
                    </Columns>
                </dx:ASPxGridView>
            </td>
            
            <td></td>
            
            <td style="vertical-align: top;">
                
                <table style="width: 100%;" class="smart_table_head"></table>
                <table style="width: 100%;" class="smart_table_default">
                    <colgroup>
                        <col width="30%" />
                        <col width="70%" />
                    </colgroup>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblWhCode" ClientInstanceNam="lblWhCode" runat="server" Text="창고코드" Width="100%"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxComboBox ID="txtWhCode" ClientInstanceName="txtWhCode" runat="server" ClientReadOnly ="true" Width="100%"></dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblWhName2" ClientInstanceName="lblWhName2" runat="server" Text="창고명" Width="100%"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtWhName2" ClientInstanceName="txtWhName2" ClientReadOnly ="true" runat="server" Width="100%"></dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblLocationName" ClientInstanceName="lblLocationName" runat="server" Text="보관장소명(검색)" Width="100%" ForeColor="#CC0099" Font-Bold="True"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtLocationName" ClientInstanceName="txtLocationName" runat="server" Width="100%"></dx:ASPxTextBox>
                        </td>
                    </tr>
                </table>

                <table width="100%" style="margin-top: 7px; margin-bottom: 7px;">
                    <tr>
                        <td align="right">
                            <dx:ASPxButton ID="btnSearch2" runat="server" AutoPostBack="false" Text="검색" Width="80">
                                <Image IconID="actions_search_16x16devav"></Image>
                                <ClientSideEvents Click="btnSearch2_Click" />
                            </dx:ASPxButton>
                            <img src="/SmartPro/Contents/Images/dot_ver.gif" />
                            <dx:ASPxButton ID="btnSave" runat="server" AutoPostBack="false" Text="저장" Width="80">
                                <Image IconID="actions_save_16x16devav"></Image>
                                <ClientSideEvents Click="btnSave_Click" />
                            </dx:ASPxButton>
                            <img src="/SmartPro/Contents/Images/dot_ver.gif" />
                                <dx:ASPxButton ID="btnLabelPrt" ClientInstanceName="btnLabelPrt" runat="server" Text="라벨출력" AutoPostBack="false" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="btnLabelPrt_Click" />
                                </dx:ASPxButton>
                            <img src="/SmartPro/Contents/Images/dot_ver.gif" />
                                <dx:ASPxButton ID="btnQrPrt" ClientInstanceName="btnQrPrt" runat="server" Text="QR출력" AutoPostBack="false" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="btnQrPrt_Click" />
                                </dx:ASPxButton>
                        </td>
                    </tr>
                </table>

                <dx:ASPxGridView ID="Grid2" ClientInstanceName="Grid2" runat="server" Width="100%" AutoGenerateColumns="false"
                    OnCustomCallback="Grid2_CustomCallback" OnDataBinding="Grid2_DataBinding" KeyFieldName="LocationCode" 
                    OnAfterPerformCallback="Grid2_AfterPerformCallback" OnCustomColumnDisplayText="Grid2_CustomColumnDisplayText"
                    OnCustomErrorText="Grid2_CustomErrorText" OnBatchUpdate="Grid2_BatchUpdate" SettingsBehavior-AllowSort="false">

                    <Styles Header-HorizontalAlign="Center" Cell-Wrap="False" />
                    <Settings VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" ShowFooter="true" ShowStatusBar="Hidden" />
                    <SettingsPager Mode="ShowAllRecords" />
                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="Control" />
                    <SettingsEditing Mode="Batch">
                        <BatchEditSettings EditMode="Cell" StartEditAction="Click" AllowEndEditOnValidationError="false"
                            AllowValidationOnEndEdit="false" ShowConfirmOnLosingChanges="false" />
                    </SettingsEditing>
                    <%--<SettingsLoadingPanel Mode="Disabled" />--%>

                 <%--   <TotalSummary>
                        <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="LocationCode" />
                    </TotalSummary>--%>

                    <ClientSideEvents EndCallback="Grid2EndCallback" CallbackError="Grid2CallbackError"
                        BatchEditStartEditing="Grid2StartEditing" />

                    <Columns>

                        <%--<dx:GridViewCommandColumn ShowDeleteButton="true" ShowRecoverButton="true" ShowNewButtonInHeader="true" Width="10%"></dx:GridViewCommandColumn>--%>

                        <dx:GridViewDataColumn Caption="No" ReadOnly="true" Width="5%" CellStyle-HorizontalAlign="Center">
                            <EditFormSettings Visible="False" />
                        </dx:GridViewDataColumn>

                        <dx:GridViewDataCheckColumn FieldName="Chk" Caption="출력" Width="60" HeaderStyle-Paddings-Padding="0">
                        <HeaderTemplate>
                            <dx:ASPxCheckBox ID="Chk" ClientInstanceName="Chk" runat="server" Text="출력">
                                <ClientSideEvents CheckedChanged="chkGridCheckedChanged" />
                            </dx:ASPxCheckBox>
                        </HeaderTemplate>
                        <PropertiesCheckEdit ValueChecked="Y" ValueUnchecked="N" ValueType="System.String" />
                    </dx:GridViewDataCheckColumn>

                        <dx:GridViewDataTextColumn FieldName="LocationCode" Caption="보관장소" Width="30%"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LocationName" Caption="보관장소명" Width="57%"></dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataCheckColumn FieldName="UseYN" Caption="사용" Width="10%" HeaderStyle-Paddings-Padding="0">
                            <HeaderTemplate>
                                <dx:ASPxCheckBox ID="UseYN" ClientInstanceName="UseYN" runat="server" Text="사용">
                                <ClientSideEvents CheckedChanged="chkGridCheckedChanged2" />
                            </dx:ASPxCheckBox>
                            </HeaderTemplate>
                            <PropertiesCheckEdit ValueChecked="Y" ValueUnchecked="N" ValueType="System.String">
                            </PropertiesCheckEdit>
                        </dx:GridViewDataCheckColumn>

                        <dx:GridViewDataTextColumn FieldName="SaveCheck" Width="0" Visible="false"></dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>
            </td>
        </tr>
        

    </table>

    <%-- 숨김필드 --%>
    <dx:ASPxTextBox ID="txtSaveMode" ClientInstanceName="txtSaveMode" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtSiteCode" ClientInstanceName="txtSiteCode" runat="server" ClientVisible="false"></dx:ASPxTextBox>

    <%-- 데이터 조회 콜백 판넬 (Ajax) --%>
    <dx:ASPxCallbackPanel ID="callBackPanel" ClientInstanceName="callBackPanel" OnCallback="callBackPanel_Callback" runat="server">
        <ClientSideEvents EndCallback="function(s, e) { panelEndCallback(s, e) } " />
        <PanelCollection>
            <dx:PanelContent>

                <dx:ASPxHiddenField ID="hidField" ClientInstanceName="hidField" runat="server"></dx:ASPxHiddenField>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <dx:ASPxHiddenField ID="hidLang" ClientInstanceName="hidLang" runat="server"></dx:ASPxHiddenField>
</asp:Content>
