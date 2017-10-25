Public Partial Class RotatorStart
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.jQueryInclude.RegisterJQuery(Me)
    End Sub

End Class