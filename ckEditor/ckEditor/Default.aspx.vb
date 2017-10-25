Imports DTIServerControls

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cbEdit.Checked = DTISharedVariables.AdminOn
        End If
    End Sub

    Private Sub cbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEdit.CheckedChanged
        'DTISharedVariables.LoggedIn = cbEdit.Checked
        ' DTISharedVariables.AdminOn = cbEdit.Checked
        Response.Redirect(Request.Url.OriginalString)
    End Sub


End Class