<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImageCropper.aspx.vb" Inherits="MiniTests.ImageCropper" %>

<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Image Cropper Demo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <DTI:ImageCropper ID="ImageCropper1" runat="server" ImageUrl="http://avantlumiere.com/splash.png"/><br />
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <br />http://avantlumiere.com/splash.png
        <br />http://www.bungi.com/kelsea/k-11-05-big.jpg
        <br />http://img512.imageshack.us/img512/9388/untitledvv3.bmp
        <br />http://www.google.com/intl/en_ALL/images/logo.gif
        </div>
    </form>
</body>
</html>
