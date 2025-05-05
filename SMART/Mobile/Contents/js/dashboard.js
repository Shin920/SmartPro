$(document).ready(function(){
$(function(){
    $("#gnb_on,#gnb_box,#m_wrap,#click_close,#click_open").hide();
});

$(function(){
	var a=0;
	var b=0;

	$("#gnb_off,#gnb_on,#gnb_box").click(function(){
		a++;
		b=a%2;

		if(b==1){
			$("#gnb_off").fadeOut();
			$("#gnb_on,#gnb_box").fadeIn();
			$("#m_wrap").fadeIn();
		}else{
			$("#gnb_off").fadeIn();
			$("#gnb_on,#gnb_box").fadeOut();
			$("#m_wrap").fadeOut();
		}return false;
	});
});

//$(function(){
//		var a=0;
//		var b=0;
//
//		$(".gnb").click(function(){
//			a++;
//			b=a%2;
//			
//			if(b==1){                
//				$(this).addClass("on");
//				$(this).children(".gnb h1").addClass("on");
//				$(this).next(".snb").show(0);
//			}else{
//                $(this).removeClass("on");
//				$(this).children("h1").removeClass("on");
//				$(this).next(".snb").hide(0);
//			}return false;
//		});
//	});    
    
    $(".gnb").click(function(){
       if($(this).hasClass("on")){
           $(this).removeClass("on");
           $(this).children("h1").removeClass("on");
           $(this).next(".snb").hide(0);
       }else{
       		$(this).addClass("on");
            $(this).children(".gnb h1").addClass("on");
			$(this).next(".snb").show(0);
       } 
    });


});


