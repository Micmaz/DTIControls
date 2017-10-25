Public Class VideoPanel
    Inherits DTIServerControls.DTIServerControl

    Public Sub New()
        Me.settingsPageUrl = "settingsForm.aspx"
        Me.useGenericDTIControlsProperties = True
    End Sub

    Public Overrides ReadOnly Property Menu_Icon_Url() As String
        Get
            Return BaseClasses.Scripts.ScriptsURL & "VideoPanel/frames.png"
        End Get
    End Property

    Private _videoID As Integer = -1
    Public Property videoID() As Integer
        Get
            Return _videoID
        End Get
        Set(ByVal value As Integer)
            _videoID = value
        End Set
    End Property

    Private _popvideo As Boolean = False
    Public Property popupVideo() As Boolean
        Get
            Return _popvideo
        End Get
        Set(ByVal value As Boolean)
            _popvideo = value
        End Set
    End Property

    Private _autoplay As Boolean = False
    Public Property autoplay() As Boolean
        Get
            Return _autoplay
        End Get
        Set(ByVal value As Boolean)
            _autoplay = value
        End Set
    End Property

    Private Sub VideoPanel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Height.IsEmpty Then Me.Height = 150
        If Me.Width.IsEmpty Then Me.Width = 150
        If videoID > -1 Then
            If popupVideo Then
                Dim vidpre As New DTIVideoManager.VideoThumb
                vidpre.VideoId = videoID
                If Me.Width.Value > 0 Then vidpre.MaxThumbWidth = Me.Width.Value
                If Me.Height.Value > 0 Then vidpre.MaxThumbHeight = Me.Height.Value
            Else
                Dim vidview As New DTIVideoManager.VideoViewerControl
                vidview.VideoId = videoID
                vidview.AutoPlay = autoplay
                If Me.Width.Value > 0 Then vidview.MovieWidth = Me.Width.Value
                If Me.Height.Value > 0 Then vidview.MovieHeight = Me.Height.Value
                Me.Controls.Add(vidview)
            End If
        Else
            Me.Controls.Add(New LiteralControl(" "))
        End If
    End Sub

    Private Sub VideoPanel_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
        sqlhelper.checkAndCreateAllTables(New DTIMediaManager.dsMedia)
    End Sub
End Class
