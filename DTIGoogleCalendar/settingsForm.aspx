<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="settingsForm.aspx.vb" Inherits="DTIGoogleCalendar.settingsForm" %>

<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:318px;height:483px;">
        Example: <br />
            <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
            <div id="slideStart"><asp:Panel ID="pnl1" runat="server">
            </asp:Panel>
            </div>
        <br />
    <table border=0 >
    <tr><td>Allow Transparency: </td><td><asp:CheckBox ID="cbAllowTransparency" runat="server" /></td></tr>
    <tr><td>Frame Border: </td><td>
        <asp:TextBox ID="tbFrameBorder" runat="server"></asp:TextBox></td></tr>
    <tr><td style="height: 22px">Scrolling: </td><td style="height: 22px"><asp:CheckBox ID="cbScrolling" runat="server" /></td></tr>        
    <tr><td>Background Color:
        </td><td><asp:TextBox ID="tbBGColor" runat="server"></asp:TextBox></td></tr>    
    <tr><td>Border Width: </td><td>
        <asp:TextBox ID="tbBorderWidth" runat="server"></asp:TextBox></td></tr>      
    <tr><td style="height: 22px">
        Calendar URL:</td><td style="height: 22px">
        <DTI:TextBoxEncoded id="tbSrc" runat="server">
        </DTI:TextBoxEncoded></td></tr>    
    </table>
        &nbsp;<asp:Label ID="lblSrcError" runat="server" ForeColor="Red" Text="*Please enter a valid Google Calendar URL or Tag  "
            Visible="False"></asp:Label><br />
        <br />
        &nbsp;<table border=0>
    <tr><td>Width: </td><td><asp:TextBox ID="tbWidth" runat="server"></asp:TextBox></td></tr>
    <tr><td>Height: </td><td><asp:TextBox ID="tbHeight" runat="server"></asp:TextBox></td></tr>
    </table>
        </div>
    </form>
</body>
</html>
