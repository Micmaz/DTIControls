<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="settingsForm.aspx.vb" Inherits="Rotator.settingsForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript">
function setParentContent(){
     parent.$("#" + parentID()).width($("#tbWidth").val()+"px");
     parent.$("#" + parentID()).height($("#tbHeight").val()+"px");
     parent.$("#" + parentID()).css("width",$("#tbWidth").val()+"px");
     parent.$("#" + parentID()).css("height",$("#tbHeight").val()+"px");
    eval("parent.unfreeze_" + parentID() + "()"); 
}
var markup = ''
$(function() {markup=$('#slideStart').html();});
function updateRotator(){
        var easing= $("#DropDownList2").val();
        if(easing=='none') easing=null;
        $('#pnl1').cycle('stop').remove();
        $('#slideStart').append(markup);
        $('#pnl1').cycle({
            fx:     $("#DropDownList1").val(),
            timeout:    $("#tbWait").val(),
            continuous:     false,
            speed:      $("#tbSpeed").val(),
            speedIn:     null,
            speedOut:     null,
            nextId:     null,
            prevId:     null,
            easing:     easing,
            shuffle:     null,
            pause:     false,
            delay:     -1*$("#tbWait").val(),
            nowrap:     false,
            requeueTimeout:     250
        });
        $('#pnl1').css("visibility","");
}

$(function() {
    $("select").change(function(){
       updateRotator();
    });
});
    </script>
</head>
<body> 
    <form id="form1" runat="server">
    <div style="width:318px;height:483px;">
        Example: <br />
            <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
            <div id="slideStart"><asp:Panel ID="pnl1" runat="server">
            </asp:Panel>
            </div>
        <br />
    <table border=0 >
    <tr><td>Transition Effect: </td><td>
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList></td></tr>
    <tr><td>Transition Speed (ms):
        </td><td><asp:TextBox ID="tbSpeed" runat="server"></asp:TextBox></td></tr>
    <tr><td>Easing Effect: </td><td>
        <asp:DropDownList ID="DropDownList2" runat="server">
        </asp:DropDownList></td></tr>        
    <tr><td>Wait time (ms):
        </td><td><asp:TextBox ID="tbWait" runat="server"></asp:TextBox></td></tr>    
    <tr><td>Pause on mouse hover: </td><td><asp:CheckBox ID="cbPause" runat="server" /></td></tr>      
    <tr><td style="height: 22px">Randomize Slides: </td><td style="height: 22px"><asp:CheckBox ID="cbRandomize" runat="server" /></td></tr>    
    </table>
        <br />

    
    <table border=0>
    <tr><td>Width: </td><td><asp:TextBox ID="tbWidth" runat="server"></asp:TextBox></td></tr>
    <tr><td>Height: </td><td><asp:TextBox ID="tbHeight" runat="server"></asp:TextBox></td></tr>
    </table>
        <br />
        &nbsp;</div>
    </form>
</body>
</html>
