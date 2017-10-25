<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditItem.aspx.vb" Inherits="DTIAdminPanel.EditItem" %>
<%@ Register Assembly="DTIMediaManager" Namespace="DTIMediaManager" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
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
        parent.window.hs.Expander.prototype.onAfterClose = function(){
            history.go(-1);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <cc1:EditMediaControl id="editMedia1" runat="server"></cc1:EditMediaControl>
    
        <br />
        <div style="float:right;">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                <asp:Button ID="btnSave" runat="server" Text="Save" /></div><br style="clear:both;" />
    </div>
    </form>
</body>
</html>
