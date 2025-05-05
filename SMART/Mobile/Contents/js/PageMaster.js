
// popup창 파라미터
var v_mast_pop_code = null;
var v_mast_pop_name = null;
var v_mast_pop_info1 = null;
var v_mast_pop_info2 = null;
var v_mast_pop_info3 = null;
var v_mast_pop_status = null;

var v_mast_pop_callback = null;

var v_mast_statusbar_visible = true;       // 화면 하단 메시지 박스 보이기

// 1. 팝업창이 닫힐때 처리
function mastPopCodeClose() {
    mastCloseForm();
}
// 2.
function mastPopFormClose() {
    mastCloseForm();
}
// 3.
function mastOpenWindowClose() {
    mastCloseForm();
}


// loading panel show
function mastLoadingPanelShow() {
    
    var str = "Waiting... &nbsp; &nbsp; <input type=button value='cancel' onclick='mastLoadingPanelHide()'>&nbsp;";
    mastLoadingPanel.SetText(str);
    mastLoadingPanel.Show();
}

// loading panel hide
function mastLoadingPanelHide() {
    mastLoadingPanel.Hide();
}

function mastPopFormShow(form_title, form_url, form_width, form_height) {
    v_mast_pop_gubn_close = "DEVFORM";
    mastPopForm.SetWidth(form_width);
    mastPopForm.SetHeight(form_height);
    mastPopForm.SetContentUrl(form_url);
    mastPopForm.SetHeaderText(form_title);
    mastPopForm.Show();
}


// 폼 닫기
function mastCloseForm() {
    var p = parent;
    var o = opener;
    var arg = null;
    var gubn = "";
    var debug = "";

    try {
        arg = dialogArguments;
    } catch (e) {
        //
    }

    //gubn
    if (p != null && ("" + p.closeTabMenu) != "undefined") {
        gubn = "MDI";
    } else if (p != null && arg != null) {
        if (p.v_x_id == v_x_id) {
            gubn = "JSMOPEN";
        } else {
            gubn = "DEVFORM";
        }
    } else if (o == null) {

        gubn = "DEVFORM";

    } else {
        gubn = "JSOPEN";
    }

    if (gubn == "MDI") {
        parent.closeTabMenu(v_mast_pgm_id);

    } else if (gubn == "DEVFORM") {
        p.v_mast_pop_code = v_mast_pop_code;
        p.v_mast_pop_name = v_mast_pop_name;
        p.v_mast_pop_info1 = v_mast_pop_info1;
        p.v_mast_pop_info2 = v_mast_pop_info2;
        p.v_mast_pop_info3 = v_mast_pop_info3;
        p.v_mast_pop_status = v_mast_pop_status;

        p.mastPopForm.Hide();

        if (p.v_mast_pop_callback != null) {
            p.v_mast_pop_callback();
        }


    } else if (gubn == "JSOPEN") {
        o.v_mast_pop_code = v_mast_pop_code;
        o.v_mast_pop_name = v_mast_pop_name;
        o.v_mast_pop_info1 = v_mast_pop_info1;
        o.v_mast_pop_info2 = v_mast_pop_info2;
        o.v_mast_pop_info3 = v_mast_pop_info3;
        o.v_mast_pop_status = v_mast_pop_status;

        if (o.v_mast_pop_callback != null) {
            o.v_mast_pop_callback();
        }
        window.close();

    } else if (gubn == "JSMOPEN") {
        var arr = new Array(v_mast_pop_code, v_mast_pop_name, v_mast_pop_info1, v_mast_pop_info2, v_mast_pop_info3, v_mast_pop_status);
        window.returnValue = arr;
        window.close();

    } else {

        try {
            window.close();
        } catch (e) {
            //
        }
    }
}

// 메뉴열기 생성
function mastAddTabMenu(pgm_id, pgm_text, pgm_url, parent_id) {
    v_mast_pop_gubn_close = "TABFORM"
    if ("" + parent == "undefined" || "" + parent.closeTabMenu == "undefined") {
        open(pgm_url);
    } else {
        parent.addTabMenu(pgm_id, pgm_text, pgm_url, parent_id);
    }
}

// YYYYMMDD -> DevDate,  예) devDate1.SetDate(js_date8to_devdate('20020102'));
function js_date8to_devdate(p1) {
    if ("" + p1 == "") {
        return null;
    } else if (p1.length == 8) {
        return new Date(p1.substr(0, 4), parseInt(p1.substr(4, 2)) - 1, p1.substr(6, 2));
    } else {
        return null;
    }
}

//  YYYY-MM-DD -> DevDate
function js_date10to_devdate(p1) {
    if ("" + p1 == "") {
        return null;
    } else if (p1.length == 10) {
        return new Date(p1.substr(0, 4), parseInt(p1.substr(5, 2)) - 1, p1.substr(8, 2));
    } else {
        return null;
    }
}
