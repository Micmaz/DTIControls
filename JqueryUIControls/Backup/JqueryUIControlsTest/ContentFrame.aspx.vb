Public Partial Class ContentFrame
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.ThemeAdder.AddThemeToIframe(Me)
        JqueryUIControls.Dialog.registerControl(Me)
        'Threading.Thread.Sleep(2000)
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("ContentFrame2.aspx")
    End Sub
End Class