<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadMedia.aspx.vb" Inherits="DTIGallery.UploadMedia" %>
<%@ Register Assembly="DTIControls" Namespace="DTIMediaManager" TagPrefix="cc1" %>
<%@ Register Assembly="DTIControls" Namespace="DTIUploader" TagPrefix="cc2" %>
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
            margrin-right:15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:MediaUploader runat="server" ID="MediaUploader1"></cc1:MediaUploader>
        
    </div>
    </form>
</body>
</html>
