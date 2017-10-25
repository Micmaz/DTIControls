<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="settingsForm.aspx.vb" Inherits="VideoPanel.settingsForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript">
//function setParentContent(parentID,clientID){
//alert(parentID);
//     parent.$("#" + parentID).width($("#tbWidth").val()+"px");
//     parent.$("#" + parentID).height($("#tbHeight").val()+"px");
//     parent.$("#" + parentID).css("width",$("#tbWidth").val()+"px");
//     parent.$("#" + parentID).css("height",$("#tbHeight").val()+"px");
//     
//     parent.$("#" + parentID + " div embed").attr("width",$("#tbWidth").val());
//     parent.$("#" + parentID + " div embed").attr("height",$("#tbHeight").val());
//    parent.$("#" + parentID).replaceWith($("#"+ clientID).val()); 
//     $("#"+ clientID).val("");
//    eval("parent.makevid_" + parentID + "()");
//    eval("parent.unfreeze_" + parentID + "()"); 
//}
    </script>
</head>
<body> 
    <form id="form1" runat="server">
    <div style="width:300px;height:400px;">
        <asp:LinkButton ID="lbUpload" runat="server" Visible="False">Upload A New File</asp:LinkButton><br />
        <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder><br />
<br /><br />
    <table border=0>
<%--    <tr><td>Show video in a popup: </td><td><asp:CheckBox ID="cbPopup" runat="server" /></td></tr>
    <tr><td>Autoplay Video: </td><td><asp:CheckBox ID="cbautoplay" runat="server" /></td></tr>--%>
    <tr><td>Width: </td><td><asp:TextBox ID="tbWidth" runat="server"></asp:TextBox></td></tr>
    <tr><td>Height: </td><td><asp:TextBox ID="tbHeight" runat="server"></asp:TextBox></td></tr>
    </table>
    </div>
    </form>
</body>
</html>
