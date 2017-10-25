<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HighlightEditor.aspx.vb" Inherits="MiniTests.HighlightEditor" %>
<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <DTI:HighlighedEditor Text="<h5>awdawdwd</h5>
<br />
<br />
<strong><em>awddaw</em></strong>"  language="html" Height="300px" Width="400px" ID="HighlighedEditor1" runat="server"> </DTI:HighlighedEditor>
        <asp:Button ID="Button1" runat="server" Text="Button" />
    </div>
    </form>
</body>
</html>
