<%@ Page Language="C#" MasterPageFile="~/MASTER/MasterMain.master" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="glb_Main" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        // 텝메뉴간 이동 파라미터
        var v_job = null;
        var v_pgm_id = null;
        var v_pgm_text = null;
        var v_pgm_url = null;
        var v_pgm_param = null;

        // const
        var v_height_minus = 60;   // iframe 세로 길이  75, 80

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
                    //return tmp[0];    --사용시 정상적으로 이전 폼이 나오지 않는 에러 있어 주석처리
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

                //전체 닫기 이후 메인화면 표기
                addTabMenu('MAIN', '메인화면', 'MainContent.aspx');
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

                // 기존폼 보여주기
                if (tab_active_if != "") {

                    var obj = eval(tab_active_if);

                    if ("undefined" != obj) {
                        if ("undefined" != "" + obj.childFunc) {
                            obj.childFunc();
                        }
                    }
                }

                // 기존폼 재실행
                //$("#if" + pgm_id).attr("src", pgm_url + v_deli + 'pgm_id=' + pgm_id);

                return;
                //return false;
            }

            // tab 추가  (메인화면은 닫기 버튼 없음)              
            var closetab = '<a href="" id="close' + pgm_id + '" class="close">&times;</a>';
            if (pgm_id == "MAIN") {
                closetab = '<li class="main"></li>'
            }

            $("#tabul").append('<li id="t' + pgm_id + '" class="ntabs">' + pgm_text + '&nbsp;&nbsp;' + closetab + '</li>');
            $("#tabcontent").append('<p style="margin:0 0 0 0;" id="c' + pgm_id + '"><iframe id="if' + pgm_id + '" name="if' + pgm_id + '" border="0" frameborder="0" width="100%" style="overflow-x: hidden; overflow-y: scroll ;" height="' + ($(window).height() - v_height_minus) + '"  src="' + pgm_url + v_deli + 'pgm_id=' + pgm_id + '" ></iframe></p>');

            // 이종현
            /*
            if (pgm_id != "MAIN") {
                txtFormID.SetText(pgm_text);
                callBackForm.PerformCallback();
            }
            */

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

                // ??? $(this).parent().next().addClass("ctab");
                // ??? $("#c" + pgm_id).next().fadeIn('fast');

                // ??? var v = $("#c" + pgm_id).next();

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


                ///try{
                ///    tab_active_if = v[0].id.substring(1);
                ///} catch (ex) {
                ///    tab_active_if = "";
                ///}

                return false;
            });
        }

        //전체닫기
        function litabAllClose() {
            var arr = tab_ids.split(';');

            for (var i = 0; i < arr.length; i++) {
                var tmp = arr[i].replace(/;/gi, "");

                if (tmp != "") {
                    v_tab_menu_obj = $("#close" + tmp);
                    v_tab_menu_obj.click();
                }
            }
        }


        // 이종현
        /*
        function panelEndCallback(s, e) {
            
        }
        */

    </script>

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
            position: relative;
            padding: 0;
            margin: 0;
            padding: 5px 0;
            /*z-index: 10;*/
            background: url(/SmartPro/Contents/Main/images/tab_back.png) repeat-x bottom;
            background-color: #d4e1dc;
        }

            #tabul li {
                display: inline;
                -webkit-border-radius: .2em .2em 0 0;
                -moz-border-radius: .2em .2em 0 0;
                border-radius: .2em .2em 0 0;
                cursor: pointer;
            }

        .ntabsClose {
            background-image: linear-gradient(white 4%, #e8f5f2 5%);
            background: url(/SmartPro/Contents/Main/images/tab_all_close.png) no-repeat;
            background-color: #e8f5f2;
            background-position: center;
            border: 1px solid #88a99d;
            margin-right: 1px;
            font-size: 12px;
            color: #333;
            padding: 5px 3px 5px 8px;
        }

        .ntabs {
            background-image: linear-gradient(white 4%, #e8f5f2 5%);
            background-color: #e8f5f2;
            border: 1px solid #88a99d;
            margin-right: 1px;
            font-size: 12px;
            color: #333;
            padding: 5px 3px 5px 8px;
        }

        .add {
            padding: 2px 8px 5px 8px;
            margin-left: 1px;
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
            background-color: #fff;
            background-image: none;
            border: 1px solid #88a99d;
            border-bottom: 1px solid #fff;
            color: #222;
            position: relative;
            font-size: 12px;
            font-weight: bold;
            padding: 5px 3px 6px 8px;
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

        .main {
            text-decoration: none;
            color: #999;
            font-weight: bold;
            font-size: 14px;
            padding: 0 4px;
            -webkit-border-radius: .2em;
            -moz-border-radius: .2em;
            border-radius: .2em;
            padding: 0 0 0 0;
        }

            .main:hover {
                background: #999;
                color: #333;
            }

        #tabcontent {
            background-color: #fff;
            position: relative;
            /*top: -0.5px;*/
            padding: 0px;
            text-align: left;
            font-weight: bold;
        }
    </style>


    <div id="tab_container">
        <ul id="tabul">
            <li id="litab" class="ntabsClose add" style="padding-top: 5px;" onclick="litabAllClose();">&nbsp;&nbsp;</li>
        </ul>

        <div style="margin: 0 0 0 0; border: 1px solid #88a99d; border-top: 0px;" id="tabcontent"></div>
    </div>

    <!-- 이종현 -->
    <%--
    <dx:ASPxTextBox ID="txtFormID" ClientInstanceName="txtFormID" runat="server" Width="100%" ClientVisible="false"/>
        
    <dx:ASPxCallbackPanel ID="callBackForm" ClientInstanceName="callBackForm" OnCallback="callBackForm_Callback" runat="server">
        <PanelCollection>
            <dx:PanelContent>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    --%>

</asp:Content>
