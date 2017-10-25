<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LoginUserControl.ascx.vb" Inherits="DTIAdminPanel.LoginUserControl" %>

<%@ Register Assembly="DTIControls" Namespace="DTIAdminPanel" TagPrefix="cc2" %>
<%@ Register Assembly="DTIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<div id="KurkLogin" runat="server" class="Kurk-login">
    <span class="Kurk-lblUser">User Name</span>
    <asp:TextBox ID="tbUser" CssClass="Kurk-tbUser" runat="server" MaxLength="50"></asp:TextBox>
    <div class="Kurk-Spacer"></div>
    <span class="Kurk-lblPass">Password</span>
    <asp:TextBox ID="tbPass" CssClass="Kurk-tbPass" runat="server" 
        TextMode="password" MaxLength="50"></asp:TextBox>
    <div class="Kurk-Spacer"></div>
    <asp:CheckBox ID="cbRemember" CssClass="Kurk-Remember" runat="server" Text="Remember Me" />
    <asp:Button ID="btnLogin" CssClass="Kurk-btnLogin" runat="server" Text="Login" />
    <div class="Kurk-Spacer"></div>
    <asp:HyperLink ID="linkForgot" CssClass="Kurk-Forgot" runat="server">Forgot Password?</asp:HyperLink>
    <div class="Kurk-Spacer"></div>
</div>

<cc1:InfoDiv ID="lblErrorAttempts" CssClass="Kurk-Error Kurk-Attempts" runat="server" isError="true" Visible="false">
</cc1:InfoDiv>
<cc1:InfoDiv ID="lblError" CssClass="Kurk-Error Kurk-Invalid" runat="server" isError="true" Visible="false">
</cc1:InfoDiv>

<cc1:Dialog ID="diAdminSetup" runat="server" Modal="true" Title="Create New User" Width="400px">
    <div style="width:250px; margin:0 auto;">
        <div style="padding-top: 10px">
            User Name
        </div>
        <div>
            <asp:TextBox ID="tbuser2" runat="server" Width="240px" MaxLength="50"></asp:TextBox>
        </div>
        <div style="height:10px">
            <span id="sprequ" style="color:Red;visibility:hidden;">Required Field</span>
        </div>
        <div style="padding-top: 10px">
            E-mail
        </div>
        <div>
            <asp:TextBox ID="tbEmail" runat="server" Width="240px" MaxLength="50"></asp:TextBox>
        </div>
        <div>
            <span id="spreqe" style="color:Red;visibility:hidden;">Required Field</span>
            <span id="spEmails" style="color:Red;visibility:hidden;">Invalid Email Address</span>
        </div>
        <div style="padding-top: 10px">
            Password
        </div>
        <div>
            <asp:TextBox ID="tbpass2" runat="server" TextMode="password" Width="240px" 
                MaxLength="50"></asp:TextBox>
        </div>
        <div>
            <span id="spreqp" style="color:Red;visibility:hidden;">Required Field</span>
        </div>
        <div style="padding-top: 10px">
            Confirm Password
        </div>
        <div>
            <asp:TextBox ID="tbConfirm" runat="server" TextMode="password" Width="240px" 
                MaxLength="50"></asp:TextBox>
        </div>
        <div>
            <span id="spConfirm" style="color:Red;visibility:hidden;">Passwords Do Not Match</span>
        </div>
    </div>
    <asp:Panel ID="pnlPwd" runat="server" style="padding-top:10px">
        <cc2:PasswordStrength ID="PasswordStrength1" runat="server"></cc2:PasswordStrength>
    </asp:Panel>
       
                 
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
</cc1:Dialog>


<div id="loggedInControl" runat="server">You are currently Logged In <asp:LinkButton ID="lbLogout" runat="server">Logout</asp:LinkButton></div>
<div id="adminOn" runat="server">Admin Mode is currently on</div>