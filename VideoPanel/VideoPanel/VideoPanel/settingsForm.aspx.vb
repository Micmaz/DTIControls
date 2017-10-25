Public Partial Class settingsForm
    Inherits DTIServerControls.SettingsEditorPage
    Friend WithEvents ul As New DTIMediaManager.MediaUploader
    Private vidpre1 As New DTIVideoManager.VideoViewerControl

    Public ReadOnly Property ctrl() As VideoPanel
        Get
            Return DTISeverControlTarget
        End Get
    End Property

    Private Sub settingsForm_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ul.FileFilter = DTIUploader.DTIUploaderControl.FileFilters.Videos
        ul.RedirectURL = Me.Request.Url.AbsoluteUri
        Me.ph1.Controls.Add(ul)
        If ctrl.videoID > -1 Then
            vidpre1.VideoId = ctrl.videoID
            vidpre1.MovieHeight = 150
            vidpre1.MovieWidth = 150
            Me.ph1.Controls.Add(vidpre1)
            lbUpload.Visible = True
            ul.Visible = False
        End If
    End Sub

    Private Sub ul_VideoSaved(ByVal video_id As Integer) Handles ul.VideoSaved
        ctrl.videoID = video_id
        saveControl()
    End Sub

    Private Sub settingsForm_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.tbWidth.Text = ctrl.Width.Value
        Me.tbHeight.Text = ctrl.Height.Value
        'cbautoplay.Checked = ctrl.autoplay
        'cbPopup.Checked = ctrl.popupVideo
    End Sub

    Private Sub settingsForm_saveClicked() Handles Me.saveClicked
        If tbWidth.Text.Trim.Length > 0 Then
            ctrl.Width = tbWidth.Text
        End If
        If tbHeight.Text.Trim.Length > 0 Then
            ctrl.Height = tbHeight.Text
        End If
        'ctrl.autoplay = cbautoplay.Checked
        'ctrl.popupVideo = cbPopup.Checked
    End Sub

    Protected Sub lbUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbUpload.Click
        ul.Visible = True
        vidpre1.Visible = False
        lbUpload.Visible = False
    End Sub
End Class