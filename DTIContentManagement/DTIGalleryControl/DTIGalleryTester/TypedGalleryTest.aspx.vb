Imports DTIServerControls.DTISharedVariables

Partial Public Class TypedGalleryTest
    Inherits BaseClasses.BaseSecurityPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cbEdit.Checked = AdminOn
        End If
    End Sub

    Private Sub cbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEdit.CheckedChanged
        AdminOn = cbEdit.Checked
        LoggedIn = cbEdit.Checked
        Response.Redirect(Request.Url.OriginalString)
    End Sub
End Class