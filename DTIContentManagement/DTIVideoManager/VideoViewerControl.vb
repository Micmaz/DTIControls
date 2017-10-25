''' <summary>
''' Control to render a streamable video and a playback controls to a page.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class VideoViewerControl
    Inherits VideoBase
#Else
        <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Public Class VideoViewerControl
            Inherits VideoBase
#End If

    Private _videoPath As String = ""

    ''' <summary>
    ''' Path to the flash video file. 
    ''' If not set it first defaults to: 
    ''' 1) the query string "vidFile" 
    ''' 2) the videoRow data. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Path to the flash video file.    If not set it first defaults to:    1) the query string ""vidFile""    2) the videoRow data.")> _
    Public Property videoPath() As String
        Get
            If mypage Is Nothing OrElse mypage.Request.QueryString("vidFile") Is Nothing Then
                If _videoPath = "" Then
                    Dim path As String = DTIServerControls.DTISharedVariables.UploadFolderDefault.Replace("\", "/").Trim("/")
                    _videoPath = "/" & path & "/" & MyFlashWrapper.OutputFileNamingConvention & VideoId & ".flv.swf?streamer=fake.flv"
                End If
                Return _videoPath
            Else
                Return mypage.Request.QueryString("vidFile")
            End If
        End Get
        Set(ByVal value As String)
            _videoPath = value
        End Set
    End Property

    Private _screenShotPath As String = ""

    ''' <summary>
    ''' Path to the flash video screenshot file. 
    ''' If not set it first defaults to: 
    ''' 1) the query string "vidscreenfile" 
    ''' 2) the videoRow data. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Path to the flash video screenshot file.    If not set it first defaults to:    1) the query string ""vidscreenfile""    2) the videoRow data.")> _
    Public Property screenShotPath() As String
        Get
            If mypage Is Nothing OrElse mypage.Request.QueryString("vidscreenfile") Is Nothing Then
                If _screenShotPath = "" Then
                    _screenShotPath = "~/res/DTIVideoManager/ViewVideoScreenShot.aspx?id=" & VideoId
                End If
                Return _screenShotPath
            Else
                Return mypage.Request.QueryString("vidscreenfile")
            End If
        End Get
        Set(ByVal value As String)
            _screenShotPath = value
        End Set
    End Property

    Private _vidId As Integer = -1
    Public Property VideoId() As Integer
        Get
            If mypage Is Nothing OrElse mypage.Request.QueryString("VidId") Is Nothing Then
                Return _vidId
            Else
                Return mypage.Request.QueryString("vidId")
            End If
        End Get
        Set(ByVal value As Integer)
            _vidId = value
        End Set
    End Property

    Private _autoPlay As Boolean = False
    Public Property AutoPlay() As Boolean
        Get
            If mypage Is Nothing OrElse mypage.Request.QueryString("autoplay") Is Nothing Then
                Return _autoPlay
            Else
                Return Not mypage.Request.QueryString("autoplay") Is Nothing
            End If
        End Get
        Set(ByVal value As Boolean)
            _autoPlay = value
        End Set
    End Property

    Private _width As Integer = -1
    Public Property MovieWidth() As Integer
        Get
            If mypage Is Nothing OrElse mypage.Request.QueryString("width") Is Nothing Then
                Return _width
            Else
                Return mypage.Request.QueryString("width")
            End If
        End Get
        Set(ByVal value As Integer)
            _width = value
        End Set
    End Property

    Private _height As Integer = -1
    Public Property MovieHeight() As Integer
        Get
            If mypage Is Nothing OrElse mypage.Request.QueryString("height") Is Nothing Then
                Return _height
            Else
                Return mypage.Request.QueryString("height")
            End If
        End Get
        Set(ByVal value As Integer)
            _height = value
        End Set
    End Property

    Private scriptph As New LiteralControl
    Private Sub VideoViewerControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim hlGetFlash As New HyperLink()
        hlGetFlash.Text = "Get the Flash Player"
        hlGetFlash.NavigateUrl = "http://www.macromedia.com/go/getflashplayer"
        Controls.Add(hlGetFlash)
        Controls.Add(scriptph)
    End Sub

    Private Sub VideoViewerControl_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        registerSWFJSFile(mypage)
        registerInitScript(Me.ClientID, mypage)
    End Sub

    Public Sub registerSWFJSFile(ByRef addPage As Page)
        jQueryLibrary.jQueryInclude.addScriptFile(addPage, "/DTIVideoManager/swfobject.js", Nothing, True)
        'registerClientScriptFile("swfobject", "/res/DTIVIdeoManager/swfobject.js", addPage)
        'registerClientStartupScriptBlock("hsVideoCSS", "parent.window.hs.Expander.prototype.onBeforeExpand = function () {" & _
        '    "if(this.a.href.indexOf('VideoViewer.aspx') > -1) {" & _
        '    "$('.highslide-wrapper, .highslide-outline').css('background','none');" & _
        '    "$('.highslide-html').css('background-color','transparent');}};", True)
    End Sub

    Public Sub registerInitScript(ByVal elementId As String, ByRef addPage As Page, Optional ByVal vidWidth As Integer = -1, Optional ByVal vidHeight As Integer = -1, Optional ByVal videoID As Integer = -1)
        If vidWidth = -1 Then
            vidWidth = MovieWidth
        End If
        If vidHeight = -1 Then
            vidHeight = MovieHeight
        End If
        If videoID = -1 Then
            videoID = Me.VideoId
        End If
        If videoID > -1 AndAlso (vidHeight = -1 OrElse vidWidth = -1) Then
            Dim dt As New DataTable
            sqlhelper.FillDataTable("select height, width from DTIVideoManager where id = @id", dt, videoID)
            If dt.Rows.Count > 0 Then
                If vidHeight = -1 AndAlso vidWidth = -1 Then
                    vidHeight = dt.Rows(0)("height")
                    vidWidth = dt.Rows(0)("width")
                ElseIf vidHeight = -1 Then
                    vidHeight = (dt.Rows(0)("height") / dt.Rows(0)("width")) * vidWidth
                Else
                    vidWidth = (dt.Rows(0)("width") / dt.Rows(0)("height")) * vidHeight
                End If

            End If
        End If
        Dim randnum As String = elementId
        Dim InitScript As String = "function makevid_" & randnum & "(){ var s1 = new SWFObject(""" & _
            BaseClasses.Scripts.ScriptsURL & "DTIVideoManager/player.swf" & _
            """,""ply"",""" & vidWidth & """,""" & vidHeight & """,""9"",""#FFFFFF""); " & _
            "s1.addParam(""allowfullscreen"",""true""); " & _
            "s1.addParam(""allowscriptaccess"",""always""); " & _
            "s1.addParam(""wmode"",""opaque""); " & _
            "s1.addParam(""autostart"",""" & AutoPlay & """); " & _
            "s1.addParam(""flashvars"",""file=" & videoPath & "&image=" & screenShotPath & "&width=" & vidWidth & "&height=" & vidHeight & """); " & _
            "s1.write(""" & elementId & """);} " & vbCrLf & "$(function(){makevid_" & randnum & "();});"
        jQueryLibrary.jQueryInclude.addScriptBlock(addPage, InitScript)

    End Sub

    Private Sub VideoViewerControl_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
        sqlhelper.checkAndCreateAllTables(New dsDTIVideo)
    End Sub
End Class
