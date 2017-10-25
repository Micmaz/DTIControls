Partial Public Class settingsForm
    Inherits DTIServerControls.SettingsEditorPage

    Public ReadOnly Property ctrl() As DTIMapServerControl
        Get
            Return DTISeverControlTarget
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tbAddress.Text = ctrl.Address
        tbTitle.Text = ctrl.AddressTitle
        tbGoogleKey.Text = ctrl.GoogleKey

    End Sub

    Private Sub Page_saveClicked() Handles Me.saveClicked
        ctrl.Address = tbAddress.Text
        ctrl.AddressTitle = tbTitle.Text
        ctrl.GoogleKey = tbGoogleKey.Text
    End Sub

End Class