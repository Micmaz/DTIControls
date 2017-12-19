<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_ATestApplication.Default" %>

<%@ Register Assembly="DTIControls" Namespace="DTIAdminPanel" TagPrefix="cc3" %>

<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="DTI" %>

<%@ Register Assembly="DTIControls" Namespace="JqueryUIControls" TagPrefix="cc2" %>

<%@ Register Assembly="DTIControls" Namespace="DTIContentManagement" TagPrefix="DTIEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	
	<asp:Button ID="btnTurnEditOn" runat="server" Text="Toggle Edit mode" OnClick="btnTurnEditOn_Click" />
	
	<%@ Register Assembly="DTIControls" Namespace="DTIContentManagement" TagPrefix="DTIEdit" %>
	<DTIEdit:EditPanel ID="EditPanel1" runat="server">
		<h1>Edit stuff here!</h1>
	</DTIEdit:EditPanel>
	<br />
	<cc2:ColorPicker color="#FF0000" runat="server" ID="cpcolor1"></cc2:ColorPicker>
	<br />
	<cc2:Autocomplete ID="Autocomplete1" runat="server"></cc2:Autocomplete>
	<br />
	<DTI:Tagger ID="Tagger1" runat="server"></DTI:Tagger>
	<br />
	<DTI:StarRater ID="StarRater1" runat="server"></DTI:StarRater>
	<br />
	<cc3:LoginControl ID="LoginControl" runat="server"></cc3:LoginControl>
</asp:Content>
