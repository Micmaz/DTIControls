<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ToolTipTest.aspx.vb" Inherits="JqueryUIControlsTest.ToolTipTest" %>

<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <cc1:ToolTip ID="ToolTip3" OpenEffect="fadeIn" runat="server" Text="Mouseover Link">
    bare minium
    </cc1:ToolTip>
    
    <br /><asp:Label ID="Label1" runat="server">Mouse Over this too. </asp:Label>Other content
    <br />Other contentOther contentOther contentOther contentOther content


     <cc1:ToolTip ID="ToolTip7" runat="server" OpenEffect="slideDown"  TargetControlID="Label1" Sticky="true" title="My Title" ClosePosition="title" CloseText="X" Arrows="true" >
        Test TEst TEst
       <asp:Button ID="Button1" runat="server" Text="Button" />
    </cc1:ToolTip>

    </div>
    </form>
</body>
</html>
