Partial Public Class ContentFrame2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        jQueryLibrary.ThemeAdder.AddThemeToIframe(Me)
    End Sub

End Class