Partial Public Class _Default
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For i As Integer = 0 To 100
            Dim x As New serverControl
            x.contentType = "DynamicControl" & i
            Me.Controls.Add(x)
        Next
        If Not IsPostBack Then
            cbEdit.Checked = DTIServerControls.DTISharedVariables.AdminOn
        End If
    End Sub

    Private Sub cbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEdit.CheckedChanged
        DTIServerControls.DTISharedVariables.LoggedIn = cbEdit.Checked
        DTIServerControls.DTISharedVariables.AdminOn = cbEdit.Checked
        Response.Redirect(Request.Url.OriginalString)
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        DTIServerControls.DTISharedVariables.AdminOn = True
        DTIServerControls.DTISharedVariables.siteEditMainID = 1
        Response.Redirect("default.aspx", True)
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        DTIServerControls.DTISharedVariables.AdminOn = True
        DTIServerControls.DTISharedVariables.siteEditMainID = 2
        Response.Redirect("default.aspx", True)
    End Sub

    Private Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        DTIServerControls.DTISharedVariables.AdminOn = True
        DTIServerControls.DTISharedVariables.siteEditMainID = 0
        Response.Redirect("default.aspx", True)
    End Sub
End Class