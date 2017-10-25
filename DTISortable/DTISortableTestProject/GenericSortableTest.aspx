<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenericSortableTest.aspx.vb" Inherits="DTISortableTestProject.GenericSortableTest" %>
<%@ Register Assembly="DTISortable" Namespace="DTISortable" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
     <style type="text/css"> 
        .SortablePlaceHolder { 
	        height: 1.5em; 
	        line-height: 1.2em;
	        border: thin dashed red;	
        }
     </style> 
     
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:CheckBox ID="cbEdit" runat="server" AutoPostBack="true" Text="Edit" /><br /><br />
        This fires the orderchanged even after any post
        <cc2:DTISortableGeneric ID="Sortable1" runat="server" Width="600px">
       <%-- <div id="div1" runat="server">
        test
        </div>
       <div id="div2" runat="server">
       test2
       </div>
           --%>
        </cc2:DTISortableGeneric>
        <br />
        <br />
        this one fire the orderchanged even after an item is dropped
        <cc2:DTISortableGeneric ID="sortable2" runat="server" Width="600px">
            
        </cc2:DTISortableGeneric>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Button" />
    </div>
    </form>
</body>
</html>
