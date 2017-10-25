<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchUserControl.ascx.vb" Inherits="DTIContentManagement.SearchUserControl" %>
<%@ Register Assembly="DTIControls" Namespace="HighslideControls" TagPrefix="cc1" %>
<script type="text/javascript">
    hs.Expander.prototype.onInit = function () {
        if(this.a.id == '<%=hsSearch.ClientId %>') {
            $('#<%=SearchTextBoxClientId %>').val($('#<%=tbSearch.ClientId %>').val());
            $('#<%=SearchButtonClientId %>').click(); 
        }
    }
</script>
<asp:TextBox ID="tbSearch" runat="server" Width="150" style="vertical-align:top;"></asp:TextBox><cc1:Highslider id="hsSearch" runat="server" HighslideDisplayMode="HTML"></cc1:Highslider>
        