$(function () {
	getHighestZ();
	$(".dtiContentEdit, .summernote").click(function (e) { e.preventDefault(); activate(this); });
	$(".dtiContentEdit, .summernote").on('dragenter', function (e) { activate(this); }); //Allows dragging images without having to click the edit area first.
	$(document).keydown(function (e) {
		if (e.which == 27) {
			endEdit();
		}
	});

	//Checks if there is a 301 redirect and sets the upload page accordingly. Image post data was not redirected. most other requests should be transparent.
	var xhr = new XMLHttpRequest();
	xhr.open('GET', uploadUrl, true);
	xhr.onreadystatechange = function () {
		if (this.readyState == 4 && this.status == 200) {
			uploadUrl = xhr.responseURL;
		}
	};
	xhr.send();

	//  $(document).on('click', ":not('.summernote')", function (e) {
	//    dest(curreditor);
	//    e.preventDefault();
	//  });
});
var uploadUrl = "~/res/DTIContentManagement/Uploader.aspx";
var curreditor;
function addAtEnd(text) {
	if (curreditor) {	
		$(".note-editable").append(text);
	}
}

function addContent(htmlContent) {
	if (curreditor) {
		var div = document.createElement('div');
		div.innerHTML = htmlContent;
		$(curreditor).summernote('insertNode', div.firstChild );
	}
}

function clearContent(text) {
	if (curreditor) {
		$(".note-editable").html("");
	}
}

function dest(item) {
	if (item) $(item).summernote('destroy');
}

function endEdit() {
	dest(curreditor);
}

function getOptions(item) {
	var options = $(item).data("summernoteOptions");
	if (!options)
		options = getDefaultOptions(item);
	return options;
}

function setOptions(item, options) {
	$(item).data("summernoteOptions", $.extend(getDefaultOptions(item), options));
}

function addTools(item, additionalTools) {
	var options = getOptions(item);
	options.toolbar = $.merge(options.toolbar, additionalTools);
	setOptions(item, options);
}

function addTool(item, toolName, toolgrp) {
	if (!toolgrp) toolgrp = toolName + '_grp';
	var options = getOptions(item);
	var grpindex = -1;

	for (var i = 0; i < options.toolbar.length; i++) {
		if (options.toolbar[i][0] == toolgrp) { grpindex = i; }
	}
	if (grpindex == -1) {
		grpindex = options.toolbar.length;
		options.toolbar.push([toolgrp, []]);
	}
	options.toolbar[grpindex][1].push(toolName);
	setOptions(item, options);
}



function addButton(item, buttonHTML, title, clickFunction, buttonGroup) {
	addButtonsToEditor(item, { [title]: $.summernote.plugins.dialogHelper().makeButton(buttonHTML, title, clickFunction) });
	if (!buttonGroup) { buttonGroup = title + '_grp'; }
	addTool($(item), title, buttonGroup);
}

function addButtonsToEditor(item, additionalButtons) {
	var options = getOptions(item);
	options.buttons = $.extend(options.buttons, additionalButtons);
	setOptions(item, options);
}

function addIframeDialogButton(item, buttonHTML, title, url, buttonGroup) {
	addButtonsToEditor(item, { [title]: $.summernote.plugins.dialogHelper().makeIframeButton(buttonHTML, title, url) });
	if (!buttonGroup) { buttonGroup = title + '_grp'; }
	addTool($(item), title, buttonGroup);
}

function setHtmlEditorValues() {
	endEdit();
	$(".dtiContentEdit").each(function () {
		var $parent = $(this).parent();
		$parent.find("#" + $parent.attr("id") + "_Hidden").val($.base64.encode($(this).html()));
	})
	return true;
}

function activate(item) {
	dest(curreditor);
	curreditor = item;
	$(item).summernote(getOptions(item));
};

