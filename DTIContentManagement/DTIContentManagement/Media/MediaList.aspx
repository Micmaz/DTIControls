<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MediaList.aspx.vb" Inherits="DTIContentManagement.MediaList" %>
<%@ Register Assembly="DTIControls" Namespace="DTIGallery" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Media List</title>
    <script type="text/javascript">
        String.prototype.startsWith = function(pattern){
          return !this.lastIndexOf(pattern,0);
        };
        
        parent.window.hs.isHsAnchor = function(a) {
            if (a.onclick && a.onclick.toString().indexOf('insertMedia') > -1) {
                return null;
            } else {
                return (a.onclick && a.onclick.toString().replace(/\s/g, ' ').match(/hs.(htmlE|e)xpand/));
            }
        };
        
        function insertMedia(mediaHtml)
        {
			if (parent.window.DTISummernote.getCurreditor()){
				(parent.window.DTISummernote.getCurreditor()).addContent(mediaHtml);
				parent.window.DTISummernote.closeDialog();
            }
        }

        
    </script>
<style>
        .thumbnailPictureSpan {
            display:inline-block; 
	        width:150px; 
	        height:180px; 	        
        }
        
        .Gallery_Button_Div {
            width:100%;
        }
        .Gallery_Holder {
            height: 380px;
            width: 550px;
            font-size: smaller;
        }
        .Gallery_Button_Div input{
            vertical-align:middle;
        }
        .thumbImageCell {
            vertical-align:middle;
        }
    </style>

</head>
<body><script type="text/javascript">
        function deleteMedia(mediaId)
        {
            if (confirm("Are you sure you want to delete this item?")) {
                $.ajax({
                  type: "POST",
                  url: "~/res/DTIContentManagement/DeleteMedia.aspx?i=" + mediaId,
                  contentType: "application/json; charset=utf-8",
                  dataType: "json"
                });
                $('#<%=mediaGall.mediaSearcher.btnSearch.ClientId %>').click();
            }
        }
       </script>
    <form id="form1" runat="server">
    <div style="width:100%; text-align:right"><a href="UploadMedia.aspx">Upload Media</a></div>
    <div>
        <cc1:DTISlideGallery runat="server" id="mediaGall"></cc1:DTISlideGallery>
    </div>
    </form>
</body>
</html>
