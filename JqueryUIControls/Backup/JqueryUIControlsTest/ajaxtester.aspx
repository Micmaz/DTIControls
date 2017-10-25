<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ajaxtester.aspx.vb" Inherits="JqueryUIControlsTest.ajaxtester" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:Panel ID="pnl1" runat="server">
        stuff here
        </asp:Panel>
        <cc1:AjaxCall renderControlsBack="true" ID="ajax1" javascriptCallTimer="1000" runat="server"/>
    </div>
    </form>
</body>
</html>
