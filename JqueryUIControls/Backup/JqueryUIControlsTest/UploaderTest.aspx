﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploaderTest.aspx.vb" Inherits="JqueryUIControlsTest.UploaderTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
<!--Start Uploader -->
    <%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>
    <cc1:Uploader uploadPath="uploads" buttonText="<img style='width: 45px;height:45px;' src='camera.png'/>"  dropAreaText="" ID="testul" style="" runat="server" />
<!--End Uploader -->

    </div>
    </form>
</body>
</html>
