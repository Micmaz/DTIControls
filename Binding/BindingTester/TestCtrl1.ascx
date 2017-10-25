<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TestCtrl1.ascx.vb" Inherits="BindingTester.TestCtrl1" %>
<%@ Register Assembly="JqueryUIControls" Namespace="JqueryUIControls" TagPrefix="cc1" %>
    <div>
      Label ID:  <asp:Label ID="lbId" runat="server" ></asp:Label><br />
      updateDate: <cc1:DatePicker ID="dpUpdateDate" runat="server"> </cc1:DatePicker><br />
       tbTitle: <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox> <br />
       acAuthor: <cc1:Autocomplete ID="acAuthor" runat="server"></cc1:Autocomplete> <br />
       tbTitle: <asp:TextBox ID="tbDesc" TextMode="MultiLine" runat="server"></asp:TextBox> <br />
    </div>
    <br /><br />