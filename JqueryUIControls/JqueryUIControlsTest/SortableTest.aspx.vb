Public Class SortableTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Sortable1.Controls.Add(New LiteralControl("<div>test</div>"))
        Sortable1.Controls.Add(New LiteralControl("<p>test2</p>"))
    End Sub

End Class