<%@ Page Language="vb" validateRequest="false" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="BindingTester._Default" %>

<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      Label ID:  <asp:Label ID="lbId" runat="server" ></asp:Label><br />
      updateDate: <cc1:DatePicker ID="dpUpdateDate" runat="server"> </cc1:DatePicker><br />
       tbTitle: <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox> <br />
       acAuthor: <cc1:Autocomplete ID="acAuthor" runat="server"></cc1:Autocomplete>
    </div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    <asp:Button ID="btnSave" runat="server" Text="Save" />
    </form>
</body>
</html>
