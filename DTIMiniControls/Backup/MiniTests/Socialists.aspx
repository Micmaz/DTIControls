<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Socialists.aspx.vb" Inherits="MiniTests.Socialists" %>

<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <DTI:StarRater ID="StarRater1" runat="server"> </DTI:StarRater><br />
        <DTI:AddThisControl ID="AddThisControl1" runat="server"></DTI:AddThisControl>
    <br />
    <asp:Button runat="Server" ID="b1" Text="Submit" />
    </div>
    </form>
</body>
</html>
