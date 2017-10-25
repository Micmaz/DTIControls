Public Partial Class DatePickerTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryThemes.ThemeAdder.AddTheme(Me.Page, jQueryThemes.ThemeAdder.themes.eggplant)
    End Sub

End Class