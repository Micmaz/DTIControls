<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="JqueryUIControlsTest.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <%--<link href="style.css" rel="stylesheet" type="text/css" />--%>

    <style type="text/css" >
    body
    {
       <%-- font-size:10px;--%>
        }
    </style>
    <script type="text/javascript" language="javascript">
        function rethemeBody() {
            themeObjectOverridable('ui-widget', 'body', 'bodyOverride', ['font-size']);
            //themeObjectOverridable(cssClassName, objectSelector, styleId, excludeAttribs)
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id='tmp' class='ui-widget'>Widget object</div>
    
        Default no theme.<br />
        <a href="javascript:void(0)" onclick="changeup()">changeup</a><br />
        <asp:Button ID="Button1" runat="server" Text="Button" />
&nbsp;button<br />
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
        Dropdown<br />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        Textbox</div>
    </form>
</body>
</html>
