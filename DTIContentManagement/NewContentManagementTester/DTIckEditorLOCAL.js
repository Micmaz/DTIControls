CKEDITOR.config.protectedSource.push(/<protected>[\s\S]*<\/protected>/g);
CKEDITOR.config.allowedContent = true;
CKEDITOR.disableAutoInline = true;
//CKEDITOR.config.codemirror = {
//    lineWrapping: false
//};
CKEDITOR.on('instanceCreated', function (event) {
    if (!$('#topToolbar').length) {
        $("form").prepend("<div id='topToolbar' style='position: fixed; top: 0px; display: none; z-index:99999;'>");
        //$('#topToolbar').hide();
    }
    var editor = event.editor,
		element = editor.element;

    editor.on('change', function (ev) {
        if (element.getAttribute('id')) {
            $("#" + element.getAttribute('id').replace("_HTML", "_Hidden")).val($.urlEncode(editor.getData()));
            //$("#" + element.getAttribute('id').replace("_HTML", "_Hidden")).val(editor.getData());
        }
    });

    editor.on('simpleuploads.imageDropped', function (ev) {
        var img = ev.data.element;
        if (img && img.tagName.toLowerCase() == "img") {
            var scale = 200 / img.height;
            img.height = 200;
            img.width = scale * img.width;
        }
    });

    editor.on('simpleuploads.finishedUpload', function (ev) {
        if (element.getAttribute('id')) {
            var img = ev.data.element;
            $("#" + element.getAttribute('id').replace("_HTML", "_Hidden")).val($.urlEncode(editor.getData()));
            //$("#" + element.getAttribute('id').replace("_HTML", "_Hidden")).val(editor.getData());
        }
    });

    editor.on('blur', function (ev) {
        hidebuttons(editor)
    });

    editor.on('configLoaded', function () {

        var element = editor.element;
        var toolbar = $('#cke_' + element.getAttribute('id'));
        editor.config.toolbar = element.getAttribute('id');
    });
});

var editor;
CKEDITOR.on('currentInstance', function (event) {
    if (CKEDITOR.currentInstance) {
        editor = CKEDITOR.currentInstance;
        editor.setReadOnly(false);
        var element = editor.element;
        var toolbar = $('#cke_' + element.getAttribute('id'));
        if (editor.config.sharedSpaces)
            setupToolbar(toolbar);
        showbuttons($("#" + element.getAttribute('id')).parent());
    }
});

CKEDITOR.on('instanceReady', function (ev) {
    var Tags = ['div', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'p', 'pre', 'ul', 'li', 'table', 'td', 'tr', 'block​quote'];
    var rules = {
        indent: true,
        breakBeforeOpen: true,
        breakAfterOpen: true,
        breakBeforeClose: true,
        breakAfterClose: true
    };
    for (var i = 0; i < Tags.length; i++) {
        ev.editor.dataProcessor.writer.setRules(Tags[i], rules);
    }
    //Tags = ['a', 'span','font','b','u','li'];
    Tags = ['li'];
    rules = {
        indent: true,
        breakBeforeOpen: true,
        breakAfterOpen: false,
        breakBeforeClose: false,
        breakAfterClose: true
    };

    for (var i = 0; i < Tags.length; i++) {
        ev.editor.dataProcessor.writer.setRules(Tags[i], rules);
    }

});

function showbuttons(editArea) {
    if (editArea.find(".btnarea").length == 0) {
        var btnarea = $("<div class='btnarea cke_focus'></div>");
        editArea.append(btnarea);
        btnarea.hide();
        editArea.find(".dtiCKEButton").each(function () {
            $(this).show();
            if ($(this).button) $(this).button();
            $(this).appendTo(btnarea);
        });
        btnarea.fadeIn();

    } else {
        editArea.find(".btnarea").fadeIn();
    }
    //var toolbar = $('#cke_' + element.getAttribute('id'));
    if (CKEDITOR.currentInstance.config.sharedSpaces)
        ckeDisplayToolbar(null);
    //$('#topToolbar').fadeIn();
}

function hidebuttons(editor) {
    var element = editor.element;
    $('#' + element.getAttribute('id')).parent().find(".btnarea").fadeOut();
    var toolbar = $('#cke_' + element.getAttribute('id'));
    if (editor.config.sharedSpaces)
        ckeHideToolbar(toolbar);
}

function setupToolbar(toolbar) {
    //toolbar.css("display", "none");
    //toolbar.css("position", "fixed");
    toolbar.css("top", "0px");
    toolbar.css("left", "0px");
    toolbar.css("width", "100%");

    if ($("#lowerbtn").length == 0) {
        var lowerbtn = $("<img id='lowerbtn' style='position:absolute;bottom:0;left:50%;width:59px;' class='lowerbtn' src='~/res/BaseClasses/Scripts.aspx?d=&f=ckEditor/extendup.gif'/>")
        $(toolbar).append(lowerbtn);
        var toolheight = $('#topToolbar').outerHeight(true) - 10;
        lowerbtn.toggle(
                    function () {
                        $(this).attr("src", "~/res/BaseClasses/Scripts.aspx?d=&f=ckEditor/extend.gif");
                        //$(toolbar).animate({ "top": "-=" + toolheight + "px" }, "slow");
                        $('#topToolbar').animate({ "top": "-=" + toolheight + "px" }, "slow");
                    },
                    function () {
                        $(this).attr("src", "~/res/BaseClasses/Scripts.aspx?d=&f=ckEditor/extendup.gif");
                        if ($('#topToolbar').css("top").replace("px", "") < 0)
                            $('#topToolbar').animate({ "top": "+=" + toolheight + "px" }, "slow");

                        //if ($(toolbar).css("top").replace("px", "") < 0)
                        //    $(toolbar).animate({ "top": "+=" + toolheight + "px" }, "slow");
                    });
    }
    $(toolbar).append($("#lowerbtn"));
}

var visited = {};
function ckeDisplayToolbar(toolbar) {
    $('#topToolbar').stop(true).fadeTo(200, 1);
}
function ckeHideToolbar(toolbar) {
    $('#topToolbar').stop(true).fadeTo(200, 0, function () { $(this).hide(); });
}

var reconnect_count = 0;
function Reconnect() {
    reconnect_count++;
    window.status = 'Session keepalive sent: ' + reconnect_count.toString() + ' time(s)';
    var img = new Image(1, 1);
    img.src = '~/res/ckeditor/keepAlive.aspx';
}

function keepAlive() {
    if (reconnect_count == 0) {
        Reconnect();
        window.setInterval('Reconnect()', 120000);
    }
}

$(function () {
    keepAlive();
})

