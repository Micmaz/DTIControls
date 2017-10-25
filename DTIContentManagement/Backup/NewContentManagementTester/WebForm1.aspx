<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site2.Master" CodeBehind="WebForm1.aspx.vb" Inherits="NewContentManagementTester.WebForm1" %>
<%@ Register Assembly="DTIContentManagement" Namespace="DTIContentManagement" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<cc1:EditPanel runat="server" id="ep1"></cc1:EditPanel>

    <asp:Button ID="Button1" runat="server" Text="process" /><br />
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
</asp:Content>
