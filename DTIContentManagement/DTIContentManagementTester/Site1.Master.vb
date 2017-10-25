Public Partial Class Site1
    Inherits System.Web.UI.MasterPage

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        loggedinControl1.LoggedIn = True
        loggedinControl1.EditOn = True
        Response.Redirect(Request.Url.OriginalString)
    End Sub
End Class