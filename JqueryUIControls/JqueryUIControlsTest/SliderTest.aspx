<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SliderTest.aspx.vb" Inherits="JqueryUIControlsTest.SliderTest" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
     $(function() {
         $('#tpPeriod a').bind('click', function() {
            if ($(this).attr('class')) {
                $('#tpSelectedTime .period')
                        .val($(this).attr('class'));
            }
            return false;
        });
        $(".timepickerSubmit").button();
    });
    </script>
</head>
<body>
    <form id="form1" runat="server">
   <%-- <div>
    <cc1:Slider id="Slider1" runat="server"></cc1:Slider>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" />
    </div>--%>
    <cc1:DatePicker value="" ID="DatePicker1" runat="server"></cc1:DatePicker>
    <div id="timepicker" style="width:236px;text-align:center;padding:4px;" class="ui-widget-content ui-corner-all">
        <div id="tpSelectedTime" class="ui-widget-header ui-corner-all" style="padding:4px;">
            <asp:TextBox ID="tbSelHrs" runat="server" style="width:15px">01</asp:TextBox> :
            <asp:TextBox ID="tbSelMins" runat="server" style="width:15px">00</asp:TextBox> 
            <asp:TextBox ID="tbPeriod" CssClass="period" runat="server" style="width:20px">am</asp:TextBox>
        </div>
        <div id="tpPeriod"> <a class="am" href="#">AM</a> | <a class="pm" href="#">PM</a></div>
        <div id="tpBody">
            <span style="float:left">01</span>
            <cc1:Slider id="slHrs" runat="server" Value="6" Min="1" Max="12" style="margin-left:8px;margin-top:4px;margin-bottom:20px;width:180px;float:left"></cc1:Slider>         
            <span>12</span>
            <br style="clear:both;" />
            <span style="float:left">00</span>
            <cc1:Slider id="slMin" runat="server" Max="55" Step="5" style="margin-left:8px;margin-top:4px;width:180px;float:left"></cc1:Slider>
            <span>55</span>
            <br style="clear:both;" />
        </div>
        <div style="text-align:right;padding:4px;margin-top:10px;border-top: solid thin grey;">
            <input class="timepickerSubmit" type="submit" value="OK" />
        </div>
    </div>
    </form>
</body>
</html>
