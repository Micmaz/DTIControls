<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site2.Master" CodeBehind="default.aspx.vb" Inherits="DTIAdminPanelTestProject._default" 
    title="Untitled Page" %>
<%@ Register Assembly="DTIAdminPanel" Namespace="DTIAdminPanel" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<cc1:LoginControl runat="server" ID="loginControl1" EnablePasswordStrength="true" isAdmin="false" />
    <asp:Button ID="btnlogin" runat="server" Text="login" />
    <asp:Button ID="btnlogout" runat="server" Text="logout" />
    <asp:Button ID="btnediton" runat="server" Text="editon" />
    <asp:Button ID="btnlayouton" runat="server" Text="layouton" />
    
    
</asp:Content>
