Imports HighslideControls
Imports HighslideControls.SharedHighslideVariables

''' <summary>
''' simple control that shows a clickable title screenshot of a video to expand and watch the movie
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class VideoThumb
    Inherits Highslider
#Else
        <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Public Class VideoThumb
            Inherits Highslider
#End If
    Private _videoId As Integer = -1

    ''' <summary>
    ''' id of the data row associated with the video file
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("id of the data row associated with the video file")> _
    Public Property VideoId() As Integer
        Get
            Return _videoId
        End Get
        Set(ByVal value As Integer)
            _videoId = value
        End Set
    End Property

    ''' <summary>
    ''' Inherited property, cannot be set directly.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Inherited property, cannot be set directly.")> _
    Public Overrides Property ExpandURL() As String
        Get
            If MyBase.ExpandURL = "" Then
                MyBase.ExpandURL = "~/res/DTIVideoManager/VideoViewer.aspx?Id=" & VideoId & "&width=" & _
                    VideoWidth & "&height=" & VideoHeight
            End If
            Return MyBase.ExpandURL
        End Get
        Set(ByVal value As String)
            'MyBase.ExpandURL = value
        End Set
    End Property

    Private _maxWidth As Integer = _thumbSize

    ''' <summary>
    ''' The maximum width of the thumbnail in pixels. If set it will keep the original aspect ratio.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The maximum width of the thumbnail in pixels. If set it will keep the original aspect ratio.")> _
    Public Property MaxThumbWidth() As Integer
        Get
            Return _maxWidth
        End Get
        Set(ByVal value As Integer)
            _maxWidth = value
        End Set
    End Property

    Private _maxHeight As Integer = _thumbSize

    ''' <summary>
    ''' The maximum height of the thumbnail in pixels. If set it will keep the original aspect ratio.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The maximum height of the thumbnail in pixels. If set it will keep the original aspect ratio.")> _
    Public Property MaxThumbHeight() As Integer
        Get
            Return _maxHeight
        End Get
        Set(ByVal value As Integer)
            _maxHeight = value
        End Set
    End Property

    Private _refreshImage As Boolean = False

    ''' <summary>
    ''' Reload the image; Usefull after the thumbnail has changed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Reload the image; Usefull after the thumbnail has changed.")> _
    Public Property RefreshImage() As Boolean
        Get
            Return _refreshImage
        End Get
        Set(ByVal value As Boolean)
            _refreshImage = value
        End Set
    End Property

    Private _thumbSize As Integer = 120

    ''' <summary>
    ''' The default max height and width in pixels.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The default max height and width in pixels.")> _
    Public Property ThumbSize() As Integer
        Get
            Return _thumbSize
        End Get
        Set(ByVal value As Integer)
            _thumbSize = value
        End Set
    End Property

    ''' <summary>
    ''' Url of the thumbnail. This property can't be set manually.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Url of the thumbnail. This property can't be set manually.")> _
    Public Overrides Property ThumbURL() As String
        Get
            If MyBase.ThumbURL = "" Then
                MyBase.ThumbURL = "~/res/DTIVideoManager/ViewVideoScreenShot.aspx?Id=" & VideoId & _
                    "&maxHeight=" & MaxThumbHeight & "&maxWidth=" & MaxThumbWidth & "&showPlayOverlay=1"
            End If
            Dim rdm As New Random
            If RefreshImage Then
                MyBase.ThumbURL &= "&r=" & rdm.Next(1000)
            End If
            Return MyBase.ThumbURL
        End Get
        Set(ByVal value As String)
            'MyBase.ThumbURL = value
        End Set
    End Property

    ''' <summary>
    ''' Overrides hislide display modes
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Overrides hislide display modes")> _
    Public Overrides Property HighslideDisplayMode() As HighslideDisplayModes
        Get
            Return HighslideDisplayModes.Iframe
        End Get
        Set(ByVal value As HighslideDisplayModes)
        End Set
    End Property

    Private _vidWidth As Integer = 500

    ''' <summary>
    ''' The playback width in pixels of the playback pop-up.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The playback width in pixels of the playback pop-up.")> _
    Public Property VideoWidth() As Integer
        Get
            Return _vidWidth
        End Get
        Set(ByVal value As Integer)
            _vidWidth = value
        End Set
    End Property

    Private _vidHeight As Integer = 400

    ''' <summary>
    ''' The playback height in pixels of the playback pop-up.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The playback height in pixels of the playback pop-up.")> _
    Public Property VideoHeight() As Integer
        Get
            Return _vidHeight
        End Get
        Set(ByVal value As Integer)
            _vidHeight = value
        End Set
    End Property

    Public Sub New()

        MyBase.new()

        'Dim videoOverlay As New HighslideControls.HighslideOverlay
        'videoOverlay.position = "top right"
        'videoOverlay.overlayId = "videoCloseOverlay"
        'videoOverlay.useOnHtml = True
        'If Not videoAdded Then
        '    videoOverlay.Text = "<div id='videoCloseOverlay' class='closebutton' onclick='return hs.close(this)' title='Close'></div>"
        '    videoAdded = True
        'End If
        HighslideVariablesString = "dimmingOpacity: 0.75, wrapperClassName: 'borderless', outlineType: null, height: " & VideoHeight + 80 & ", width: " & VideoWidth + 30
        If Not HighslideVariables.Contains("preserveContent") Then
            HighslideVariables.Add("preserveContent", False)
        End If
        MaxThumbHeight = 120
        MaxThumbWidth = 120
    End Sub
End Class
