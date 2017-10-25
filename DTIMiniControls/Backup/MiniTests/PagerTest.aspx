<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PagerTest.aspx.vb" Inherits="MiniTests.PagerTest" %>

<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <DTI:Pager ID="pager1" runat="server" RaiseEventOnPageChange="true" Width="100%" />
    </div>
        <asp:Button ID="Button1" runat="server" Text="Button" /><asp:LinkButton ID="LinkButton1"
            runat="server">LinkButton</asp:LinkButton>
    </form>
</body>
</html>
