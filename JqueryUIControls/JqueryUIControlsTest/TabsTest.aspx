<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TabsTest.aspx.vb" Inherits="JqueryUIControlsTest.TabsTest" %>

<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:Tabs ID="tabs" runat="server"></cc1:Tabs>
         <cc1:Tabs ID="tabs1" runat="server">
        
         <Tabs>
            <cc1:Tab id="tab1" runat="server" title="test1">
                <span>hhhh</span>test1
                <asp:Button ID="Button1" runat="server" Text="Button" />
            </cc1:Tab>
            <cc1:Tab ID="tab2" runat="server" Title="test2">
                test 2
            </cc1:Tab>
         </Tabs>
         </cc1:Tabs>
        <asp:Button ID="Button2" runat="server" Text="Button" />
    </div>
    </form>
</body>
</html>
