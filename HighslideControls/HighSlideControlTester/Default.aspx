<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="HighSlideControlTester._Default" %>
<%@ Register Assembly="HighslideControls" Namespace="HighslideControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
<%--    <cc1:HighslideHeaderControl runat="server" id="highslideheader" >
       hs.wrapperClassName = 'draggable-header';
    
    hs.Expander.prototype.onInit = function (sender) {
    
//  if(this.custom)  if (this.custom.Fixed) { return confirm("YES"); }

//   var s = "The current properties of sender:\n\n";
//   for (var x in sender) {
//      if (typeof sender[x] != "function")
//         s += x +": "+ sender[x] +"\n";
//   }
//   s += "\nClick OK to continue, or Cancel to cancel the expander."
// return confirm(s);
return true;
}

    </cc1:HighslideHeaderControl>--%>
    <script language=javascript>
    function openPopup(){
        <%=Highslider1.openJSFunction %>;
    }
    </script>
    
    <cc1:Highslider runat="server" isFixed=true ID="hsGalleryImage" ThumbURL="/ava.gif" ExpandURL="/logo.jpg" HighslideDisplayMode="Image" />
    
    <cc1:Highslider runat="server" ID="Highslider1" HighslideDisplayMode="HTML">This is a test.</cc1:Highslider>
    <div class="highslide-maincontent">What do we have here?</div>
    
    <cc1:Highslider width="400" height="400" runat="server" ID="Highslider2" HighslideDisplayMode="HTML">This is another test. w/h</cc1:Highslider>
    <div class="highslide-maincontent">What do we have over here?</div>
    </div>

    <cc1:Highslider runat="server" height=400 width=500 isFixed=true ID="Highslider3" ExpandURL="default.aspx" HighslideDisplayMode="Iframe">This Content is fixed.</cc1:Highslider>
    
    <cc1:Highslider  runat="server" height=400 width=500 ID="Highslider4" HighslideDisplayMode="HTML">.NET Form Test</cc1:Highslider>

    <asp:Panel CssClass="highslide-maincontent" runat="server" ID="pnlDynamic">
        <asp:Button ID="Button1" runat="server" Text="Button" /></asp:Panel>
<br /><br />
<a href="#" onclick="return openPopup();">AAAAAA</a>
<input type=button onclick="return openPopup();" />
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    long page
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    </form>
</body>
</html>
