# DTIControls
.NET control suite. Includes jquery UI controls and a lite content management suit.

```HTML
  <%@ Register Assembly="DTIControls" Namespace="DTIContentManagement" TagPrefix="DTIEdit" %>
  <DTIEdit:EditPanel ID="EditPanel1" runat="server">
    <h1>Edit stuff here!</h1>
  </DTIEdit:EditPanel>
    
  <asp:Button ID="btnTurnEditOn" runat="server" Text="Toggle Edit mode" OnClick="btnTurnEditOn_Click" />
```

Code behind:
```C#
      protected void btnTurnEditOn_Click(object sender, EventArgs e)
		  {
			  DTIControls.Share.EditModeOn = !DTIControls.Share.EditModeOn;
		  }
```
