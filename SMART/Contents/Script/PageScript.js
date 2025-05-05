/********************************************************************************
 * 생성자 : 유태한 
 * 생성일자 : 2021.03-05                                                         *
 * 화면구성을 위해 필요한 코어 스크립트 추가를 원하면 회의를 통해서 추가하여 진행필요. *
/********************************************************************************/

var v_mast_statusbar_visible = true;       // 화면 하단 메시지 박스 보이기 여부

var v_mast_pop_gubn = null;

var v_mast_pop_code = null;
var v_mast_pop_name = null;
var v_mast_pop_data1 = null;
var v_mast_pop_data2 = null;
var v_mast_pop_data3 = null;
var v_mast_pop_data4 = null;
var v_mast_pop_data5 = null;
var v_mast_pop_data6 = null;
var v_mast_pop_data7 = null;
var v_mast_pop_data8 = null;
var v_mast_pop_data9 = null;
var v_mast_pop_data10 = null;
var v_mast_pop_data11 = null;
var v_mast_pop_data12 = null;

var v_mast_pop_data13 = null;
var v_mast_pop_data14 = null;
var v_mast_pop_data15 = null;
var v_mast_pop_data16 = null;
var v_mast_pop_data17 = null;
var v_mast_pop_data18 = null;
var v_mast_pop_data19 = null;
var v_mast_pop_data20 = null;
var v_mast_pop_data21 = null;
var v_mast_pop_data22 = null;
var v_mast_pop_data23 = null;
var v_mast_pop_data24 = null;
var v_mast_pop_data25 = null;
var v_mast_pop_data26 = null;
var v_mast_pop_data27 = null;
var v_mast_pop_data28 = null;
var v_mast_pop_data29 = null;
var v_mast_pop_data30 = null;

var v_mast_pop_status = null;


//하단 메시지 표시
function mastPageFooter(arg1) {
    mastPageFooterLabel.GetMainElement().style.color = 'black';
    mastPageFooterLabel.SetText(arg1);
}

//하단 메시지 오류 표시
function mastPageFooterErr(arg1) {
    alert(arg1);
    mastPageFooterLabel.GetMainElement().style.color = 'red';
    mastPageFooterLabel.SetText(arg1);
}

//로딩판넬 표시
function mastLoadingPanelShow() {
    var str = "Waiting... &nbsp; &nbsp; <input type=button value='cancel' onclick='mastLoadingPanelHide()'>&nbsp;";
    mastLoadingPanel.SetText(str);
    mastLoadingPanel.Show();
}

//로딩판넬 닫기
function mastLoadingPanelHide() {
    mastLoadingPanel.Hide();
}

// 0. modal팝업창 show
function mastPopFormShow(form_title, form_url, form_width, form_height) {
    mastPopForm.SetWidth(form_width);
    mastPopForm.SetHeight(form_height);
    mastPopForm.SetContentUrl(form_url);
    mastPopForm.SetHeaderText(form_title);
    mastPopForm.Show();
}

// 1. 생성번호 팝업 (CreateNo)
function mastPopCreateNoShow(param, form_title, form_width, form_height) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/CreateNoPopup.aspx?Action=Load" + param);
    mastPopForm.SetWidth(form_width);
    mastPopForm.SetHeight(form_height);
    mastPopForm.SetHeaderText(form_title);
    mastPopForm.Show();
}

// 2. 공통으로 사용되는 팝업
function mastCommonPopupShow(param, form_title, form_width, form_height) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/CommonPopup.aspx?Action=Load" + param);
    mastPopForm.SetWidth(form_width);
    mastPopForm.SetHeight(form_height);
    mastPopForm.SetHeaderText(form_title);
    mastPopForm.Show();
}

// 3. CsMaster (거래처) 팝업창 show
function mastPopCustShow(param, form_title) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/CsMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(570);
    mastPopForm.SetHeight(500);
    if (!form_title) {
        form_title = "거래처정보";
    }
    mastPopForm.SetHeaderText(form_title);
    mastPopForm.Show();
}

// 4. ItemSiteMaster (품목) 팝업창 show
function mastPopItemShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/ItemSiteMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(800);
    mastPopForm.SetHeight(550);
    mastPopForm.SetHeaderText("품목정보");
    mastPopForm.Show();
}

// 7. 품목그룹 팝업창
function mastPopItemGroupShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/ItemGroupMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(360);
    mastPopForm.SetHeaderText("품목군");
    mastPopForm.Show();
}

