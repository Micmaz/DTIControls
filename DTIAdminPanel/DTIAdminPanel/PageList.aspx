<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PageList.aspx.vb" Inherits="DTIAdminPanel.PageList" %>
<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <script
  src="https://code.jquery.com/jquery-1.3.2.min.js"
  integrity="sha256-yDcKLQUDWenVBazEEeb0V6SbITYKIebLySKbrTp2eJk="
  crossorigin="anonymous" ></script>
<head runat="server">
    <title>Page List</title>
</head>
<body>
    <form id="form1" runat="server">
	<script type="text/javascript" src="<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/pagelist.js"></script>
	<link rel="stylesheet" type="text/css" href='<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/iframe-default.css' />
    <link rel="stylesheet" type="text/css" href='<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/DTIAdminPanel.css' />
        <asp:HiddenField ID="HiddenField1" runat="server" /> 
        <asp:HiddenField ID="hfMenus" runat="server" />
        <asp:HiddenField ID="hfPages" runat="server" />
        <asp:HiddenField ID="hfRefresh" runat="server" />
        <div>
	        <table style="width:100%">
                <tr>
                <td colspan="2"style="background: url('<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/Gradiant2.jpg') repeat-y ;">
                    <h2 style="margin-top: 0px; margin-bottom: 0px;"><span style="float:left; ">Page List</span></h2>
                    <span style="float:left"><label style="padding-left:50px;" id="lblurl" runat="server"></label></span>               
                    <span style="float:right"></span>
                    </td>
                    <td colspan="1">
                        <%--&nbsp;<asp:Button ID="Button1" runat="server" Text="Rename Sortables" Width="122px" />--%></td>
                    <td colspan="1">
                        <asp:Button ID="btnSave" runat="server" Text="Save" /></td>
                </tr>           
                <tr>
                    <td >
                        Menu Items 
                        <cc1:ToolTip ID="tooltip2" runat="server" DropShadow="true" HtmlTag="span" Text="" >
                        This tree represents how your menu will look and behave
                        </cc1:ToolTip></td>
                    <td>
                        Pages 
                        <cc1:ToolTip ID="tooltip1" runat="server" DropShadow="true" HtmlTag="span" Text="" >
                        This tree represents all the available pages in the site.  To add a page to the menu
                        simply drag the page and drop it into
                        the Menu Items tree</cc1:ToolTip></td>
                    <td>
                            Add Page
                    </td>
                    <td style="width: 200px;" valign="top">
                            You can right click any item in the Page List tree or the Menu Items tree to perform
                            more functions like duplicating pages
                    </td>
                </tr>
                <tr>
                    <td >
                        <img alt="expand" style="cursor:pointer" src="<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/add.png" onclick="$.tree.reference('tlMenuItems').open_all();" />
                        <img alt="collapse" style="cursor:pointer" src="<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/remove.png" onclick="$.tree.reference('tlMenuItems').close_all();" />            
                    </td>
                    <td style="vertical-align:top;">
                        <img alt="expand" style="cursor:pointer" src="<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/add.png" onclick="$.tree.reference('tlPages').open_all();" />
                        <img alt="collapse" style="cursor:pointer" src="<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/remove.png" onclick="$.tree.reference('tlPages').close_all();" />            
                    </td>
                    <td valign="top">
                    </td>
                    <td valign="top">
                    </td>
                </tr>
	            <tr>
	                <td style="background: url('<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/Gradiant1.jpg') repeat-x ;" valign="top">
	                    <div style="height:310px; overflow: auto">
	                     <cc1:TreeList ID="tlMenuItems" runat="server" CssClass="demo" 
                                CheckStyle="None"  MultiTreeEnabled="true" OnSelectCallBack="showMenuLink"
                                NewNodeText="New MenuItem" EmptyNodeText="Drag/Drop a Page"
                                CreateMenuText="Create Item" OnDropCallBack="Dropped" MultiSelection="ctrl"
                                AutoInsertOnDrop="false" onCreateCallBack="doMICreate"
                                OnLoadCallBack="function(tree) {tree.close_all();}" />

                        </div>
                    </td>                        
	                <td style="background: url('<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/Gradiant1.jpg') repeat-x ;" valign="top" rowspan="3">                       
                        <div style="height:310px; overflow: auto">
                            <cc1:TreeList ID="tlPages" runat="server" CssClass="demo" 
                                CheckStyle="None" MultiTreeEnabled="false"
                                Drag_Copy="on" OnLoadCallBack="function(tree) {tree.close_all();}"
                                NewNodeText="New Folder" EmptyNodeText="No Pages"
                                CreateMenuText="Create Folder" onCreateCallBack="doPLCreate"
                                OnSearchCallBack="search" OnRenameCallBack="renamePL" />
                        </div>
                        <asp:Button ID="btnAddStatic" runat="server" Text="Button" />                        
                    </td>              
                    <td rowspan="3" style="height: 200px; text-align: left;" valign="top">
                        <table style="background: url('<%=BaseClasses.Scripts.ScriptsURL()%>/DTIAdminPanel/Gradiant2.jpg') repeat-y ;">
                           <%-- <tr>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblTemplate" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Value="1">Full Page</asp:ListItem>
                                        <asp:ListItem Value="2">2 Columns</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td valign="top">
                                    Name:</td>
                                <td>
                                    <asp:TextBox ID="tbPageName" runat="server" onkeyup="removeSpaces();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Master Page:</td>
                                <td>
                                    <asp:DropDownList ID="ddlMasterPage" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Label ID="lblError" runat="server" ForeColor="red" Visible="false">Page Names must be Unique</asp:Label>
                                    <asp:Button ID="btnAdd" runat="server" Text="Add" /></td>
                            </tr>
                        </table>
                                             
                    </td>
                    <td rowspan="3" style="text-align:left" valign="top">
                    </td>
                </tr>
                <tr>                    
                    <td colspan="3" valign="top">
                        <span style="visibility:hidden">.</span><label id="lbl" runat="server"></label>
                    </td>                    
                    <td colspan="1" valign="top">
                    </td>
                </tr>
            </table> 
            <div style="visibility:hidden"><asp:Button ID="btnDuplicate" runat="server" Text="Dup" />
                </div>
        </div>      
    </form>
</body>
</html>
