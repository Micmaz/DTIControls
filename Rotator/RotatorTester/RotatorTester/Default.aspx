<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="RotatorTester._Default" %>

<%@ Register Assembly="Rotator" Namespace="Rotator" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body style="background-color:Blue;">
    <form id="form1" runat="server">
    <div>
        <asp:CheckBox ID="cbEdit" runat="server" AutoPostBack="true" />

        <cc1:Rotator contentType="TEST" ID="Rotator1" runat="server">
        </cc1:Rotator>
<br />
        <cc1:Rotator contentType="TEST2" ID="Rotator2" runat="server">
        </cc1:Rotator>

    </div>
    </form>
</body>
</html>
