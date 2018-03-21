var DTI = (function ($) {

function jqalert(msg, t, m) {
    if (m == null)
        m = true;
    if (t == null)
        t = '';

    $('<div id="jqalert-dialog"><p>' + msg + '</p></div>').dialog({
        title: t,
        modal: m,
        resizable:false,
        close: function (event, ui) { $('body').find('#jqalert-dialog').remove(); },
        open: function (event, ui) {
            $('.ui-widget-overlay').css('z-index', getMaxZ("*") + 1);
            $('.ui-dialog').css('z-index', getMaxZ("*") + 1);
        },
        buttons: {
            Ok: function() {
                $(this).dialog( "close" );
            }
        }
    });

}

var getMaxZ = function (selector) {
    return Math.max.apply(null, $(selector).map(function () {
        var z;
        return isNaN(z = parseInt($(this).css("z-index"), 10)) ? 0 : z;
    }));
};

var jqconfrimConfirmedWanttoMakeitLongSonoConfusion = false;
function jqconfirm(obj, m, t) {
    if (jQuery.isFunction(obj))
        jqconfrimConfirmedWanttoMakeitLongSonoConfusion = false;
    if (!jqconfrimConfirmedWanttoMakeitLongSonoConfusion) {
        if (t == null)
            t = '';

        $('<div id="jqconfirm-dialog"><p>' + m + '</p></div>').dialog
        ({
            modal: true,
            title: t,
            resizable: false,
            draggable: false,
            close: function (event, ui) { $('body').find('#jqconfirm-dialog').remove(); },
			open: function (event, ui) {
				$('.ui-widget-overlay').css('z-index', getMaxZ("*") + 1);
				$('.ui-dialog').css('z-index', getMaxZ("*") + 1);
			},
            buttons:
            {
                'Yes': function () {
                    
                    jqconfrimConfirmedWanttoMakeitLongSonoConfusion = true;
                    if (obj) {
                        if (jQuery.isFunction(obj)) {
                            obj(true);
                            jqconfrimConfirmedWanttoMakeitLongSonoConfusion = false;
                        }
                        else $(obj).click();
                    }
					$(this).dialog('close');
                },
                'No': function () {
                    $(this).dialog('close');
                    if (jQuery.isFunction(obj)) obj(false);
                }
            }
        });
    }
    if (jqconfrimConfirmedWanttoMakeitLongSonoConfusion) {
        jqconfrimConfirmedWanttoMakeitLongSonoConfusion = false;
        return true;
    }
    return false
}

function reThemePage() {
    if ($("#ThemeCheckBoxes").length >0)
        $('input:checkbox').checkbox();
    if ($("#ThemeCheckBoxes").length > 0)
        $('input:radio').radio();
    if ($("#ThemeButtons").length > 0)
        $('button, input:submit, input:button, input:reset').button();
    if ($("#ThemeTextBoxes").length > 0)
        $('input[type=text],input[type=password],textarea,select').textbox();
}

function loadWaitScreen(msg) {      
	if (msg == null)
		msg = '<strong>Loading......Please Wait</strong>';
	$('<div class="loadingDiv"><p>' + msg + '</p></div>').dialog({
		title: '',
		modal: true,
		resizable: false,
		show: 'fade',
		hide: 'fade'                  
	}).siblings('.ui-dialog-titlebar').remove();
}

function unLoadWaitScreen() {
	$('.loadingDiv').remove();
}


/************ Ajaxify Standard forms *******************************/
      
        
function getTagIndex(str, startindex) {
    while (str.charAt(startindex) != '<') {
        startindex--;
        if (startindex <= 0) return 0;
    }
    return startindex;
}

function ShowAjaxError(responce) {
    $("<div></div>").html(responce.responseText).dialog({ modal: true, width: 500, height: 500 }).dialog('open');
    return false;
}

function ajaxSubmitButton(updateAreaList, button) {
    $(button).unbind('click');
    $(button).click(function() {
        var options = {// Make submits hidden. and cool.
            success: function(data) { refreshPage(updateAreaList, true, data, true); },
            error: ShowAjaxError,
            beforeSubmit: function(formData, jqForm, options) {
                formData.push({ name: $(button).attr("name"), type: "submit", required: false, value: $(button).attr("value") });
            }
        };
        addCSSRule(".ui-effects-wrapper", "display", "inline");
        $("form").ajaxSubmit(options);
        return false;
    });
}

function refreshPage(selectorList, updateViewstate, data, executeScript, returnSubstring) {
    var fullResp = data;
    if (!returnSubstring) {
        //data = data.substring(getTagIndex(data, data.indexOf("<" + "body"))); //Concatinate the search so that the string is not found in this script block
        //data = data.substring(data.indexOf('>') + 1, data.indexOf("</" + "body" + ">")); //Concatinate the search so that the string is not found in this script block
    } else {
        data = data.substring(data.indexOf(returnSubstring) + returnSubstring.length); //Concatinate the search so that the string is not found in this script block
        data = data.substring(0, data.indexOf(returnSubstring)); //Concatinate the search so that the string is not found in this script block
    }
    if (updateViewstate) {    //Fix the viewstate and event validation.
        $("#__VIEWSTATE").val($("#__VIEWSTATE", data).val());
        $("#__EVENTVALIDATION").val($("#__EVENTVALIDATION", data).val());
    }
    if (!selectorList) {
        makePageAjaxy();
        return;
    }
    var selectors = selectorList.split(" ");
    for (i = 0; i < selectors.length; i++) {
        var selector = selectors[i];
        var endIndex = 0;
        $(selector).each(function() {  //for each item in this selector (Slided in new objects and slides out old ones)
            var old = $(this).clone();
            var newElem = $(selector, data);
            $(this).outerHTML(newElem.outerHTML());
            //$(this).html(newElem.html());
            newElem = $(this);
            $(this).find('input:checkbox').checkbox();
            $(this).find('input:radio').radio();
            $(this).find('button, input:submit, input:button, input:reset').button();
            $(this).find('input[type=text],input[type=password],textarea,select').textbox();
            //$.each($(newElem)[0].attributes, function(i, attrib) {
            //    $(this).attr(attrib.name, attrib.value);
            //});

            if (showAnimation && showAnimation.name == 'none') { } else {
                $(this).children().each(function() {  //Slide in new objects
                    if ($(this).attr("id") && $(old).find("#" + $(this).attr("id")).length == 0) {
                        //$(this).hide().show("explode", {}, 300);
                        if (!showAnimation) {
                            $(this).hide().slideDown("slow");
                        } else {
                            $(this).hide().show(showAnimation.name, showAnimation.parms, showAnimation.speed);
                            //$(this).hide().show(showAnimation.name, showAnimation.parms, showAnimation.speed);
                        }
                    }
                });
                var lastmatch = null;
                $(old).children().each(function() {  //Slide out old missing objects
                    if ($(this).attr("id"))
                        if ($(newElem).find("#" + $(this).attr("id")).length == 0) {
                        if (lastmatch == null) {
                            $(newElem).prepend($(this));
                        } else { lastmatch.after($(this)) }
                        lastmatch = $(this);
                        //$(this).hide("explode", {}, 300, function() { $(this).remove(); });
                        if (!hideAnimation) {
                            $(this).slideUp("slow", function() { $(this).remove(); });
                        } else {
                            $(this).hide(hideAnimation.name, hideAnimation.parms, hideAnimation.speed, function() { $(this).remove(); });
                            //$(this).hide(hideAnimation.name, hideAnimation.parms, hideAnimation.speed, function() { $(this).remove(); });
                        }
                    } else {
                        lastmatch = $(newElem).find("#" + $(this).attr("id"));
                    }
                });
            }
        });
        var skipfirst = 0;
        if (executeScript) {

            var loadAllScript = function() {
                var pb = __doPostBack;
                $(fullResp).filter("script").each(function() {
                    if (!$(this).attr("src")) {
                        //if ($(this).attr("id") && $("#" + $(this).attr("id")).length == 0)
                        if ($(this).attr("id") != "thiscode" && $(this).attr("id") != "bodyFadein")
                            $.globalEval(this.text || this.textContent || this.innerHTML || '');
                    }
                });
                __doPostBack = pb;
                jQuery.ready();
            };
            var loaded = false;
            $(fullResp).filter("script").each(function() {
                if ($(this).attr("src") && $("script[src='" + $(this).attr("src") + "']").length == 0) {
                    $.getScript($(this).attr("src"), function() { loadAllScript(); });
                    loaded = true;
                }
            });
            if (!loaded)
                loadAllScript();
        }
        reThemePage();
    }
    dataobj = null;
}
   
window.showAnimation = null;
window.hideAnimation = null;
function setAnimation(name, speed, optionalParms) {
    setShowAnimation(name, speed, optionalParms);
    setHideAnimation(name, speed, optionalParms);
}
function setShowAnimation(name, speed, optionalParms) {
    showAnimation = createAnimation(name, speed, optionalParms);
}
function setHideAnimation(name, speed, optionalParms) {
    hideAnimation = createAnimation(name, speed, optionalParms);
}
function createAnimation(aname, aspeed, optionalParms) {
    if (!aname) return null;
    if (aname=="") return null;
    if (!aspeed) aspeed = 300;
    if (optionalParms == null) optionalParms = {};
    return {name:aname,speed:aspeed,parms:optionalParms};
}


var latch = true;
function doRefresh(selectorList, updateViewstate) {
    if (updateViewstate == null) updateViewstate = true;
    //var re = /<\s*([a-z]+)\b[^>]*>([\S\s]*?)<\s*\/\s*\1\s*>|<[^>]*>/;
    if ($('.ui-dialog:visible').length == 0 && latch) {   //Don't do a page update if a dialog is open. That could get messy.
        latch = false
        $.get(window.location.href + "?&ajaxUniqueDate=" + new Date().getTime(), function(data) {
            refreshPage(selectorList, updateViewstate, data);
        });
        latch = true;
    }    
}

function updateAreas(filterList, delay) {
    if (delay == null) {
        doRefresh(filterList);
    } else {
        if (delay < 10000)
            delay = 10000;
        setInterval("doRefresh('" + filterList + "');", delay);
    }
}

    function ajaxSubmitButtonSearchString(updateAreaList, button, searchString) {
        $(button).unbind('click');
        $(button).click(function() {
            var options = {// Make submits hidden. and cool.
            success: function(data) { refreshArea(updateAreaList, true, data, true, searchString); },
                error: ShowAjaxError,
                beforeSubmit: function(formData, jqForm, options) {
                    formData.push({ name: $(button).attr("name"), type: "submit", required: false, value: $(button).attr("value") });
                }
            };
            addCSSRule(".ui-effects-wrapper", "display", "inline");
            $("form").ajaxSubmit(options);
            return false;
        });
    }

    function refreshArea(selector, updateViewstate, data, executeScript, searchString,doneCallback) {
        //var fullResp = data;
        if (updateViewstate) { //Fix the viewstate and event validation.
            $("#__VIEWSTATE").val($("#__VIEWSTATE", data).val());
            $("#__EVENTVALIDATION").val($("#__EVENTVALIDATION", data).val());
        }
        if (!searchString) {
            data = data.substring(getTagIndex(data, data.indexOf("<" + "body"))); //Concatinate the search so that the string is not found in this script block
            data = data.substring(data.indexOf('>') + 1, data.indexOf("</" + "body" + ">")); //Concatinate the search so that the string is not found in this script block
        } else {
            data = data.substring(data.indexOf(searchString) + searchString.length); //Concatinate the search so that the string is not found in this script block
            data = data.substring(0, data.lastIndexOf(searchString)); //Concatinate the search so that the string is not found in this script block
        }
            var endIndex = 0;
            var newElem = $(data);
            $(selector).outerHTML(newElem.outerHTML());
            //$(this).html(newElem.html());
            newElem = $(this);
            var skipfirst = 0;
            if (executeScript) {
                var loadAllScript = function() {
					if(eval("typeof __doPostBack")!="undefined")
						var pb = __doPostBack;
					
                    $(data).filter("script").each(function() {
                        if (!$(this).attr("src")) {
                            if ($(this).attr("id") != "thiscode" && $(this).attr("id") != "bodyFadein")
                                $.globalEval(this.text || this.textContent || this.innerHTML || '');
                        }
                    });
					if(eval("typeof __doPostBack")!="undefined")
						__doPostBack = pb;
                    jQuery.ready();
                };
                var loaded = false;
                $(data).filter("script").each(function() {
                    if ($(this).attr("src") && $("script[src='" + $(this).attr("src") + "']").length == 0) {
                        $.getScript($(this).attr("src"), function() { loadAllScript(); });
                        loaded = true;
                    }
                });
                if (!loaded)
                    loadAllScript();
            }
            reThemePage();
        
        dataobj = null;
    } 

function makePageAjaxy(updateAreaList,executeScript) {
    var options = {// Make submits hidden. and cool.
        success: function(data) { refreshPage(updateAreaList, true, data, executeScript); },
        error: ShowAjaxError
    };
    addCSSRule(".ui-effects-wrapper", "display", "inline");
    $(theForm).ajaxForm(options);
    __doPostBack = function(eventTarget, eventArgument) {
        if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
            theForm.__EVENTTARGET.value = eventTarget;
            theForm.__EVENTARGUMENT.value = eventArgument;
            $(theForm).ajaxSubmit(options);
        }
    }
}

(function($) {
    $.fn.outerHTML = function(s) {
        return (s)
			? this.before(s).remove()
			: $('<p>').append(this.eq(0).clone()).html();
    }
})(jQuery);

function addCSSRule(a, b, c) { for (var d = 0; d < document.styleSheets.length; d++) { var e = document.styleSheets[d]; var f = e.cssRules || e.rules; var g = a.toLowerCase(); for (var h = 0, i = f.length; h < i; h++) { if (f[h].selectorText && f[h].selectorText.toLowerCase() == g) { if (c != null) { f[h].style[b] = c; return } else { if (e.deleteRule) { e.deleteRule(h) } else if (e.removeRule) { e.removeRule(h) } else { f[h].style.cssText = "" } } } } } var e = document.styleSheets[0] || {}; if (e.insertRule) { var f = e.cssRules || e.rules; e.insertRule(a + "{ " + b + ":" + c + "; }", f.length) } else if (e.addRule) { e.addRule(a, b + ":" + c + ";", 0) } }

function css(a) {
    var sheets = document.styleSheets, o = {};
    for (var i in sheets) {
        var rules = sheets[i].rules || sheets[i].cssRules;
        for (var r in rules) {
            try {
                if (a.is(rules[r].selectorText)) {
                    o = $.extend(o, css2json(rules[r].style), css2json(a.attr('style')));
                }
            } catch (er) { }
        }
    }
    return o;
}

function css2json(css) {
    var s = {};
    if (!css) return s;
    if (css instanceof CSSStyleDeclaration) {
        for (var i in css) {
            if ((css[i]).toLowerCase) {
                s[(css[i]).toLowerCase()] = (css[css[i]]);
            }
        }
    } else if (typeof css == "string") {
        css = css.split("; ");
        for (var i in css) {
            var l = css[i].split(": ");
            s[l[0].toLowerCase()] = (l[1]);
        };
    }
    return s;
}

function themeObjectOverridable(cssClassName, objectSelector, styleId, excludeAttribs) {
    if (!excludeAttribs) excludeAttribs = [];
    var item = $("<div class='" + cssClassName + "'></div>");
    $("body").append(item);
    var style = css(item);
    var styleText = ""
    jQuery.each(style, function (i, val) {
        if (jQuery.inArray(i.toLowerCase(), excludeAttribs)==-1)
        { styleText += i + ":" + val + "; "; }
    });
    if (styleId) {
        $("head link#" + styleId).remove();
    } else {
        styleId = "";
    }
    $("<style type='text/css' id='" + styleId + "'> " + objectSelector + "{" + styleText + "} </style>").prependTo("head");
    item.remove();
    return false;
}

function rethemeBody() {
    themeObjectOverridable('ui-widget', 'body', 'bodyOverride');
    //themeObjectOverridable(cssClassName, objectSelector, styleId, excludeAttribs)
}

window.jqalert = jqalert;
window.getMaxZ = getMaxZ;
window.jqconfrimConfirmedWanttoMakeitLongSonoConfusion = jqconfrimConfirmedWanttoMakeitLongSonoConfusion;
window.jqconfirm = jqconfirm;
window.reThemePage = reThemePage;
window.loadWaitScreen = loadWaitScreen;
window.unLoadWaitScreen = unLoadWaitScreen;
window.getTagIndex = getTagIndex;
window.ShowAjaxError = ShowAjaxError;
window.ajaxSubmitButton = ajaxSubmitButton;
window.refreshPage = refreshPage;
window.setAnimation = setAnimation;
window.setHideAnimation = setHideAnimation;
window.createAnimation = createAnimation;
window.doRefresh = doRefresh;
window.updateAreas = updateAreas;
window.ajaxSubmitButtonSearchString = ajaxSubmitButtonSearchString;
window.refreshArea = refreshArea;
window.makePageAjaxy = makePageAjaxy;
window.addCSSRule = addCSSRule;
window.themeObjectOverridable = themeObjectOverridable;
window.css2json = css2json;
window.rethemeBody = rethemeBody;

	return {
		jqalert: jqalert,
		getMaxZ: getMaxZ,
		jqconfrimConfirmedWanttoMakeitLongSonoConfusion: jqconfrimConfirmedWanttoMakeitLongSonoConfusion,
		jqconfirm: jqconfirm,
		reThemePage: reThemePage,
		loadWaitScreen: loadWaitScreen,
		unLoadWaitScreen: unLoadWaitScreen,
		getTagIndex: getTagIndex,
		ShowAjaxError: ShowAjaxError,
		ajaxSubmitButton: ajaxSubmitButton,
		refreshPage: refreshPage,
		setAnimation: setAnimation,
		setHideAnimation: setHideAnimation,
		createAnimation: createAnimation,
		doRefresh: doRefresh,
		updateAreas: updateAreas,
		ajaxSubmitButtonSearchString: ajaxSubmitButtonSearchString,
		refreshArea: refreshArea,
		makePageAjaxy: makePageAjaxy,
		addCSSRule: addCSSRule,
		themeObjectOverridable: themeObjectOverridable,
		css2json: css2json,
		rethemeBody: rethemeBody,
}



})(jQuery);



