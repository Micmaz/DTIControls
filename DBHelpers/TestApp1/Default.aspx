<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="TestApp1._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
<script type="text/javascript" src="~/res/BaseClasses/Scripts.aspx?d=&f=TestApp1/highslide-full.min.js" language="javascript"></script>     
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="btnCreate" runat="server" Text="Create Tables" />
    
        <br />
        <br />
    
        <asp:Button ID="btnFill" runat="server" Text="Fill Tables" />
    
        <br />
        <br />
    
        <asp:Button ID="btnAdd" runat="server" Text="Add data to Tables" />
    
        <br />
        <br />
    
        
        <asp:Button ID="btnDelete" runat="server" Text="Delete Last row" />
    
        
        <br />
        <br />
    
        
        <asp:Button ID="btnUpdate" runat="server" Text="Update Tables" />
    
        <br />
        <br />
    
        <asp:Button ID="btnDrop" runat="server" Text="Drop Tables" />
    
        <br />
     
        <br />
        <asp:Button ID="btnUpdate0" runat="server" Text="Update Tables Use DA" />
     
        <br />
        <br />
    
         <asp:Button ID="btnDoAll" runat="server" Text="Do All actions" />
       
    
        <br />
    
        <br />
        <asp:Label ID="lblOutput" runat="server" Text=""></asp:Label>
        <br />
    
        <br />
    <a href="~/res/BaseClasses/ListResources.aspx">List Embedded Resources</a>
    </div>
        <asp:Button ID="Button1" runat="server" Text="SwitchDB" Height="26px" />
        <asp:Button ID="btnExcel" runat="server" Text="excel" /><br />
        <br />
        <asp:Button ID="Button6" runat="server" Text="test no PK" Height="26px" />
        <br />
        <br />
        <asp:Button ID="Button3" runat="server" Text="ConcatTest" /><br />
        <asp:Button ID="Button2" runat="server" Text="SortedPAge" />
        <asp:TextBox ID="tbpage" runat="server">100</asp:TextBox>
        <br /><asp:Button ID="Button4" runat="server" Text="Create Multikey tbl" /><br />
        <asp:Button ID="Button5" runat="server" Text="Export Data" />
        <br />
        Destination :<asp:TextBox ID="tbConnection" runat="server" Width="826px">Data Source=testdb.db3;temp_store=2;cache_size=16000;synchronous=OFF;Pooling=true;FailIfMissing=false;</asp:TextBox><br />
        <asp:Literal ID="litAccounts" runat="server"></asp:Literal>
    </form>
</body>
</html>
