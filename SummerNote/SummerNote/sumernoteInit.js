$(function () {
	$(".dtiContentEdit, .summernote").click(function (e) { e.preventDefault(); activate(this); });
	$(document).keydown(function (e) {
		if (e.which == 27) {
			endEdit();
		}
	});
	//  $(document).on('click', ":not('.summernote')", function (e) {
	//    dest(curreditor);
	//    e.preventDefault();
	//  });
});

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
		$parent.find("#" + $parent.attr("id") + "_Hidden").val(encodeURIComponent($(this).html()));
	})
	return true;
}

function activate(item) {
	dest(curreditor);
	curreditor = item;
	$(item).summernote(getOptions(item));
};

function sendFile(files, editor) {
	for (var i = 0; i < files.length; i++) {
		var file = files[i];
		data = new FormData();
		data.append("file", file);
		$.ajax({
			data: data,
			type: "POST",
			url: "~/res/DTIContentManagement/Uploader.aspx",
			cache: false,
			contentType: false,
			processData: false,
			success: function (url) {
				editor.summernote('insertImage', url);
			}
		});
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
				bootstrapiso.insertBefore($(item));

				$('.note-popover, .note-editor .modal, .note-editor .note-toolbar').appendTo(bootstrapiso);
				$('.note-toolbar').wrap('<div class="panel-default"></div>');
				var div = $("<div style='width:40px;height: 7px;position: absolute;left: 50%;'><i class='fa fa-chevron-up rotate' style='position:absolute;top:0;font-size:7px;' ></i></div>")
				div.appendTo($('.note-toolbar'));
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