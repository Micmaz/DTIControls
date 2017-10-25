/**
* @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
* For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {

    // %REMOVE_START%
    // The configuration options below are needed when running CKEditor from source files.
    //config.plugins = 'dialogui,dialog,a11yhelp,dialogadvtab,basicstyles,bidi,blockquote,clipboard,button,panelbutton,panel,floatpanel,colorbutton,colordialog,templates,menu,contextmenu,div,resize,toolbar,elementspath,enterkey,entities,popup,filebrowser,find,fakeobjects,flash,floatingspace,listblock,richcombo,font,forms,format,horizontalrule,htmlwriter,iframe,wysiwygarea,image,indent,indentblock,indentlist,smiley,justify,link,list,liststyle,magicline,maximize,newpage,pagebreak,pastetext,pastefromword,preview,print,removeformat,save,selectall,showblocks,showborders,sourcearea,specialchar,menubutton,scayt,stylescombo,tab,table,tabletools,undo,wsc,youtube,tableresize,stylesheetparser,sourcedialog,placeholder,mediaembed,iframedialog,divarea,confighelper,codemirror,backgrounds,autosave,autogrow,htmlbuttons,symbol,onchange,toolbarswitch,xml,oembed,insertpre';
    //config.skin = 'moonocolor';
    // %REMOVE_END%config.language = 'en';
    CKEDITOR.basePath = "~/res/BaseClasses/Scripts.aspx?f=ckEditor/"
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;
    //config.skin = 'moonocolor';
    // Define changes to default configuration here. For example:
    config.language = 'en';
    CKEDITOR.lang.en.dir="ltr";
    // config.uiColor = '#AADC6E';
    config.extraPlugins = 'sharedspace,simpleuploads,history,uploader';
    config.filebrowserUploadUrl = "~/res/DTIContentManagement/Uploader.aspx";
    //config.simpleuploads_respectDialogUploads = true; 
};
