<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TypedGalleryTest.aspx.vb" Inherits="DTIGalleryTester.TypedGalleryTest" %>

<%@ Register Assembly="DTIGallery" Namespace="DTIGallery" TagPrefix="cc1" %>
<%@ Register Assembly="DTIMediaManager" Namespace="DTIMediaManager" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style>
        .Gallery_Holder {
            height: 450px;
            width: 1100px;
            border-style: dotted;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <asp:CheckBox runat="server" ID="cbEdit" AutoPostBack="true" Text="Edit" /><br /><br />
         <cc1:DTIImageGallery runat="server" ID="DTIGallery1"></cc1:DTIImageGallery>
        <div>
            &nbsp;</div>
    
    </div>
    </form>
</body>
</html>
