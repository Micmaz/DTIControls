<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucImage.ascx.vb" Inherits="DTIContentManagement.ucImage" %>
<%@ Register Assembly="DTIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>
<div class="imgcontainer">
    <div class="imgdelete">
        <asp:Button ID="lbDelete" runat="server" Text="X" />
    </div>
    <div id="divImg" runat="server" class="img">
        <a id="lkImage" runat="server" class="lightbox"><span id="sp_<%=ImageID %>" class="roll2" style="opacity: 0;"></span></a>
        <a href="~/res/DTIImageManager/ViewImage.aspx?Id=<%=ImageID %>" onclick="parent.window.focus();parent.window.iframe = getIframe(this);return parent.window.hs.expand(this, {slideshowGroup: 'mediaGall', align: 'center'})"><span id="Span1" class="roll" style="opacity: 0;"></span></a>
         
        <cc1:LazyImgLoad ID="LazyImgLoad1" runat="server" />
        
    </div>
    <div class="imgselector">
    <asp:Literal ID="litButtons" runat="server"></asp:Literal><br />
    <a id="ctl10_ctl03_hlInsertImage" onclick="insertMedia('&lt;img src=&quot;~/res/DTIImageManager/ViewImage.aspx?Id=<%=ImageID %>&amp;maxWidth=200&quot; &gt;&lt;/a&gt;'); return false;" href="javascript:void(0);">Insert Image</a><br>
<a id="ctl10_ctl03_hlInsertImageThumb" onclick="insertMedia('&lt;a href=&quot;~/res/DTIImageManager/ViewImage.aspx?Id=<%=ImageID %>&quot; id=&quot;&quot; title=&quot;Click to Enlarge&quot; class=&quot;highslide&quot; onclick=&quot;return hs.expand(this, { })&quot;&gt;&lt;img src=&quot;~/res/DTIImageManager/ViewImage.aspx?Id=<%=ImageID %>&amp;maxHeight=120&amp;maxWidth=120&quot; title=&quot;Click to enlarge&quot; /&gt;&lt;/a&gt;'); return false;" href="javascript:void(0);">Insert ZoomNail</a><br>
<%--
<a href="~/res/DTIMediaManager/EditMediaItem.aspx?mid=<%=ImageID %>" id="ctl10_ctl03_hsEdit" title="Click to Enlarge" class="highslide" onclick="parent.window.focus();parent.window.iframe = getIframe(this);return parent.window.hs.htmlExpand(this, {width: 650, objectType: 'iframe'})">Edit</a>
&nbsp;
<a id="ctl10_ctl03_btnDeleteImage" onclick="deleteMedia(4); return false;" href="../../../../../../res/DTIContentManagement/#">Delete</a>--%>
        
    </div>
</div>