var index_highest = 0;
function getHighestZ() {
	if (index_highest == 0) { 
		$("*").each(function () {
			// always use a radix when using parseInt
			var index_current = parseInt($(this).css("zIndex"), 10);
			if (index_current > index_highest) {
				index_highest = index_current;
			}
		});
	}
return index_highest;
}

function sendFile(files, editor) {
	for (var i = 0; i < files.length; i++) {
		var file = files[i];
		data = new FormData();

		if (file.size > 990000) { //Chunk files larger than ~900K
			var chunkSize = 1024 * 1024;
			var fileName = file.name;
			var fileSize = file.size;
			var chunkCount = Math.ceil(file.size / chunkSize, chunkSize);
			var chunk = 0;
			while (chunk <= chunkCount) {
				data = new FormData();
				var offset = chunk * chunkSize;
				data.append("fileName", fileName);
				data.append("filesize", fileSize);
				data.append('chunk', chunk);
				data.append('chunkCount', chunkCount);
				data.append('offset', offset);
				var blob = file.slice(offset, offset + chunkSize)
				data.append("blob", blob, fileName);

				$.ajax({
					data: data,
					type: "POST",
					url: uploadUrl,
					cache: false,
					contentType: false,
					processData: false,
					async: false,
					success: function (url) {
						if (chunk == chunkCount)
							editor.summernote('insertImage', url);
					}
				});
				chunk++;
			}
		} else {
			data.append("file", file);
			$.ajax({
				data: data,
				type: "POST",
				url: uploadUrl,
				cache: false,
				contentType: false,
				processData: false,
				success: function (url) {
					editor.summernote('insertImage', url);
				}
			});
		}
	}
}

function getDefaultOptions(item) {
	return {
		tabsize: 2,
		airMode: true,
		prettifyHtml: false,
		dialogsInBody: false,
		toolbar: [
			['style', ['style', 'bold', 'italic', 'underline', 'clear']],
			['font', ['strikethrough', 'superscript', 'subscript']],
			['fontname', ['fontname']],
			['fontsize', ['fontsize']],
			['color', ['color']],
			['para', ['ul', 'ol', 'paragraph', 'height']],
			['insert', ['hr', 'table', 'link', 'videoAttributes', 'picture']],
			['misc', ['undo', 'redo', 'fullscreen', 'codeview']]
			//['save', ['save']]
		],
		buttons: {
			//save: $.summernote.plugins.dialogHelper().makeIframeButton('<i class="note-icon-save"/>', 'Save', 'Iframe.html')
		},
		popover: {
			image: [
				['custom', ['imageAttributes']],
				['imagesize', ['imageSize100', 'imageSize50', 'imageSize25']],
				['float', ['floatLeft', 'floatRight', 'floatNone']],
				['remove', ['removeMedia']]
			],
			link: [
				['link', ['linkDialogShow', 'unlink']]
			],
			air: [
				['color', ['color']],
				['font', ['bold', 'underline', 'clear']],
				['para', ['ul', 'paragraph']],
				['table', ['table']],
				['insert', ['link', 'picture']]
			]
		}, callbacks: {
			onInit: function () {
				//<div class="bootstrap-iso">


				var bootstrapiso = $('<div class="bootstrap-iso">')
				var $toolbar = $('.note-toolbar');
				bootstrapiso.insertBefore($(item));
				$toolbar.css("z-index", getHighestZ() + 1);
				$toolbar.css("width", "100%");
				$('.note-popover, .note-editor .modal, .note-editor .note-toolbar').appendTo(bootstrapiso);
				$toolbar.wrap('<div class="panel-default"></div>');
				var div = $("<div id='toggleToolbar' style='width:40px;height: 7px;position: absolute;left: 50%;'><i class='fa fa-chevron-up rotate' style='position:absolute;top:0;font-size:7px;' ></i></div>")
				div.appendTo($toolbar);
				var i = 0;
				div.click(function () {
					$(this).find("i").toggleClass("down");
					if (i == 0) {
						div.parent().stop().animate({top:'-'+(div.parent().height()-7)+'px'},500);
						i = 1;
					} else {
						div.parent().stop().animate({ top: '0px' }, 500);
						i = 0;
					}
				});
			},
			onDestroy: function () {
				$('.bootstrap-iso').remove();
			},
			onImageUpload: function (files) {
				sendFile(files, $(this));
			}
		},
		imageAttributes: {
			imageDialogLayout: 'default', // default|horizontal
			icon: '<i class="note-icon-pencil"/>',
			removeEmpty: false // true = remove attributes | false = leave empty if present
		},
		displayFields: {
			imageBasic: true,  // show/hide Title, Source, Alt fields
			imageExtra: true, // show/hide Alt, Class, Style, Role fields
			linkBasic: true,   // show/hide URL and Target fields for link
			linkExtra: false   // show/hide Class, Rel, Role fields for link
		}
	};
}

