function openDialogHelper(item, elementID, URL) {
    item.html($("<iframe id='" + elementID + "_iframe' name='" + elementID + "_iframe' frameborder='0' " +
    "marginWidth='0' marginHeight='0' ALLOWTRANSPARENCY='true' scrolling='auto' " +
    "width='100%' height='100%' background='transparent' style='display:none;' " +
    "src='" + URL + "' />"));
    $('#' + elementID + '_iframe').load(function () {
        $(this)[0].style.display = 'block';
        $(this).hide().fadeIn(500);
        $(this).contents().find('body').show().css('display', 'block');
        $(this).get(0).contentWindow.window.onbeforeunload = function () {
            $('#' + elementID + '_iframe').fadeOut(500, function () {
                $(this).css('display', 'none');
            });
        };
    });
}

function createDialogURL(URL, height, width, id, title, modal) {
    return createDlg(URL, title, height, width, modal, id);
}

String.prototype.hashCode = function() {
    var hash = 0;
    if (this.length == 0) return hash;
    for (i = 0; i < this.length; i++) {
        char = this.charCodeAt(i);
        hash = ((hash << 5) - hash) + char;
        hash = hash & hash; // Convert to 32bit integer
    }
    return hash;
}

function setDlgIcon(imageUrl,dlg) {
    if (window != window.top) {
        return parent.setDlgIcon(imageUrl,getCurrentIframe().parent());
    }
    if(dlg){
        dlg.parent().find('.dlgIcon').remove();
        dlg.parent().find('.ui-dialog-title').prepend('<img class="dlgIcon" src="' + imageUrl + '" style="max-width:20px;max-height:20px;vertical-align: middle;"/>&nbsp;');
    	return dlg.parent().find('.dlgIcon');
    }   
    return null;
}

function getCurrentIframe() {
		var iframe = null;
		if (top === self)
			return false; // Not in an iframe
    $('iframe', parent.document).each(function (value) {
        if ($(this).prop('src').indexOf(document.domain) > -1) {
            if ($(this).contents().find('body').html() == $(document).contents().find('body').html()) {
                iframe = $(this);
            }
        }
    });
    return iframe;
}

function addButtons(dlg, Buttons) {
    dlg = $(dlg);
    var iframe = dlg.find('iframe');
    var newbuttons = [];
    if (Buttons != null)
        jQuery.each(Buttons, function(key, value) {

            if (jQuery.isPlainObject(value)) {
                for (var akey in value)
                    key = akey;

                value = Buttons[index][key];
            }
            var buttonName = key;
            var isSelector = false;
            if (key.indexOf("#") != 0 && key.indexOf(".") != 0)
                key = "#" + key;
            else
                isSelector = true;
            var frameButton = iframe.contents().find(key);
            if (frameButton.length > 0) {
                buttonName = frameButton.attr("value");
                frameButton.css('display', 'none');
            }
            if (frameButton.length > 0 || !isSelector) {
                newbuttons.push({
                    text: buttonName,
                    click: function() {
                        var retval = true;
                        if (jQuery.isFunction(value)) retval = !value();
                        frameButton.click()
                        if (retval) $(this).dialog("close");
                    }
                });
            }
        });
    var options = { buttons: newbuttons };
    dlg.dialog('option', options);
}

function createDlg(URL, title, height, width, modal, id, Buttons) {
    if (window != window.top) {
        return parent.createDlg(URL, title, height, width, modal, id, Buttons);
    }
    if (!title) title = "";
    if (!width) width = 400;
    if (!height) height = 400;
    if (!id) id = "dynamicDiv_" + URL.hashCode();

    if ($('#' + id).length == 0) {
        var dlgDiv = $("<div id='" + id + "' Title='" + title + "' style='height:" + height + "px;width:" + width + "px; overflow: hidden;'></div>")
        $($('form')[0]).append(dlgDiv);
        dlgDiv.dialog({
            autoOpen: false,
            open: function (event, ui) {
                dlgDiv.parent().hide();
                $('body').first().css('overflow', 'hidden');
                $('#' + id + '_iframe').load(function() { addButtons($('#' + id), Buttons); });
                dlgDiv.parent().fadeIn(10); //I'm doing a fade-in so you don't see the parent reposition itself.
                openDialogHelper($(this), id, URL);
				$('.ui-widget-overlay').css('z-index', getMaxZ("*") + 1);
				$('.ui-dialog').css('z-index', getMaxZ("*") + 1);
            },
            close: function (event, ui) {
                $('body').first().css('overflow', '');
            },
            buttons: {},
            modal: modal,
            width: width,
            height: height,
            hide: "fade"
        }).dialogExtend({ 'dblclick': 'maximize' });
    }
    //$('#' + id).appendTo($('form:first'));
    $('#' + id).dialog('open');
	return $('#' + id);
}

function addButtonsOnLoad(dlg, Buttons) {
    dlg = $(dlg);
    var iframe = dlg.find('iframe');
    iframe.load(function() { addButtons(dlg, Buttons); });
}

function closeCurrentDlg() {
    var iframe = getCurrentDlg();
    if(iframe)
        window.parent.closeAll($(iframe).parent().parent().find('.ui-dialog-content').attr('id'));
}

function getCurrentDlg(){
      if (top === self) 
        return false; // Not in an iframe   
    var iframe = getCurrentIframe();
    if(!iframe) return false; //Can't find the iframe.. 
    if (iframe.parent().hasClass("ui-dialog-content")){ //is it in a dialog  
        return iframe;
    }
    return false;
}

function addButtonsFromFrame(Buttons) {  
    var iframe = getCurrentDlg();
    if(!iframe) return false; //Can't find the iframe or dialog.. 
    parent.addButtons($(iframe.parent()), Buttons);
    parent.addButtonsOnLoad($(iframe.parent()), Buttons);
    
}

    