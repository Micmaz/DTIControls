<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ChangePasswordControl.ascx.vb" Inherits="DTIAdminPanel.ChangePasswordControl" %>

<%@ Register Assembly="DTIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>
<style type="text/css">
    .kurk-label{padding-top:10px}
    .kurk-text input {width:240px}
</style>
<div class="kurk-label kurk-pass">
    Password
</div>
<div class="kurk-text">
    <asp:TextBox ID="tbpass" CssClass="kurk-tbpass" runat="server" TextMode="password"></asp:TextBox>
</div>
<div class="kurk-valid kurk-required">
    <asp:RequiredFieldValidator ID="valReq" runat="server" ErrorMessage="Required Field" ControlToValidate="tbpass"></asp:RequiredFieldValidator>
</div>
<div class="kurk-spacer"></div>
<div class="kurk-label kurk-confirm">
    Confirm Password
</div>
<div class="kurk-text">
    <asp:TextBox ID="tbConfirm" CssClass="kurk-tbconfirm" runat="server" TextMode="password" ControlToValidate="tbConfirm"></asp:TextBox>
</div>
<div class="kurk-valid kurk-comp">
    <asp:CompareValidator ID="valComp" runat="server" 
        ErrorMessage="Passwords Do Not Match" ControlToCompare="tbpass" 
        ControlToValidate="tbConfirm"></asp:CompareValidator>
</div>
<div class="kurk-spacer"></div>
<div class="kurk-button">
    <asp:Button ID="btnChange" runat="server" Text="Button" />
</div>
<cc1:InfoDiv ID="InfoDiv1" CssClass="kurk-error" runat="server" isError="true" Visible="false">
    Invalid User
</cc1:InfoDiv>
