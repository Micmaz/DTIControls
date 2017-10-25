<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TimerPickerTest.aspx.vb" Inherits="JqueryUIControlsTest.TimerPickerTest" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:TimePicker id="TimePicker1" runat="server" HourGrid="5" MinuteGrid="10" StepMinute="5"></cc1:TimePicker>
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <cc1:TimePicker id="TimePicker2" runat="server" ShowTimeAndDate="true" ButtonImageOnly="true" MaxDate="0"></cc1:TimePicker>
        <br />
        <cc1:TimePicker ID="tpTimeIn" runat="server" Clock24Hour="true"></cc1:TimePicker>
        </div>
    </form>
</body>
</html>
