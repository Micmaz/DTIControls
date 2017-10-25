<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="autoCompleteTest.aspx.vb" Inherits="MiniTests.autoCompleteTest" %>

<%@ Register Assembly="DTIMinicontrols" Namespace="DTIMinicontrols" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:AutocompleteDropDown ID="AutocompleteDropDown1" runat="server"></cc1:AutocompleteDropDown><asp:Button ID="Button1" runat="server" Text="Button" /><%--<input type="button" onclick="alert($('#AutocompleteDropDown1').indexOf());" />--%><br />
        <cc1:AutocompleteDropDown ID="AutocompleteDropDown2" runat="server"></cc1:AutocompleteDropDown>
    </div>
    </form>
</body>
</html>
