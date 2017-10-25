<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="VideoInsertOptions.ascx.vb" Inherits="DTIContentManagement.VideoInsertOptions" %>
<%@ Register Assembly="DTIControls" Namespace="HighslideControls" TagPrefix="cc1" %>
<asp:HyperLink runat="server" ID="hlInsertVideo" Text="Insert Video" NavigateUrl="#"></asp:HyperLink><br />
<asp:HyperLink runat="server" ID="hlInsertVideoThumb" Text="Insert Video ZoomNail" NavigateUrl="#"></asp:HyperLink><br />
<asp:hyperlink runat="server" ID="hlDeleteVideo" Text="Delete Video" NavigateUrl="#" ></asp:hyperlink><br />
<cc1:Highslider ID="hsEdit" runat="server" HighslideDisplayMode="Iframe">Edit Video</cc1:Highslider><br />