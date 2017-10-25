function FreezeItEl(elementID, overlayId) {
    var ele = $("#" + elementID);
    var offset = ele.position();
    $("#" + overlayId).css({
			    position:           "absolute",
			    top:                offset.top,
			    left:               offset.left,
				minWidth:			ele.outerWidth(),
				width:              ele.outerWidth(),
				margin:             0,
				minHeight:			ele.outerHeight(),
				height: 			ele.outerHeight()
	 });
	 $("#" + overlayId + " > table").css({height: ele.innerHeight()});
	 $("#" + overlayId).fadeIn();
}

function UnfreezeItEl(overlayId) {
    $("#" + overlayId).fadeOut();
}

function unFreezeIt(elementID) {
    var overlayId = elementID+"_freezer";
    $("#" + overlayId).fadeOut();
}

function FreezeIt(elementID,bgcolor,opacity) {
    var ele = $("#" + elementID);
    var offset = ele.position();
    var overlayId = elementID+"_freezer";
    if(!bgcolor)bgcolor="#000000";
    if(!opacity)opacity=0.7;
    if(!$("#" + overlayId).length){
        var freezeHTML = "<div id='"+overlayId+"' style=\"display:none; position:absolute; z-index:1000; background-color:"+bgcolor+"; opacity: "+opacity+"; -ms-filter:'progid:DXImageTransform.Microsoft.Alpha(Opacity="+(opacity*100)+")'; filter: alpha(opacity="+(opacity*100)+");\"></div>";
        $("form:first").append(freezeHTML);
    }
    $("#" + overlayId).css({
			    position:           "absolute",
			    top:                offset.top,
			    left:               offset.left,
				minWidth:			ele.outerWidth(),
				width:              ele.outerWidth(),
				margin:             0,
				minHeight:			ele.outerHeight(),
				height: 			ele.outerHeight()
	 });
	 $("#" + overlayId + " > table").css({height: ele.innerHeight()});
	 $("#" + overlayId).fadeIn();
}
