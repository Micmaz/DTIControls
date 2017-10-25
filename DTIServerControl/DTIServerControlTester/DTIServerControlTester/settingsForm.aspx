<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="settingsForm.aspx.vb" Inherits="DTIServerControlTester.settingsForm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Enter some new text to have it show up.
        </div>
        Text:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
        Width:<asp:TextBox ID="tbWid" runat="server"></asp:TextBox><br />
        Height:<asp:TextBox ID="tbHeight" runat="server"></asp:TextBox><br />                
    </form>
</body>
</html>
