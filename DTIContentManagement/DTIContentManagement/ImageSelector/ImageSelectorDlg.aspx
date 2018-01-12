<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImageSelector.aspx.vb" Inherits="DTIContentManagement.ImageSelectorDlg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Image Selector</title>
        <script type="text/javascript">
            String.prototype.startsWith = function (pattern) {
                return !this.lastIndexOf(pattern, 0);
            };

            parent.window.hs.isHsAnchor = function (a) {
                if (a.onclick && a.onclick.toString().indexOf('insertMedia') > -1) {
                    return null;
                } else {
                    return (a.onclick && a.onclick.toString().replace(/\s/g, ' ').match(/hs.(htmlE|e)xpand/));
                }
            };

            function insertMedia(mediaHtml) {
				if (parent.window.DTISummernote.getCurreditor()) {
					parent.window.DTISummernote.addContent(mediaHtml);
					parent.window.DTISummernote.closeDialog();
					// @param {Node} node
					//$(parent.window.curreditor).summernote('insertNode', $(mediaHtml));
					//parent.window.curreditor.insertHtml(mediaHtml);
                    
                }
            }

        function deleteMedia(mediaId) {
              if (confirm("Are you sure you want to delete this item?")) {
                  $.ajax({
                      type: "POST",
                      url: "~/res/DTIContentManagement/DeleteMedia.aspx?i=" + mediaId,
                      contentType: "application/json; charset=utf-8",
                      dataType: "json"
                  });

              }
          }
       </script>
</head>
<body>
    <form id="form1" runat="server">


    <style type="text/css">
        .img {height:100px;width:100px;display: table-cell;vertical-align: middle;border: 3px solid #fff;}
        .imgcontainer {position:relative;width:100px;padding: 0 5px 5px 5px;float: left;text-align: center;}
        .imgselector{white-space: nowrap;overflow: hidden;text-overflow: ellipsis;}
        .clearfix { clear:both; }
        .innerdiv{height: 400px;overflow-x: hidden;overflow-y: scroll;}  
        .imgdelete{position: absolute;top: 0;right: 0;}   
        .selected{border-color:#0000FF;}   
    </style> 
    <script type="text/javascript" src="<%=BaseClasses.Scripts.ScriptsURL()%>DTIContentManagement/jquery.lightbox-0.5.min.js"></script>
    <script type="text/javascript"> 
        function imgSelected(imageid, func, name) {
            func(imageid, name);
            $('#<%=HiddenField1.ClientID()%>').val(imageid);
        }

        function getImageIDs() {
            return $('#<%=HiddenField1.ClientID()%>').val();
        }
        //$(function () { addButtonsFromFrame({ 'Ok': function () { return false; }}); })
    </script>
    <asp:PlaceHolder ID="ph1" runat="server"/>
    <%--<uc1:ucImageSelector ID="ucImageSelector1" runat="server" HideButton="true" />--%>
    <asp:HiddenField ID="HiddenField1" runat="server" /> 
    </form>
</body>
</html>
