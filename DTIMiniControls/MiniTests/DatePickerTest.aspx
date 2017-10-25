<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DatePickerTest.aspx.vb" Inherits="MiniTests.DatePickerTest" %>
<%@ Register Assembly="DTIMinicontrols" Namespace="DTIMinicontrols" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <cc1:DatePicker ID="datepicker1" runat="server" ChangeMonth="true" changeyear="true" buttonimageonly="true"></cc1:DatePicker>    
    </div>
    </form>
</body>
</html>
