<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditMediaItem.aspx.vb" Inherits="DTIMediaManager.EditMediaItem" %>

<%@ Register Assembly="DTIControls" Namespace="DTIMediaManager" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style>
        .DTICancelButton{
            color: red;
            float: left;
            cursor: pointer;
            font-weight: bolder;
            font-family: Verdana;
        }
        .DTITagText{
            float: left;
        }
        .DTICurrentTag{
            margin: 5px;
            float: left;
        }
        .DTIPopularTag{
            margin: 5px;
            color: blue;
            float: left;
            cursor: pointer;
            text-decoration: underline;
        }
        .DTIlblCurrentTags {
            clear: both;
        }
        .DTIAddTags {
            clear: both;
        }
    </style>
    <script type="text/javascript">
   
    
    if(window.hs !== undefined) {
        hs.Expander.prototype.onAfterClose = function() {
            if(this.a.href.indexOf("ManipulateImage.aspx") > -1) {
                $(this.a).parent().children('span[id$="lblSize"]').html($(this.a).children('img').width() + 'x' + $(this.a).children('img').height());
            }
        };    
    } else if(parent.window.hs !== undefined) {
        parent.window.hs.Expander.prototype.onAfterClose = function() {
            if(this.a.href.indexOf("ManipulateImage.aspx") > -1) {
                $(this.a).parent().children('span[id$="lblSize"]').html($(this.a).children('img').width() + 'x' + $(this.a).children('img').height());
            }
        };      
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:EditMediaControl ID="EditMediaControl1" runat="server">
        </cc1:EditMediaControl>
    </div>
    <div style="text-align:right"><asp:Button runat="server" ID="btnSave" Text="Save" /></div>
    </form>
</body>
</html>
