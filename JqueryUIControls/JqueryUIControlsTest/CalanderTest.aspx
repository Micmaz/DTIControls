<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CalanderTest.aspx.vb" Inherits="JqueryUIControlsTest.CalanderTest" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
    <script type="text/javascript" language="javascript">





        /*   $(function(){
        $('<div></div').load("http://www.google.com", function() {
        var container = $(this);
        container.dialog({})
        }); 
        }); */
</script>
<script type="text/javascript" language="javascript">

  
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    <cc1:ThemePicker useCookie="true" ID="ThemePicker1" runat="server"/>
    <a href="javascript:void(0)" onclick="createDlg('ContentFrame.aspx?asd=1');">dlg Link1</a>

        <cc1:Dialog ID="Dialog2" OpenerText="open Dialog" OpenerType="Button" runat="server">
        <span id="clickme">Click me!</span>
    </cc1:Dialog>
           <cc1:Calendar MonthViewLimit="3" ajaxLoad="true" runat="server" DefaultView="month" ID="cal1"></cc1:Calendar> 
           
    </div>
    </form>
</body>
</html>
