<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="DTIMediaManagerTesterer._Default" %>
<%@ Register Assembly="DTIGallery" Namespace="DTIGallery" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
    <cc1:DTISocialGallery runat="server" ID="DTISocialGallery1"></cc1:DTISocialGallery>
    </div>
    </form>
</body>
</html>