// 9. 유저그룹 팝업창
function mastPopUserGroupShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/UserGroupMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("유저그룹");
    mastPopForm.Show();
}

// 10. PcMaster 팝업창
function mastPopPcMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/PcMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("손익센터정보");
    mastPopForm.Show();
}

// 11. CcMaster 팝업창
function mastPopCcMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/CcMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("코스트센터정보");
    mastPopForm.Show();
}

// 12. AccMaster 팝업창
function mastPopAccMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/AccMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("계정정보");
    mastPopForm.Show();
}

// 13. HSMaster 팝업창
function mastPopHsMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/HsMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("HS정보");
    mastPopForm.Show();
}

// 14. 국가 팝업창
function mastPopNationMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/NationMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("국가정보");
    mastPopForm.Show();
}

// 16. 창고 팝업창
function mastPopWhMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/WhMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("창고정보");
    mastPopForm.Show();
}

// 17. 부서 팝업창 - 트리뷰
function mastPopTreeDeptShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/DeptMaster_Tree.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("부서정보");
    mastPopForm.Show();
}

// 18. 공정 팝업창 - 트리뷰
function mastOperationMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/OperationMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(400);
    mastPopForm.SetHeaderText("공정");
    mastPopForm.Show();
}

// 19. 설비팝업
function mastEquipMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/EquipMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(480);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("설비정보");
    mastPopForm.Show();
}


// 20. 작업장팝업
function mastNppWcMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/NppWcMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("작업장정보");
    mastPopForm.Show();
}

// 위치정보 (Location)
function mastLocationMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/LocationMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("위치정보");
    mastPopForm.Show();
}

// 22. 그룹 팝업창 - 트리뷰
function mastPopTreeEquipGroupShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/EquipGroupMaster_Tree.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("그룹정보");
    mastPopForm.Show();
}

// 23. AssetMaster 팝업창
function mastPopAssetMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/AssetMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("자산정보");
    mastPopForm.Show();
}

// 24. 실사결과 팝업창
function mastPopInspNoShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/InspPopup.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("정기실사내역");
    mastPopForm.Show();
}

// 25. 실사조정 팝업창
function mastPopInspNoShow01(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/InspPopup01.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("정기실사내역");
    mastPopForm.Show();
}

// 26. Lot 팝업창
function mastPopLotShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/LotMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(600);
    mastPopForm.SetHeight(450);
    mastPopForm.SetHeaderText("재고정보");
    mastPopForm.Show();
}

// 27. 구성품 팝업창
function mastPopChildEquipShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/ChildEquip.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("구성품");
    mastPopForm.Show();
}

// 28. 수리번호 팝업창
function mastPopEquipRepairShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/EquipRepair.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("수리번호");
    mastPopForm.Show();
}

// 29. EmpDeptMaster (사원,부서정보) 팝업창 show
function mastPopEmpDeptShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/EmpMaster_Dept.aspx?Action=Load" + param);
    mastPopForm.SetWidth(600);
    mastPopForm.SetHeight(400);
    mastPopForm.SetHeaderText("사원정보");
    mastPopForm.Show();
}

// 30. IrNo (출하정보) 팝업창 show
function mastPopIrNoShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/IrNoPopup.aspx?Action=Load" + param);
    mastPopForm.SetWidth(890);
    mastPopForm.SetHeight(360);
    mastPopForm.SetHeaderText("출하정보");
    mastPopForm.Show();
}

// 30. IrNo (출하정보) 팝업창 show
function mastPopInventoryShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/InventoryMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(550);
    mastPopForm.SetHeight(360);
    mastPopForm.SetHeaderText("재고정보");
    mastPopForm.Show();
}

// 31. Lot제품 팝업창
function mastPopLot01Show(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/LotMaster01.aspx?Action=Load" + param);
    mastPopForm.SetWidth(800);
    mastPopForm.SetHeight(450);
    mastPopForm.SetHeaderText("제품재고정보");
    mastPopForm.Show();
}

// 32. 자산그릅 팝업창 - 트리뷰
function mastPopTreeAssetClassShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/AssetClass_Tree.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("자산그룹");
    mastPopForm.Show();
}

// 33. 계정정보 팝업창 - 트리뷰
function mastPopTreeAccMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/AccMaster_Tree.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("계정정보");
    mastPopForm.Show();
}

