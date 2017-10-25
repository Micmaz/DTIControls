<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditMediaUserControl.ascx.vb" Inherits="DTIMediaManager.EditMediaUserControl" %>
<%@ Register Assembly="DTIControls" Namespace="DTITagManager" TagPrefix="cc2" %>
<table>
    <tr>
        <td style="width: 200px">
            <asp:Label runat="server" ID="lblDateAdded" Text="Date Added: "></asp:Label>
            <asp:Label runat="server" ID="lblDateAddedValue"></asp:Label>
        </td>
        <td id="tdTitle" runat="server">
            <asp:Label ID="lblTitle" runat="server" Text="Title (Optional): "></asp:Label><br />
            <asp:TextBox ID="tbTitle" runat="server" Width="400px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td rowspan="2" style="width: 200px">
            <asp:PlaceHolder runat="server" ID="phContentEditor"></asp:PlaceHolder>
        </td>
        <td>
            <asp:Label ID="lblDescription" runat="server" Text="Description (Optional): "></asp:Label><br />
            <asp:TextBox ID="tbDesc" runat="server" Height="100px" TextMode="MultiLine" Width="400px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Tags (Optional):"></asp:Label><br />
            <cc2:TagManager runat="server" id="tagger"></cc2:TagManager>
        </td>
    </tr>    
</table>