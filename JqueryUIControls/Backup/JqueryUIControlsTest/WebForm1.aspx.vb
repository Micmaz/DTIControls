Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("theme") IsNot Nothing Then jQueryLibrary.ThemeAdder.AddTheme(Me)
    End Sub

End Class