// 34.회계영역정보 팝업창 - 트리뷰
function mastPopTreeAreaMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/Area_Tree.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("회계영역정보");
    mastPopForm.Show();
}

// 35. 사업장정보 팝업창 - 트리뷰
function mastPopTreeSiteMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/SiteMaster_Tree.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("사업장정보");
    mastPopForm.Show();
}

// 36. 작업장 팝업창
function mastPopWcMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/WcMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("작업장정보");
    mastPopForm.Show();
}

// 36. 작업지시 팝업창
function mastPopWkMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/WkMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(700);
    mastPopForm.SetHeight(400);
    mastPopForm.SetHeaderText("작업지시정보");
    mastPopForm.Show();
}

// 37. 공정 팝업창2 - 트리뷰
function mastOperation2MasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/OperationMaster2.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("공정");
    mastPopForm.Show();
}

// 38. 라인팝업
function mastNppLineMasterShow(param) {
    v_mast_pop_gubn == "COMMON";
    v_mast_pop_gubn_close = "COMMONFORM";
    mastPopForm.SetContentUrl("/SmartPro/Common/Forms/LineMaster.aspx?Action=Load" + param);
    mastPopForm.SetWidth(400);
    mastPopForm.SetHeight(350);
    mastPopForm.SetHeaderText("라인정보");
    mastPopForm.Show();
}

// 공통 팝업창 닫기 이벤트
function mastCloseForm() {
    var p = parent;

    try {
        arg = dialogArguments;
    } catch (e) {
        //
    }

    p.v_mast_pop_code = v_mast_pop_code;
    p.v_mast_pop_name = v_mast_pop_name;
    p.v_mast_pop_data1 = v_mast_pop_data1;
    p.v_mast_pop_data2 = v_mast_pop_data2;
    p.v_mast_pop_data3 = v_mast_pop_data3;
    p.v_mast_pop_data4 = v_mast_pop_data4;
    p.v_mast_pop_data5 = v_mast_pop_data5;
    p.v_mast_pop_data6 = v_mast_pop_data6;
    p.v_mast_pop_data7 = v_mast_pop_data7;
    p.v_mast_pop_data8 = v_mast_pop_data8;
    p.v_mast_pop_data9 = v_mast_pop_data9;
    p.v_mast_pop_data10 = v_mast_pop_data10;
    //추가
    p.v_mast_pop_data11 = v_mast_pop_data11;
    p.v_mast_pop_data12 = v_mast_pop_data12;
    p.v_mast_pop_data13 = v_mast_pop_data13;
    p.v_mast_pop_data14 = v_mast_pop_data14;
    p.v_mast_pop_data15 = v_mast_pop_data15;
    p.v_mast_pop_data16 = v_mast_pop_data16;
    p.v_mast_pop_data17 = v_mast_pop_data17;
    p.v_mast_pop_data18 = v_mast_pop_data18;
    p.v_mast_pop_data19 = v_mast_pop_data19;
    p.v_mast_pop_data20 = v_mast_pop_data20;

    p.v_mast_pop_data21 = v_mast_pop_data21;
    p.v_mast_pop_data22 = v_mast_pop_data22;
    p.v_mast_pop_data23 = v_mast_pop_data23;
    p.v_mast_pop_data24 = v_mast_pop_data24;
    p.v_mast_pop_data25 = v_mast_pop_data25;


    p.v_mast_pop_status = v_mast_pop_status;

    p.mastPopForm.Hide();

    if (p.v_mast_pop_callback != null) {
        p.v_mast_pop_callback();
    }
}

// 팝업창에서 엔터키 이벤트
function mastCodeCallback(param) {
    v_mast_pop_param = param;
    mastCallback.SendCallback(param);
    mastLoadingPanelShow();
}


