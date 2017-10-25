function jqalert(msg, t, m){
    if (m == null)
        m = true;
    if (t == null){
        $('<p>' + msg + '</p>').dialog({
            title: t,
            modal: m,
            buttons: {
                Ok: function() {
                    $(this).dialog( "close" );
                }
            }
        }).siblings('.ui-dialog-titlebar').remove();  
    }else{
        $('<p>' + msg + '</p>').dialog({
            title: t,
            modal: m,
            buttons: {
                Ok: function() {
                    $(this).dialog( "close" );
                }
            }
        });  
    }
}

function loadWaitScreen(msg) {      
	if (msg == null)
		msg = '<strong>Loading......Please Wait</strong>';
	$('<div class="loadingDiv"><p>' + msg + '</p></div>').dialog({
		title: '',
		modal: true,
		resizable: false,
		show: 'fade',
		hide: 'blind'                  
	}).siblings('.ui-dialog-titlebar').remove();
}

function unLoadWaitScreen() {
	$('.loadingDiv').remove();
}

function jqconfirm(msg, t, okcallback, cancelcallback, m){
    var ret = null;

    if (m = null)
        m = true;
    if (t == null){
        $('<p>' + msg + '</p>').dialog({
            title: t,
            modal: m,
            buttons: {
                Ok: function() {
                    $(this).dialog( "close" );
                    okcallback();
                    return;
                },
                Cancel: function() {
                    $(this).dialog( "close" );
                    cancelcallback();
                    return;
                }
            }
        }).siblings('.ui-dialog-titlebar').remove(); 
    }else{
        $('<p>' + msg + '</p>').dialog({
            title: t,
            modal: m,
            buttons: {
                Ok: function() {
                    $(this).dialog( "close" );
                    okcallback();
                    return;
                },
                Cancel: function() {
                    $(this).dialog( "close" );
                    cancelcallback();
                    return;
                }
            }
        }); 
    }
}