<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LoginUserControl.ascx.vb" Inherits="DTIAdminPanel.LoginUserControl" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>
<%@ Register Assembly="DTIAdminPanel" Namespace="DTIAdminPanel" TagPrefix="cc2" %>
<table id="tabLogin" runat="server">
    <tr>
        <td>User Name</td>
        <td>
            <asp:TextBox ID="tbUser" runat="server" Width="180px"></asp:TextBox></td>
        <td>
        </td>
    </tr>
    <tr>
        <td>Password</td>
        <td>
            <asp:TextBox ID="tbPass" runat="server" TextMode="password" Width="180px"></asp:TextBox></td>
        <td>
            <cc1:Dialog ID="diForgotPass" runat="server" Modal="true" Title="Forgot Password" Width="500px" openerText="Forgot Password?" OpenerType="link">
                <cc1:InfoDiv id="InfoDiv2" HideIcon="true" runat="server">Please enter the e-mail address you registered with and 
                then click SUBMIT. Within a few minutes you’ll receive an e-mail with instructions on how to 
                reset your password.</cc1:InfoDiv>
                <div style="margin: 0 auto;width:300px;padding-top:20px">
                    E-mail Address: <asp:TextBox ID="tbForgotEmail" runat="server"></asp:TextBox>         
                </div>
                <asp:Button ID="btnForgotPassSubmit" runat="server" Text="Submit" />
            </cc1:Dialog>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right">
            <asp:Button ID="btnLogin" runat="server" Text="Login" /></td>
        <td>
            <cc1:Dialog ID="diRegister" runat="server" Modal="true" Title="Online Registration" Width="550px" openerText="Register Now" OpenerType="link">
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
                </table>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
                <cc2:PasswordStrength ID="PasswordStrength1" runat="server"></cc2:PasswordStrength>
                <cc1:InfoDiv id="idRegisterError" HideIcon="false" isError="true" Visible="false" runat="server"></cc1:InfoDiv>
                <asp:CheckBox ID="cbTerms" runat="server" />
            </cc1:Dialog>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <%--<asp:Label ID="lblerror" runat="server" ForeColor="Red" Text="Username and/or password is incorrect"></asp:Label>--%>
            <cc1:InfoDiv id="lblError" isError="true" runat="server">Username/Password is incorrect</cc1:InfoDiv></td>
    </tr>
</table>