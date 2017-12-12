# DTIControls
.NET control suite. Includes jquery UI controls and a lite content management suit.


This component will create the tables in the database with the connection string named "ConnectionString". The content manager handels images and content history. If there is no connection string it will use SQLite and create a local folder called "Database"

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
