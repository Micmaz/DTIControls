<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PreviewFiles.aspx.vb" Inherits="DTIMediaManager.PreviewFiles" %>
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
