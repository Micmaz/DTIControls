<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AutoCompleteTest.aspx.vb" Inherits="JqueryUIControlsTest.AutoCompleteTest" %>

<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link type="text/css" href="jquery.ui.selectmenu.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:ThemePicker ID="ThemePicker1" runat="server">
        </cc1:ThemePicker>
        Dynamic: <cc1:Autocomplete ID="acDynamic" runat="server"></cc1:Autocomplete><br />
        <asp:DropDownList ID="DropDownList2" runat="server">
                    <asp:ListItem>asdasd</asp:ListItem>
            <asp:ListItem>aaaaa</asp:ListItem>
            <asp:ListItem>bbbbb</asp:ListItem>
        </asp:DropDownList><asp:ListBox ID="ListBox1" runat="server">
            <asp:ListItem>asdasd</asp:ListItem>
            <asp:ListItem>aaaaa</asp:ListItem>
            <asp:ListItem>bbbbb</asp:ListItem>
        </asp:ListBox>
        <cc1:AjaxCall runat="server" ID="jsfunc1"></cc1:AjaxCall>
    <cc1:Autocomplete ID="AutoComplete1"  runat="server" AutoPostBack="False"></cc1:Autocomplete><asp:Button ID="Button1" runat="server" Text="Button" />
    </div>
    <br /><br /><asp:TextBox ID="TextBox1" runat="server">asdasd</asp:TextBox>
        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem>select 1</asp:ListItem>
            <asp:ListItem Value="1">aa</asp:ListItem>
            <asp:ListItem Value="2">bb</asp:ListItem>
        </asp:DropDownList><%--    <a href="#" onclick="addvalueToAutoComplete('TextBox1','AutoComplete1','TextBox1');">asdasd</a>
--%>    
 <input id="Button2" type="button" onclick="jsfunc1('send stuff');" value="button" />

</form>
</body>
</html>
