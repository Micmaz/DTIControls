<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ImageInsertOptions.ascx.vb" Inherits="DTIContentManagement.ImageInsertOptions" %>
<%@ Register Assembly="DTIControls" Namespace="HighslideControls" TagPrefix="cc1" %>
<asp:HyperLink runat="server" ID="hlInsertImage" Text="Insert Image" NavigateUrl="#"></asp:HyperLink><br />
<asp:HyperLink runat="server" ID="hlInsertImageThumb" Text="Insert ZoomNail" NavigateUrl="#"></asp:HyperLink><br />
<asp:HyperLink runat="server" ID="btnDeleteImage" Text="Delete Image" NavigateUrl="#"></asp:HyperLink><br />
<cc1:Highslider ID="hsEdit" runat="server" HighslideDisplayMode="Iframe">Edit Image</cc1:Highslider><br />