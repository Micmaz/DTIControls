<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="DTIAjaxTester._Default" %>

<%@ Register Assembly="DTIAjax" Namespace="DTIAjax" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">   
    <div>
<cc1:jsonSeverConrol jsCompleteFunc="done" ajaxReturn="html" ID="JsonSeverConrol1" runat="server" workerclass="DTIAjaxTester.WorkerClass" >
</cc1:jsonSeverConrol>
<a href=# onclick="callhtml()">aaaaa</a>
    </div>
<body>
	<div id="images">
        <br />
        <br />
        <cc1:jsonServerControlTimer interval="5000" action="doit" jsCompleteFunc="doAlert" workerclass="DTIAjaxTester.WorkerClass" ID="JsonServerControlTimer1" runat="server">
        </cc1:jsonServerControlTimer>
</div>
    <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True"></asp:TextBox>
   
    <asp:LinkButton ID="LinkButton1" runat="server">LinkButton</asp:LinkButton>
    <asp:ImageButton ID="ImageButton1" runat="server" />
    <cc1:DTIupdatepanel ID="DTIupdatepanel1" runat="server" Height="203px" Width="385px">
    Test here
        <asp:Label ID="Label1" runat="server" Height="23px" Width="260px"></asp:Label>
         <asp:Button ID="Button1" runat="server" Text="Button" />
        </cc1:DTIupdatepanel>    
        start
        <div id="HTMLAJAXTEST"></div>
        end
<script>
function postbk(e){
alert(e);
}
function done(data){
alert(data);
}

function doAlert(data){
 if(data && data != ""){
    alert(data);
 }
}

function donehtml(htmlstr){
    $('#HTMLAJAXTEST').html(htmlstr);
}

function callhtml(){
    ajaxJsonSeverConrol1('returnHTMLWithString',{some:'Input text to server',another:'testme'},donehtml);
}

$(function(){
    ajaxJsonSeverConrol1('returnHTML','',donehtml);
});

</script>
    
    </form>
</body>
</html>
