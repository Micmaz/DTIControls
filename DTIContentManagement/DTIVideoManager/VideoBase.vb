Imports DTIServerControls

''' <summary>
''' Base control for all video controls.  Contains info on how videos are stored
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class VideoBase
    Inherits DTIServerBase
#Else
        <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
        Public Class VideoBase
            Inherits DTIServerBase
#End If

    ''' <summary>
    ''' Video conversion helper class, handles video file saving, converting etc. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Video conversion helper class, handles video file saving, converting etc.")> _
    Public ReadOnly Property MyFlashWrapper() As ffmpegHelper
        Get
            Dim _myFlashWrapper As ffmpegHelper = Session("MyFlashWrapper")
            If _myFlashWrapper Is Nothing Then
                Session("MyFlashWrapper") = New ffmpegHelper(sqlhelper)
                _myFlashWrapper = Session("MyFlashWrapper")
                _myFlashWrapper.OutputPath = HttpContext.Current.Server.MapPath(DTIServerControls.DTISharedVariables.UploadFolderDefault)
                If Not System.IO.Directory.Exists(_myFlashWrapper.OutputPath) Then
                    System.IO.Directory.CreateDirectory(_myFlashWrapper.OutputPath)
                End If
                '_myFlashWrapper.ffmpegExePath = mypage.MapPath("/res/DTIUploaderControl/ffmpeg.exe")
                '_myFlashWrapper.Flvtool2ExePath = mypage.MapPath("/res/DTIUploaderControl/flvtool2.exe")
            End If
            _myFlashWrapper.sqlhelper = sqlhelper
            Return Session("MyFlashWrapper")
        End Get
    End Property

    Private Sub mypage_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles mypage.InitComplete
        HighslideControls.HighslideHeaderControl.addToPage(mypage)
    End Sub

End Class
