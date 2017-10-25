Public Partial Class settingsForm
    Inherits DTIServerControls.SettingsEditorPage

    Private Sub Page_PreRender2(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        tbWid.Text = control.Width.Value
        tbHeight.Text = control.Height.Value
        TextBox1.Text = control.something
    End Sub

    Private Sub Page_saveClicked() Handles Me.saveClicked
        control.something = Me.TextBox1.Text
        control.Width = Me.tbWid.Text
        control.Height = tbHeight.Text
    End Sub


    Private ReadOnly Property control() As serverControl
        Get
            Return DTISeverControlTarget
        End Get
    End Property

End Class