<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Gallery.ascx.vb" Inherits="DTIGallery.Gallery" %>
<%@ Register Assembly="DTIControls" Namespace="HighslideControls" TagPrefix="cc1" %>
<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="cc2" %>
<%@ Register Assembly="DTIControls" Namespace="DTIAjax" TagPrefix="cc3" %>
<cc3:ajaxSeverConrol jsCompleteFunc="done" ajaxReturn="html" ID="AjaxSeverConrol1" runat="server" >
</cc3:ajaxSeverConrol>
<div class="Control_Holder" runat="server" id="pnlGalleryHolder">
        <asp:Panel runat="server" ID="pnlSearch" CssClass="Search_Holder" style="display:none;"></asp:Panel>
        <cc2:FreezeIt runat="server" ID="freezePanel" DisplayOnAnyPostback="false" UseCentering="true"><asp:Image runat="server" ID="imgWait" ImageUrl="<%=BaseClasses.Scripts.ScriptsURL()%>DTIGallery/defaultWait.gif" /></cc2:FreezeIt>
        <asp:panel ID="pnlThumbHolder" runat="server" CssClass="Gallery_Holder" Height="350"></asp:panel>
        <asp:Panel runat="server" ID="pnlUploadLinks" CssClass="Upload_Links"></asp:Panel>
        <div class="Gallery_Button_Div" id="Gallery_Button_Div" runat="server" style="display:none;">
            <asp:ImageButton ID="btnGalleryFirst" CssClass="First_Button" runat="server"  ImageUrl="<%=BaseClasses.Scripts.ScriptsURL()%>DTIGallery/gallery_first.png"/>
            <asp:ImageButton id="btnGalleryBack" cssclass="Back_Button" runat="server" ImageUrl="<%=BaseClasses.Scripts.ScriptsURL()%>DTIGallery/gallery_back.png"  />
            <asp:TextBox ID="tbGalleryPage" CssClass="Page_Textbox" style="display:none;vertical-align:middle;" runat="server" Width="40">1</asp:TextBox>
            <asp:Label ID="lblSlash" CssClass="Pages_Label" Font-Bold="True" runat="server" > / </asp:Label>
            <asp:Label ID="lblGalleryTotalPages" CssClass="Pages_Label" Font-Bold="True" runat="server" ></asp:Label>
            <asp:ImageButton ID="btnGalleryPage" CssClass="Page_Button" runat="server" style="display:none;" ImageUrl="<%=BaseClasses.Scripts.ScriptsURL()%>DTIGallery/gallery_go.png" />
            <asp:ImageButton id="btnGalleryFwd" cssclass="Forward_Button" runat="server" ImageUrl="<%=BaseClasses.Scripts.ScriptsURL()%>DTIGallery/gallery_fwd.png"  />
            <asp:ImageButton ID="btnGalleryLast" CssClass="Last_Button" runat="server" ImageUrl="<%=BaseClasses.Scripts.ScriptsURL()%>DTIGallery/gallery_last.png"  />
        </div>
</div>