//For ie, the console is used in jquery without a check. this should fix that.
if (!window.console) console = {log: function() {}}; 

/*!
 * jQuery Form Plugin
 * version: 3.09 (16-APR-2012)
 * @requires jQuery v1.3.2 or later
 *
 * Examples and documentation at: http://malsup.com/jquery/form/
 * Project repository: https://github.com/malsup/form
 * Dual licensed under the MIT and GPL licenses:
 *    http://malsup.github.com/mit-license.txt
 *    http://malsup.github.com/gpl-license-v2.txt
 */
(function(a) { function e() { if (!a.fn.ajaxSubmit.debug) return; var b = "[jquery.form] " + Array.prototype.join.call(arguments, ""); if (window.console && window.console.log) { window.console.log(b) } else if (window.opera && window.opera.postError) { window.opera.postError(b) } } function d(b) { var c = b.target; var d = a(c); if (!d.is(":submit,input:image")) { var e = d.closest(":submit"); if (e.length === 0) { return } c = e[0] } var f = this; f.clk = c; if (c.type == "image") { if (b.offsetX !== undefined) { f.clk_x = b.offsetX; f.clk_y = b.offsetY } else if (typeof a.fn.offset == "function") { var g = d.offset(); f.clk_x = b.pageX - g.left; f.clk_y = b.pageY - g.top } else { f.clk_x = b.pageX - c.offsetLeft; f.clk_y = b.pageY - c.offsetTop } } setTimeout(function() { f.clk = f.clk_x = f.clk_y = null }, 100) } function c(b) { var c = b.data; if (!b.isDefaultPrevented()) { b.preventDefault(); a(this).ajaxSubmit(c) } } "use strict"; var b = {}; b.fileapi = a("<input type='file'/>").get(0).files !== undefined; b.formdata = window.FormData !== undefined; a.fn.ajaxSubmit = function(c) { function y(b) { function F(b) { if (p.aborted || E) { return } try { C = x(o) } catch (c) { e("cannot access response document: ", c); b = w } if (b === v && p) { p.abort("timeout"); return } else if (b == w && p) { p.abort("server abort"); return } if (!C || C.location.href == j.iframeSrc) { if (!s) return } if (o.detachEvent) o.detachEvent("onload", F); else o.removeEventListener("load", F, false); var d = "success", f; try { if (s) { throw "timeout" } var g = j.dataType == "xml" || C.XMLDocument || a.isXMLDoc(C); e("isXml=" + g); if (!g && window.opera && (C.body === null || !C.body.innerHTML)) { if (--D) { e("requeing onLoad callback, DOM not available"); setTimeout(F, 250); return } } var h = C.body ? C.body : C.documentElement; p.responseText = h ? h.innerHTML : null; p.responseXML = C.XMLDocument ? C.XMLDocument : C; if (g) j.dataType = "xml"; p.getResponseHeader = function(a) { var b = { "content-type": j.dataType }; return b[a] }; if (h) { p.status = Number(h.getAttribute("status")) || p.status; p.statusText = h.getAttribute("statusText") || p.statusText } var i = (j.dataType || "").toLowerCase(); var k = /(json|script|text)/.test(i); if (k || j.textarea) { var m = C.getElementsByTagName("textarea")[0]; if (m) { p.responseText = m.value; p.status = Number(m.getAttribute("status")) || p.status; p.statusText = m.getAttribute("statusText") || p.statusText } else if (k) { var q = C.getElementsByTagName("pre")[0]; var r = C.getElementsByTagName("body")[0]; if (q) { p.responseText = q.textContent ? q.textContent : q.innerText } else if (r) { p.responseText = r.textContent ? r.textContent : r.innerText } } } else if (i == "xml" && !p.responseXML && p.responseText) { p.responseXML = G(p.responseText) } try { B = I(p, i, j) } catch (b) { d = "parsererror"; p.error = f = b || d } } catch (b) { e("error caught: ", b); d = "error"; p.error = f = b || d } if (p.aborted) { e("upload aborted"); d = null } if (p.status) { d = p.status >= 200 && p.status < 300 || p.status === 304 ? "success" : "error" } if (d === "success") { if (j.success) j.success.call(j.context, B, "success", p); if (l) a.event.trigger("ajaxSuccess", [p, j]) } else if (d) { if (f === undefined) f = p.statusText; if (j.error) j.error.call(j.context, p, d, f); if (l) a.event.trigger("ajaxError", [p, j, f]) } if (l) a.event.trigger("ajaxComplete", [p, j]); if (l && ! --a.active) { a.event.trigger("ajaxStop") } if (j.complete) j.complete.call(j.context, p, d); E = true; if (j.timeout) clearTimeout(t); setTimeout(function() { if (!j.iframeTarget) n.remove(); p.responseXML = null }, 100) } function A() { function g() { try { var a = x(o).readyState; e("state = " + a); if (a && a.toLowerCase() == "uninitialized") setTimeout(g, 50) } catch (b) { e("Server abort: ", b, " (", b.name, ")"); F(w); if (t) clearTimeout(t); t = undefined } } var b = h.attr("target"), c = h.attr("action"); f.setAttribute("target", m); if (!d) { f.setAttribute("method", "POST") } if (c != j.url) { f.setAttribute("action", j.url) } if (!j.skipEncodingOverride && (!d || /post/i.test(d))) { h.attr({ encoding: "multipart/form-data", enctype: "multipart/form-data" }) } if (j.timeout) { t = setTimeout(function() { s = true; F(v) }, j.timeout) } var i = []; try { if (j.extraData) { for (var k in j.extraData) { if (j.extraData.hasOwnProperty(k)) { i.push(a('<input type="hidden" name="' + k + '">').attr("value", j.extraData[k]).appendTo(f)[0]) } } } if (!j.iframeTarget) { n.appendTo("body"); if (o.attachEvent) o.attachEvent("onload", F); else o.addEventListener("load", F, false) } setTimeout(g, 15); f.submit() } finally { f.setAttribute("action", c); if (b) { f.setAttribute("target", b) } else { h.removeAttr("target") } a(i).remove() } } function x(a) { var b = a.contentWindow ? a.contentWindow.document : a.contentDocument ? a.contentDocument : a.document; return b } var f = h[0], g, i, j, l, m, n, o, p, q, r, s, t; var u = !!a.fn.prop; if (a(":input[name=submit],:input[id=submit]", f).length) { alert('Error: Form elements must not have name or id of "submit".'); return } if (b) { for (i = 0; i < k.length; i++) { g = a(k[i]); if (u) g.prop("disabled", false); else g.removeAttr("disabled") } } j = a.extend(true, {}, a.ajaxSettings, c); j.context = j.context || j; m = "jqFormIO" + (new Date).getTime(); if (j.iframeTarget) { n = a(j.iframeTarget); r = n.attr("name"); if (!r) n.attr("name", m); else m = r } else { n = a('<iframe name="' + m + '" src="' + j.iframeSrc + '" />'); n.css({ position: "absolute", top: "-1000px", left: "-1000px" }) } o = n[0]; p = { aborted: 0, responseText: null, responseXML: null, status: 0, statusText: "n/a", getAllResponseHeaders: function() { }, getResponseHeader: function() { }, setRequestHeader: function() { }, abort: function(b) { var c = b === "timeout" ? "timeout" : "aborted"; e("aborting upload... " + c); this.aborted = 1; n.attr("src", j.iframeSrc); p.error = c; if (j.error) j.error.call(j.context, p, c, b); if (l) a.event.trigger("ajaxError", [p, j, c]); if (j.complete) j.complete.call(j.context, p, c) } }; l = j.global; if (l && 0 === a.active++) { a.event.trigger("ajaxStart") } if (l) { a.event.trigger("ajaxSend", [p, j]) } if (j.beforeSend && j.beforeSend.call(j.context, p, j) === false) { if (j.global) { a.active-- } return } if (p.aborted) { return } q = f.clk; if (q) { r = q.name; if (r && !q.disabled) { j.extraData = j.extraData || {}; j.extraData[r] = q.value; if (q.type == "image") { j.extraData[r + ".x"] = f.clk_x; j.extraData[r + ".y"] = f.clk_y } } } var v = 1; var w = 2; var y = a("meta[name=csrf-token]").attr("content"); var z = a("meta[name=csrf-param]").attr("content"); if (z && y) { j.extraData = j.extraData || {}; j.extraData[z] = y } if (j.forceSync) { A() } else { setTimeout(A, 10) } var B, C, D = 50, E; var G = a.parseXML || function(a, b) { if (window.ActiveXObject) { b = new ActiveXObject("Microsoft.XMLDOM"); b.async = "false"; b.loadXML(a) } else { b = (new DOMParser).parseFromString(a, "text/xml") } return b && b.documentElement && b.documentElement.nodeName != "parsererror" ? b : null }; var H = a.parseJSON || function(a) { return window["eval"]("(" + a + ")") }; var I = function(b, c, d) { var e = b.getResponseHeader("content-type") || "", f = c === "xml" || !c && e.indexOf("xml") >= 0, g = f ? b.responseXML : b.responseText; if (f && g.documentElement.nodeName === "parsererror") { if (a.error) a.error("parsererror") } if (d && d.dataFilter) { g = d.dataFilter(g, c) } if (typeof g === "string") { if (c === "json" || !c && e.indexOf("json") >= 0) { g = H(g) } else if (c === "script" || !c && e.indexOf("javascript") >= 0) { a.globalEval(g) } } return g } } function x(b) { var d = new FormData; for (var e = 0; e < b.length; e++) { d.append(b[e].name, b[e].value) } if (c.extraData) { for (var f in c.extraData) if (c.extraData.hasOwnProperty(f)) d.append(f, c.extraData[f]) } c.data = null; var g = a.extend(true, {}, a.ajaxSettings, c, { contentType: false, processData: false, cache: false, type: "POST" }); if (c.uploadProgress) { g.xhr = function() { var a = jQuery.ajaxSettings.xhr(); if (a.upload) { a.upload.onprogress = function(a) { var b = 0; var d = a.loaded || a.position; var e = a.total; if (a.lengthComputable) { b = Math.ceil(d / e * 100) } c.uploadProgress(a, d, e, b) } } return a } } g.data = null; var h = g.beforeSend; g.beforeSend = function(a, b) { b.data = d; if (h) h.call(b, a, c) }; a.ajax(g) } if (!this.length) { e("ajaxSubmit: skipping submit process - no element selected"); return this } var d, f, g, h = this; if (typeof c == "function") { c = { success: c} } d = this.attr("method"); f = this.attr("action"); g = typeof f === "string" ? a.trim(f) : ""; g = g || window.location.href || ""; if (g) { g = (g.match(/^([^#]+)/) || [])[1] } c = a.extend(true, { url: g, success: a.ajaxSettings.success, type: d || "GET", iframeSrc: /^https/i.test(window.location.href || "") ? "javascript:false" : "about:blank" }, c); var i = {}; this.trigger("form-pre-serialize", [this, c, i]); if (i.veto) { e("ajaxSubmit: submit vetoed via form-pre-serialize trigger"); return this } if (c.beforeSerialize && c.beforeSerialize(this, c) === false) { e("ajaxSubmit: submit aborted via beforeSerialize callback"); return this } var j = c.traditional; if (j === undefined) { j = a.ajaxSettings.traditional } var k = []; var l, m = this.formToArray(c.semantic, k); if (c.data) { c.extraData = c.data; l = a.param(c.data, j) } if (c.beforeSubmit && c.beforeSubmit(m, this, c) === false) { e("ajaxSubmit: submit aborted via beforeSubmit callback"); return this } this.trigger("form-submit-validate", [m, this, c, i]); if (i.veto) { e("ajaxSubmit: submit vetoed via form-submit-validate trigger"); return this } var n = a.param(m, j); if (l) { n = n ? n + "&" + l : l } if (c.type.toUpperCase() == "GET") { c.url += (c.url.indexOf("?") >= 0 ? "&" : "?") + n; c.data = null } else { c.data = n } var o = []; if (c.resetForm) { o.push(function() { h.resetForm() }) } if (c.clearForm) { o.push(function() { h.clearForm(c.includeHidden) }) } if (!c.dataType && c.target) { var p = c.success || function() { }; o.push(function(b) { var d = c.replaceTarget ? "replaceWith" : "html"; a(c.target)[d](b).each(p, arguments) }) } else if (c.success) { o.push(c.success) } c.success = function(a, b, d) { var e = c.context || c; for (var f = 0, g = o.length; f < g; f++) { o[f].apply(e, [a, b, d || h, h]) } }; var q = a("input:file:enabled[value]", this); var r = q.length > 0; var s = "multipart/form-data"; var t = h.attr("enctype") == s || h.attr("encoding") == s; var u = b.fileapi && b.formdata; e("fileAPI :" + u); var v = (r || t) && !u; if (c.iframe !== false && (c.iframe || v)) { if (c.closeKeepAlive) { a.get(c.closeKeepAlive, function() { y(m) }) } else { y(m) } } else if ((r || t) && u) { x(m) } else { a.ajax(c) } for (var w = 0; w < k.length; w++) k[w] = null; this.trigger("form-submit-notify", [this, c]); return this }; a.fn.ajaxForm = function(b) { b = b || {}; b.delegation = b.delegation && a.isFunction(a.fn.on); if (!b.delegation && this.length === 0) { var f = { s: this.selector, c: this.context }; if (!a.isReady && f.s) { e("DOM not ready, queuing ajaxForm"); a(function() { a(f.s, f.c).ajaxForm(b) }); return this } e("terminating; zero elements found by selector" + (a.isReady ? "" : " (DOM not ready)")); return this } if (b.delegation) { a(document).off("submit.form-plugin", this.selector, c).off("click.form-plugin", this.selector, d).on("submit.form-plugin", this.selector, b, c).on("click.form-plugin", this.selector, b, d); return this } return this.ajaxFormUnbind().bind("submit.form-plugin", b, c).bind("click.form-plugin", b, d) }; a.fn.ajaxFormUnbind = function() { return this.unbind("submit.form-plugin click.form-plugin") }; a.fn.formToArray = function(c, d) { var e = []; if (this.length === 0) { return e } var f = this[0]; var g = c ? f.getElementsByTagName("*") : f.elements; if (!g) { return e } var h, i, j, k, l, m, n; for (h = 0, m = g.length; h < m; h++) { l = g[h]; j = l.name; if (!j) { continue } if (c && f.clk && l.type == "image") { if (!l.disabled && f.clk == l) { e.push({ name: j, value: a(l).val(), type: l.type }); e.push({ name: j + ".x", value: f.clk_x }, { name: j + ".y", value: f.clk_y }) } continue } k = a.fieldValue(l, true); if (k && k.constructor == Array) { if (d) d.push(l); for (i = 0, n = k.length; i < n; i++) { e.push({ name: j, value: k[i] }) } } else if (b.fileapi && l.type == "file" && !l.disabled) { if (d) d.push(l); var o = l.files; if (o.length) { for (i = 0; i < o.length; i++) { e.push({ name: j, value: o[i], type: l.type }) } } else { e.push({ name: j, value: "", type: l.type }) } } else if (k !== null && typeof k != "undefined") { if (d) d.push(l); e.push({ name: j, value: k, type: l.type, required: l.required }) } } if (!c && f.clk) { var p = a(f.clk), q = p[0]; j = q.name; if (j && !q.disabled && q.type == "image") { e.push({ name: j, value: p.val() }); e.push({ name: j + ".x", value: f.clk_x }, { name: j + ".y", value: f.clk_y }) } } return e }; a.fn.formSerialize = function(b) { return a.param(this.formToArray(b)) }; a.fn.fieldSerialize = function(b) { var c = []; this.each(function() { var d = this.name; if (!d) { return } var e = a.fieldValue(this, b); if (e && e.constructor == Array) { for (var f = 0, g = e.length; f < g; f++) { c.push({ name: d, value: e[f] }) } } else if (e !== null && typeof e != "undefined") { c.push({ name: this.name, value: e }) } }); return a.param(c) }; a.fn.fieldValue = function(b) { for (var c = [], d = 0, e = this.length; d < e; d++) { var f = this[d]; var g = a.fieldValue(f, b); if (g === null || typeof g == "undefined" || g.constructor == Array && !g.length) { continue } if (g.constructor == Array) a.merge(c, g); else c.push(g) } return c }; a.fieldValue = function(b, c) { var d = b.name, e = b.type, f = b.tagName.toLowerCase(); if (c === undefined) { c = true } if (c && (!d || b.disabled || e == "reset" || e == "button" || (e == "checkbox" || e == "radio") && !b.checked || (e == "submit" || e == "image") && b.form && b.form.clk != b || f == "select" && b.selectedIndex == -1)) { return null } if (f == "select") { var g = b.selectedIndex; if (g < 0) { return null } var h = [], i = b.options; var j = e == "select-one"; var k = j ? g + 1 : i.length; for (var l = j ? g : 0; l < k; l++) { var m = i[l]; if (m.selected) { var n = m.value; if (!n) { n = m.attributes && m.attributes["value"] && !m.attributes["value"].specified ? m.text : m.value } if (j) { return n } h.push(n) } } return h } return a(b).val() }; a.fn.clearForm = function(b) { return this.each(function() { a("input,select,textarea", this).clearFields(b) }) }; a.fn.clearFields = a.fn.clearInputs = function(b) { var c = /^(?:color|date|datetime|email|month|number|password|range|search|tel|text|time|url|week)$/i; return this.each(function() { var d = this.type, e = this.tagName.toLowerCase(); if (c.test(d) || e == "textarea") { this.value = "" } else if (d == "checkbox" || d == "radio") { this.checked = false } else if (e == "select") { this.selectedIndex = -1 } else if (b) { if (b === true && /hidden/.test(d) || typeof b == "string" && a(this).is(b)) this.value = "" } }) }; a.fn.resetForm = function() { return this.each(function() { if (typeof this.reset == "function" || typeof this.reset == "object" && !this.reset.nodeType) { this.reset() } }) }; a.fn.enable = function(a) { if (a === undefined) { a = true } return this.each(function() { this.disabled = !a }) }; a.fn.selected = function(b) { if (b === undefined) { b = true } return this.each(function() { var c = this.type; if (c == "checkbox" || c == "radio") { this.checked = b } else if (this.tagName.toLowerCase() == "option") { var d = a(this).parent("select"); if (b && d[0] && d[0].type == "select-one") { d.find("option").selected(false) } this.selected = b } }) }; a.fn.ajaxSubmit.debug = false })(jQuery)
