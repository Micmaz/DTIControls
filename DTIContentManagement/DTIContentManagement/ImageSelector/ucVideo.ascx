<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucVideo.ascx.vb" Inherits="DTIContentManagement.ucVideo" %>
<%@ Register Assembly="DTIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>
<div class="imgcontainer">
    <div class="imgdelete">
        <asp:Button ID="lbDelete" runat="server" Text="X" />
    </div>
    <div id="divImg" runat="server" class="img">
        <a id="lkImage" runat="server" class="lightbox iframe"><span id="sp_<%=ImageID %>" class="roll" style="opacity: 0;"></span></a>
        <cc1:LazyImgLoad ID="LazyImgLoad1" runat="server" Width="100px" />
    </div>
    <div class="imgselector">
        <asp:Literal ID="litButtons" runat="server"></asp:Literal>
    </div>
</div>