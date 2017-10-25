/*
 Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 For licensing, see LICENSE.html or http://ckeditor.com/license
*/
CKEDITOR.plugins.add("resizewithwindow",{init:function(c){function d(){var a=50;c.getCommand("maximize").state==CKEDITOR.TRISTATE_ON?a=jQuery(window).height():(a=jQuery(c.element.$).parents("div:first"),e=jQuery(".cke_inner",a),a=a.height());var d=jQuery(".cke_contents",e),f=jQuery(".cke_top",e),b=8;jQuery.browser.mozilla&&(b=11);jQuery.browser.webkit&&(b=12);jQuery.browser.msie&&(b=10);e.height(a-b);f=f.outerHeight(!0);b=13;-1<c.config.extraPlugins.indexOf("wordcount")&&(b=jQuery(".cke_bottom",e).outerHeight(!0),
b+=7);d.height(a-(f+b)+"px")}var e;c.on("instanceReady",function(){jQuery(function(){jQuery(window).resize(function(){d()});d()})});c.on("afterCommandExec",function(a){("toolbarCollapse"==a.data.name||"maximize"==a.data.name||"source"==a.data.name)&&d()});c.on("dataReady",function(){d()});c.on("triggerResize",function(){d()})}});