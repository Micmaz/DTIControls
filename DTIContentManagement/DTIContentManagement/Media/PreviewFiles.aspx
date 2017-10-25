<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PreviewFiles.aspx.vb" Inherits="DTIContentManagement.PreviewFiles" %>
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
        hs.Expander.prototype.onBeforeExpand = null;    
        hs.Expander.prototype.onAfterClose = function() {
            if(this.a.href.indexOf("ManipulateImage.aspx") > -1) {
                $(this.a).parent().children('span[id$="lblSize"]').html($(this.a).children('img').width() + 'x' + $(this.a).children('img').height());
            }
        };    
        hs.Expander.prototype.onAfterExpand = null;
        hs.Expander.prototype.onBeforeClose = null;    
        hs.previousOrNext = function (el, op) {
	        var exp = hs.getExpander(el);
	        if (exp) return hs.transit(exp.getAdjacentAnchor(op), exp);
	        else return false;
        }
        hs.isHsAnchor = function (a) {
	        return (a.onclick && a.onclick.toString().replace(/\s/g, ' ').match(/hs.(htmlE|e)xpand/));
        };
    } else if(parent.window.hs !== undefined) {
        parent.window.hs.Expander.prototype.onBeforeExpand = null;    
        parent.window.hs.Expander.prototype.onAfterClose = function() {
            if(this.a.href.indexOf("ManipulateImage.aspx") > -1) {
                $(this.a).parent().children('span[id$="lblSize"]').html($(this.a).children('img').width() + 'x' + $(this.a).children('img').height());
            }
        };      
        parent.window.hs.Expander.prototype.onAfterExpand = null;
        parent.window.hs.Expander.prototype.onBeforeClose = null;    
        parent.window.hs.previousOrNext = function (el, op) {
	        var exp = parent.window.hs.getExpander(el);
	        if (exp) return parent.window.hs.transit(exp.getAdjacentAnchor(op), exp);
	        else return false;
        }
        parent.window.hs.isHsAnchor = function (a) {
	        return (a.onclick && a.onclick.toString().replace(/\s/g, ' ').match(/hs.(htmlE|e)xpand/));
        };
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
       <div>
        <asp:PlaceHolder ID="phFilePreview" runat="server"></asp:PlaceHolder>
    </div>
    <div style="text-align:right;">
        <asp:Button runat="server" ID="btnSave" Text="Save" />
    </div>
    </form>
</body>
</html>
