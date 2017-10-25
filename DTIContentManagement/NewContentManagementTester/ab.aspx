<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ab.aspx.vb" Inherits="NewContentManagementTester.ab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
	<script>

	    function showbuttons(editArea) {
	        if (editArea.find(".btnarea").length == 0) {
	            var btnarea = $("<div class='btnarea cke_focus'></div>")
	            btnarea.css({
	                "background-color": "#F2F2F2",
	                "position": "absolute",
	                "-moz-border-radius": "0px 0px 5px 5px",
	                "width": "200px",
	                "height": "28px",
	                "z-index": "5"
	            });
	            editArea.append(btnarea);
	            btnarea.hide();
	            editArea.find(".dtiCKEButton").each(function () {
	                $(this).show();
	                if ($(this).button) $(this).button();
	                btnarea.height($(this).height()+10);
	                $(this).appendTo(btnarea);
	                $(this).css("position", "absolute");
	                
	            });
	            btnarea.fadeIn();
	            
	        } else {
	        editArea.find(".btnarea").fadeIn();
	        }
            
	    }

	    function hidebuttons(editor) {
	        //alert("boom");
	        var element = editor.element;
	        $('#' + editor.element.getAttribute('id')).parent().find(".btnarea").fadeOut();
	    }


    </script>
    
    <script title="History Tool" language="javascript" type="text/javascript">
        CKEDITOR.plugins.add('historyeditor', {
            icons: 'historyeditor',
            init: function (a) {
                CKEDITOR.dialog.addIframe('historyeditor_dialog', 'History Tool', '~/res/DTIContentManagement/History.aspx?c=newEPtest1&m=0', 620, 400, function () {  });
                var cmd = a.addCommand('historyeditor', { exec: historyeditor_onclick });
                cmd.modes = { wysiwyg: 1, source: 1 };
                cmd.canUndo = false;
                a.ui.addButton('historyeditor', { label: 'History Tool', command: 'historyeditor', icon: '~/DTIContentManagement/reload.gif' });
            }
        })
        function historyeditor_onclick(e) {
            var editor = CKEDITOR.currentInstance;
            editor.openDialog('historyeditor_dialog');
        }
        CKEDITOR.config.extraPlugins += 'historyeditor,';
		</script>

<%--    <div contenteditable="true">
    Test area
    </div>--%>
    <br /><br /><br /><br /><br /><br /><br /><br />
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
