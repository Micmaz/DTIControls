<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Homepage.aspx.vb" Inherits="DTIAdminPanelTestProject.Homepage" %>
<%@ Register Assembly="DTIAdminPanel" Namespace="DTIAdminPanel" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .selected {color:red;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float:left; width:49%">
            <h2>Normal Menu</h2>
            <cc1:Menu ID="Menu1" runat="server" Template="simple" SelectedCss="selected">
            </cc1:Menu>
            
          <%--  <h2>Depth of Menu 1</h2> 
            <cc1:Menu ID="Menu3" runat="server" DepthOfMenu="1" Template="simple" SelectedCss="selected">
            </cc1:Menu> --%>
        </div>
        <div style="float:right;width:49%">
           <%-- <h2>Fixed Depth 1</h2>    
            <cc1:Menu ID="Menu4" runat="server" FixedDepth="1" Template="simple" SelectedCss="selected">
            </cc1:Menu> 
            
            <h2>Fixed Depth 1 & Fixed Depth Travel 1</h2>     
            <cc1:Menu ID="Menu2" runat="server" FixedDepth="1" FixedDepthtravel="2" Template="simple" SelectedCss="selected">
            </cc1:Menu> --%>
        </div> 
        <br style="clear:both" />    
    </div>
    </form>
</body>
</html>
