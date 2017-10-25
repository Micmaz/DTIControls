<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ManipulateImage.aspx.vb" Inherits="DTIImageManager.ManipulateImage" %>

<%@ Register Assembly="DTIControls" Namespace="DTIImageManager" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crop Image</title>
    <style type="text/css">
        #myCropper{
    max-height: 900px;
max-width: 100%;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div><center>
        <cc1:ImageManipulator ID="ImageManipulator1" runat="server">
        </cc1:ImageManipulator>
    </center>
    </div>
    </form>
</body>
</html>
