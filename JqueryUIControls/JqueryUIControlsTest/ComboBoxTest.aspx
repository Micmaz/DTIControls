<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ComboBoxTest.aspx.vb" Inherits="JqueryUIControlsTest.ComboBoxTest" %>

<%@ Register assembly="JqueryUIControls" namespace="JqueryUIControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body class="ui-widget"> 
    <form id="form1" runat="server">
    <div>
    <cc1:ThemePicker useCookie="true" Visible="true" ID="ThemePicker1" runat="server"/>
        <cc1:ComboBox ID="ComboBox1" runat="server">
            <asp:ListItem>test</asp:ListItem>
            <asp:ListItem>Test2</asp:ListItem>
            <asp:ListItem>test3</asp:ListItem>
            <asp:ListItem>asdf</asp:ListItem>
            <asp:ListItem>1234</asp:ListItem>
        </cc1:ComboBox>
        <br />
<asp:Button ID="btnConfrim" runat="server" Text="Confirm" OnClientClick="return jqconfirm(this,'test', 'test title');" />
    
    </div>
    </form>
</body>
</html>
