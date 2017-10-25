<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Tester1.aspx.vb" Inherits="ckeditTester.Tester1" %>
<%@ Register Assembly="SummerNote" Namespace="SummerNote" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	        <cc1:SummerNote ID="SummerNote2" runat="server" Height="300px" Width="500px" DivCssClass="divClass" ToolbarMode="PageTop">
        </cc1:SummerNote>
</asp:Content>
