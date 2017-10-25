<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="settingsForm.aspx.vb" Inherits="DTIMapControl.settingsForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <table>
    <tr>
        <td>
            <asp:Label ID="lblTitle" runat="server" Text="Location Title"></asp:Label>
        </td>
        <td>
            <asp:TextBox runat="server" ID="tbTitle" Width="275"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblAddress" runat="server" Text="Location Address"></asp:Label>
        </td>
        <td>
            <asp:TextBox runat="Server" ID="tbAddress" Width="275"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblGoogleKey" runat="server" Text="Google Map API Key"></asp:Label>
        </td>
        <td>
            <asp:TextBox runat="Server" ID="tbGoogleKey" Width="275"></asp:TextBox>
        </td>
    </tr>
</table>  
    </form>
</body>
</html>
