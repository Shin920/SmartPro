<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterPage.master" AutoEventWireup="true" CodeFile="OM034.aspx.cs" Inherits="MNG_OM_Master_OM034_OM034" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">

    <script type="text/javascript">

        function init() {
            jqUpdateSizeH();
            btnDelete.SetEnabled(false);
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 100;  // 상단 제외 영역 높이
            var h_min = 70;  //grid 최소 높이

            if (h - h_static > h_min) {
                Grid01.SetHeight(h - h_static);
            } else {
                Grid01.SetHeight(h_min);
            }
        };
        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        //popup
        //부서팝업
        function pupDeptClick(sGubun, sFlag) {
            v_mast_pop_callback = callbackDeptMaster

            var sCode = txtDeptCode.GetText();
            var sName = txtDeptName.GetText();

            if (sGubun == "C") {
                // 클릭
                mastCommonPopupShow("&Gubun=Dept&Condition=DeptCode/DeptName/&W=&Code=" + sCode + "&Name=" + sName, "부서정보", 350, 400);

            } else if (sGubun == "K") {
                if (sFlag == "C") {
                    sName = "";
                }
                else if (sFlag == "N") {
                    sCode = "";
                }
                if (sCode != "" || sName != "") {
                    // 키보드
                    mastCodeCallback("&Gubun=Dept&Condition=DeptCode/DeptName/&W=" + "&Code=" + sCode + "&Name=" + sName);
                }
            }
        }

        function callbackDeptMaster() {
            if (v_mast_pop_code != "") {
                txtDeptCode.SetText(v_mast_pop_code);
                txtDeptName.SetText(v_mast_pop_name);
            }
        }

        //부서팝업2
        function pupDept2Click(sGubun, sFlag) {
            v_mast_pop_callback = callbackDeptMaster2

            var sCode = txtDeptCode2.GetText();
            var sName = txtDeptName2.GetText();

            if (sGubun == "C") {
                // 클릭
                //mastPopDeptShow("&Code=" + sCode + "&Name=" + sName);
                mastPopTreeDeptShow("");
            } else if (sGubun == "K") {
                if (sFlag == "C") {
                    sName = "";
                }
                else if (sFlag == "N") {
                    sCode = "";
                }
                if (sCode != "" || sName != "") {
                    // 키보드
                    mastCodeCallback("&Gubun=Dept&Condition=DeptCode/DeptName/" + "&Code=" + sCode + "&Name=" + sName);
                }
            }
        }

        function callbackDeptMaster2() {
            if (v_mast_pop_code != "") {
                txtDeptCode2.SetText(v_mast_pop_code);
                txtDeptName2.SetText(v_mast_pop_name);
            }
        }

        //사원팝업
        function pupEmpClick(sGubun, sFlag) {

            v_mast_pop_callback = callbackEmpMaster
            var sDeptCode = txtDeptCode.GetText();

            var sCode = txtEmpCode.GetText();
            var sName = txtEmpName.GetText();

            if (sGubun == "C") {

                // 클릭
                mastCommonPopupShow("&Gubun=EMP&Condition=EmpCode/EmpName/&Code=" + sCode + "&Name=" + sName + "&W=AND B.DeptCode =" + sDeptCode, "담당자", 350, 400);

            } else if (sGubun == "K") {
                if (sFlag == "C") {
                    sName = "";
                }
                else if (sFlag == "N") {
                    sCode = "";
                }
                if (sCode != "" || sName != "") {
                    // 키보드
                    mastCodeCallback("&Gubun=EMP&Condition=EmpCode/EmpName/&Code=" + sCode + "&Name=" + sName + "&W=AND B.DeptCode =" + sDeptCode);
                }
            }
        }

        function callbackEmpMaster() {
            if (v_mast_pop_code != "") {
                txtEmpCode.SetText(v_mast_pop_code);
                txtEmpName.SetText(v_mast_pop_name);
            }
        }

        function btnSearch_Click() {
            mastLoadingPanelShow();
            Grid01.PerformCallback();
        }

        function btnNew_Click() {

            txtSaveMode.SetText("Y");
            btnDelete.SetEnabled(false);
            txtEmpCode2.Clear();
            txtEmpName2.Clear();
            ddlSiteName.SetSelectedIndex(0);
            txtDeptCode2.Clear();
            txtDeptName2.Clear();
            txtMailID.Clear();
            txtTelNo.Clear();
            chkInoutAdmin.Clear();
            txtHourCost.SetText("0");
            calEmpBDate.SetValue(new Date());
            cbJob.SetSelectedIndex(0);
        }

        function btnSave_Click() {
            var msg = hidLang.Get("M00004");

            if (!confirm(msg)) {
                return;
            }

            mastLoadingPanelShow();
            hidField.Clear();
            hidField.Set("job", "SAVE");
            hidField.Set("EmpBDate", js_devdateto_date8(calEmpBDate.GetDate()));
            callBackPanel.PerformCallback();
        }

        function btnDelete_Click() {

            var msg = hidLang.Get("M00005");
            if (!confirm(msg)) {
                return;
            }

            hidField.Clear();
            hidField.Set("job", "DELETE");
            callBackPanel.PerformCallback();
        }

        function Grid01EndCallback() {
            Grid01.SetFocusedRowIndex(-1);
            mastLoadingPanelHide();
        }

        function Grid01RowDblClick() {
            //Loading Show
            mastLoadingPanelShow();

            Grid01.GetRowValues(Grid01.GetFocusedRowIndex(), "EmpCode;SiteCode", Grid01RowDblClick2);
        }

        function Grid01RowDblClick2(values) {
            hidField.Clear();
            hidField.Set("job", "SELECT");
            hidField.Set("EmpCode", values[0].trim());
            hidField.Set("SiteCode", values[1].trim());

            callBackPanel.PerformCallback();
        }

        function panelEndCallback(s, e) {
            var striDigit1 = txtDigitNo_glAmnt1.GetText();
            var striRouding1 = txtDigitType_glAmnt1.GetText();
            var job = hidField.Get("job");
            var ret = hidField.Get("status");
            var msg = hidField.Get("msg");

            if (ret == "N") {
                //에러일때
                mastLoadingPanelHide();
                mastPageFooterErr(msg);
            }
            else if (ret == "Y") {
                if (job == "SELECT") {
                    
                    txtSaveMode.SetText("N");
                    txtEmpCode2.SetText(hidField.Get("EMPCODE"));
                    txtEmpName2.SetText(hidField.Get("EMPNAME"));
                    ddlSiteName.SetValue(hidField.Get("SITECODE"));
                    txtDeptCode2.SetText(hidField.Get("DEPTCODE"));
                    txtDeptName2.SetText(hidField.Get("DEPTNAME"));
                    txtMailID.SetText(hidField.Get("EMAIL"));
                    txtTelNo.SetText(hidField.Get("MOBILENO"));
                    chkInoutAdmin.SetValue(hidField.Get("INOUTADMIN") == "Y");
                    calEmpBDate.SetValue(js_date8to_devdate(hidField.Get("EMPBDATE")));

                    txtHourCost.SetText(hidField.Get("HOURCOST"));
                    cbJob.SetValue(hidField.Get("JOBWORK"));

                    btnDelete.SetEnabled(true);

                } else if (job == "SAVE") {
                    mastPageFooter(msg);
                    mastLoadingPanelHide();
                    btnDelete.SetEnabled(true);
                }
                else if (job == "DELETE") {
                    mastPageFooter("삭제가 완료 되었습니다.");
                    btnNew_Click();
                    btnSearch_Click();
                }
            }
        }

        function ucPhotoFileUploadComplete(s, e) {

        }

    </script>

    <table width="100%">
        <colgroup>
            <col width="50%" />
            <col width="1%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top;">

                <table width="100%" class="smart_table_head"></table>               
                
                <table width="100%" class="smart_table_default">
                    <colgroup>
                        <col width="20%" />
                        <col width="67%" />
                        <col width="13%" />
                    </colgroup>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblSite" runat="server" Text="소속사업장"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxComboBox ID="ddlSiteCode" ClientInstanceName="ddlSiteCode" runat="server" Width="50%" 
                                ValueField="SiteCode" TextField="SiteName" ValueType="System.String"></dx:ASPxComboBox>
                        </td>
                        <td class="smart_table_td_default">
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="조회" AutoPostBack="false" UseSubmitBehavior="false">
                                <Image IconID="actions_search_16x16devav"></Image>
                                <ClientSideEvents Click="btnSearch_Click" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                <table width="100%" class="smart_table_foot"></table>

                <table style="height: 5px;"></table>

                <dx:ASPxGridView ID="Grid01" ClientInstanceName="Grid01" runat="server" Width="100%" AutoGenerateColumns="false"
                    OnCustomCallback="Grid01_CustomCallback" OnDataBinding="Grid01_DataBinding"
                    KeyFieldName="EmpCode;EmpName;DeptCode;DeptName;SiteCode;SiteName;HourCost"
                    OnAfterPerformCallback="Grid01_AfterPerformCallback" OnCustomColumnDisplayText="Grid01_CustomColumnDisplayText">

                    <Styles Header-HorizontalAlign="Center" Cell-Wrap="False" />
                    <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" />
                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
                    <SettingsPager Mode="ShowAllRecords" />
                    <%--<SettingsLoadingPanel Mode="Disabled" />--%>

                    <TotalSummary>
                        <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="UserGroup" />
                    </TotalSummary>

                    <ClientSideEvents EndCallback="Grid01EndCallback" RowDblClick="Grid01RowDblClick" />

                    <Columns>
                        <dx:GridViewDataColumn Caption="No" ReadOnly="true" Width="30" CellStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>

                        <dx:GridViewDataTextColumn FieldName="EmpCode" Caption="사원코드" Width="15%" CellStyle-HorizontalAlign="Center" />
                        <dx:GridViewDataTextColumn FieldName="EmpName" Caption="사원명" Width="20%" CellStyle-HorizontalAlign="Left" />
                        <dx:GridViewDataTextColumn FieldName="DeptCode" Caption="부서코드" Width="15%" CellStyle-HorizontalAlign="Center" />
                        <dx:GridViewDataTextColumn FieldName="DeptName" Caption="부서명" Width="27%" CellStyle-HorizontalAlign="Left" />
                        <dx:GridViewDataTextColumn FieldName="SiteCode" Visible="false" />
                        <dx:GridViewDataTextColumn FieldName="SiteName" Caption="소속사업장" Width="23%" CellStyle-HorizontalAlign="Left" />
                        <dx:GridViewDataTextColumn FieldName="HourCost" Caption="시급" Width="0%" CellStyle-HorizontalAlign="Right" />
                    </Columns>
                </dx:ASPxGridView>

            </td>
            <td>&nbsp</td>
            <td style="vertical-align: top;">
                <table width="100%" class="smart_table_head"></table>
                <table width="100%" class="smart_table_default">
                    <colgroup>
                        <col width="30%" />
                        <col width="70%" />
                    </colgroup>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblEmpCode" runat="server" Text="사원코드"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtEmpCode2" ClientInstanceName="txtEmpCode2" runat="server" Width="100%" MaxLength="8" ></dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblEmpName" runat="server" Text="사원명"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtEmpName2" ClientInstanceName="txtEmpName2" runat="server" Width="100%" MaxLength="20" ></dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblSiteName" ClientInstanceNam="lblSiteName" runat="server" Text="소속사업장"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxComboBox ID="ddlSiteName" ClientInstanceName="ddlSiteName" runat="server" Width="100%" 
                                ValueField="SiteCode" TextField="SiteName" ValueType="System.String"></dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblDept2" runat="server" Text="부서"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <table style="width: 100%;">
                                <colgroup>
                                    <col width="35%" />
                                    <col width="1" />
                                    <col width="55%" />
                                    <col width="3" />
                                    <col width="10%" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dx:ASPxTextBox ID="txtDeptCode2" ClientInstanceName="txtDeptCode2" runat="server" Width="100%" MaxLength="8" >
                                            <ClientSideEvents TextChanged="function(s, e) { pupDept2Click('K', 'C'); }"
                                                KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtDeptName2" ClientInstanceName="txtDeptName2" runat="server" Width="100%" MaxLength="20" >
                                            <ClientSideEvents TextChanged="function(s, e) { pupDept2Click('K', 'N'); }"
                                                KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td><img src="/SmartPro/Contents/Images/dot_ver.gif" /></td>
                                    <td>
                                        <dx:ASPxImage ID="pupDept2" ClientInstanceName="pupDept2" runat="server" Cursor="pointer" ImageUrl="/SmartPro/Contents/Images/popup.gif">
                                            <ClientSideEvents Click="function(s, e) { pupDept2Click('C'); }" />
                                        </dx:ASPxImage>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblMailID" runat="server" Text="메일ID"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtMailID" ClientInstanceName="txtMailID" runat="server" Width="100%" MaxLength="40" ></dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblTelNo" runat="server" Text="전화번호"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtTelNo" ClientInstanceName="txtTelNo" runat="server" Width="100%" MaxLength="20" ></dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblEmpBdate" runat="server" Text="입사일"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxDateEdit ID="calEmpBDate" ClientInstanceName="calEmpBDate" runat="server" Width="100%" ></dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblJob" runat="server" Text="직무"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxComboBox ID="cbJob" ClientInstanceName="cbJob" runat="server" Width="100%" TextField="CodeName" ValueField="CodeCode" ValueType="System.String"></dx:ASPxComboBox>
                        </td>
                    </tr>                    
                </table>
                <table width="100%" class="smart_table_foot"></table>

                <table width="100%" style="margin-top: 7px; margin-bottom: 7px;">
                    <tr>
                        <td align="right">
                            <dx:ASPxButton ID="btnNew" runat="server" AutoPostBack="false" Text="신규" Width="80" UseSubmitBehavior="false">
                                <Image IconID="actions_new_16x16office2013"></Image>
                                <ClientSideEvents Click="btnNew_Click" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnSave" runat="server" AutoPostBack="false" Text="저장" Width="80" UseSubmitBehavior="false">
                                <Image IconID="actions_save_16x16devav"></Image>
                                <ClientSideEvents Click="btnSave_Click" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnDelete" ClientInstanceName="btnDelete" runat="server" AutoPostBack="false" Text="삭제" Width="80" UseSubmitBehavior="false">
                                <Image IconID="edit_delete_16x16"></Image>
                                <ClientSideEvents Click="btnDelete_Click" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <%-- 숨김필드 --%>
    <dx:ASPxTextBox ID="txtSaveMode" ClientInstanceName="txtSaveMode" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtDigitNo_glAmnt1" ClientInstanceName="txtDigitNo_glAmnt1" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtDigitType_glAmnt1" ClientInstanceName="txtDigitType_glAmnt1" runat="server" ClientVisible="false"></dx:ASPxTextBox>

    <dx:ASPxLabel ID="lblInoutAdmin" runat="server" Text="출고권한" ClientVisible="false"></dx:ASPxLabel>
    <dx:ASPxCheckBox ID="chkInoutAdmin" ClientInstanceName="chkInoutAdmin" runat="server" Checked="false" ClientVisible="false"></dx:ASPxCheckBox>
    <dx:ASPxLabel ID="lblHourCost" runat="server" Text="원가계산" ClientVisible="false"></dx:ASPxLabel>
    <dx:ASPxTextBox ID="txtHourCost" ClientInstanceName="txtHourCost" runat="server" Width="100%" MaxLength="16" ClientVisible="false"></dx:ASPxTextBox>

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