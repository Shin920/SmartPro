<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/Main.master" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">    

    <style type="text/css">

        body {
            padding: 0;
            margin: 0;
        }
        #main {
            background: #0099cc;
            margin-top: 0;
            padding: 2px 0 4px 0;
            text-align: center;
        }
            #main a {
                color: #ffffff;
                text-decoration: none;
                font-size: 12px;
                font-weight: bold;
            }
                #main a:hover {
                    text-decoration: underline;
                }
        #tab_container {
            margin: 0 auto;
            width: 100%;
            margin-top: 0px;
        }
        #tabul {
            padding: 0;
            margin: 0;
            padding: 5px 0;
            z-index: 10;
        }
            #tabul li {
                display: inline;
                -webkit-border-radius: .2em .2em 0 0;
                -moz-border-radius: .2em .2em 0 0;
                border-radius: .2em .2em 0 0;
                cursor: pointer;
            }
        .ntabs {
            background: #BDC7D5;
            margin-right: 1px;
            font-size: 10px;
            font: 11px Verdana, Geneva, sans-serif;
            color: #333;
            border: 1px solid #BDC7D5;
            padding: 2px 3px 5px 8px;
        }
        .add {
            padding: 2px 8px 5px;
        }
        #addtab {
            font-size: 16px;
            text-decoration: none;
            position: relative;
            top: 2px;
            color: #333;
        }
            #addtab:hover {
                color: #999;
            }
        .ctab {
            background: LIGHTSKYBLUE;
            position: relative;
            font-size: 12px;
            font: 11px Verdana, Geneva, sans-serif;
            font-weight: bold;
            padding: 5px 3px 6px 8px;
            border-bottom-width: 0;
        }
        .close {
            text-decoration: none;
            color: #999;
            font-weight: bold;
            font-size: 14px;
            padding: 0 4px;
            -webkit-border-radius: .2em;
            -moz-border-radius: .2em;
            border-radius: .2em;
        }
            .close:hover {
                background: #999;
                color: #333;
            }

        #tabcontent {
            position: relative;
            top: -1px;
            border: 0px;
            padding: 0px;
            text-align: left;
            font-weight: bold;
        }

    </style>


    <script type="text/javascript">

        // 텝메뉴간 이동 파라미터
        var v_job = null;
        var v_pgm_id = null;
        var v_pgm_text = null;
        var v_pgm_url = null;
        var v_pgm_param = null;

        // const
        var v_height_minus = 95;   // iframe 세로 길이  75, 80
            
        function jqUpdateSize() {
               
            // Get the dimensions of the viewport
            var w = $(window).width();
            var h = $(window).height();
                
            var arr = tab_ids.split(';');
            for (var i = 0; i < arr.length; i++) {
                var tmp = arr[i].replace(/;/gi, "");
                if (tmp != "") {
                    var fName = document.getElementById("if" + tmp);
                    if (fName != null) {

                        fName.height = h - v_height_minus;
                    }
                }
            }
        };


        // resize event handler
        $(window).resize(jqUpdateSize);     // When the browser changes size


        // 텝 닫기
        var v_tab_menu_obj = null;

        function closeTabMenu(pgm_id) {

            if (pgm_id == "") {
                return;
            }

            v_tab_menu_obj = $("#close" + pgm_id);
            setTimeout("closeTabMenuTimeout()", 180);
        }

        // DevExpress close Button click 이벤트 유예 시간 할당
        function closeTabMenuTimeout() {
            if (v_tab_menu_obj != null) {
                v_tab_menu_obj.click();
            }
            v_tab_menu_obj = null;
        }


            // 텝메뉴 참조사이트 http://superdit.com/2011/05/08/create-dynamic-tabs-with-jquery/
            var tab_cnt = 0;
            var tab_ids = "";
            var tab_parent_child = "";
            var tab_active_if = "";


            // 활성화 할 텝 메뉴 검색
        function tabMenuFindPrev(pgm_id) {

            // parent~child 관계 체크
            var strParChild = tab_parent_child.split(";;").join(';');
            if (strParChild.substring(0, 1) == ";") { strParChild = strParChild.substring(1); }
            if (strParChild.substring(strParChild.length - 1) == ";") { strParChild = strParChild.substring(0, strParChild.length - 1); }

            var arrParChild = strParChild.split(";");

            for (var i = 0; i < arrParChild.length; i++) {
                // parent:child 관계 검색
                var tmp = arrParChild[i].split(":");
                if (tmp[1] == pgm_id) {
                    tab_parent_child = tab_parent_child.replace(";" + tmp[0] + ":" + tmp[1] + ";", "");
                    return tmp[0];
                }
            }


            // 메뉴 표시 순서 체크
            var str = tab_ids.split(";;").join(';');

            if (str.substring(0, 1) == ";") { str = str.substring(1); }
            if (str.substring(str.length - 1) == ";") { str = str.substring(0, str.length - 1); }

            var arr = str.split(";");
            var prev = "";

            for (var i = 0; i < arr.length; i++) {
                // 이전 메뉴 찾기
                if (i >= 1) { prev = arr[i - 1]; }
                if (pgm_id == arr[i]) {
                    break;
                }
            }

            if (arr.length >= 2 && prev == "") {
                // 이전 메뉴가 없는 경우 마지막 메뉴를 선택함
                prev = arr[arr.length - 1];
            }
            return prev;
        }

        // 텝메뉴 추가
        function tabMenuAddMenu(pgm_id, parent_id) {
            tab_ids += ";" + pgm_id + ";";
            tab_cnt++;
            tab_active_if = "if" + pgm_id;     //active tab 보관

            if ("" + parent_id != "undefined") {
                tab_parent_child += ";" + parent_id + ":" + pgm_id + ";";
            }
        }

        // 텝메뉴 제거
        function tabMenuDelMenu(pgm_id) {
            tab_cnt--;
            tab_ids = tab_ids.replace(";" + pgm_id + ";", "");
        }

        $(function () {

            // 최초 탭
            $("#addtab, #litab").click(function () {
                tab_cnt++;
                $("#tabcontent p").hide();
                //메뉴 상단에 + 표시되어 있는 버튼 - 사용안함으로 주석처리 -
                //addtab('board','게시판', '/form/CM/UCM_BOARD_L010.aspx');
                return false;
            });

        });

            // 텝메뉴에 화면 표시
        function addTabMenu(pgm_id, pgm_text, pgm_url, parent_id) {
            $("#tabcontent p").hide();
            return addtab(pgm_id, pgm_text, pgm_url, parent_id);
        }

        function addtab(pgm_id, pgm_text, pgm_url, parent_id) {
            var v_deli = "";

            if (pgm_url.substring(pgm_url.length - 5).toUpperCase() == ".ASPX") {
                v_deli = "?";
            } else {
                v_deli = "&";
            }

            // 중복 tab 체크
            if (tab_ids.indexOf(";" + pgm_id + ";") >= 0) {
                $("#tabul li").removeClass("ctab");
                $("#t" + pgm_id).addClass("ctab");
                $("#tabcontent p").hide();
                $("#c" + pgm_id).fadeIn('fast');

                tab_active_if = "if" + pgm_id;     //active tab 보관

                $("#if" + pgm_id).attr("src", pgm_url + v_deli + 'pgm_id=' + pgm_id);

                return false;
            }

            // tab 추가                
            var closetab = '<a href="" id="close' + pgm_id + '" class="close">&times;</a>';
            $("#tabul").append('<li id="t' + pgm_id + '" class="ntabs">' + pgm_text + '&nbsp;&nbsp;' + closetab + '</li>');
            $("#tabcontent").append('<p style="margin:0 0 0 0;" id="c' + pgm_id + '"><iframe id="if' + pgm_id + '" name="if' + pgm_id + '" border="0" frameborder="0" width="100%" style="overflow-x: hidden; overflow-y: hidden ;" height="' + ($(window).height() - v_height_minus) + '"  src="' + pgm_url + v_deli + 'pgm_id=' + pgm_id + '" ></iframe></p>');

            $("#tabul li").removeClass("ctab");
            $("#t" + pgm_id).addClass("ctab");


            // 텝메뉴 추가
            tabMenuAddMenu(pgm_id, parent_id);


            // tab click event
            $("#t" + pgm_id).bind("click", function () {
                $("#tabul li").removeClass("ctab");
                $("#t" + pgm_id).addClass("ctab");
                $("#tabcontent p").hide();
                $("#c" + pgm_id).fadeIn('fast');
                tab_active_if = "if" + pgm_id;     //active tab 보관

                // childFunc() 실행
                if (tab_active_if != "") {

                    var obj = eval(tab_active_if);

                    if ("undefined" != obj) {
                        if ("undefined" != "" + obj.childFunc) {
                            obj.childFunc();
                        }
                    }
                }

            });

            // tab close event
            $("#close" + pgm_id).bind("click", function () {

                // activate the previous tab
                $("#tabul li").removeClass("ctab");
                $("#tabcontent p").hide();

                $(this).parent().remove();
                $("#c" + pgm_id).remove();

                var prev_id = tabMenuFindPrev(pgm_id);

                // 텝메뉴 제거
                tabMenuDelMenu(pgm_id);

                if (prev_id != "") {
                    $("#tabul li").removeClass("ctab");
                    $("#t" + prev_id).addClass("ctab");
                    $("#tabcontent p").hide();
                    $("#c" + prev_id).fadeIn('fast');
                    tab_active_if = "if" + prev_id;     //active tab 보관
                }

                return false;
            });
        }


    </script>


    <%-- 선택 메뉴를 다시 보기를 원한다면. style="display: none;" 코드를 삭제하시면 됩니다. --%>
    <div id="tab_container">
        <ul id="tabul" style="display: none ;">
            <li id="litab" class="ntabs add"><a href="" id="addtab">+</a></li>
        </ul>            
        <table width="100%" border="1" cellpadding="0" cellspacing="0" style="display: none;"><tr><td></td></tr></table>
        <div style="margin:0 0 0 0; overflow: hidden; " id="tabcontent"></div>
    </div>

</asp:Content>