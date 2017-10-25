<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="History.aspx.vb" Inherits="DTIContentManagement.History" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Update History</title>
<script language="javascript" type="text/javascript">
<!--

	function Button1_onclick() {
		if (parent.window.curreditor) {
			parent.clearContent();
			parent.addContent(document.getElementById("repcontent").innerHTML);
		}      
    }

// -->
</script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlRedirectPg" Visible="false" runat="server">
<%--    <script language="javascript">
            var editContainer = parent.$('#' + parent.CKEDITOR.currentInstance.element.getAttribute('id')).parent();
            var c = editContainer.attr('contentType');
            var m = editContainer.attr('mainId');
            window.location.href = 'history.aspx?c=' + c + '&m=' + m;
    </script>--%>
    </asp:Panel>
    <div>
        Date Changed: &nbsp;<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True"
            Width="165px">
        </asp:DropDownList><br />
        <br />
        <input id="Button1" type="button" value="Paste to Editor" language="javascript" onclick="return Button1_onclick()" /><br />
        <br />
        <div id="repcontent">
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div></div>
    </form>
</body>
</html>