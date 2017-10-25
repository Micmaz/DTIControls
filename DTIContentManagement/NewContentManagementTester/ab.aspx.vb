Public Class ab
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ed As New ckEditor.ckEditor
        ed.ID = "neweditor"
        ed.Toolbarlayout = ckEditor.Toolbar.ToolbarLayout.FullToolbar
        ed.ToolbarMode = ckEditor.ckEditor.ToolbarModes.PageTop
        PlaceHolder1.Controls.Add(ed)

        PlaceHolder1.Controls.Add(New ckEditor.ckEditor)
    End Sub

End Class