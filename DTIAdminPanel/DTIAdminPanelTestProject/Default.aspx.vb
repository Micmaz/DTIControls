Public Partial Class _default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnlogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnlogin.Click
        DTIAdminPanel.LoginControl.LoginMode = DTIAdminPanel.LoginControl.LoginModes.Preview
        Response.Redirect(Request.Url.OriginalString)
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnlogout.Click
        DTIAdminPanel.LoginControl.LoginMode = DTIAdminPanel.LoginControl.LoginModes.LoggedOut
        Response.Redirect(Request.Url.OriginalString)
    End Sub

    Protected Sub btnediton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnediton.Click
        DTIAdminPanel.LoginControl.LoginMode = DTIAdminPanel.LoginControl.LoginModes.EditOn
        Response.Redirect(Request.Url.OriginalString)
    End Sub

    Protected Sub btnlayouton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnlayouton.Click
        DTIAdminPanel.LoginControl.LoginMode = DTIAdminPanel.LoginControl.LoginModes.LayoutOn
        Response.Redirect(Request.Url.OriginalString)
    End Sub
End Class