<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="JqueryUIControlsTest.login" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="tbUserID" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="login" />
         <cc1:Autocomplete ID="tbSearch" runat="server" Width="250px"></cc1:Autocomplete>
             <cc1:InfoDiv ID="InfoDiv1" runat="server" Visible="true">
        test
    </cc1:InfoDiv> 
    </div>

    </form>
</body>
</html>
