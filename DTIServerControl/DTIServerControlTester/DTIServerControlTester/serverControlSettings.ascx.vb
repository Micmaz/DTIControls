Public Partial Class serverControlSettings
    Inherits DTIServerControls.DTISettingsControl

    Private Sub Page_settingsCreated(ByRef ParentControl As DTIServerControls.DTIServerControl) Handles Me.settingsCreated
        Me.Label1.Text = ParentControl.contentType
        Me.Label2.Text = ParentControl.MainID
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

    End Sub
End Class