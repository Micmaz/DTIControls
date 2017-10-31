<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_ATestApplication.Default" %>

<%@ Register Assembly="DTIControls" Namespace="DTIContentManagement" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	
	<asp:Button ID="btnTurnEditOn" runat="server" Text="Toggle Edit mode" OnClick="btnTurnEditOn_Click" />
	<cc1:EditPanel ID="EditPanel1" runat="server">
		<h1>Edit stuff here!</h1>
	</cc1:EditPanel>

</asp:Content>
