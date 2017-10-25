function endRequestHS() 
{
    if(window.hs !== undefined) {
        hs.updateAnchors();
    } else if(parent.window.hs !== undefined) {
        parent.window.hs.updateAnchors();
    }

    if (window.addthis) {
        window.addthis.ost = 0;
        window.addthis.ready();
    }
    
    if ($.query.load(window.location.hash).get("image")) {
        $.each($('#' + $.query.load(window.location.hash).get("image")), function() {
            if(parseInt($(this).closest('.Control_Holder').children('.Gallery_Button_Div').children('.Page_Textbox').val().trim()) == parseInt($(this).closest('.Gallery_Page').attr('id').substring($(this).closest('.Gallery_Page').attr('id').indexOf('_page') + 5))) {
                this.onclick();
                return;
            }
        });
    }
}

var hsWrapperBG = '';
var hsOutlineBG = '';
var hsHTMLBGC = '';

if(window.hs !== undefined) {

    hs.Expander.prototype.onBeforeExpand = function () {
        if(this.a.href.indexOf("VideoViewer.aspx") > -1) {
            hsWrapperBG = $('.highslide-wrapper').css('background');
            hsOutlineBG = $('.highslide-outline').css('background');
            hsHTMLBGC = $('.highslide-html').css('background-color');
            $('.highslide-wrapper, .highslide-outline').css('background','none');
            $('.highslide-html').css('background-color','transparent');    
        }
    }
    
    hs.Expander.prototype.onAfterClose = function () {
        if(this.a.href.indexOf("VideoViewer.aspx") > -1) {
            $('.highslide-wrapper').css('background',hsWrapperBG);
            $('.highslide-outline').css('background',hsOutlineBG);
            $('.highslide-html').css('background-color',hsHTMLBGC);    
        }
    }

    hs.Expander.prototype.onAfterExpand = function () {
        if ($(this.a).closest(".thumbImageCell").length > 0) {
            window.location.hash = "#" + $.query.load(window.location.href).set("image", this.a.id);
        }
    } 

    hs.Expander.prototype.onBeforeClose = function () {
       window.location.hash = "#" + $.query.load(window.location.href).remove("image");
    }
    
    hs.previousOrNext = function (el, op) {
	    var exp = hs.getExpander(el);
	    if (exp) {
	        var myThumbId = $(exp.a).closest('.thumbnailPictureSpan').attr('id');
	        if(op == 1 && myThumbId == $(exp.a).closest('.Gallery_Page').children('.thumbnailPictureSpan:last').attr('id')) {
                var prefix = $.query.load(window.location.href).get("image");
                if (prefix) {
                    prefix = prefix.substring(0, prefix.lastIndexOf("_"));
                    prefix = prefix.substring(0, prefix.lastIndexOf("_") + 1);
                    while (prefix.indexOf("_") > -1) {
                        prefix = prefix.replace("_", "$");
                    }
                    $(exp.a).closest('.Control_Holder').children('.Gallery_Button_Div').children('.Forward_Button').click();
                }
            }
            else if(op == -1 && myThumbId == $(exp.a).closest('.Gallery_Page').children('.thumbnailPictureSpan:first').attr('id')) {
                var prefix = $.query.load(window.location.href).get("image");
                if (prefix) {
                    prefix = prefix.substring(0, prefix.lastIndexOf("_"));
                    prefix = prefix.substring(0, prefix.lastIndexOf("_") + 1);
                    while (prefix.indexOf("_") > -1) {
                        prefix = prefix.replace("_", "$");
                    }
                    $(exp.a).closest('.Control_Holder').children('.Gallery_Button_Div').children('.Back_Button').click();
                }
            }
	        return hs.transit(exp.getAdjacentAnchor(op), exp);
	    }
	    else return false;
    }

} else if(parent.window.hs !== undefined) {
    parent.window.hs.Expander.prototype.onBeforeExpand = function () {
        if(this.a.href.indexOf("VideoViewer.aspx") > -1) {
            hsWrapperBG = $('.highslide-wrapper').css('background');
            hsOutlineBG = $('.highslide-outline').css('background');
            hsHTMLBGC = $('.highslide-html').css('background-color');
            $('.highslide-wrapper, .highslide-outline').css('background','none');
            $('.highslide-html').css('background-color','transparent');      
        }
    }
    
    parent.window.hs.Expander.prototype.onAfterClose = function () {
        if(this.a.href.indexOf("VideoViewer.aspx") > -1) {
            $('.highslide-wrapper').css('background',hsWrapperBG);
            $('.highslide-outline').css('background',hsOutlineBG);
            $('.highslide-html').css('background-color',hsHTMLBGC);    
        }
    }
    
    parent.window.hs.Expander.prototype.onAfterExpand = function () {
       if ($(this.a).closest(".thumbImageCell").length > 0) {
            window.location.hash = "#" + $.query.load(window.location.href).set("image", this.a.id);
       }
    } 

    parent.window.hs.Expander.prototype.onBeforeClose = function () {
       window.location.hash = "#" + $.query.load(window.location.href).remove("image");
    }
    
    parent.window.hs.previousOrNext = function (el, op) {
	   var exp = parent.window.hs.getExpander(el);
	    if (exp) {
	        var myThumbId = $(exp.a).closest('.thumbnailPictureSpan').attr('id');
	        if(op == 1 && myThumbId == $(exp.a).closest('.Gallery_Page').children('.thumbnailPictureSpan:last').attr('id')) {
                var prefix = $.query.load(window.location.href).get("image");
                if (prefix) {
                    prefix = prefix.substring(0, prefix.lastIndexOf("_"));
                    prefix = prefix.substring(0, prefix.lastIndexOf("_") + 1);
                    while (prefix.indexOf("_") > -1) {
                        prefix = prefix.replace("_", "$");
                    }
                    $(exp.a).closest('.Control_Holder').children('.Gallery_Button_Div').children('.Forward_Button').click();
                }
            }
            else if(op == -1 && myThumbId == $(exp.a).closest('.Gallery_Page').children('.thumbnailPictureSpan:first').attr('id')) {
                var prefix = $.query.load(window.location.href).get("image");
                if (prefix) {
                    prefix = prefix.substring(0, prefix.lastIndexOf("_"));
                    prefix = prefix.substring(0, prefix.lastIndexOf("_") + 1);
                    while (prefix.indexOf("_") > -1) {
                        prefix = prefix.replace("_", "$");
                    }
                    $(exp.a).closest('.Control_Holder').children('.Gallery_Button_Div').children('.Back_Button').click();
                }
            }
	        return parent.window.hs.transit(exp.getAdjacentAnchor(op), exp);
	    }
	    else return false;
    }
}