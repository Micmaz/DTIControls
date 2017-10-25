<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="DTISortableTestProject._Default" %>

<%@ Register Assembly="DTIContentManagement" Namespace="DTIContentManagement" TagPrefix="cc1" %>
<%@ Register Assembly="DTISortable" Namespace="DTISortable" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    
    <style type="text/css"> 
        
        .outer {
	         float:left;
	         margin: 10px;
	         padding: 10px;
	         width: 300px;
        }
         
        .SortablePlaceHolder { 
	        height: 2em; 
	        line-height: 1.5em;
	        background-color:#EEEEEE;
        }
        
        .outerMenu2 {
            position: absolute;
            right: 10px;
            bottom:10px;
        }
        
        .widgetMenuHandle {
            background-color:red;
        }
        
        
        .widgetMenuItem:hover {
            color: green;
        }
        </style> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:CheckBox ID="cbEdit" runat="server" AutoPostBack="true" Text="Edit" /><asp:CheckBox ID="cbReorder" runat="server" AutoPostBack="true" Text="Re-Order" /><br /><br />

         <cc2:DTISortable ID="SortableServer1" runat="server" Width="500px" contentType="firstDiv" HandleText="++Handle++">
            <%--<asp:Panel ID="testPanel1" runat="server">
                test Panel Insert<asp:CheckBox ID="CheckBox1" runat="server" />
            </asp:Panel>
            test Panel 2 bla de bla bah muhahahaha <asp:Button ID="Button2" runat="server" Text="Button" />--%>
            <%-- <cc1:EditPanel ID="EditPanel1" contentType="asdfasdf" runat="server">
             
             </cc1:EditPanel>      --%> 
                  Test Adding Edit Panel by sortable
         </cc2:DTISortable>
         test
       <%--  <cc2:DTISortable ID="SortableServer2" runat="server" Width="400px" contentType="secondDiv" HandleText="++Handle++">
           <asp:Panel ID="Panel1" runat="server">
                
            </asp:Panel>
         </cc2:DTISortable>--%>
        <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
       <%-- <div class="outerMenu2">
            <cc2:DTIWidgetMenu ID="Menu1" runat="server" HandleText="++Handle++"/>
            <cc2:DTIRecycleBin ID="Recycle1" runat="server" NumberOfItems="5" HandleText="++Handle++"/>
        </div>--%>
        <cc1:EditPanel ID="EditPanel2" contentType="asdfasdf1" runat="server">
             test this
             </cc1:EditPanel>     
        <asp:Button ID="Button1" runat="server" Text="Button" />
    </div>
    </form>
</body>
</html>
