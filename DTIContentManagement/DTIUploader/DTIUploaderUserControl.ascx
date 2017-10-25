<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DTIUploaderUserControl.ascx.vb" Inherits="DTIUploader.DTIUploaderUserControl" %>

<div id="fileSizeWarning" runat="server" class="fileSizeWarning" >
    <asp:Label runat="server" id="lblFileSizeWarning" Text="*File Size Can Not Exceed "></asp:Label>
    <asp:Label ID="FileSizeLimit" runat="server" Text=""></asp:Label>
    <asp:Label ID="errorMessage" runat="server" Text="Please check your file size and try again." style="display:none"></asp:Label>
</div><br />
<asp:Panel runat="server" ID="pnlUploaderHolder"></asp:Panel>
<div style="text-align:right">
    <asp:LinkButton runat="server" ID="btnWeb" style="padding-right:20px;" Text="Upload a file from the web" /><asp:LinkButton runat="server" ID="btnToggleMode"></asp:linkbutton>
</div>
