<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="base64Tests.aspx.vb" Inherits="MiniTests.base64Tests" %>

<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="divPanel">howdy&lt;&#8230;&gt;</div>
        <DTI:HiddenFieldEncoded ID="HiddenFieldEncoded1" runat="server" ReferenceId="divPanel" />
        <DTI:TextBoxEncoded ID="TextBoxEncoded1" runat="server"> </DTI:TextBoxEncoded>
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <br />
        result 1:&nbsp;
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        result 2:&nbsp;
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label></div>
    </form>
</body>
</html>