function mastOnCallbackComplete(s, e) {
    var ret = e.result;
    var arr;

    mastLoadingPanelHide();

    if (ret != "") {
        arr = ret.split('|');
        v_mast_pop_gubn = arr[2];

        if (arr[0] == "1") {
            //한개인 경우 바로 표현
            if (arr[2] == "EMP"  || arr[2] == "DEPT" || arr[2] == "IGM" || arr[2] == "UGM" || arr[2] == "USER" || arr[2] == "WH"
                || arr[2] == "CC" || arr[2] == "ACC" || arr[2] == "ACCMASTER" || arr[2] == "HS" || arr[2] == "NA" || arr[2] == "REG" || arr[2] == "OPER" || arr[2] == "OPER2" || arr[2] == "EQUIP" || arr[2] == "NPPWC"
                || arr[2] == "LOC" || arr[2] == "WC" || arr[2] == "WC2" || arr[2] == "MSMCHN" || arr[2] == "WORKGROUP" || arr[2] == "WORKEMP" || arr[2] == "VWGLMNG" || arr[2] == "VWGLMNGTY2"
                || arr[2] == "ASSET"
                || arr[2] == "NPPWORKGROUP" || arr[2] == "NPPPROUT" || arr[2] == "INSP2" || arr[2] == "INGRED" || arr[2] == "CE"
            ) {
                v_mast_pop_status = "OK";
                v_mast_pop_code = arr[3];
                v_mast_pop_name = arr[4];
            }
            if (arr[2] == "ITEM" || arr[2] == "CS" || arr[2] == "CODE" || arr[2] == "HRWORKGROUP" || arr[2] == "HRWORKEMP" || arr[2] == "PAYITEM" || arr[2] == "HRWORKTYPE"
                || arr[2] == "WKSCHE" || arr[2] == "EMPDEPT" || arr[2] == "GLSTDDES" || arr[2] == "CARDCS" || arr[2] == "PARENTCS"
                || arr[2] == "NPPOT" || arr[2] == "NPPBOMITEM" || arr[2] == "INSP2" || arr[2] == "INGRED" || arr[2] == "AE" || arr[2] == "AE3"
                || arr[2] == "NPPOT"
                || arr[2] == "GLACCCUM"
                || arr[2] == "CARDCS"
                || arr[2] == "LOTINVEN"
                || arr[2] == "TRGROUPEMP"
                || arr[2] == "MNGEMP"
                || arr[2] == "NPPWC"
                || arr[2] == "COALTWKNO"
                || arr[2] == "COALTWC"
                || arr[2] == "ITEM2"
            ) {
                var iLength = arr.length;
                v_mast_pop_status = "OK";
                v_mast_pop_code = arr[3];
                v_mast_pop_name = arr[4];
                if (arr[5] != null) v_mast_pop_data1 = arr[5];
                if (arr[6] != null) v_mast_pop_data2 = arr[6];
                if (arr[7] != null) v_mast_pop_data3 = arr[7];
                if (arr[8] != null) v_mast_pop_data4 = arr[8];
                if (arr[9] != null) v_mast_pop_data5 = arr[9];
                if (arr[10] != null) v_mast_pop_data6 = arr[10];
                if (arr[11] != null) v_mast_pop_data7 = arr[11];
                if (arr[12] != null) v_mast_pop_data8 = arr[12];
                if (arr[13] != null) v_mast_pop_data9 = arr[13];
                if (arr[14] != null) v_mast_pop_data10 = arr[14];
            }

            //함수실행
            if (v_mast_pop_callback != null) {
                v_mast_pop_callback();
            }
            
        } else if (arr[0] == "0") {
            mastPageFooterErr("검색된 항목이 없습니다.");
        } else {
            //팝업실행
            switch (arr[2]) {
                case "EMP":
                    mastCommonPopupShow(arr[1], "사원정보", 350, 400);
                    break;
                case "CODE":
                    mastCommonPopupShow(arr[1], "공통코드", 350, 400);
                    break;
                case "AE":
                    mastCommonPopupShow(arr[1], "배부기준", 350, 400);
                    break;
                case "AE3":
                    mastCommonPopupShow(arr[1], "배부기준", 350, 400);
                    break;
                case "CS":
                    mastPopCustShow(arr[1]);
                    break;
                case "DEPT":
                    mastCommonPopupShow(arr[1], "부서정보", 350, 400);
                    break;
                case "IGM":
                    mastCommonPopupShow(arr[1], "품목군정보", 350, 400);
                    break;
                case "UGM":
                    mastPopUserGroupShow(arr[1]);
                    break;
                case "USER":
                    mastCommonPopupShow(arr[1], "유저정보", 350, 400);
                    break;
                case "ITEM":
                    mastPopItemShow(arr[1]);
                    break;
                case "CC":
                    mastPopCcMasterShow(arr[1]);
                    mastCommonPopupShow(arr[1], "코스트센터정보", 350, 400);
                    break;
                case "ACC":
                    mastPopAccMasterShow(arr[1]);
                    break;
                case "ACCMASTER":
                    mastCommonPopupShow(arr[1], "계정정보", 350, 400);
                    break;
                case "HS":
                    mastPopHsMasterShow(arr[1]);
                    break;
                case "NA":
                    mastPopNationMasterShow(arr[1]);
                    break;
                case "REG":
                    mastCommonPopupShow(arr[1], "지역정보", 350, 400);
                    break;
                case "OPER":
                    mastOperationMasterShow(arr[1]);
                    break;
                case "EQ":
                    mastEquipMasterShow(arr[1]);
                    break;
                case "NPPWC":
                    mastNppWcMasterShow(arr[1]);
                    break;
                case "WH":
                    mastPopWhMasterShow(arr[1]);
                    break;
                case "LOC":
                    mastLocationMasterShow(arr[1]);
                    break;
                case "EMPDEPT":
                    mastPopEmpDeptShow(arr[1]);
                    break;
                case "WC":
                    mastCommonPopupShow(arr[1], "공정정보", 350, 400);
                    break;
                case "MSMCHN":
                    mastCommonPopupShow(arr[1], "계측기정보", 500, 400);
                    break;
                case "WORKGROUP":
                    mastCommonPopupShow(arr[1], "근무조정보", 350, 400);
                    break;
                case "HRWORKGROUP":
                    mastCommonPopupShow(arr[1], "근무조정보", 350, 400);
                    break;
                case "WORKEMP":
                    mastCommonPopupShow(arr[1], "근무자정보", 350, 400);
                    break;
                case "HRWORKEMP":
                    mastCommonPopupShow(arr[1], "근무자정보", 350, 400);
                    break;
                case "PAYITEM":
                    mastCommonPopupShow(arr[1], "급여항목", 350, 400);
                    break;
                case "HRWORKTYPE":
                    mastCommonPopupShow(arr[1], "작업유형", 350, 400);
                    break;
                case "WKSCHE":
                    mastCommonPopupShow(arr[1], "근무 스케줄", 650, 400);
                    break;
                case "WKSCHE":
                    mastCommonPopupShow(arr[1], "근무 스케줄", 650, 400);
                    break;
                case "GLSTDDES":
                    mastCommonPopupShow(arr[1], "표준적요", 650, 400);
                    break;
                case "VWGLMNG":
                    mastCommonPopupShow(arr[1], "관리항목선택", 650, 400);
                    break;
                case "VWGLMNGTY2":
                    mastCommonPopupShow(arr[1], "관리항목선택", 650, 400);
                    break;
                case "ASSET":
                    mastCommonPopupShow(arr[1], "자산코드", 350, 400);
                    break;
                case "GLMNGMASTER":
                    mastCommonPopupShow(arr[1], "관리항목선택", 350, 400);
                    break;
                case "INSP2":
                    mastCommonPopupShow(arr[1], "검사항목", 650, 400);
                    break;
                case "LOTINVEN":
                    mastPopInventoryShow(arr[1], "재고정보", 650, 400);
                    break;
                case "TRGROUPEMP":
                    mastCommonPopupShow(arr[1], "담당자", 650, 400);
                    break;
                case "MNGEMP":
                    mastCommonPopupShow(arr[1], "담당자", 650, 400);
                    break;
                case "INGRED":
                    mastCommonPopupShow(arr[1], "성분코드", 350, 400);
                    break;
                case "CE":
                    mastCommonPopupShow(arr[1], "원가요소", 350, 400);
                    break;
                case "ITEM2": //ItemMaster
                    mastCommonPopupShow(arr[1], "품목정보", 350, 400);
                    break;
            }
        }
    }
}

// 텝메뉴 생성
function mastAddTabMenu(pgm_id, pgm_text, pgm_url, parent_id) {
    v_mast_pop_gubn_close = "TABFORM"
    if ("" + parent == "undefined" || "" + parent.closeTabMenu == "undefined") {
        open(pgm_url);
    } else {
        parent.addTabMenu(pgm_id, pgm_text, pgm_url, parent_id);
    }
}

// 텝메뉴 생성
function mastParentAddTabMenu(pgm_id, pgm_text, pgm_url, parent_id) {
    v_mast_pop_gubn_close = "TABFORM"
    if ("" + parent.parent == "undefined" || "" + parent.parent.closeTabMenu == "undefined") {
        open(pgm_url);
    } else {
        parent.parent.addTabMenu(pgm_id, pgm_text, pgm_url, parent_id);
    }
}