<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="DTIServerControlTester._Default" %>
<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>
<%@ Register Assembly="DTIServerControlTester" Namespace="DTIServerControlTester"
    TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:CheckBox ID="cbEdit" runat="server" AutoPostBack="true" />
        <cc1:serverControl MainID="2" contentType="TEST" ID="ServerControl1" runat="server" Height="200px" Width="226px">
        </cc1:serverControl>
        <cc1:serverControl MainID="1" contentType="TEST2" ID="ServerControl2" runat="server" Height="108px" Width="141px">
        </cc1:serverControl>
        <cc1:serverControl ID="ServerControl3" runat="server" Height="247px" 
            Width="417px" >TEST3
        <asp:Label runat="server">Dynamic text entered below:<br></asp:Label>
<asp:Label runat="server"></asp:Label>

        <asp:Label runat="server">Dynamic text entered below:<br></asp:Label>
<asp:Label runat="server"></asp:Label>
        </cc1:serverControl>        
        <asp:Button ID="Button1" runat="server" Text="Button1" />
        <asp:Button ID="Button2" runat="server" Text="Button2" />
        <asp:Button ID="Button3" runat="server" Text="ButtonAll" />
        <cc1:serverControl MainID="2"  ID="ServerControl4" runat="server" Height="251px" Width="362px">
        </cc1:serverControl>
    </div>
    </form>
</body>
</html>
