/*
 Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 For licensing, see LICENSE.html or http://ckeditor.com/license
*/
(function(){CKEDITOR.editor.prototype.loadToolbar=function(a){this._.events.themeSpace||CKEDITOR.plugins.registered.toolbar.init(this);a&&(this.config.toolbar=a);var a=this.fire("uiSpace",{space:"top",html:""}),b=this.ui.spaceId("top");document.getElementById(b).innerHTML=a.html}})();
CKEDITOR.plugins.add("toolbarswitch",{requires:["button","toolbar"],init:function(a){var b=a.config.toolbar;a.on("beforeCommandExec",function(c){"maximize"==c.data.name&&(a.config.toolbar==b?a.loadToolbar(a.config.maximizedToolbar):a.loadToolbar(b))})}});