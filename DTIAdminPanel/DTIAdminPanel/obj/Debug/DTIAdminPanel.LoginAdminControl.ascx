<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LoginAdminControl.ascx.vb" Inherits="DTIAdminPanel.LoginAdminControl" %>
<%@ Register Assembly="DTIAdminPanel" Namespace="DTIAdminPanel" TagPrefix="cc2" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>
<table id="tabLogin" runat="server">
    <tr>
        <td>User Name</td>
        <td>
            <asp:TextBox ID="tbUser" runat="server" Width="180px"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Password</td>
        <td>
            <asp:TextBox ID="tbPass" runat="server" TextMode="password" Width="180px"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2" align="right">
            <asp:Button ID="btnLogin" runat="server" Text="Login" /></td>
    </tr>
    <tr>
        <td align="left" colspan="2">
            <cc1:InfoDiv id="lblError" isError="true" runat="server">Username/Password is incorrect</cc1:InfoDiv></td>
    </tr>
</table>
<cc1:Dialog ID="diAdminSetup" runat="server" Modal="true" Title="New Username and Password for site editing" Width="550px" Height="300px">
    <table width="100%">
        <tr>
            <td align="right" style="width: 121px">User Name</td>
            <td style="width: 185px">
                <asp:TextBox ID="tbuser2" runat="server" Width="180px"></asp:TextBox>
                </td>
            <td>
                <span id="sprequ" style="color:Red;display:none;">Required Field</span>
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 121px">
                E-mail</td>
            <td style="width: 185px">
                <asp:TextBox ID="tbEmail" runat="server" Width="180px"></asp:TextBox></td>
            <td>
                <span id="spreqe" style="color:Red;display:none;">Required Field</span>
                <span id="spEmails" style="color:Red;display:none;">Invalid Email Address</span>
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 121px">Password</td>
            <td style="width: 185px">
                <asp:TextBox ID="tbpass2" runat="server" TextMode="password" Width="180px"></asp:TextBox>
            </td>
            <td>
                <span id="spreqp" style="color:Red;display:none;">Required Field</span>
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 121px">
                Confirm Password</td>
            <td style="width: 185px">
                <asp:TextBox ID="tbConfirm" runat="server" TextMode="password" Width="180px"></asp:TextBox></td>
            <td>
                <span id="spConfirm" style="color:Red;display:none;">Passwords Do Not Match</span></td>
        </tr>       
        <tr>
            <td colspan="3">
                &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" /></td>
        </tr>
    </table>
    <cc2:PasswordStrength ID="PasswordStrength1" runat="server"></cc2:PasswordStrength>
</cc1:Dialog>


<div id="loggedInControl" runat="server">You are currently Logged In <asp:LinkButton ID="lbLogout" runat="server">Logout</asp:LinkButton></div>
<div id="adminOn" runat="server">Admin Mode is currently on</div>