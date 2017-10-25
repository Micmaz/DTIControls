	(function($) {
	    $.fn.center = function() {
	        return this.each(function() {
				var $this = $(this);
				var frameXOffset = 0, frameYOffset = 0, windowHeight = 0, windowWidth = 0;
				if (top.document != $this.attr("ownerDocument")) {
								    $this.fadeOut("fast");
					var $newdiv;
					var newdivID = "tempCenterDivID"+$this.attr("id");
					$("#"+newdivID,top.window.document.documentElement).remove();
					$newdiv=$this.clone();
					$newdiv.attr("id",newdivID);
					if($newdiv.attr('outerHTML'))  //ie hack
					    $(top.document.body).append($newdiv.attr('outerHTML'));
					else
					$newdiv.appendTo($(top.document.body));
					$newdiv=$("#"+newdivID,top.window.document)
					$newdiv.center();
				} else {
					//we are not in a frame
					var arrPageSizes = getWindowSize();
					windowWidth =arrPageSizes[0];
					windowHeight = arrPageSizes[1];
				var elHeight = $this.height();
				var newTop = ((windowHeight/2) - (elHeight/2)) - frameYOffset + $(top.window.document.documentElement).scrollTop();
				$this.css ({
					left: ((windowWidth/2) - ($this.width()/2)) - frameXOffset + $(top.window.document.documentElement).scrollLeft(),
					top: newTop,
					position: "absolute",
					display: "none"
				});
				$this.fadeIn();
				}
			});
		};
	})(jQuery);
	
	
 function getWindowSize() {
  var myWidth = 0, myHeight = 0, myTop=0, myLeft=0;
  if( typeof( top.window.innerWidth ) == 'number' ) {
    //Non-IE
    myWidth = top.window.innerWidth;
    myHeight = top.window.innerHeight;
  } else if( top.document.documentElement && ( top.document.documentElement.clientWidth || top.document.documentElement.clientHeight ) ) {
    //IE 6+ in 'standards compliant mode'
    myWidth = top.document.documentElement.clientWidth;
    myHeight = top.document.documentElement.clientHeight;
  } else if( top.document.body && ( top.document.body.clientWidth || top.document.body.clientHeight ) ) {
    //IE 4 compatible
    myWidth = top.document.body.clientWidth;
    myHeight = top.document.body.clientHeight;
  }
  
  arrayPageSize = new Array(myWidth,myHeight);
  return arrayPageSize;
}