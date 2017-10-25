<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PageEditControl.ascx.vb" Inherits="DTIAdminPanel.PageEditControl" %>
<asp:HiddenField ID="HiddenField1" runat="server" />
<script type="text/javascript">
        function removeSpaces()
        {
            var pagetxt = document.getElementById('<%=tbPageName.ClientID%>').value.replace(/[^a-zA-Z0-9\s\_]/g, "");
            var linktxt = document.getElementById('<%=tbLink.ClientID%>').value;
            var lbl = document.getElementById('<%=lbl.ClientID%>');
            
            document.getElementById('<%=tbPageName.ClientID%>').value = pagetxt;        
            
            if (linktxt == '')
                lbl.innerHTML = 'http://' + window.location.host + '/page/' + pagetxt + '.aspx';
            else if (linktxt.startsWith('http://'))
                lbl.innerHTML = linktxt;
            else
                lbl.innerHTML = 'http://' + window.location.host + '/' + linktxt;
        }  
        
        String.prototype.startsWith = function(str) {return (this.match("^"+str)==str)}      
</script>
<table cellpadding="4" cellspacing="4" style="width: 400px">
    <tr>
        <td>
            Page Name</td>
        <td>
            <asp:TextBox ID="tbPageName" runat="server" onkeyup="removeSpaces();"></asp:TextBox></td>
    </tr>    
    <tr>
        <td>
            Static Page Link</td>
        <td>
            <asp:TextBox ID="tbLink" runat="server" onkeyup="removeSpaces();"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2">
        <label id="lbl" runat="server"></label>
        </td>
    </tr>
    <tr>    
        <td>
            Master Page</td>
        <td>
            <asp:DropDownList ID="ddlMasterPage" runat="server">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox ID="cbAdmin" runat="server" />
            Admin Only
            <br />
            <asp:CheckBox ID="cbUsers" runat="server" Visible="false" />
            <%--Users Only--%>
        </td>
    </tr>
   <%-- <tr>
        <td colspan="2">
            <asp:RadioButtonList ID="rblTemplate" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="1">Full Page</asp:ListItem>
                <asp:ListItem Value="2">2 Columns</asp:ListItem>
                <asp:ListItem Value="3">2 Column w/Header</asp:ListItem>
                    <asp:ListItem Value="0">Edit Panel</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>--%>
</table>
