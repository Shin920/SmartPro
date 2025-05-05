
// YYYY -> DevDate,  예) devDate1.SetDate(js_date4to_devdate('2022'));
function js_date4to_devdate(p1) {
    if ("" + p1 == "") {
        return null;
    } else if (p1.length == 4) {
        return new Date(p1.substr(0, 4));
    } else {
        return null;
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
// YYYYMM -> DevDate,  예) devDate1.SetDate(js_date6to_devdate('200201'));
function js_date6to_devdate(p1) {
    if ("" + p1 == "") {
        return null;
    } else if (p1.length == 6) {
        return new Date(p1.substr(0, 4) + "-" + p1.substr(4, 2));
        //return new Date(p1.substr(0, 4), parseInt(p1.substr(4, 2)) - 1);
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
//  YYYYMMDDHHMMSS -> DevDate
function js_date14to_devdate(p1) {
    if ("" + p1 == "") {
        return null;
    } else if (p1.length == 14) {
        return new Date(p1.substr(0, 4), parseInt(p1.substr(4, 2)) - 1, p1.substr(6, 2), p1.substr(8, 2), p1.substr(10, 2), p1.substr(12, 2));
    } else {
        return null;
    }
}
//  YYYY-MM-DD HH:MM:SS -> DevDate
function js_date19to_devdate(p1) {
    if ("" + p1 == "") {
        return null;
    } else if (p1.length == 19) {
        return new Date(p1.substr(0, 4), parseInt(p1.substr(5, 2)) - 1, p1.substr(8, 2), p1.substr(11, 2), p1.substr(14, 2), p1.substr(17, 2));
    } else {
        return null;
    }
}
// DevDate -> YYYYMMDD, 예) var str =  js_devdateto_date8(ASPxDateEdit1.GetDate());
function js_devdateto_date8(dt) {
    return "" + dt.getFullYear() + ("0" + (dt.getMonth() + 1)).substr(-2) + ("0" + dt.getDate()).substr(-2);
}
// DevDate -> YYYYMM, 예) var str =  js_devdateto_date6(ASPxDateEdit1.GetDate());
function js_devdateto_date6(dt) {
    return "" + dt.getFullYear() + ("0" + (dt.getMonth() + 1)).substr(-2);
}
// DevDate -> YYYY, 예) var str =  js_devdateto_date4(ASPxDateEdit1.GetDate());
function js_devdateto_date4(dt) {
    return "" + dt.getFullYear();
}

// DevDate -> YYYY-MM-DD
function js_devdateto_date10(obj) {
    return "" + dt.getFullYear() + "-" + ("0" + (dt.getMonth() + 1)).substr(-2) + "-" + ("0" + dt.getDate()).substr(-2);
}
// DevDate -> YYYYMMDDHHMMSS
function js_devdateto_date14(dt) {
    return "" + dt.getFullYear()
        + ("0" + (dt.getMonth() + 1)).substr(-2)
        + ("0" + dt.getDate()).substr(-2)
        + ("0" + dt.getHours()).substr(-2)
        + ("0" + dt.getMinutes()).substr(-2)
        + ("0" + dt.getSeconds()).substr(-2)
        ;
}
// HHMM -> DevTime, 예) ASPxDateEdit1.SetDate(js_date8to_devdate('1345'));  
function js_time4to_devtime(p1) {
    if (p1 == "") {
        return null;
    } else if (p1.length == 4) {
        return new Date(2000, 0, 1, p1.substr(0, 2), p1.substr(2, 2), 0);
    } else {
        return null;
    }
}
// HHMMSS
function js_time6to_devtime(p1) {
    if (p1 == "") {
        return null;
    } else if (p1.length == 6) {
        return new Date(2000, 0, 1, p1.substr(0, 2), p1.substr(2, 2), p1.substr(4, 2));
    } else {
        return null;
    }
}
// 예) ASPxDateEdit1.SetDate(js_date8to_devdate('13:45'));
function js_time5to_devtime(p1) {
    if (p1 == "") {
        return null;
    } else if (p1.length == 5) {
        return new Date(2000, 0, 1, p1.substr(0, 2), p1.substr(3, 2), 0);
    } else {
        return null;
    }
}
// DevTime -> HHMM, 예) var str =  js_devtimeto_time4(ASPxTimeEdit1.GetDate());
function js_devtimeto_time4(dt) {
    return "" + ("0" + dt.getHours()).substr(-2) + ("0" + dt.getMinutes()).substr(-2);
}
// HH:MM
function js_devtimeto_time5(dt) {
    return "" + ("0" + dt.getHours()).substr(-2) + ":" + ("0" + dt.getMinutes()).substr(-2);
}
// HHMMSS
function js_devtimeto_time6(dt) {
    return "" + ("0" + dt.getHours()).substr(-2) + ("0" + dt.getMinutes()).substr(-2) + ("0" + dt.getSeconds()).substr(-2);
}

//null => "" 변환
function getStr(obj) {
    var ret = "";
    if (obj == null) {
        return ret;
    } else {
        return "" + obj;
    }
}

/*
* 날짜포맷 yyyy-MM-dd 변환
*/
function getFormatDate(date) {
    var year = date.getFullYear();
    var month = (1 + date.getMonth());
    month = month >= 10 ? month : '0' + month;
    var day = date.getDate();
    day = day >= 10 ? day : '0' + day;
    return year + '-' + month + '-' + day;
}