"use strict"; jQuery.base64 = (function ($) { var _PADCHAR = "=", _ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/", _VERSION = "1.0"; function _getbyte64(s, i) { var idx = _ALPHA.indexOf(s.charAt(i)); if (idx === -1) { throw "Cannot decode base64" } return idx } function _decode(s) { var pads = 0, i, b10, imax = s.length, x = []; s = String(s); if (imax === 0) { return s } if (imax % 4 !== 0) { throw "Cannot decode base64" } if (s.charAt(imax - 1) === _PADCHAR) { pads = 1; if (s.charAt(imax - 2) === _PADCHAR) { pads = 2 } imax -= 4 } for (i = 0; i < imax; i += 4) { b10 = (_getbyte64(s, i) << 18) | (_getbyte64(s, i + 1) << 12) | (_getbyte64(s, i + 2) << 6) | _getbyte64(s, i + 3); x.push(String.fromCharCode(b10 >> 16, (b10 >> 8) & 255, b10 & 255)) } switch (pads) { case 1: b10 = (_getbyte64(s, i) << 18) | (_getbyte64(s, i + 1) << 12) | (_getbyte64(s, i + 2) << 6); x.push(String.fromCharCode(b10 >> 16, (b10 >> 8) & 255)); break; case 2: b10 = (_getbyte64(s, i) << 18) | (_getbyte64(s, i + 1) << 12); x.push(String.fromCharCode(b10 >> 16)); break }return x.join("") } function _getbyte(s, i) { var x = s.charCodeAt(i); if (x > 255) { throw "INVALID_CHARACTER_ERR: DOM Exception 5" } return x } function _encode(s) { if (arguments.length !== 1) { throw "SyntaxError: exactly one argument required" } s = String(s); var i, b10, x = [], imax = s.length - s.length % 3; if (s.length === 0) { return s } for (i = 0; i < imax; i += 3) { b10 = (_getbyte(s, i) << 16) | (_getbyte(s, i + 1) << 8) | _getbyte(s, i + 2); x.push(_ALPHA.charAt(b10 >> 18)); x.push(_ALPHA.charAt((b10 >> 12) & 63)); x.push(_ALPHA.charAt((b10 >> 6) & 63)); x.push(_ALPHA.charAt(b10 & 63)) } switch (s.length - imax) { case 1: b10 = _getbyte(s, i) << 16; x.push(_ALPHA.charAt(b10 >> 18) + _ALPHA.charAt((b10 >> 12) & 63) + _PADCHAR + _PADCHAR); break; case 2: b10 = (_getbyte(s, i) << 16) | (_getbyte(s, i + 1) << 8); x.push(_ALPHA.charAt(b10 >> 18) + _ALPHA.charAt((b10 >> 12) & 63) + _ALPHA.charAt((b10 >> 6) & 63) + _PADCHAR); break }return x.join("") } return { decode: _decode, encode: _encode, VERSION: _VERSION } }(jQuery));