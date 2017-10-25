<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ForgotPasswordControl.ascx.vb" Inherits="DTIAdminPanel.ForgotPasswordControl" %>
<%@ Register Assembly="DTIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

    <div class="kurk-message">
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    </div>
    <div class="kurk-label">
        <asp:Label ID="lblText" runat="server" Text="Email"></asp:Label></div>
    <div class="kurk-text">
        <asp:TextBox ID="tbEmailRecovery" runat="server" Width="300px"></asp:TextBox>
    </div>
    <div class="kurk-spacer"></div>   
    <cc1:InfoDiv id="idSent" CssClass="kurk-success" runat="server" visible="false"></cc1:InfoDiv>
    <cc1:InfoDiv id="idEmailError" CssClass="kurk-error" runat="server" iserror="true" visible="false"></cc1:InfoDiv>
    <asp:Button ID="btnSendEmail" runat="server" Text="Send" />

