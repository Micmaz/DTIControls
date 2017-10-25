<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="MiniTests._Default" %>

<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        Editor Example: 
    <DTI:HighlighedEditor Text="<h5>awdawdwd</h5>
<br />
<br />
<strong><em>awddaw</em></strong>"  language="html" Height="300px" Width="400px" ID="HighlighedEditor1" runat="server"> </DTI:HighlighedEditor>
<br>
        <asp:Label ID="lblHLEditor" runat="server" Text=""></asp:Label><asp:Button ID="Button2"
            runat="server" Text="Button" />
<a href="#" onclick="editor_HighlighedEditor1.toTextArea();">test</a>
<DTI:HighlighedEditor Text="Select * from tables w here i = 10"  theme="night" language="java" Height="300px" Width="400px" ID="HighlighedEditor2" runat="server"> </DTI:HighlighedEditor>
   <br />     <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" /><br />
    <div>
    
    Rollover image example:<br />
        <DTI:RollOverImage ID="RollOverImage1" runat="server" ImageRollOverUrl="~/imgs/literature_f2.gif"
            ImageUrl="~/imgs/literature.gif" /><br /><hr />     
        TextBoxEncoded example:<br />
        <DTI:TextBoxEncoded ID="TextBoxEncoded1" runat="server"> </DTI:TextBoxEncoded>
        <asp:Button ID="btnEncode" runat="server" Text="Encode" /><br />
        (It's really a text box that allows you to submit html)<br />
        <asp:Label ID="lblEncoded" runat="server"></asp:Label><br />
        <br />
        <hr />     
        HiddenField Encoded example1:<br /> ##<a id="submitover">Mouseover to submit</a>##
        <script language="javascript">
        $(document).ready(function(){
            $("#submitover").bind("mouseenter", function(e){
                $("form:first").trigger("submit");
            }); 
        });
        </script>
        <!--<input id="hidfld" type="text" onchange="document.getElementById('HiddenFieldEncoded1').value = document.getElementById('hidfld').value;"/>-->
        <input id="hidfld" type="text"/>
        <DTI:HiddenFieldEncoded ID="HiddenFieldEncoded1" runat="server" Value="<b>This has html in it.</b>" />
        <asp:Button ID="Button1" runat="server" Text="Encode" />
        &nbsp;<br />
        (It's really a text box that allows you to submit html)<br />
        <asp:Label ID="lblHidEncoded" runat="server"></asp:Label><br />
        <br />
        <hr />   
        Inline Text Edit Box:<br />
        <DTI:EditLabel ID="EditLabel1" runat="server" Text="Test"/>
        <br />
        <hr />     
        Script Block:<br />
        <DTI:ScriptBlock ID="ScriptBlock1" runat="server"/>
        <input type="button" onclick="showAlert();" value="Test" />
        <br />
        <hr />
        Freeze Screen Example:<br /> 
        <input type="button" value="Javascript Freeze"  OnClick="FreezeScreen(); setTimeout('UnfreezeScreen();', 1000); return false;"/>
        <asp:button runat="server" ID="btnPostbackFreezeExample" Text="Postback Example"/><br />
                   <ContentTemplate>
                <asp:Button runat="server" ID="btnAjaxFreezeExample" Text="AJAX Example" OnClick="btnAjaxFreezeExample_Click" />
            </ContentTemplate>
        </div>
        <iframe src="iframed.aspx" width="300px" height="200px"></iframe>
        <DTI:FreezeScreen ID="FreezeScreen1" runat="server" BackgroundColor="#E6E0CE" BackgroundOpacity="0.6" 
            DisplayOnPartialPostback="false">
            <div style="background-color:White;border:solid 1px gray; width:200px" >
            WE DOIN STUFF!!<br /><br /><img src="/imgs/orderLoader.gif" /><br /><br />
            </div>
        </DTI:FreezeScreen>
        <DTI:FreezeScreen ID="FreezeScreen2" runat="server" BackgroundColor="#555555" BackgroundOpacity="0.8" 
            DisplayOnPartialPostback="true" DisplayOnAnyPostback="false">
            <div style="background-color:White;border:solid 1px gray; width:200px" >
            WE DOIN
            AJAX STUFF!! With special chars !@#" ' " '           
            <br /><br />
            <img alt="" src="/imgs/orderLoader.gif" /><br /><br />
            </div>
        </DTI:FreezeScreen>
    </form>
</body>
</html>
