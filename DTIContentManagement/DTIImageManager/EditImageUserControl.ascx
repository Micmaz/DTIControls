<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditImageUserControl.ascx.vb" Inherits="DTIImageManager.EditImageUserControl" %>
<%@ Register Assembly="DTIControls" Namespace="HighslideControls" TagPrefix="cc1" %>
<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="cc2" %>
<asp:panel id="pnlEdit" runat="server">
    <cc1:Highslider ID="imgPreviewEdit" runat="server" Visible="false"></cc1:Highslider><br />
    <asp:Label ID="lblSize" runat="server"></asp:Label>
</asp:panel>