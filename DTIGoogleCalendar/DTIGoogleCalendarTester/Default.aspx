<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="DTIGoogleCalendarTester._Default" %>
<%@ Register Assembly="DTIGoogleCalendar" Namespace="DTIGoogleCalendar" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style>
        .full-width-field {
            width:240px;
        }
        .field-value {
            padding-bottom:1em;
            white-space:nowrap;
        }
        .field-name, .field-value, .field-description {
            font-size:83%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <iframe src="http://www.google.com/calendar/embed" ></iframe>
    <div>
        <asp:CheckBox runat="server" ID="cbEdit" AutoPostBack="true" Text="Edit" /><br />
        <cc1:DTIGoogleCal runat="server" ID="DTIGoogleCal1"></cc1:DTIGoogleCal>
    </div>
    </form>
</body>
</html>
