<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DTIMapEditUserControl.ascx.vb" Inherits="DTIMapControl.DTIMapEditUserControl" %>
<table align="right">
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
    <tr>
        <td align="right" colspan="2">
            <asp:Button runat="Server" ID="btnSave" Text="Save" />
           
        </td>
    </tr>
</table>  


