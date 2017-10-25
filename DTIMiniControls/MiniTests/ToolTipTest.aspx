<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ToolTipTest.aspx.vb" Inherits="MiniTests.ToolTipTest" %>
<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .cluetip-newTheme {
          background-color: #FFFCD7;
          padding: 1px;
          border: solid 1px black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%--<cc2:ToolTip ID="ToolTip1" runat="server" 
        Text="Rounded Theme" 
        Title="Title Goes Here"
        Sticky="true" MouseOutClose="true"
        ClosePosition="title" CloseText="X"
        Arrows="true" CSSTheme="rounded" DropShadow="false">
    Rounded theme that is sticky but closes when you mouse out, and has arrows.  Be sure to set
    DropShadow to false if using rounded
    </cc2:ToolTip>
    <br />
    <cc2:ToolTip ID="ToolTip5" runat="server" Text="Calls Javascript"
             Sticky="true"
             onActivate="function(){alert('onActivate');return true;}" >
    calls javascript to verify if the popup should be opened
    </cc2:ToolTip>
    <br />
    <cc2:ToolTip ID="ToolTip2" runat="server" 
        Text="jTip Theme" 
        Title="Title Goes Here"
        Sticky="true" MouseOutClose="true"
        ClosePosition="title" CloseText="X"
        Arrows="true" CSSTheme="jtip">
    Jtip theme same as rounded but differnt theme
    </cc2:ToolTip>
    <br />
    <cc2:ToolTip ID="ToolTip3" runat="server" Text="Default">
    bare minium
    </cc2:ToolTip>
     <br />
    <cc2:ToolTip ID="ToolTip4" runat="server" DropShadow="true" 
        CSSTheme="newTheme" Text="Custom Css" TopOffset="-40" LeftOffset="-30" 
        PositionTooltip="mouse" MouseTracking="true">
    simple custom css
    </cc2:ToolTip>
     <br />
    <cc2:ToolTip ID="ToolTip6" runat="server" Text="Iframed Test" 
                Local="false" href="/TagsTest.aspx" Sticky="true"
                Width="500" />
    <br />--%>

    <asp:Label ID="Label1" runat="server"
        Text="Label">test asdf asdfa sdfa sdf asdf </asp:Label>
     <cc2:ToolTip ID="ToolTip7" runat="server" Text="Default" TargetControlID="Label1"
      Sticky="true" MouseOutClose="true" title="AASDAS "
        ClosePosition="title" CloseText="X"
        Arrows="true" >
        bare minium
       <asp:Button ID="Button1" runat="server" Text="Button" />
    </cc2:ToolTip>
    </div>
    </form>
</body>
</html>
