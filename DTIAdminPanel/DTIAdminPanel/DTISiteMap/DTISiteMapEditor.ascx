<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DTISiteMapEditor.ascx.vb" Inherits="DTIAdminPanel.DTISiteMapEditor" %>

<%@ Register Assembly="DTIControls" Namespace="DTIMiniControls" TagPrefix="cc1" %>
<script type="text/javascript">
        

        function Dropped(node, refnode, type, tree){
            if (tree.get_type(node) != "folder"){       
                tree.create({
                    attributes : {
                        "rel" : "link",
                        "val" : "X"
                    },
                    data : {
                        title : tree.get_text(node)
                    }
                }, refnode, type, "link", true);
            }
        }
        
        function doMICreate(node, refnode, type, tree, rb){ 
            if (tree.get_type(node) != "EmptyTreeHolder") {     
                if (tree.get_nodes().length == 2 && 
                        ($('#' + tree.get_nodes()[0][0]).attr('rel') == "EmptyTreeHolder" || 
                         $('#' + tree.get_nodes()[1][0]).attr('rel') == "EmptyTreeHolder")){
                    tree.remove(refnode,true);
                }
                if (node.getAttribute("val") != null){
                    node.setAttribute("rel", "link");
                } 
            }      
        }
</script>
<table>
    <tr>
        <td valign="top">
       <%-- <cc1:TreeList ID="tlMenuItems" runat="server" CssClass="demo" 
                CheckStyle="None"  MultiTreeEnabled="true"
                EmptyNodeText="Drag/Drop a Page" DeleteMenuText="Remove Page"
                OnDropCallBack="Dropped" onCreateCallBack="doMICreate" />--%>
            <cc1:TreeList ID="tlMenuItems" runat="server" CssClass="demo" 
                CheckStyle="None"  MultiTreeEnabled="true"
                EmptyNodeText="Drag/Drop a Page" DeleteMenuText="Remove Page"
                OnDropCallBack="Dropped" AutoInsertOnDrop="false" 
                onCreateCallBack="doMICreate" />
        </td>                        
        <td valign="top">
            <cc1:TreeList ID="tlPages" runat="server" CssClass="demo" 
                CheckStyle="None" MultiTreeEnabled="false"                   
                 EmptyNodeText="No Pages" ShowContextMenu="false"
                 Drag_Copy="on" No_Int_Copy="true" ReadModeOnly="true" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align:right;">
            <asp:Button ID="btnSave" runat="server" Text="Save" /></td>
    </tr>
</table>