<%@ Control Language="vb" AutoEventWireup="false" EnableViewState="false" CodeBehind="ThumbControl.ascx.vb" Inherits="DTIGallery.ThumbControl" %>
<%@ Register Assembly="DTIControls" Namespace="HighslideControls" TagPrefix="cc1" %>
<asp:label runat="server" ID="lblThumb" cssclass="thumbnailPictureSpan" style="display:inline-block;">
<asp:PlaceHolder runat="server" ID="phThumbControls"></asp:PlaceHolder>
</asp:label>




