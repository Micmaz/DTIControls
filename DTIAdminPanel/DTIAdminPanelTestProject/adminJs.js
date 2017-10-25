//$(document).ready(function() {
//	var move = $("#" + DTIDivID).innerHeight();
//	//document.ready gets hit mystically twice when the change happens
//	if(!$("#DTIOriginalBody").length){
//		$("#aspnetForm").wrapInner("<div id=\"DTIOriginalBody\"></div>");
//		$("#" + DTIDivID).insertBefore("#DTIOriginalBody");
//		$("#DTIOriginalBody").css("background-image", $("body").css("background-image"));
//		$("#DTIOriginalBody").css("background-color", $("body").css("background-color"));
//		$("#DTIOriginalBody").css("background-repeat", $("body").css("background-repeat"));
//		$("#DTIOriginalBody").css("margin", $("body").css("margin"));
//		$("#DTIOriginalBody").css("padding", $("body").css("padding"));
//		$("body").css("background-image", "");
//		$("body").css("background-color", "");
//		$("body").css("background-repeat", "");
//		$("body").css("margin", "");
//		$("body").css("padding", "");
//		try{
//		var leftpad = $("#aspnetForm").attr("offsetLeft");
//		$("#DTIOriginalBody").css("margin-left", -leftpad);
//		$("#DTIOriginalBody").css("width", $("body").innerWidth()+leftpad*2);
//		$("#DTIOriginalBody").css("height", $("body").innerHeight());
//		}
//		catch(err){}		
//	}
//	jQuery.each($("*"), function(){
//		if ( $(this).css('position') == 'absolute' )
//        {
//            $(this).css ( {'top' : $(this).offset().top + move });
//        }
//	});
//});

