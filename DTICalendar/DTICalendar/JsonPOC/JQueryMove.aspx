<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="JQueryMove.aspx.vb" Inherits="DTICalendar.JQueryMove" %>
<%@ Register Assembly="DTICalendar" Namespace="DTICalendar" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript" src="JQueryPageJS.aspx"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <cc1:JsonTest ID="JsonTest3" runat="server" CssClass="Yesterday">
    Yesterday
    </cc1:JsonTest> 
    <cc1:JsonTest ID="JsonTest2" runat="server" CssClass="Today">
    Today
    </cc1:JsonTest>
    <cc1:JsonTest ID="JsonTest1" runat="server" CssClass="Tomorrow">
    Tomorrow
    </cc1:JsonTest>
    </div>
    </form>
</body>
</html>
