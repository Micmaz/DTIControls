<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="WebFileUploader.ascx.vb" Inherits="DTIUploader.WebFileUploader" %>
<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="cc1" %>
<asp:Panel runat="Server" ID="pnlFilesHolder"></asp:Panel>
<cc1:Tagger ID="Tagger1" runat="server"  />
<asp:Button runat="server" ID="btnUpload" Text="Upload" />