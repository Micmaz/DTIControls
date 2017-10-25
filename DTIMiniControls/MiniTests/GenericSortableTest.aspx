<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenericSortableTest.aspx.vb" Inherits="MiniTests.GenericSortableTest" %>

<%@ Register Assembly="DTIMiniControls" Namespace="DTIMiniControls" TagPrefix="cc2" %>


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
        This fires the orderchanged event after any post
        <cc2:Sortable ID="Sortable1" runat="server" Width="600px">
        </cc2:Sortable>
        <br />
        <br />
        this one fires the orderchanged event after an item is dropped
        <cc2:Sortable ID="sortable2" runat="server" Width="600px" AutoPostBack="true">
        </cc2:Sortable>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Post Back" />
    </div>
    This one is databound to a table
        <cc2:Sortable ID="Sortable3" runat="server">
        </cc2:Sortable>
    
    </form>
</body>
</html>
