<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploaderServerControlTest.aspx.vb" Inherits="DTIUploaderTester.UploaderServerControlTest" %>

<%@ Register Assembly="DTIUploader" Namespace="DTIUploader" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:DTIUploaderControl ID="DTIUploaderControl1" AJAXEnabled="false" runat="server" >
        </cc1:DTIUploaderControl>
    
    </div>
    </form>
</body>
</html>
