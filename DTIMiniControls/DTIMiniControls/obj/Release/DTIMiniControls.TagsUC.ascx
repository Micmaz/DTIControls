<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TagsUC.ascx.vb" Inherits="DTIMiniControls.TagsUC" %>
<script language="javascript" type="text/javascript">
//<![CData[
$(document).ready(function(){
    $("#<%=btnAdd.ClientId() %>").click(function(){
        var tagsSplit = $("#<%=tbNewTags.ClientId() %>").val().split("<%=SeparatorCharacter %>");
        for (var i = 0; i < tagsSplit.length; i++){ 
            if (tagsSplit[i].trim() != "")
                addCurrentTag(tagsSplit[i].trim(), '<%=DTICurrTags.ClientId() %>', false);
        } 
        $("#<%=tbNewTags.ClientId() %>").val("");
    });
}); 


//]]>   
</script>
<div class="DTITags">
    <div class="DTIlblCurrentTags">
        <asp:Label ID="lblCurrTags" runat="server" Text="Current Tags"></asp:Label></div>
    <asp:Panel id="DTICurrTags" runat="server">
        <asp:HiddenField ID="hfTags" runat="server" />
    </asp:Panel>
    <div runat="server" id="DTIPopularTagHolder">
    <div class="DTIlblCurrentTags"><asp:Label ID="lblPopularTags" runat="server" Text="Popular Tags"></asp:Label></div>
    <asp:panel id="DTIPopularTags" runat="server">
    </asp:panel>
    </div><br />
    <div class="DTIAddTags">
        <asp:label runat="server" ID="lblAddTags" Text="Add tags, separated by semi-colon: "></asp:label><asp:TextBox ID="tbNewTags" runat="server"></asp:TextBox>&nbsp;
        <input id="btnAdd" type="button" value="Add" runat="server"/><br />
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" /></div>
</div>