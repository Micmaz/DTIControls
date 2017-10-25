<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploaderTest.aspx.vb" Inherits="JqueryUIControlsTest.UploaderTest" %>

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
    <cc1:Uploader uploadPath="uploads" ID="testul" style="" runat="server" />
<!--End Uploader -->
		<asp:Button ID="Button1" runat="server" Text="Button" />
    	<br />
		<asp:Label ID="lblFileList" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
