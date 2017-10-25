<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PasswordStrengthTest.aspx.vb" Inherits="DTIAdminPanelTestProject.PasswordStrengthTest" %>
<%@ Register Assembly="DTIAdminPanel" Namespace="DTIAdminPanel" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="password"></asp:TextBox>
        <cc1:PasswordStrength ID="PasswordStrength1" runat="server" CssClass="testClass"></cc1:PasswordStrength>
        <cc1:LoginControl ID="LoginUser1" runat="server" ></cc1:LoginControl>
    </div>
    </form>
</body>
</html>
