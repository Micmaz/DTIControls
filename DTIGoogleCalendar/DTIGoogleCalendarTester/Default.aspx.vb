Imports DTIServerControls

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cbEdit.Checked = DTIServerControls.DTISharedVariables.AdminOn
        End If
    End Sub

    Private Sub cbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEdit.CheckedChanged
        DTIServerControls.DTISharedVariables.AdminOn = cbEdit.Checked
        DTIServerControls.DTISharedVariables.LoggedIn = cbEdit.Checked
        Response.Redirect(Request.Url.OriginalString)
    End Sub

End Class