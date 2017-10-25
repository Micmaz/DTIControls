<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="jQueryLibraryTester._Default" %>

<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<%@ Register assembly="jQueryLibrary" namespace="jQueryLibrary" tagprefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="content-type" content="text/html; charset=utf-8" />
<title></title> 
<%--<style type="text/css">.ui-widget{ font-size:0.8em; }</style>--%>
<script type="text/javascript" language="javascript">
function checkall(checked){
       $( "#cb1").prop('checked', checked);
       $( "#cb2").prop('checked', checked);
       $( "#cb3").prop('checked', checked);
       $( "#cb4").prop('checked', checked);
       $('input:checkbox').checkbox();
}
</script>
</head>
<body>
    <form id="form1" runat="server">
<%--    <cc1:ThemePicker ID="themes" runat="server" useCookie="true"></cc1:ThemePicker>--%>
    <h2>TEST
        

        
    </h2><h1>test2</h1>
    <a href="#" onclick="$('#cball').click();return false;">checkCA</a>
    <a href="#" onclick="$('input:checkbox').uncheckbox();return false;">uncheckbox</a>
    <a href="#" onclick="$('input:checkbox').checkbox();return false;">checkbox</a>
    <a href="#" onclick="$('input:radio').unradio();return false;">unradio</a>
    <a href="#" onclick="$('input:radio').radio();return false;">radio</a>    
    
    checkall <input onclick="checkall($(this).prop('checked'));" type="checkbox" id="cball" name="cball" value="" />
    <input type="checkbox" id="cb1" value="" />
    <input type="checkbox" id="cb2" value="" />
    <input type="checkbox" id="cb3" value="" />
    <input type="checkbox" id="cb4" value="" />
    
<table>
<tr><td>Enter Name </td><td><input type="text"  /></td></tr>
<tr><td>It has a Datepicker</td><td><input type="text" class="date" /></td>

<tr><td><input type="checkbox" name="language" value="java" />Java</td></tr>
<tr><td><input disabled="disabled" checked="checked" type="checkbox" name="language" value="php" />PHP</td></tr>
<tr><td><input type="checkbox" checked="checked" name="language" value="javascript" />Javascript</td></tr>
<tr><td><input type="checkbox" name="language" value="html/css" />HTML/CSS</td></tr>

<tr><td><input type="radio" disabled="disabled" checked="checked" name="choice" /> Web Developer</td></tr>
<tr><td><input type="radio" name="choice" /> Web Designer</td></tr>
<tr><td><input type="radio" name="choice" /> UI/UE Developer</td></tr>
<tr><td><input type="radio" name="choice" /> Tester</td></tr>
    <tr><td>Experience <select> 
<option >Fresher</option>
<option>&lt;1 year</option>
<option>1-2 years</option>
<option>2-3 years</option>
<option>3-4 years</option>
<option>4-5 years</option>
<option>5-6 years</option>
 </select></td></tr>
<tr><td>Experience <select multiple="multiple"> 
<option >Fresher</option>
<option>&lt;1 year</option>
<option>1-2 years</option>
<option>2-3 years</option>
<option>3-4 years</option>
<option>4-5 years</option>
<option>5-6 years</option>
 </select></td></tr>
<tr><td>Tell Us about Yourself </td><td><textarea></textarea></td></tr>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
<tr><td><input type="reset" /></td><td><input type="submit" value="Enter" /></td></tr>

</table>
    <asp:Button ID="btnConfirm" OnClientClick="return jqconfrim(this,'are you sure?','Confrim Action');" runat="server" Text="Confirm" />
    <input type="button" value="alert" onclick="jqalert('jquery alert!');" />
    <br />

	<cc1:Autocomplete ID="AutoComplete1" runat="server"></cc1:Autocomplete>
	<asp:Button ID="Button1" runat="server" Text="Button1" OnClientClick="loadWaitScreen()" />
<br />
        <cc1:DatePicker ID="DatePicker1" runat="server" ShowOtherMonths="true" > </cc1:DatePicker>
<br />
<br /><br /><br /><br /><br /><br /><br />
<cc1:Tabs ID="tabs" runat="server" Visible="false"></cc1:Tabs>
    </form>
</body>
</html>
