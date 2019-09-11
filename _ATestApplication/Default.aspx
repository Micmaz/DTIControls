<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_ATestApplication.Default" %>

<%@ Register Assembly="DTIControls" Namespace="DTIAdminPanel" TagPrefix="admin" %>

<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<%@ Register Assembly="DTIControls" Namespace="JqueryUIControls" TagPrefix="jqueryUI" %>

<%@ Register Assembly="DTIControls" Namespace="DTIContentManagement" TagPrefix="DTIEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	
	<asp:Button ID="btnTurnEditOn" runat="server" Text="Toggle Edit mode" OnClick="btnTurnEditOn_Click" />
	<asp:Button ID="Button1" runat="server" Text="Toggle Admin mode" OnClick="btnTurnAdminOn_Click" />
	<%@ Register Assembly="DTIControls" Namespace="DTIContentManagement" TagPrefix="DTIEdit" %>
	<DTIEdit:EditPanel ID="EditPanel1" runat="server">
		<h1>Edit stuff here!</h1>
	</DTIEdit:EditPanel>
	<br />
	<jqueryUI:ColorPicker color="#FF0000" runat="server" ID="cpcolor1"></jqueryUI:ColorPicker>
	<br />
	<jqueryUI:Autocomplete ID="Autocomplete1" runat="server"></jqueryUI:Autocomplete>
	<br />
	<DTI:Tagger ID="Tagger1" runat="server"></DTI:Tagger>
	<br />
	<DTI:StarRater ID="StarRater1" runat="server"></DTI:StarRater>
	<br />
	<admin:LoginControl ID="LoginControl" runat="server"></admin:LoginControl>




</asp:Content>
