<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VideoUploader.aspx.vb" Inherits="DTIVideoManager.VideoUploader" %>

<%@ Register Assembly="DTIControls" Namespace="DTIVideoManager" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        
    <div>
        <cc1:VideoUploaderControl ID="VideoUploaderControl1" runat="server">
        </cc1:VideoUploaderControl>
    </div>
    </form>
</body>
</html>
