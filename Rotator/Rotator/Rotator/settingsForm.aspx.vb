Public Partial Class settingsForm
    Inherits DTIServerControls.SettingsEditorPage

    Public ReadOnly Property ctrl() As Rotator
        Get
            Return DTISeverControlTarget
        End Get
    End Property

    Private Sub settingsForm_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.tbWidth.Text = ctrl.Width.Value
        Me.tbHeight.Text = ctrl.Height.Value
        DropDownList1.SelectedValue = ctrl.Transition
        If Not ctrl.Easing = "null" Then
            DropDownList2.SelectedValue = ctrl.Easing
        End If

        tbSpeed.Text = ctrl.TransitionSpeed
        tbWait.Text = ctrl.waitTime
        cbPause.Checked = ctrl.Pause
        cbRandomize.Checked = ctrl.Randomize

        exrotator.fx = DropDownList1.SelectedValue
        exrotator.speed = tbSpeed.Text
        exrotator.timeout = tbWait.Text
        exrotator.pause = cbPause.Checked
        exrotator.random = cbRandomize.Checked
        exrotator.easing = DropDownList2.SelectedValue
    End Sub

    Private Sub settingsForm_saveClicked() Handles Me.saveClicked
        If tbWidth.Text.Trim.Length > 0 Then
            ctrl.Width = tbWidth.Text
        End If
        If tbHeight.Text.Trim.Length > 0 Then
            ctrl.Height = tbHeight.Text
        End If
        ctrl.Transition = DropDownList1.SelectedValue
        ctrl.TransitionSpeed = tbSpeed.Text
        ctrl.waitTime = tbWait.Text
        ctrl.Pause = cbPause.Checked
        ctrl.Randomize = cbRandomize.Checked
        ctrl.Easing = DropDownList2.SelectedValue
    End Sub

    Dim exrotator As New DTIMiniControls.DivRotator
    Private Sub settingsForm_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.excludeProperties &= "text"
        Me.supressWritingToHiddenDiv = True
        For Each val As String In [Enum].GetNames(GetType(Rotator.effect))
            Me.DropDownList1.Items.Add(val)
        Next
        For Each val As String In [Enum].GetNames(GetType(Rotator.easingEnum))
            Me.DropDownList2.Items.Add(val)
        Next
        Me.maxPropertySearchDepth = 4
        exrotator.Container = pnl1
        Me.ph1.Controls.Add(exrotator)

        For i As Integer = 1 To 7
            Dim x As New System.Web.UI.WebControls.Image
            x.ImageUrl = BaseClasses.Scripts.ScriptsURL & "Rotator/" & i & ".png"
            x.Width = 150
            x.Height = 150
            pnl1.Controls.Add(x)
        Next
    End Sub
End Class