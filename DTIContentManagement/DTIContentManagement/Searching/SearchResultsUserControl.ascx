<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchResultsUserControl.ascx.vb" Inherits="DTIContentManagement.SearchResultsUserControl" %>
<%@ Register Assembly="DTIControls" Namespace="DTIContentManagement" TagPrefix="cc1" %>
<%@ Register Assembly="DTIControls" Namespace="DTIGallery" TagPrefix="cc2" %>
<asp:Panel runat="server" ID="buttonHolder" Visible="false">
<asp:LinkButton ID="btnPages" runat="server">Pages</asp:LinkButton>
<asp:LinkButton ID="btnMedia" runat="server" style="padding-left:20px;">Media</asp:LinkButton><br /><br /></asp:Panel>
<cc1:SearchResultGallery runat="server" ID="gallPages"></cc1:SearchResultGallery>
<cc2:DTISlideGallery runat="server" id="gallMedia" Visible="false"></cc2:DTISlideGallery>