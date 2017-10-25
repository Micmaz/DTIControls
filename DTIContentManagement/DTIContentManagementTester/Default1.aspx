<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default1.aspx.vb" Inherits="DTIContentManagementTester._Default1" %>
<%@ Register Assembly="DTIContentManagement" Namespace="DTIContentManagement" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:CheckBox runat="server" ID="cbEdit" AutoPostBack="true" Text="Edit" /><br /><br /><br /><br />
        <cc1:EditPanel runat="server" id="editPanel1" contenttype="newEPtest"></cc1:EditPanel>
        <cc1:EditPanel runat="server" id="editPanel2" contenttype="newEPtest1"></cc1:EditPanel>
    </div>
    </form>
</body>
</html>
