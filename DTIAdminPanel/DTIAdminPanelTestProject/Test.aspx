<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Test.aspx.vb" Inherits="DTIAdminPanelTestProject.test" 
    title="Untitled Page" %>
<%@ Register Assembly="DTISortable" Namespace="DTISortable" TagPrefix="cc2" %>
<%@ Register Assembly="DTIAdminPanel" Namespace="DTIAdminPanel" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2>Depth of Menu 1</h2> 
    <cc1:Menu ID="Menu3" runat="server" >
        
    </cc1:Menu> 
			<div class="cl">&nbsp;</div>
			<div class="whole">			
				<%--<cc2:DTISortable ID="DTISortable1" runat="server" contentType="test456" HandleText="Drag Me" ZIndex="10000000">
				    <b>Test some stuff</b>test Page
                    <asp:Button ID="Button2" runat="server" Text="Button" /><asp:CheckBox ID="CheckBox1" runat="server" />
                    <asp:Panel ID="Panel1" runat="server">
                        real panel
                    </asp:Panel>
                    test other stuff<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
				</cc2:DTISortable>--%>
				<cc2:DTISortable ID="sortable2" runat="server" contentType="testpage" ItemsClassName="Post"></cc2:DTISortable>
			</div>
			<div class="cl">&nbsp;</div>
			<div id="footer">
				<p><a href="#">Privacy Policy</a><span>|</span><a href="#">Legal Terms</a><br>Copyright © 2008 VetStreet</p>
			</div>
	
    
    Test Subnav<br />
    
</asp:Content>
