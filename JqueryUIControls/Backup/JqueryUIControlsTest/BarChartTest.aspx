<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BarChartTest.aspx.vb" Inherits="JqueryUIControlsTest.BarChartTest" %>

<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:PollChart ID="PollChart1" runat="server" style="width:300px">
        </cc1:PollChart>
    </div>
    </form>
</body>
</html>
