<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VideoViewer.aspx.vb" Inherits="DTIVideoManager.VideoViewer" %>

<%@ Register Assembly="DTIControls" Namespace="DTIVideoManager" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style>
        body { background-color: transparent; background: none; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:VideoViewerControl ID="VideoViewerControl1" runat="server">
        </cc1:VideoViewerControl>
    
    </div>
    </form>
</body>
</html>
