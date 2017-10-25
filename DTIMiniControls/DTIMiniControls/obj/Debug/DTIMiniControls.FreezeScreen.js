function FreezeScreenEl(elementID) { 
    if(typeof UnfreezeScreen == 'function') {UnfreezeScreen();}
	$(top.document.body).css({overflow: "hidden" });
	$(top.document.body).parent().css({overflow: "hidden" });    
    var arrPageSizes = getWindowSize();
    var $topDiv=$('#'+ elementID);
	$('#overlayInnerTable_' + elementID,$topDiv).css({height: arrPageSizes[1]});
	$topDiv.css({height: arrPageSizes[1]});
	$topDiv.css({
			    position:           "absolute",
			    top:                0,
			    left:               0,
				width:				arrPageSizes[0],
				margin:             0,
				height:				arrPageSizes[1]
	 });
	 $topDiv.center();
}

function UnfreezeScreenEl(elementID) {
    $("#" + elementID).fadeOut();
	if($("#tempCenterDivID" + elementID,top.document)) $("#tempCenterDivID" + elementID,top.document).fadeOut();
	$(top.document.body).css({overflow: "auto"});
	$(top.document.body).parent().css({overflow: "auto" });
}

$(document).ready(function(){
  breakoutFrameset();
});

function breakoutFrameset(){
    var frm = $('iframe',top.document.body);
    if (top.document != window.document){
    if (frm.length == 0) {
       top.window.location="~/res/DTIMiniControls/iframed.aspx?url="+top.document.location;
    }}
}





