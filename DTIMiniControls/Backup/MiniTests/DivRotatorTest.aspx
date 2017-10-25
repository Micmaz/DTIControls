<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DivRotatorTest.aspx.vb" Inherits="MiniTests.DivRotatorTest" %>
<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Div Rotator Test</title>
    <style type="text/css">
    .sizeme{
        height: 100px;
        width: 100px;
        padding: 10px;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="Panel2" runat="server">
    <div class="sizeme" style="background-color:blue"><div>no</div><div>wait</div></div>
    <div class="sizeme" style="background-color:red"><img src="http://dti-it.com/images/proximityscreen.jpg" alt=""/></div>
    <div class="sizeme" style="background-color:green"><img src="http://dti-it.com/res/ContentHolder/ViewImage.aspx?id=15&amp;Width=250" style="border-style: solid; border-width: 0px;" alt=""/></div>
    <div class="sizeme" style="background-color:yellow"></div>
    </asp:Panel>
    <br /><br />
        <DTI:DivRotator ID="DivRotator2" runat="server">
        </DTI:DivRotator>
        <asp:Panel ID="Panel1" runat="server">
            <div class="sizeme" style="background-color:blue"><div>no</div><div>wait</div></div>
    <div class="sizeme" style="background-color:red"></div>
    <div class="sizeme" style="background-color:green"></div>
    <div class="sizeme" style="background-color:yellow"></div>
        </asp:Panel>
        <DTI:DivRotator ID="DivRotator1" runat="server">
        </DTI:DivRotator>
        
    </form>
</body>
</html>
