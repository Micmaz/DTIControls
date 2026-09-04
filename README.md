# DTIControls
.NET control suite. Includes jquery UI controls and a lite content management suit.


This component will create the tables in the database with the connection string named "ConnectionString". The content manager handels images and content history. If there is no connection string it will use SQLite and create a local folder called "Database"

Markup:
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

## Precompiled sites (EnableUpdateable=false)

The suite's admin pages and user controls are embedded resources served at `~/res/<Assembly>/<File>` and compiled at runtime. A site precompiled as non-updatable (`aspnet_compiler` without `-u`) can't compile them at runtime, so they must be precompiled ahead of time:

1. Build the solution, then run `.\_DTIControls\PrecompileEmbeddedPages.ps1` (or build with `/p:PrecompileEmbeddedPages=true`).
2. Copy the `*.dll` and `*.compiled` files from `_Output\PrecompiledResources\` into your precompiled site's `bin\` folder — plus the matching subfolder for each satellite assembly you deploy (Reporting, Chart.js, ...). See the generated `README.txt` there for details.

Non-precompiled sites need none of this; the virtual path provider keeps serving the embedded resources directly.
