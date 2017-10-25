<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Default.aspx.vb" Inherits="DTIContentManagementTester._Default" 
    title="Untitled Page" %>
<%@ Register Assembly="ckEditor" Namespace="ckEditor" TagPrefix="cc1" %>    
<%@ Register Assembly="DTISortable" Namespace="DTISortable" TagPrefix="cc2" %>
<%@ Register Assembly="DTIContentManagement" Namespace="DTIContentManagement" TagPrefix="cc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="cl">&nbsp;</div>
			<div class="left">
			<asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" />
				<cc2:DTISortable ID="DTISortable1" runat="server" contentType="News" HandleText="Drag Me" ZIndex="10000000">
				</cc2:DTISortable>
			</div>
			<div class="right">
                
				<div class="news">
					<cc2:DTISortable ID="DTISortable2" runat="server" contentType="aboutUs" HandleText="Drag Me" ZIndex="10000000">
					</cc2:DTISortable>
					
				</div>
			</div>
    <asp:Button ID="Button1" runat="server" Text="Mainid 1" />
    <asp:Button ID="Button2" runat="server" Text="Mainid 2" />
			<br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
			<br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
			<cc3:EditPanel runat="server" ID="EdasdasitPanel1" MainID="2" >This is main id 2</cc3:EditPanel>
			<br /><br />
			<cc3:EditPanel runat="server" ID="EditPanelasdasd" >This is mainid 0 </cc3:EditPanel>
			<br /><br />
			<cc3:EditPanel runat="server" ID="mainid1" MainID="1" >This is mainid1</cc3:EditPanel>
			<br />
			<cc3:EditPanel runat="server" ID="mainid2" MainID="2" >This is mainid2</cc3:EditPanel>
			<div class="cl">&nbsp;</div>
			<div id="footer">
			<cc3:EditPanel runat="server" ID="EditPanelStatic" contentType="overviewdemo" StaticMode="true" Mode="write"><strong>Click me to Edit!</strong></cc3:EditPanel>
			  <%--  <cc1:ckEditor ID="CkEditor1" runat="server" Height="200px" Width="300px" DivCssClass="divClass" ToolbarMode="PageTop"></cc1:ckEditor>--%>
				<p><a href="#">Privacy Policy</a><span>|</span><a href="#">Legal Terms</a><br>Copyright © 2008 VetStreet</p>
			</div>
    
</asp:Content>
