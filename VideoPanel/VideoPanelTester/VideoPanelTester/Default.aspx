<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="VideoPanelTester._Default" %>

<%@ Register Assembly="VideoPanel" Namespace="VideoPanel" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:CheckBox ID="cbEdit" runat="server" AutoPostBack="true" />

        <cc1:VideoPanel contentType="TEST" ID="VideoPanel1" runat="server">
        </cc1:VideoPanel>

    </div>
    </form>
</body>
</html>
