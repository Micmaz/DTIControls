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

    <%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="DTImini" %>
     <script type="text/javascript" src="<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/pagelist.js"></script>
<%--    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jstree/3.2.1/themes/default/style.min.css" />    <script src="https://cdnjs.cloudflare.com/ajax/libs/jstree/3.2.1/jstree.min.js"></script>--%>
<table>
    	            <tr>
	                <td style="background: url('<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/Gradiant1.jpg') repeat-x ;" valign="top">
	                    <div style="height:310px; overflow: auto">
	                     <DTImini:TreeList ID="tlMenuItems" runat="server" CssClass="demo" 
                                CheckStyle="None"  MultiTreeEnabled="true" OnSelectCallBack="showMenuLink"
                                NewNodeText="New MenuItem" EmptyNodeText="Drag/Drop a Page"
                                CreateMenuText="Create Item" OnDropCallBack="Dropped" MultiSelection="ctrl"
                                AutoInsertOnDrop="false" onCreateCallBack="doMICreate"
                                OnLoadCallBack="function(tree) {tree.close_all();}" ><li>Root node 1</li></DTImini:TreeList>

                        </div>
                    </td>                        
	                <td style="background: url('<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/Gradiant1.jpg') repeat-x ;" valign="top" rowspan="3">                       
                        <div style="height:310px; overflow: auto">
                            <DTImini:TreeList ID="tlPages" runat="server" CssClass="demo" 
                                CheckStyle="None" MultiTreeEnabled="false"
                                Drag_Copy="on" OnLoadCallBack="function(tree) {tree.close_all();}"
                                NewNodeText="New Folder" EmptyNodeText="No Pages"
                                CreateMenuText="Create Folder" onCreateCallBack="doPLCreate"
                                OnSearchCallBack="search" OnRenameCallBack="renamePL" >
                                <DTImini:TreeListUL runat="server" ID="ul1"><DTImini:TreeListItem runat="server" ID="TreeListItem1" Text="asdasd"></DTImini:TreeListItem></DTImini:TreeListUL></DTImini:TreeList>
                        </div>
                        <asp:Button ID="btnAddStatic" runat="server" Text="Button" />      
                        <div id="html1">
  <ul>
    <li>Root node 1</li>
    <li>Root node 2</li>
  </ul>
</div>
                    </td>              

</tr>
</table>
</asp:Content>
