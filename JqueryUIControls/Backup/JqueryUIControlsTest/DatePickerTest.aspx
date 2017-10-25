<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DatePickerTest.aspx.vb" Inherits="JqueryUIControlsTest.DatePickerTest" %>

<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <cc1:DatePicker value="" ID="DatePicker1" runat="server"></cc1:DatePicker>
        <br /><asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </div>
    <asp:Button ID="Button1" runat="server" Text="Button" />
    </form>
</body>
</html>
