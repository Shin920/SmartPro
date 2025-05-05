<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MASTER/MasterPage.master" CodeFile="OM044.aspx.cs" Inherits="MNG_OM_Master_CsMaster_OM044_OM044" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">

    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>

    <script type="text/javascript">

        //권한부여
        var vPermition_I = false;
        var vPermition_D = false;

        function init() {
            if (txtPermition_I.GetText() == "Y") {
                vPermition_I = true;
            }
            if (txtPermition_D.GetText() == "Y") {
                vPermition_D = true;
            }
            btnSave.SetEnabled(true && vPermition_I);
            btnDelete.SetEnabled(false);

            jqUpdateSizeH();
            //딜레이후 실행. 에디트 그리드 사이즈 관련.
            window.setTimeout(function () { GridS1.PerformCallback("NEW"); }, 100);
        }

        function jqUpdateSizeH() {
            // Get the dimensions of the viewport
            //var w = $(window).width();
            var h = $(window).height();
            var h_static = 180;  // 상단 제외 영역 높이  195
            var h_min = 70;  //grid 최소 높이
            var h_static2 = 680;  // 상단 제외 영역 높이

            if (h - h_static > h_min) {
                Grid.SetHeight(h - h_static);
                GridS1.SetHeight(h - h_static2);
            } else {
                Grid.SetHeight(h_min);
                GridS1.SetHeight(h_min);
            }
        };
        // resize event handler
        $(window).resize(jqUpdateSizeH);     // When the browser changes size

        
        //국가팝업
        function pupNationClick(sGubun, sFlag) {

            v_mast_pop_callback = callbackNationMaster

            var sCode = txtNationCode.GetText();
            var sName = txtNationName.GetText();

            if (sGubun == "C") {
                // 클릭
                mastCommonPopupShow("&Gubun=NA&Condition=NationCode/NationName/&Code=" + sCode + "&Name=" + sName, hidLang.Get("L04039"), 350, 400);

            } else if (sGubun == "K") {
                if (sFlag == "C") {
                    sName = "";
                }
                else if (sFlag == "N") {
                    sCode = "";
                }
                if (sCode != "" || sName != "") {
                    // 키보드
                    mastCodeCallback("&Gubun=NA&Condition=NationCode/NationName/&Code=" + sCode + "&Name=" + sName);
                }
            }
        }

        function callbackNationMaster() {
            if (v_mast_pop_code != "") {
                txtNationCode.SetText(v_mast_pop_code);
                txtNationName.SetText(v_mast_pop_name);
            }
        }

        //모기업팝업
        function pupParentCsClick(sGubun, sFlag) {
            v_mast_pop_callback = callbackParentCsMaster

            var sCode = txtParentCs.GetText();
            var sName = txtParentCsName.GetText();

            if (sGubun == "C") {
                // 클릭
                mastPopCustShow("&Code=" + sCode + "&Name=" + sName, hidLang.Get("L04041"));

            } else if (sGubun == "K") {
                if (sFlag == "C") {
                    sName = "";
                }
                else if (sFlag == "N") {
                    sCode = "";
                }
                if (sCode != "" || sName != "") {
                    // 키보드
                    mastCodeCallback("&Gubun=Cs" + "&Code=" + sCode + "&Name=" + sName);
                }
            }
        }

        function callbackParentCsMaster() {
            if (v_mast_pop_code != "") {
                txtParentCs.SetText(v_mast_pop_code);
                txtParentCsName.SetText(v_mast_pop_name);
            }
        }

        //은행
        function pupBankClick(sGubun, sFlag) {
            v_mast_pop_callback = callbackBankMaster

            var sCode = txtBankCode.GetText();
            var sName = txtBankName.GetText();

            if (sGubun == "C") {
                // 클릭
                mastPopCustShow("&Code=" + sCode + "&Name=" + sName + "&W= AND A.CsGubun = 'BNK'", "은행정보"); //L05404

            } else if (sGubun == "K") {
                if (sFlag == "C") {
                    sName = "";
                }
                else if (sFlag == "N") {
                    sCode = "";
                }
                if (sCode != "" || sName != "") {
                    // 키보드
                    mastCodeCallback("&Gubun=Cs" + "&Code=" + sCode + "&Name=" + sName + "&W= AND A.CsGubun = 'BNK'");
                }
            }
        }

        function callbackBankMaster() {
            if (v_mast_pop_code != "") {
                txtBankCode.SetText(v_mast_pop_code);
                txtBankName.SetText(v_mast_pop_name);
            }
        }

        function rdoDomesticChanged() {
            cmbRegionCode.PerformCallback();
        }

        function btnNew_S_Click() {
            cmbCsType_S.SetSelectedIndex(0);
            txtCsCode_S.SetText("");
            txtCsName_S.SetText("");
            cmbCsUse_S.SetValue("Y");

            Grid.PerformCallback("NEW");
        }

        function btnSearch_Click() {
            mastLoadingPanelShow();
            Grid.PerformCallback();
        }

        function GridEndCallback(s, e) {
            var vFlag = s.cp_ret_flag.toString();
            var vMsg = s.cp_ret_message.toString();

            if (vFlag == "N") {
                mastPageFooterErr(vMsg);
            } else {
                mastPageFooter(vMsg);
            }

            mastLoadingPanelHide();
        } 
            
        function GridRowDblClick(s, e) {

            var strVal = Grid.GetRowKey(Grid.GetFocusedRowIndex());
            if (strVal == "" || strVal == null) {
                return
            }

            txtCsCode.SetText(strVal);

            if (txtCsCode.GetText() == "") {
                return;
            }

            //mastLoadingPanelShow();

            hidField.Clear();
            hidField.Set("job", "SELECT");
            callBackPanel.PerformCallback();
            GridS1.PerformCallback();
        }

        function btnNew_Click() {

            txtSaveMode.SetText("Y");

            txtCsCode.Clear();
            txtCsName.Clear();
            txtCsNameFull.Clear();
            chkCsUse.SetValue(true);

            //tab1
            cmbCsType.SetSelectedIndex(0);
            chkArCheck.SetValue(false);
            chkApCheck.SetValue(false);
            chkOutCheck.SetValue(false);
            
            rdoCompany.Clear();
            rdoDomestic.Clear();
            txtNationCode.Clear();
            txtNationName.Clear();
            cmbRegionCode.SetSelectedIndex(0);
            txtCsZip.Clear();
            txtCsAddress.Clear();
            
            
            txtCsTel.Clear();
            txtCsFax.Clear();
            txtCsUrl.Clear();
            txtCsEmail.Clear();
            txtRegNo.Clear();
            txtCsIndustry.Clear();
            txtCsItems.Clear();
            txtParentCs.Clear();
            txtParentCsName.Clear();
            txtCsChief.Clear();
            
            cmbTaxCodeAr.SetSelectedIndex(0);
            cmbTaxCodeAp.SetSelectedIndex(0);
            cmbCsClass1.SetSelectedIndex(0);
            cmbCsClass2.SetSelectedIndex(0);
            
            
            txtCsDescr.Clear();
            txtInitial.Clear();

            //tab2
            cmbPayConditionAr.SetSelectedIndex(0);
            cmbPayConditionAp.SetSelectedIndex(0);
            cmbCsGrade.SetSelectedIndex(0);
            cmbCreditGrade.SetSelectedIndex(0);
            txtBankCode.Clear();
            txtBankName.Clear();
            txtAccountNo.Clear();
            cmbCurrCode.SetSelectedIndex(0);

            cmbIncoTerm.SetSelectedIndex(0);
            txtLeadTime.Clear();
            cmbTransCode.SetSelectedIndex(0);
            
            imgPackingLogo.SetImageUrl("");
            

            cbSendMethod.SetSelectedIndex(0);
            txtTranTelNo.Clear();
            txtTranEmail.Clear();
            txtTranFax.Clear();
            txtExtraPlan1.Clear();
            txtExtraPlan2.Clear();
            txtExtraPlan3.Clear();

            // 트러스빌 사용여부
            chkTrus_Use.SetValue(false);
            chkTrus_1.SetValue(false);
            chkTrus_2.SetValue(false);


            btnDelete.SetEnabled(false);
            btnSave.SetEnabled(true && vPermition_I);
            txtEmpCode.Clear();
            txtEmpName.Clear();
            GridS1.PerformCallback("NEW");
        }

        function btnSave_Click() {
            var vCsType = cmbCsType.GetValue();
            if (vCsType != "150" && vCsType != "250" && vCsType != "650")  // 해외매출처, 해외매입처, 공공기관 제외
            {
                if (txtRegNo.GetText() == "") {
                    mastPageFooterErr(hidLang.Get("M01018"));
                    return;
                }
            }
            if (cmbCsType.GetSelectedItem().GetColumnText("TypeCode") == "") {
                mastPageFooterErr(hidLang.Get("M01016"));
                return;
            }

            mastLoadingPanelShow();

            hidField.Clear();
            hidField.Set("job", "SAVECHECK");
            callBackPanel.PerformCallback();
        }

        function btnDelete_Click() {
            if (txtCsCode.GetText() == "") {
                return;
            }

            if (!confirm(hidLang.Get("M00005"))) {
                return;
            }

            mastLoadingPanelShow();

            hidField.Clear();
            hidField.Set("job", "DELETE");
            callBackPanel.PerformCallback();
        }

        function panelEndCallback(s, e) {
            var job = hidField.Get("job");
            var ret = hidField.Get("status");
            var msg = hidField.Get("msg");

            if (ret == "N") {
                //에러일때
                mastPageFooterErr(msg);
                mastLoadingPanelHide();
            } else if (ret == "Y")  {
                if (job == "SELECT") {

                    txtSaveMode.SetText("N");

                    txtCsCode.SetText(hidField.Get("CSCODE"));
                    txtCsName.SetText(hidField.Get("CSNAME"));
                    txtCsNameFull.SetText(hidField.Get("CSNAMEFULL"));
                    chkCsUse.SetValue(hidField.Get("CSUSE") == "Y");

                    //tab1
                    cmbCsType.SetValue(hidField.Get("CSTYPE"));
                    chkArCheck.SetValue(hidField.Get("ARCHECK") == "Y");
                    chkApCheck.SetValue(hidField.Get("APCHECK") == "Y");
                    chkOutCheck.SetValue(hidField.Get("OUTCHECK") == "Y");
                    
                    rdoCompany.SetValue(hidField.Get("COMCHECK"));
                    rdoDomestic.SetValue(hidField.Get("FOREIGNCHECK"));
                    rdoDomesticChanged();
                    txtNationCode.SetText(hidField.Get("NATIONCODE"));
                    txtNationName.SetText(hidField.Get("NATIONNAME"));
                    cmbRegionCode.SetValue(hidField.Get("REGIONCODE"));
                    txtCsZip.SetText(hidField.Get("CSZIP"));
                    txtCsAddress.SetText(hidField.Get("CSADDRESS"));
                    
                    
                    txtCsTel.SetText(hidField.Get("CSTEL"));
                    txtCsFax.SetText(hidField.Get("CSFAX"));
                    txtCsUrl.SetText(hidField.Get("CSURL"));
                    txtCsEmail.SetText(hidField.Get("CSEMAIL"));
                    txtRegNo.SetText(hidField.Get("REGNO"));
                    txtCsIndustry.SetText(hidField.Get("CSINDUSTRY"));
                    txtCsItems.SetText(hidField.Get("CSITEMS"));
                    txtParentCs.SetText(hidField.Get("PARENTCS"));
                    txtParentCsName.SetText(hidField.Get("PARENTCSNAME"));
                    txtCsChief.SetText(hidField.Get("CSCHIEF"));
                    
                    cmbTaxCodeAr.SetValue(hidField.Get("TAXCODEAR"));
                    cmbTaxCodeAp.SetValue(hidField.Get("TAXCODEAP"));
                    cmbCsClass1.SetValue(hidField.Get("CSCLASS1"));
                    cmbCsClass2.SetValue(hidField.Get("CSCLASS2"));
                                        
                    txtCsDescr.SetText(hidField.Get("CSDESCR"));
                    txtInitial.SetText(hidField.Get("CSINITIAL"));

                    //tab2
                    cmbPayConditionAr.SetValue(hidField.Get("PAYCONDITIONAR"));
                    cmbPayConditionAp.SetValue(hidField.Get("PAYCONDITIONAP"));
                    cmbCsGrade.SetValue(hidField.Get("CSGRADE"));
                    cmbCreditGrade.SetValue(hidField.Get("CREDITGRADE"));
                    txtBankCode.SetText(hidField.Get("BANKCODE"));
                    txtBankName.SetText(hidField.Get("BANKNAME"));
                    txtAccountNo.SetText(hidField.Get("ACCOUNTNO"));
                    cmbCurrCode.SetValue(hidField.Get("CURRCODE"));
                    
                    cmbIncoTerm.SetValue(hidField.Get("INCOTERM"));
                    txtLeadTime.SetText(hidField.Get("LEADTIME"));
                    cmbTransCode.SetValue(hidField.Get("TRANSCODE"));
                    if (hidField.Get("IMG_URL") != "" && hidField.Get("IMG_URL") != null) {
                        imgPackingLogo.SetImageUrl(hidField.Get("IMG_URL"));
                    } else {
                        imgPackingLogo.SetImageUrl(null);
                    }
                    
                    cbSendMethod.SetValue(hidField.Get("SENDMETHOD"));
                    txtTranTelNo.SetText(hidField.Get("TRANTELNO"));
                    txtTranEmail.SetText(hidField.Get("TRANEMAIL"));
                    txtTranFax.SetText(hidField.Get("TRANFAX"));
                    txtExtraPlan1.SetText(hidField.Get("EXTRAFIELD1"))
                    txtExtraPlan2.SetText(hidField.Get("EXTRAFIELD2"))
                    txtExtraPlan3.SetText(hidField.Get("EXTRAFIELD3"))
                    
                    // 트러스빌 사용여부
                    chkTrus_Use.SetValue(hidField.Get("TRUSBILL_YN") == "Y");

                    // 트러스빌
                    if (hidField.Get("TRUSBILL_STATUS") == "A") {
                        chkTrus_1.SetValue(true);
                        chkTrus_2.SetValue(false);
                    } else if (hidField.Get("TRUSBILL_STATUS") == "B") {
                        chkTrus_1.SetValue(false);
                        chkTrus_2.SetValue(true);
                    } else {
                        chkTrus_1.SetValue(false);
                        chkTrus_2.SetValue(false);
                    }

                    txtEmpCode.SetText(hidField.Get("EMPCODE"));
                    txtEmpName.SetText(hidField.Get("EMPNAME"));
                    

                    btnDelete.SetEnabled(true && vPermition_D);

                    mastLoadingPanelHide();

                } else if (job == "SAVECHECK") {

                    if (msg != "") {
                        if (!confirm(msg)) {
                            mastLoadingPanelHide();
                            return;
                        }
                    }

                    hidField.Clear();
                    hidField.Set("job", "SAVE");
                    hidField.Set("TypeCode", cmbCsType.GetSelectedItem().GetColumnText("TypeCode"));

                    callBackPanel.PerformCallback();

                } else if (job == "SAVE") {

                    //채번된 거래처 코드 지정.
                    txtCsCode.SetText(hidField.Get("CsCode"));
                    txtSaveMode.SetText("N");
                    mastLoadingPanelHide();

                } else if (job == "DELETE") {
                    mastPageFooterErr(hidLang.Get("M00003"));
                    Grid.PerformCallback();
                    btnNew_Click(null, null);
                    mastLoadingPanelHide();
                }
            }
        }

        function filePackingLogoFileUploadComplete(e) {
            var sText = e.errorText;
            if (sText != "") {
                mastPageFooterErr(sText);
            } else {
                mastPageFooter(hidLang.Get("M00182"));
            }
        }

        var uploadFileName = "";
        function btnUpload_Click() {
            try {
                var msg = '';
                if (filePackingLogo.GetText(0) == "") {
                    mastPageFooterErr(hidLang.Get("M00181"));
                    return;
                }
                else {
                    uploadFileName = filePackingLogo.GetText(0);
                    hidField.Clear();
                    hidField.Set("CsCode", txtCsCode.GetText());
                    filePackingLogo.Upload();
                }

            } catch (e) { }
        }

        //우편번호
        function btnAdress_Click() {
            var width = 500; //팝업의 너비
            var height = 600; //팝업의 높이
            new daum.Postcode({
                width: width, //생성자에 크기 값을 명시적으로 지정해야 합니다.
                height: height,
                oncomplete: function (data) {
                    // 팝업에서 검색결과 항목을 클릭했을때 실행할 코드를 작성하는 부분.

                    // 도로명 주소의 노출 규칙에 따라 주소를 조합한다.
                    // 내려오는 변수가 값이 없는 경우엔 공백('')값을 가지므로, 이를 참고하여 분기 한다.
                    var fullRoadAddr = data.roadAddress; // 도로명 주소 변수
                    var extraRoadAddr = ''; // 도로명 조합형 주소 변수

                    // 법정동명이 있을 경우 추가한다. (법정리는 제외)
                    // 법정동의 경우 마지막 문자가 "동/로/가"로 끝난다.
                    if (data.bname !== '' && /[동|로|가]$/g.test(data.bname)) {
                        extraRoadAddr += data.bname;
                    }
                    // 건물명이 있고, 공동주택일 경우 추가한다.
                    if (data.buildingName !== '' && data.apartment === 'Y') {
                        extraRoadAddr += (extraRoadAddr !== '' ? ', ' + data.buildingName : data.buildingName);
                    }
                    // 도로명, 지번 조합형 주소가 있을 경우, 괄호까지 추가한 최종 문자열을 만든다.
                    if (extraRoadAddr !== '') {
                        extraRoadAddr = ' (' + extraRoadAddr + ')';
                    }
                    // 도로명, 지번 주소의 유무에 따라 해당 조합형 주소를 추가한다.
                    if (fullRoadAddr !== '') {
                        fullRoadAddr += extraRoadAddr;
                    }

                    // 우편번호와 주소 정보를 해당 필드에 넣는다.
                    txtCsZip.SetText(data.zonecode); //5자리 새우편번호 사용
                    txtCsAddress.SetText(fullRoadAddr);
                }
            }).open({
                left: (window.screen.width / 2) - (width / 2),
                top: (window.screen.height / 2) - (height / 2)
            });
        }
        //담당자팝업 (입력)
        function pupEmpClick(sGubun, sFlag) {

            v_mast_pop_callback = callbackEmpMaster

            var sCode = txtEmpCode.GetText();
            var sName = txtEmpName.GetText();
            var sW = "";
            
            if (sGubun == "C") {
                // 클릭
                mastPopEmpDeptShow("&Code=" + sCode + "&Name=" + sName + "&W=" + sW);

            } else if (sGubun == "K") {
                if (sFlag == "C") {
                    sName = "";
                }
                else if (sFlag == "N") {
                    sCode = "";
                }
                if (sCode != "" || sName != "") {
                    // 키보드
                    mastCodeCallback("&Gubun=EMPDEPT" + "&Code=" + sCode + "&Name=" + sName + "&W=" + sW);
                }
            }
        }

        function callbackEmpMaster() {
            if (v_mast_pop_code != "") {
                txtEmpCode.SetText(v_mast_pop_code);
                txtEmpName.SetText(v_mast_pop_name);
            }
        }

        function GridS1EndCallback(s, e) {
            GridS1.SetFocusedRowIndex(-1);
            var vFlag = s.cp_ret_flag.toString();
            var vMsg = s.cp_ret_message.toString();
            var vJob = s.cp_ret_job.toString();
            if (s.cp_ret_flag == null || s.cp_ret_flag == "undefined") {
                return;
            }

            if (vFlag == "N") {
                mastPageFooterErr(vMsg);
                mastLoadingPanelHide();
            }
            else {
                if (vJob == "SAVE") {
                    txtSaveMode.SetText("N");
                    mastLoadingPanelHide();
                    mastPageFooter(vMsg);
                    btnDelete.SetEnabled(true);
                    btnSearch_Click();
                }
                else if (vJob == "SELECT") {

                }
                else {
                    for (i = 0; i < GridS1.pageRowCount; i++) {
                        GridS1.batchEditApi.SetCellValue(i, "SaveCheck", "Y", null, true);
                    }
                }
            }

            mastLoadingPanelHide();

            s.cp_ret_flag = "";
            s.cp_ret_message = "";
            s.cp_ret_job = "";
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
            visibleIndex = e.visibleIndex;
        }
    </script>

    <table style="width:100%;">
        <colgroup>
            <col width="40%" />
            <col width="1" />
            <col width="60%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top;">
                <table style="width: 100%;" class="smart_table_head"></table>
                <table style="width: 100%;">
                    <colgroup>
                        <col width="30%" />
                        <col width="70%" />
                    </colgroup>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblCsType_S" ClientInstanceName="lblCsType_S" runat="server" Text="거래처유형" Width="100%"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxComboBox ID="cmbCsType_S" ClientInstanceName="cmbCsType_S" runat="server" Width="70%" IncrementalFilteringMode="None"
                                ValueField="CodeCode" TextField="CodeName" ValueType="System.String" DropDownStyle="DropDownList" >
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblCsCode_S" ClientInstanceName="lblCsCode_S" runat="server" Text="거래처정보" Width="100%"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default" colspan="3">
                            <table width="100%">
                                <colgroup>
                                    <col width="25%" />
                                    <col width="1" />
                                    <col width="75%" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dx:ASPxTextBox ID="txtCsCode_S" ClientInstanceName="txtCsCode_S" runat="server" Width="100%">
                                            <%--<ClientSideEvents TextChanged="function(s, e) { pupCs_SClick('K', 'C'); }"
                                                KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />--%>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp</td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtCsName_S" ClientInstanceName="txtCsName_S" runat="server" Width="60%">
                                            <%--<ClientSideEvents TextChanged="function(s, e) { pupCs_SClick('K', 'N'); }"
                                                KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />--%>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblCsUse_S" ClientInstanceName="lblCsUse_S" runat="server" Text="사용여부" Width="100%"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxComboBox ID="cmbCsUse_S" ClientInstanceName="cmbCsUse_S" runat="server" Width="70%" IncrementalFilteringMode="None">
                                <Items>
                                    <dx:ListEditItem Text="전체" Value="" />
                                    <dx:ListEditItem Text="사용중인것만" Value="Y" Selected="true" />
                                    <dx:ListEditItem Text="사용안함" Value="N" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;" class="smart_table_foot"></table>

                <table style="width: 100%;">
                    <tr style="height: 5px;"></tr>
                    <tr>
                        <td align="right" style="margin-right: 5px;">
                            <dx:ASPxButton ID="btnNew_S" ClientInstanceName="btnNew_S" runat="server" AutoPostBack="false" Text="신규" Width="80" UseSubmitBehavior="false">
                                <Image IconID="actions_new_16x16office2013"></Image>
                                <ClientSideEvents Click="btnNew_S_Click" />
                            </dx:ASPxButton>
                            <img src="/SmartPro/Contents/Images/dot_ver.gif" />
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="조회" AutoPostBack="false" UseSubmitBehavior="false">
                                <Image IconID="actions_search_16x16devav"></Image>
                                <ClientSideEvents Click="btnSearch_Click" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr style="height: 5px;"></tr>
                </table>

                <dx:ASPxGridView ID="Grid" ClientInstanceName="Grid" runat="server" Width="100%" AutoGenerateColumns="false"
                    OnCustomCallback="Grid_CustomCallback" OnDataBinding="Grid_DataBinding" KeyFieldName="CsCode" 
                    OnAfterPerformCallback="Grid_AfterPerformCallback" OnCustomColumnDisplayText="Grid_CustomColumnDisplayText">

                    <Styles Header-HorizontalAlign="Center" Cell-Wrap="False" />
                    <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" />
                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowSort="true" AllowSelectSingleRowOnly="true" ColumnResizeMode="Control" AllowDragDrop="false" />
                    <SettingsPager PageSize="100" AlwaysShowPager="true" />
                    <SettingsLoadingPanel Mode="Disabled" />

                    <TotalSummary>
                        <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="CsCode" />
                    </TotalSummary>

                    <ClientSideEvents EndCallback="GridEndCallback" RowDblClick="GridRowDblClick" />

                    <Columns>
                        <dx:GridViewDataColumn Caption="No" ReadOnly="true" Width="70" CellStyle-HorizontalAlign="Center"></dx:GridViewDataColumn>

                        <dx:GridViewDataTextColumn FieldName="CsCode" Caption="거래처코드" Width="30%" CellStyle-HorizontalAlign="Center" />
                        <dx:GridViewDataTextColumn FieldName="CsName" Caption="거래처명" Width="70%" />

                    </Columns>
                </dx:ASPxGridView>

            </td>
            <td>&nbsp</td>
            <td style="vertical-align: top;">

                <table style="width: 100%;" class="smart_table_head"></table>
                <table style="width: 100%;">
                    <colgroup>
                        <col width="15%" />
                        <col width="35%" />
                        <col width="15%" />
                        <col width="35%" />
                    </colgroup>
                    <tr>
                        <th class="smart_table_th_center" >
                            <dx:ASPxLabel ID="lblCsCode" ClientInstanceName="lblCsCode" runat="server" Text="거래처코드" CssClass="txt_blue"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtCsCode" ClientInstanceName="txtCsCode" runat="server" Width="30%"></dx:ASPxTextBox>
                        </td>    
                        <td></td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center" >
                            <dx:ASPxLabel ID="lblCsName" ClientInstanceName="lblCsName" runat="server" Text="거래처명" CssClass="txt_blue"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default">
                            <dx:ASPxTextBox ID="txtCsName" ClientInstanceName="txtCsName" runat="server" Width="100%"></dx:ASPxTextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <th class="smart_table_th_center">
                            <dx:ASPxLabel ID="lblCsNameFull" ClientInstanceName="lblCsNameFull" runat="server" Text="거래처법인명" CssClass="txt_blue"></dx:ASPxLabel>
                        </th>
                        <td class="smart_table_td_default" colspan="3">
                            <table style="width: 85%;">
                                <colgroup>
                                    <col width="70%" />
                                    <col width="1" />
                                    <col width="30%" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dx:ASPxTextBox ID="txtCsNameFull" ClientInstanceName="txtCsNameFull" runat="server" Width="100%"></dx:ASPxTextBox>
                                    </td>
                                    <td>&nbsp</td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chkCsUse" ClientInstanceName="chkCsUse" runat="server" Text="사용함" Checked="true"></dx:ASPxCheckBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;" class="smart_table_foot"></table>

                <table style="height: 5px;"></table>

                <%-- 텝화면구현 --%>
                <dx:ASPxPageControl ID="tabMain" ClientInstanceName="tabMain" runat="server" Width="100%" Height="100%" ActiveTabIndex="0">
                    <TabPages>
                        <dx:TabPage Name="tabPage1" Text="상세정보">
                            <ContentCollection>
                                <dx:ContentControl>

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
                                                <dx:ASPxLabel ID="lblCsType" ClientInstanceName="lblCsType" runat="server" Text="거래처유형" CssClass="txt_blue"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbCsType" ClientInstanceName="cmbCsType" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="CodeCode" TextField="CodeName" ValueType="System.String" DisplayFormatString="{0}" TextFormatString="{0}">
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="CodeName" Caption="유형" />
                                                        <dx:ListBoxColumn FieldName="CodeCode" ClientVisible="false" />
                                                        <dx:ListBoxColumn FieldName="TypeCode" ClientVisible="false" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsGubun" ClientInstanceName="lblCsGubun" runat="server" Text="거래처구분" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <table style="width: 100%;">
                                                    <colgroup>
                                                        <col width="25%" />
                                                        <col width="25%" />
                                                        <col width="25%" />
                                                        <col width="25%" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxCheckBox ID="chkArCheck" ClientInstanceName="chkArCheck" runat="server" Text="매출"></dx:ASPxCheckBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxCheckBox ID="chkApCheck" ClientInstanceName="chkApCheck" runat="server" Text="매입"></dx:ASPxCheckBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxCheckBox ID="chkOutCheck" ClientInstanceName="chkOutCheck" runat="server" Text="외주"></dx:ASPxCheckBox>
                                                        </td>
                                                        <td>
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCompany" ClientInstanceName="lblCompany" runat="server" Text="법인여부" CssClass="txt_blue"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxRadioButtonList ID="rdoCompany" ClientInstanceName="rdoCompany" runat="server" RepeatColumns="2" Width="70%"
                                                    Paddings-Padding="0" ItemSpacing="0" Border-BorderWidth="0">
                                                    <Items>
                                                        <dx:ListEditItem Text="법인" Value="Y" />
                                                        <dx:ListEditItem Text="개인" Value="N" />
                                                    </Items>
                                                </dx:ASPxRadioButtonList>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblDomestic" ClientInstanceName="lblDomestic" runat="server" Text="해외여부" CssClass="txt_blue"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxRadioButtonList ID="rdoDomestic" ClientInstanceName="rdoDomestic" runat="server" RepeatColumns="2" Width="70%"
                                                    Paddings-Padding="0" ItemSpacing="0" Border-BorderWidth="0">
                                                    <ClientSideEvents SelectedIndexChanged="rdoDomesticChanged" />
                                                    <Items>
                                                        <dx:ListEditItem Text="국내" Value="N" />
                                                        <dx:ListEditItem Text="해외" Value="Y" />
                                                    </Items>
                                                </dx:ASPxRadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblNationCode" ClientInstanceName="lblNationCode" runat="server" Text="국가" CssClass="txt_blue"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <table style="width: 100%;">
                                                    <colgroup>
                                                        <col width="35%" />
                                                        <col width="1" />
                                                        <col width="65%" />
                                                        <col width="1" />
                                                        <col width="25" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtNationCode" ClientInstanceName="txtNationCode" runat="server" Width="100%">
                                                                <ClientSideEvents TextChanged="function(s, e) { pupNationClick('K', 'C'); }"
                                                                    KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtNationName" ClientInstanceName="txtNationName" runat="server" Width="100%">
                                                                <ClientSideEvents TextChanged="function(s, e) { pupNationClick('K', 'N'); }"
                                                                    KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxImage ID="pupNation" ClientInstanceName="pupNation" runat="server" Cursor="pointer" ImageUrl="/SmartPro/Contents/Images/popup.gif">
                                                                <ClientSideEvents Click="function(s, e) { pupNationClick('C'); }" />
                                                            </dx:ASPxImage>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblRegionCode" ClientInstanceName="lblRegionCode" runat="server" Text="지역" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbRegionCode" ClientInstanceName="cmbRegionCode" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="RegionCode" TextField="RegionName" ValueType="System.String" OnCallback="cmbRegionCode_Callback"></dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center" rowspan="2">
                                                <dx:ASPxLabel ID="lblAdress" ClientInstanceName="lblAdress" runat="server" Text="주소" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default" colspan="3">
                                                <table style="width: 100%;">
                                                    <colgroup>
                                                        <col width="12%" />
                                                        <col width="1" />
                                                        <col width="88%" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtCsZip" ClientInstanceName="txtCsZip" runat="server" Width="100%"></dx:ASPxTextBox>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxButton ID="btnAdress" ClientInstanceName="btnAdress" runat="server" Text="우편번호" AutoPostBack="false" UseSubmitBehavior="false">
                                                                <Image IconID="actions_search_16x16devav"></Image>
                                                                <ClientSideEvents Click="btnAdress_Click" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="smart_table_td_default" colspan="3">
                                                <dx:ASPxTextBox ID="txtCsAddress" ClientInstanceName="txtCsAddress" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsTel" ClientInstanceName="lblCsTel" runat="server" Text="전화번호" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtCsTel" ClientInstanceName="txtCsTel" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsFax" ClientInstanceName="lblCsFax" runat="server" Text="팩스번호" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtCsFax" ClientInstanceName="txtCsFax" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsUrl" ClientInstanceName="lblCsUrl" runat="server" Text="홈페이지" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtCsUrl" ClientInstanceName="txtCsUrl" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsEmail" ClientInstanceName="lblCsEmail" runat="server" Text="이메일" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtCsEmail" ClientInstanceName="txtCsEmail" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblRegNo" ClientInstanceName="lblRegNo" runat="server" Text="사업자번호" Width="100%" ForeColor="Brown" Font-Bold="true"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default"> 
                                                <dx:ASPxTextBox ID="txtRegNo" ClientInstanceName="txtRegNo" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsIndustry" ClientInstanceName="lblCsIndustry" runat="server" Text="업태" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtCsIndustry" ClientInstanceName="txtCsIndustry" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsItems" ClientInstanceName="lblCsItems" runat="server" Text="종목" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtCsItems" ClientInstanceName="txtCsItems" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblParentCs" ClientInstanceName="lblParentCs" runat="server" Text="모기업" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <table width="100%">
                                                    <colgroup>
                                                        <col width="35%" />
                                                        <col width="1" />
                                                        <col width="65%" />
                                                        <col width="1" />
                                                        <col width="25" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtParentCs" ClientInstanceName="txtParentCs" runat="server" Width="100%">
                                                                <ClientSideEvents TextChanged="function(s, e) { pupParentCsClick('K', 'C'); }"
                                                                    KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtParentCsName" ClientInstanceName="txtParentCsName" runat="server" Width="100%">
                                                                <ClientSideEvents TextChanged="function(s, e) { pupParentCsClick('K', 'N'); }"
                                                                    KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxImage ID="pupParentCs" ClientInstanceName="pupParentCs" runat="server" Cursor="pointer" ImageUrl="/SmartPro/Contents/Images/popup.gif">
                                                                <ClientSideEvents Click="function(s, e) { pupParentCsClick('C'); }" />
                                                            </dx:ASPxImage>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsChief" ClientInstanceName="lblCsChief" runat="server" Text="대표자성명" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtCsChief" ClientInstanceName="txtCsChief" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblEmpCode" ClientInstanceName="lblEmpCode" runat="server" Text="영업당담자" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <table style="width: 100%;">
                                                    <colgroup>
                                                        <col width="40%" />
                                                        <col width="1" />
                                                        <col width="60%" />
                                                        <col width="1" />
                                                        <col width="25" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtEmpCode" ClientInstanceName="txtEmpCode" runat="server" Width="100%">
                                                                <ClientSideEvents TextChanged="function(s, e) { pupEmpClick('K', 'C'); }"
                                                                    KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtEmpName" ClientInstanceName="txtEmpName" runat="server" Width="100%">
                                                                <ClientSideEvents TextChanged="function(s, e) { pupEmpClick('K', 'N'); }"
                                                                    KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxImage ID="pupEmp" ClientInstanceName="pupEmp" runat="server" Cursor="pointer" ImageUrl="/SmartPro/Contents/Images/popup.gif">
                                                                <ClientSideEvents Click="function(s, e) { pupEmpClick('C'); }" />
                                                            </dx:ASPxImage>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblTaxCodeAr" ClientInstanceName="lblTaxCodeAr" runat="server" Text="과세구분(매출)" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbTaxCodeAr" ClientInstanceName="cmbTaxCodeAr" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="TaxCode" TextField="TaxName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblTaxCodeAp" ClientInstanceName="lblTaxCodeAp" runat="server" Text="과세구분(매입)" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbTaxCodeAp" ClientInstanceName="cmbTaxCodeAp" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="TaxCode" TextField="TaxName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsClass1" ClientInstanceName="lblCsClass1" runat="server" Text="거래처분류1" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbCsClass1" ClientInstanceName="cmbCsClass1" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="CodeCode" TextField="CodeName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsClass2" ClientInstanceName="lblCsClass2" runat="server" Text="거래처분류2" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbCsClass2" ClientInstanceName="cmbCsClass2" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="CodeCode" TextField="CodeName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsDescr" ClientInstanceName="lblCsDescr" runat="server" Text="적요" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtCsDescr" ClientInstanceName="txtCsDescr" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblInitial" ClientInstanceName="lblInitial" runat="server" Text="이니셜" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtInitial" ClientInstanceName="txtInitial" runat="server" Width="100%" MaxLength="2"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <table style="width: 100%;" class="smart_table_foot"></table>

                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        
                        
                        <dx:TabPage Name="tabPage2" Text="기타정보">
                            <ContentCollection>
                                <dx:ContentControl>

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
                                                <dx:ASPxLabel ID="lblPayConditionAr" ClientInstanceName="lblPayConditionAr" runat="server" Text="수금조건(매출)" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbPayConditionAr" ClientInstanceName="cmbPayConditionAr" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="PayCondCode" TextField="PayCondName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblPayConditionAp" ClientInstanceName="lblPayConditionAp" runat="server" Text="지급조건(매입)" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbPayConditionAp" ClientInstanceName="cmbPayConditionAp" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="PayCondCode" TextField="PayCondName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCsGrade" ClientInstanceName="lblCsGrade" runat="server" Text="업체등급" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbCsGrade" ClientInstanceName="cmbCsGrade" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="CodeCode" TextField="CodeName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCreditGrade" ClientInstanceName="lblCreditGrade" runat="server" Text="신용등급" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbCreditGrade" ClientInstanceName="cmbCreditGrade" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="CodeCode" TextField="CodeName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblBankCode" ClientInstanceName="lblBankCode" runat="server" Text="결제은행" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <table width="100%">
                                                    <colgroup>
                                                        <col width="35%" />
                                                        <col width="1" />
                                                        <col width="65%" />
                                                        <col width="1" />
                                                        <col width="25" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtBankCode" ClientInstanceName="txtBankCode" runat="server" Width="100%">
                                                                <ClientSideEvents TextChanged="function(s, e) { pupBankClick('K', 'C'); }"
                                                                    KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="txtBankName" ClientInstanceName="txtBankName" runat="server" Width="100%">
                                                                <ClientSideEvents TextChanged="function(s, e) { pupBankClick('K', 'N'); }"
                                                                    KeyPress="function(s, e) { if (e.htmlEvent.keyCode == 13) { ASPxClientUtils.PreventEventAndBubble(e.htmlEvent); } } " />
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxImage ID="pupBank" ClientInstanceName="pupBank" runat="server" Cursor="pointer" ImageUrl="/SmartPro/Contents/Images/popup.gif">
                                                                <ClientSideEvents Click="function(s, e) { pupBankClick('C'); }" />
                                                            </dx:ASPxImage>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblAccountNo" ClientInstanceName="lblAccountNo" runat="server" Text="결제계좌" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtAccountNo" ClientInstanceName="txtAccountNo" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblCurrCode" ClientInstanceName="lblCurrCode" runat="server" Text="결제통화" CssClass="txt_blue"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbCurrCode" ClientInstanceName="cmbCurrCode" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="CurrCode" TextField="CurrName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                
                                            </th>
                                            <td class="smart_table_td_default">
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblIncoTerm" ClientInstanceName="lblIncoTerm" runat="server" Text="인도조건" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbIncoTerm" ClientInstanceName="cmbIncoTerm" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="CodeCode" TextField="CodeName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblLeadTime" ClientInstanceName="lblLeadTime" runat="server" Text="운송시간(일)" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtLeadTime" ClientInstanceName="txtLeadTime" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblTransCode" ClientInstanceName="lblTransCode" runat="server" Text="운송수단" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cmbTransCode" ClientInstanceName="cmbTransCode" runat="server" Width="100%" IncrementalFilteringMode="None"
                                                    ValueField="CodeCode" TextField="CodeName" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <th class="smart_table_th_center" rowspan="2">
                                                <dx:ASPxLabel ID="lblPackingLogo" ClientInstanceName="lblPackingLogo" runat="server" Text="패킹 Mark 이미지" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default" rowspan="2">
                                                <dx:ASPxImage ID="imgPackingLogo" ClientInstanceName="imgPackingLogo" runat="server" ShowLoadingImage="true" Cursor="pointer" Width="50" Height="55">
                                                    <%--<ClientSideEvents Click="function(s, e) {	imgPackingLogoClick(s,e,'MAIN'); }"></ClientSideEvents>--%>
                                                </dx:ASPxImage>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblFilePackingLogo" ClientInstanceName="lblFilePackingLogo" runat="server" Text="패킹 Mark" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">

                                                <table width="100%">
                                                    <colgroup>
                                                        <col width="71%" />
                                                        <col width="1" />
                                                        <col width="29%" />                                                  
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxUploadControl ID="filePackingLogo" ClientInstanceName="filePackingLogo" runat="server" ShowProgressPanel="true" FileUploadMode="OnPageLoad"
                                                                NullText="이미지파일을 선택하여 주십시오." Width="100%" OnFileUploadComplete="filePackingLogo_FileUploadComplete">

                                                                <ClientSideEvents FileUploadComplete="function(s, e) { filePackingLogoFileUploadComplete(e); }" />

                                                                <ValidationSettings AllowedFileExtensions=".bmp"></ValidationSettings>
                                                                <AdvancedModeSettings EnableMultiSelect="false" />
                                                            </dx:ASPxUploadControl>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <dx:ASPxButton ID="btnUpload" ClientInstanceName="btnUpload" runat="server" AutoPostBack="false" Text="업로드" style="float:right" UseSubmitBehavior="false">
                                                                <Image IconID="actions_upload_16x16"></Image>
                                                                <ClientSideEvents Click="btnUpload_Click" />
                                                            </dx:ASPxButton>
                                                        </td>                                                        
                                                    </tr>
                                                </table>                                                
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblSendMethod" ClientInstanceName="lblSendMethod" runat="server" Width="100%" Text="발송방법"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxComboBox ID="cbSendMethod" ClientInstanceName="cbSendMethod" runat="server" Width="100%"
                                                    TextField="TypeName" ValueField="TypeCode" ValueType="System.String"></dx:ASPxComboBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblTranTelNo" ClientInstanceName="lblTranTelNo" runat="server" Width="100%" Text="발송전화번호"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtTranTelNo" ClientInstanceName="txtTranTelNo" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblTranEmail" ClientInstanceName="lblTranEmail" runat="server" Width="100%" Text="발송이메일"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtTranEmail" ClientInstanceName="txtTranEmail" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblTranFax" ClientInstanceName="lblTranFax" runat="server" Width="100%" Text="발송팩스"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxTextBox ID="txtTranFax" ClientInstanceName="txtTranFax" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblExtraPlan1" ClientInstanceName="lblExtraPlan1" runat="server" Width="100%" Text="기타항목1"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default" colspan="3">
                                                <dx:ASPxTextBox ID="txtExtraPlan1" ClientInstanceName="txtExtraPlan1" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblExtraPlan2" ClientInstanceName="lblExtraPlan2" runat="server" Width="100%" Text="기타항목2"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default" colspan="3">
                                                <dx:ASPxTextBox ID="txtExtraPlan2" ClientInstanceName="txtExtraPlan2" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblExtraPlan3" ClientInstanceName="lblExtraPlan3" runat="server" Width="100%" Text="기타항목3"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default" colspan="3">
                                                <dx:ASPxTextBox ID="txtExtraPlan3" ClientInstanceName="txtExtraPlan3" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblTrus_Use" ClientInstanceName="lblTrus_Use" runat="server" Text="전자계산서 사용" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <dx:ASPxCheckBox ID="chkTrus_Use" ClientInstanceName="chkTrus_Use" runat="server" Text="사용"></dx:ASPxCheckBox>
                                            </td>
                                            <th class="smart_table_th_center">
                                                <dx:ASPxLabel ID="lblTrus_1_2" ClientInstanceName="lblTrus_1_2" runat="server" Text="전자계산서" Width="100%"></dx:ASPxLabel>
                                            </th>
                                            <td class="smart_table_td_default">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxCheckBox ID="chkTrus_1" ClientInstanceName="chkTrus_1" runat="server" Text="정방향"></dx:ASPxCheckBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxCheckBox ID="chkTrus_2" ClientInstanceName="chkTrus_2" runat="server" Text="역방향"></dx:ASPxCheckBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%;" class="smart_table_foot"></table>

                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>

                                                
                        <dx:TabPage Name="tabPage3" Text="" ClientVisible="false">
                            <ContentCollection>
                                <dx:ContentControl>                                                                        

                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>

                    </TabPages>
                </dx:ASPxPageControl>

                <table style="height: 5px;"></table>
                
                <table style="width: 100%;">                    
                    <tr>                        
                        <td align="right" style="margin-right: 5px;">
                            <dx:ASPxButton ID="btnNew" ClientInstanceName="btnNew" runat="server" AutoPostBack="false" Text="신규" Width="80" UseSubmitBehavior="false">
                                <Image IconID="actions_new_16x16office2013"></Image>
                                <ClientSideEvents Click="btnNew_Click" /> 
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnSave" ClientInstanceName="btnSave" runat="server" AutoPostBack="false" Text="저장" Width="80" UseSubmitBehavior="false">
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

                <table style="height: 5px;"></table>

                <table style="width: 100%;">                    
                    <tr>
                        <td align="left">
                            <dx:ASPxLabel ID="lblEmp" ClientInstanceName="lblEmp" runat="server" Text="담당자 정보" CssClass="title" Width="100%"></dx:ASPxLabel>
                        </td>                     
                    </tr>
                </table>
                
                <table style="height: 2px;"></table>

                <dx:ASPxGridView ID="GridS1" ClientInstanceName="GridS1" runat="server" Width="100%" KeyFieldName="EmpName;EmpTell"
                    OnCustomCallback="GridS1_CustomCallback" OnDataBinding="GridS1_DataBinding" OnCustomErrorText="GridS1_CustomErrorText" 
                    OnCustomColumnDisplayText="GridS1_CustomColumnDisplayText" OnBatchUpdate="GridS1_BatchUpdate" OnAfterPerformCallback="GridS1_AfterPerformCallback">

                    <Styles Header-HorizontalAlign="Center" Cell-Wrap="False" />
                    <Settings VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" ShowFooter="true" ShowStatusBar="Hidden"  />
                    <SettingsPager Mode="ShowAllRecords" />
                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" ColumnResizeMode="Control" AllowSort="False"  />
                    <SettingsEditing Mode="Batch">
                        <BatchEditSettings EditMode="Cell" StartEditAction="Click" ShowConfirmOnLosingChanges="false" 
                            AllowValidationOnEndEdit="false" KeepChangesOnCallbacks="False" AllowEndEditOnValidationError="false" />
                    </SettingsEditing>
                    <SettingsLoadingPanel Mode="Disabled" />


                    <TotalSummary>
                        <dx:ASPxSummaryItem SummaryType="Count" DisplayFormat="Count : #,#" FieldName="EmpName" />
                    </TotalSummary>

                    <ClientSideEvents EndCallback="GridS1EndCallback" CallbackError="GridCallbackError" BatchEditStartEditing="GridStartEditing" />

                    <Columns>
                        <dx:GridViewCommandColumn ShowDeleteButton="true" ShowRecoverButton="true" ShowNewButtonInHeader="true" Width="50" FixedStyle="Left">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataColumn Caption="No" ReadOnly="true" Width="30" CellStyle-HorizontalAlign="Center" FixedStyle="Left"></dx:GridViewDataColumn>
                        <dx:GridViewDataTextColumn FieldName="EmpName" Caption="업체담당자" Width="15%"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="EmpTell" Caption="담당자전번" Width="15%"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="EmpFaxNo" Caption="담당자팩스" Width="15%"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="emailID" Caption="담당자메일" Width="25%"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="EmpBego" Caption="비고" Width="25%"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="SaveCheck" Caption="" Width="0" CellStyle-HorizontalAlign="Center" Settings-ShowEditorInBatchEditMode="false" />
                    </Columns>
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>
    
    <%-- 데이터 조회 콜백 판넬 (Ajax) --%>
    <dx:ASPxCallbackPanel ID="callBackPanel" ClientInstanceName="callBackPanel" OnCallback="callBackPanel_Callback" runat="server">
        <ClientSideEvents EndCallback="function(s, e) { panelEndCallback(s, e) } " />
        <PanelCollection>
            <dx:PanelContent>

                <dx:ASPxHiddenField ID="hidField" ClientInstanceName="hidField" runat="server"></dx:ASPxHiddenField>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <dx:ASPxTextBox ID="txtSaveMode" ClientInstanceName="txtSaveMode" runat="server" ClientVisible="false"></dx:ASPxTextBox>

    <dx:ASPxTextBox ID="txtPermition_I" ClientInstanceName="txtPermition_I" runat="server" ClientVisible="false"></dx:ASPxTextBox>
    <dx:ASPxTextBox ID="txtPermition_D" ClientInstanceName="txtPermition_D" runat="server" ClientVisible="false"></dx:ASPxTextBox>

    <dx:ASPxHiddenField ID="hidLang" ClientInstanceName="hidLang" runat="server"></dx:ASPxHiddenField>

</asp:Content>