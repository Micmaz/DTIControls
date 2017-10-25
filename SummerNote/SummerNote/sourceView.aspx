<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="sourceView.aspx.vb" Inherits="SummerNote.sourceView" %>
<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
    html, body, textarea {height:100%;margin:0px;padding:0px;}
    </style>
    <script language="javascript" type="text/javascript">
<!--
        function onload() {
            //document.getElementById('repcontent').style.height = document.height-30 + 'px';
            editor_repcontent.setValue(parent.window.editor.getData());
            //document.getElementById('repcontent').value = parent.window.editor.getData();
        }
        function Button1_onclick() {
            if (parent.window.editor) {
                parent.window.editor.setData(document.getElementById("repcontent").value, new function () {
                    parent.window.SummerNote.dialog.getCurrent().hide();
                });

            }
        }
        function onDialogEvent(ev) {
            if (ev.name == "ok") {
                if (parent.window.editor) {
                    parent.window.editor.setData(editor_repcontent.getValue());
                    //parent.window.editor.setData(document.getElementById("repcontent").value, new function () {
                    //    parent.window.SummerNote.dialog.getCurrent().hide();
                    //});
                }
            }
        }
// -->
</script>
</head>
<body onload="onload();">
    <form id="form1" runat="server">
    <DTI:HighlighedEditor language="html" Width="100%" ID="repcontent" runat="server"> </DTI:HighlighedEditor>
        </form>
</body>
</html>
