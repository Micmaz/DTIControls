<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="ckeditTester._Default" %>
<%@ Register Assembly="SummerNote" Namespace="SummerNote" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>Test</title>
</head>
<body style="background-color:Silver">
    <form id="form1" runat="server">
<div style="width:600px;background-color:Yellow;">
    <br /><br />
    before
        <cc1:SummerNote ID="SummerNote1" runat="server" ToolbarMode="PageTop">A TEST HERE!
        </cc1:SummerNote>
    after
    <br />
</div>
        <cc1:SummerNote ID="SummerNote2" runat="server" Height="300px" Width="500px" DivCssClass="divClass" ToolbarMode="PageTop">
        </cc1:SummerNote>

<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>

    </form>
</body>
</html>
