<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="iframed.aspx.vb" Inherits="MiniTests.iframed" %>

<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div> <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <DTI:FreezeScreen ID="FreezeScreen1" runat="server"  BackgroundColor="#0000FF" BackgroundOpacity="0.6" >
        <div style="background-color:White;border:solid 1px gray; width:200px" >
            <br />An Iframed wait message.<br />
            <img src="/imgs/orderLoader.gif" /><br /><br />
            </div>
        </DTI:FreezeScreen> 
        I'm an iframe! I have a wait too!
        <asp:Button ID="Button1" runat="server" Text="Iframe Postback" />
        <input type="button" value="Move Red div to center" onclick='$("#testme").center();' />
        <input type="button" value="Iframe Javascript Freeze"  OnClick="FreezeScreen(); setTimeout('UnfreezeScreen();', 5000); return false;"/>
        <div id="testme" style=" width: 200px; border: 1px; background-color:Red;">ASDASDASDASD</div>
        
    </div><asp:Button ID="Button2" runat="server" Text="15 Second wait" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button runat="server" ID="btnAjaxFreezeExample" Text="Iframe AJAX Example" OnClick="btnAjaxFreezeExample_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
