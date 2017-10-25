<%@ Control Language="vb" AutoEventWireup="false" EnableViewState="false" CodeBehind="MediaInfoUserControl.ascx.vb" Inherits="DTIMediaManager.MediaInfoUserControl" %>
<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>
<%@ Register Assembly="DTIControls" Namespace="DTIMediaManager" TagPrefix="Media" %>
<table style="width:100%">
    <tr>
        <td style="text-align:left; font-weight:bold;" align="left">
            <asp:HyperLink ID="hlTitle" runat="server"></asp:HyperLink>
            <asp:Label ID="lblTitle" runat="server" visible="false"></asp:Label>
            <asp:Label ID="lblPubInfo" runat="server" Text=""></asp:Label>
        </td>
        <%--<td style="text-align:right" align="right">--%>
        <td>
            <asp:PlaceHolder runat="server" ID="phSocialUtilities">
                <Media:MediaRater runat="server" ID="rater"></Media:MediaRater> 
                <DTI:AddThisControl runat="Server" ID="bookmarker"></DTI:AddThisControl>
            </asp:PlaceHolder>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel runat="server" ID="pnlContent">
                <asp:Literal runat="server" ID="litDesc"></asp:Literal>
            </asp:Panel>
            <asp:PlaceHolder runat="server" ID="phComments"></asp:PlaceHolder>
        </td>
    </tr>
</table>