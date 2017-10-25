<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AccordionTest.aspx.vb" Inherits="JqueryUIControlsTest.AccordionTest" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:Accordion ID="Accordion1" runat="server">            
            <Panes>
                <cc1:AccordionPane ID="AccordionPane1" runat="server" Header="test">
                    test
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane2" runat="server" Header="test2">
                    test <asp:Button ID="Button1" runat="server" Text="Button" />
                </cc1:AccordionPane>
            </Panes>
        </cc1:Accordion>
    </div>
    </form>
</body>
</html>
