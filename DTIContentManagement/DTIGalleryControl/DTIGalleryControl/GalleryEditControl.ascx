<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GalleryEditControl.ascx.vb" Inherits="DTIGallery.GalleryEditControl" %>
<%@ Register Assembly="DTIControls" Namespace="HighslideControls" TagPrefix="cc1" %>
<table style="text-align:right">
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Items / Page"></asp:Label></td>
        <td>
            <asp:TextBox runat="server" ID="tbItemsPerPage" Width="60"  maxlength="2"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Gallery Width"></asp:Label></td>
        <td>
            <asp:TextBox runat="server" ID="tbGalleryWidth" Width="60"  maxlength="4"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label5" runat="server" Text="Gallery Height"></asp:Label></td>
        <td>
            <asp:TextBox runat="server" ID="tbGalleryHeight" Width="60"  maxlength="4"></asp:TextBox></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Thumbnail Max Width"></asp:Label></td>
        <td>
            <asp:TextBox runat="server" ID="tbThumbWidth" Width="60"  maxlength="3"></asp:TextBox></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Thumbnail Max Height"></asp:Label></td>
        <td>
            <asp:TextBox runat="server" ID="tbThumbHeight" Width="60"  maxlength="3"></asp:TextBox></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label8" runat="server" Text="Thumbnail Span Width"></asp:Label></td>
        <td>
            <asp:TextBox runat="server" ID="tbSpanWidth" Width="60"  maxlength="3"></asp:TextBox></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label9" runat="server" Text="Thumbnail Span Height"></asp:Label></td>
        <td>
            <asp:TextBox runat="server" ID="tbSpanHeight" Width="60"  maxlength="3"></asp:TextBox></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label10" runat="server" Text="Show First and Last Buttons"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbFirstAndLast" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label11" runat="server" Text="Show Paging"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbPaging" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label12" runat="server" Text="Show Searching"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbSearching" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label13" runat="server" Text="Show Uploading"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbUpload" /></td>
    </tr>
    <% If TypeOf (caller) Is DTIGallery.DTISocialGallery Then%>
    <tr>
        <td>
            <asp:Label ID="Label14" runat="server" Text="Show Thumbnail Info"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbShowThumbInfo" /></td>
    </tr>
    <tr class="ThumbnailRelated">
        <td>
            <asp:Label ID="Label15" runat="server" Text="Show Publish Date on Thumbnail"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbShowPubDateOnThumbnail" /></td>
    </tr>
    <tr class="ThumbnailRelated">
        <td>
            <asp:Label ID="Label16" runat="server" Text="Show Author on Thumbnail"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbShowAuthorOnThumbnail" /></td>
    </tr>
    <tr class="ThumbnailRelated">
        <td>
            <asp:Label ID="Label17" runat="server" Text="Show Rating on Thumbnail"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbShowRatingOnThumbnail" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label18" runat="server" Text="Show Publish Date on Caption"></asp:Label></td>
        <td>
             <asp:CheckBox runat="Server" ID="cbShowPubDateOnCaption" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label19" runat="server" Text="Show Author on Caption"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbShowAuthorOnCaption" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label20" runat="server" Text="Show Rating on Caption"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbShowRatingOnCaption" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label21" runat="server" Text="Show Sharing on Caption"></asp:Label></td>
        <td>
            <asp:CheckBox runat="Server" ID="cbShowSharingOnCaption" /></td>
    </tr>
    <%End If%>
    
    <tr>
        <td>
            <asp:Label ID="Label6" runat="server">
                Gallery Type </asp:Label><br /><asp:Label ID="Label7" runat="server" style="font-size:xx-small; width:200px;">All items uploaded to this gallery 
                will be related to this 'gallery type' identifier.  Only items uploaded to a gallery that 
                has this identifer will be displayed in the gallery.</asp:Label>
            </td>
        <td>
            <asp:TextBox runat="server" ID="tbCompType"></asp:TextBox></td>
    </tr>
        <tr>
        <td>
            <cc1:Highslider runat="server" ID="hsUpload" HighslideDisplayMode="Iframe" 
                ExpandURL="/res/DTIGallery/UploadMedia.aspx" DisplayText="Upload Media" width="650" height="450" 
                preserveContent="false"></cc1:Highslider>
            
        </td>
        <td>
            </td>
    </tr>
    <tr>
        <td colspan="2">
           <asp:Button runat="server" ID="btnSave" Text="Save" /></td>
    </tr>
</table>
