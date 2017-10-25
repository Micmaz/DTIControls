<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="maskedInput.aspx.vb" Inherits="JqueryUIControlsTest.maskedInput" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

			 <cc1:maskedTextbox ID="ssn" maskPreset="SSN" runat="server"/>
			 <br />
			<asp:Button ID="Button1" runat="server" Text="Button" />
        	 <br />
			 <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
